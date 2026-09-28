/* =====================================================================
   TallerMecanicoTD - Script_Alter.sql
   El instalador lo ejecuta cuando la base YA existe y es la primera vez
   que corre después de reinstalar. Solo agrega lo del RFN1 si falta
   (mismo contenido que Script_TallerMecanico.sql, es idempotente).
   ===================================================================== */

/* =====================================================================
   TallerMecanicoTD - Script del RFN1 "Asignar Turno de Reparación"
   ---------------------------------------------------------------------
   - Crea las tablas Cliente_56PS, Vehiculo_56PS y Turno_56PS.
   - Da de alta las patentes P60..P63 y la familia "Recepción de turnos".
   - Crea el rol "Recepcionista" y le suma la familia a los roles que
     ya administran usuarios (el administrador), para poder probar todo.
   - Deja las tablas nuevas registradas en DigitoVerificador.

   Es idempotente: se puede ejecutar más de una vez sin romper nada.
   El instalador lo ejecuta solo (DAL_BackUpRestore_56PS) si la tabla
   Turno_56PS no existe. También se puede correr a mano en SSMS; en ese
   caso, después hay que entrar con el admin y apretar "Recalcular DV".
   ===================================================================== */

USE [SistemaBase]
GO

/* ---------- Tablas ---------- */

IF OBJECT_ID(N'dbo.Cliente_56PS', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Cliente_56PS
    (
        DNI        VARCHAR(8)     NOT NULL,
        Nombre     NVARCHAR(50)   NOT NULL,
        Apellido   NVARCHAR(50)   NOT NULL,
        Telefono   VARCHAR(30)    NOT NULL,
        Direccion  NVARCHAR(100)  NOT NULL,
        Activo     BIT            NOT NULL CONSTRAINT DF_Cliente_56PS_Activo DEFAULT (1),
        CONSTRAINT PK_Cliente_56PS PRIMARY KEY (DNI)
    );
END
GO

IF OBJECT_ID(N'dbo.Vehiculo_56PS', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Vehiculo_56PS
    (
        Patente      VARCHAR(7)    NOT NULL,
        Marca        NVARCHAR(50)  NOT NULL,
        Modelo       NVARCHAR(50)  NOT NULL,
        Cedula       VARCHAR(30)   NOT NULL,
        DNI_Cliente  VARCHAR(8)    NOT NULL,
        Activo       BIT           NOT NULL CONSTRAINT DF_Vehiculo_56PS_Activo DEFAULT (1),
        CONSTRAINT PK_Vehiculo_56PS PRIMARY KEY (Patente),
        CONSTRAINT FK_Vehiculo_56PS_Cliente FOREIGN KEY (DNI_Cliente)
            REFERENCES dbo.Cliente_56PS (DNI)
            ON UPDATE NO ACTION ON DELETE NO ACTION
    );
END
GO

IF OBJECT_ID(N'dbo.Turno_56PS', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Turno_56PS
    (
        IdTurno           INT IDENTITY(1,1) NOT NULL,
        Fecha             DATE         NOT NULL,
        Horario           TIME(0)      NOT NULL,
        Estado            VARCHAR(15)  NOT NULL CONSTRAINT DF_Turno_56PS_Estado DEFAULT ('Pendiente'),
        DNI_Cliente       VARCHAR(8)   NOT NULL,
        Patente_Vehiculo  VARCHAR(7)   NOT NULL,
        DNI_Mecanico      VARCHAR(20)  NULL,   -- se completa en el RFN2 (Asignar mecánico)
        CONSTRAINT PK_Turno_56PS PRIMARY KEY (IdTurno),
        CONSTRAINT FK_Turno_56PS_Cliente FOREIGN KEY (DNI_Cliente)
            REFERENCES dbo.Cliente_56PS (DNI),
        CONSTRAINT FK_Turno_56PS_Vehiculo FOREIGN KEY (Patente_Vehiculo)
            REFERENCES dbo.Vehiculo_56PS (Patente),
        CONSTRAINT CK_Turno_56PS_Estado CHECK (Estado IN ('Pendiente', 'Confirmado', 'Cancelado', 'Atendido'))
    );

    -- Un solo turno vigente por fecha y horario (los cancelados liberan el lugar).
    -- Es la última defensa contra dos recepcionistas reservando el mismo horario.
    CREATE UNIQUE INDEX UX_Turno_56PS_FechaHorario
        ON dbo.Turno_56PS (Fecha, Horario)
        WHERE Estado <> 'Cancelado';
END
GO

/* ---------- Permisos (patentes) ---------- */

IF NOT EXISTS (SELECT 1 FROM dbo.Patentes WHERE CodigoPatente = 'P60')
    INSERT INTO dbo.Patentes (CodigoPatente, Nombre) VALUES ('P60', 'Gestionar clientes');
IF NOT EXISTS (SELECT 1 FROM dbo.Patentes WHERE CodigoPatente = 'P61')
    INSERT INTO dbo.Patentes (CodigoPatente, Nombre) VALUES ('P61', 'Gestionar vehículos');
IF NOT EXISTS (SELECT 1 FROM dbo.Patentes WHERE CodigoPatente = 'P62')
    INSERT INTO dbo.Patentes (CodigoPatente, Nombre) VALUES ('P62', 'Asignar turno');
IF NOT EXISTS (SELECT 1 FROM dbo.Patentes WHERE CodigoPatente = 'P63')
    INSERT INTO dbo.Patentes (CodigoPatente, Nombre) VALUES ('P63', 'Consultar turnos');
GO

/* ---------- Familia "Recepción de turnos" ---------- */

IF NOT EXISTS (SELECT 1 FROM dbo.Familias WHERE CodigoFamilia = 'FRECEP')
    INSERT INTO dbo.Familias (Nombre, CodigoFamilia, Activa) VALUES ('Recepción de turnos', 'FRECEP', 1);
GO

INSERT INTO dbo.FamiliaPatente (CodigoFamilia, CodigoPatente)
SELECT 'FRECEP', p.CodigoPatente
FROM dbo.Patentes p
WHERE p.CodigoPatente IN ('P60', 'P61', 'P62', 'P63')
  AND NOT EXISTS (SELECT 1 FROM dbo.FamiliaPatente fp
                  WHERE fp.CodigoFamilia = 'FRECEP' AND fp.CodigoPatente = p.CodigoPatente);
GO

/* ---------- Rol "Recepcionista" ---------- */

IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE CodigoRol = 'RRECEP')
   AND NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE Nombre = 'Recepcionista')
    INSERT INTO dbo.Roles (Nombre, CodigoRol, Activo) VALUES ('Recepcionista', 'RRECEP', 1);
GO

IF EXISTS (SELECT 1 FROM dbo.Roles WHERE CodigoRol = 'RRECEP')
   AND NOT EXISTS (SELECT 1 FROM dbo.RolFamilia WHERE CodigoRol = 'RRECEP' AND CodigoFamilia = 'FRECEP')
    INSERT INTO dbo.RolFamilia (CodigoRol, CodigoFamilia) VALUES ('RRECEP', 'FRECEP');
GO

-- El administrador (todo rol que tenga "Consultar usuarios" = P10, directo o por
-- familias anidadas) también recibe la familia, así se puede probar el RFN1 sin
-- crear usuarios nuevos.
WITH FamiliasDelRol AS
(
    SELECT rf.CodigoRol, rf.CodigoFamilia, 0 AS Nivel
    FROM dbo.RolFamilia rf

    UNION ALL

    SELECT fr.CodigoRol, ff.CodFamiliaHija, fr.Nivel + 1
    FROM FamiliasDelRol fr
    INNER JOIN dbo.FamiliaFamilia ff ON ff.CodigoFamilia = fr.CodigoFamilia
    WHERE fr.Nivel < 20
),
RolesAdmin AS
(
    SELECT rp.CodigoRol
    FROM dbo.RolPatente rp
    WHERE rp.CodigoPatente = 'P10'

    UNION

    SELECT fr.CodigoRol
    FROM FamiliasDelRol fr
    INNER JOIN dbo.FamiliaPatente fp ON fp.CodigoFamilia = fr.CodigoFamilia
    WHERE fp.CodigoPatente = 'P10'
)
INSERT INTO dbo.RolFamilia (CodigoRol, CodigoFamilia)
SELECT DISTINCT ra.CodigoRol, 'FRECEP'
FROM RolesAdmin ra
INNER JOIN dbo.Roles r ON r.CodigoRol = ra.CodigoRol AND r.Activo = 1
WHERE ra.CodigoRol <> 'RRECEP'
  AND NOT EXISTS (SELECT 1 FROM dbo.RolFamilia x
                  WHERE x.CodigoRol = ra.CodigoRol AND x.CodigoFamilia = 'FRECEP');
GO


/* ---------- A02: Reporte de turnos (RFN1) ---------- */

IF NOT EXISTS (SELECT 1 FROM dbo.Patentes WHERE CodigoPatente = 'P64')
    INSERT INTO dbo.Patentes (CodigoPatente, Nombre) VALUES ('P64', 'Reporte de turnos (RFN1)');
IF NOT EXISTS (SELECT 1 FROM dbo.Familias WHERE CodigoFamilia = 'FREPORT')
    INSERT INTO dbo.Familias (Nombre, CodigoFamilia, Activa) VALUES ('Reportes', 'FREPORT', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.FamiliaPatente WHERE CodigoFamilia = 'FREPORT' AND CodigoPatente = 'P64')
    INSERT INTO dbo.FamiliaPatente (CodigoFamilia, CodigoPatente) VALUES ('FREPORT', 'P64');
GO

-- El reporte lo ven los roles que ya tienen la familia de recepción por ser
-- administradores (ver bloque anterior) y el jefe de taller si existe.
INSERT INTO dbo.RolFamilia (CodigoRol, CodigoFamilia)
SELECT r.CodigoRol, 'FREPORT'
FROM dbo.Roles r
WHERE r.Activo = 1
  AND (r.CodigoRol = 'RJEFE'
       OR (r.CodigoRol <> 'RRECEP' AND EXISTS (SELECT 1 FROM dbo.RolFamilia x
                                               WHERE x.CodigoRol = r.CodigoRol AND x.CodigoFamilia = 'FRECEP')))
  AND NOT EXISTS (SELECT 1 FROM dbo.RolFamilia y WHERE y.CodigoRol = r.CodigoRol AND y.CodigoFamilia = 'FREPORT');
GO

/* ---------- Dígitos verificadores ---------- */
-- Se registran las tablas nuevas con DV vacío; el instalador (o el botón
-- "Recalcular DV") los calcula enseguida.

IF NOT EXISTS (SELECT 1 FROM dbo.DigitoVerificador WHERE nombreTabla = 'Cliente_56PS')
    INSERT INTO dbo.DigitoVerificador (nombreTabla, DVH, DVV) VALUES ('Cliente_56PS', '', '');
IF NOT EXISTS (SELECT 1 FROM dbo.DigitoVerificador WHERE nombreTabla = 'Vehiculo_56PS')
    INSERT INTO dbo.DigitoVerificador (nombreTabla, DVH, DVV) VALUES ('Vehiculo_56PS', '', '');
IF NOT EXISTS (SELECT 1 FROM dbo.DigitoVerificador WHERE nombreTabla = 'Turno_56PS')
    INSERT INTO dbo.DigitoVerificador (nombreTabla, DVH, DVV) VALUES ('Turno_56PS', '', '');
GO
