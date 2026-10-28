/* =====================================================================
   TallerMecanicoTD - Script del RFN2 "Registrar Diagnostico del Vehiculo"
   ---------------------------------------------------------------------
   - Crea las tablas Asistencia_56PS, Revision_56PS, Diagnostico_56PS,
     RepuestoFaltante_56PS, Reparacion_56PS y Cobro_56PS.
   - Da de alta las patentes P65..P70 y la familia "Diagnostico y reparacion".
   - Asigna permisos: recepcion (asistencia y cobro), jefe de taller
     (revision y reporte), mecanico (diagnostico y reparacion) y administrador.
   - Deja las tablas nuevas registradas en DigitoVerificador.

   Es idempotente: se puede ejecutar mas de una vez sin romper nada.
   El instalador lo ejecuta solo (DAL_BackUpRestore_96_PP) si la tabla
   Asistencia_56PS no existe. Tambien se puede correr a mano en SSMS; en ese
   caso, despues hay que entrar con el admin y apretar "Recalcular DV".

   Pasos del caso de uso (documento, RFN2):
     2  Asistencia_56PS        3-5  Revision_56PS (+ Turno.DNI_Mecanico)
     6-7 Diagnostico_56PS      8    RepuestoFaltante_56PS
     9-10 Reparacion_56PS      12   Cobro_56PS
   ===================================================================== */

USE [SistemaBase]
GO

/* ---------- Tablas ---------- */

IF OBJECT_ID(N'dbo.Asistencia_56PS', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Asistencia_56PS
    (
        IdAsistencia       INT IDENTITY(1,1) NOT NULL,
        IdTurno            INT          NOT NULL,
        Fecha              DATE         NOT NULL,
        Hora               TIME(0)      NOT NULL,
        Estado             VARCHAR(15)  NOT NULL CONSTRAINT DF_Asistencia_56PS_Estado DEFAULT ('Presente'),
        DNI_Recepcionista  VARCHAR(8)   NOT NULL,
        CONSTRAINT PK_Asistencia_56PS PRIMARY KEY (IdAsistencia),
        CONSTRAINT UQ_Asistencia_56PS_Turno UNIQUE (IdTurno),
        CONSTRAINT FK_Asistencia_56PS_Turno FOREIGN KEY (IdTurno)
            REFERENCES dbo.Turno_56PS (IdTurno),
        CONSTRAINT FK_Asistencia_56PS_Usuario FOREIGN KEY (DNI_Recepcionista)
            REFERENCES dbo.Usuario_56PS (DNI),
        CONSTRAINT CK_Asistencia_56PS_Estado CHECK (Estado IN ('Presente'))
    );
END
GO

IF OBJECT_ID(N'dbo.Revision_56PS', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Revision_56PS
    (
        IdRevision     INT IDENTITY(1,1) NOT NULL,
        IdTurno        INT            NOT NULL,
        Observaciones  NVARCHAR(500)  NOT NULL,
        Fecha          DATE           NOT NULL,
        Hora           TIME(0)        NOT NULL,
        DNI_Jefe       VARCHAR(8)     NOT NULL,
        DNI_Mecanico   VARCHAR(8)     NOT NULL,   -- orden de inspeccion (paso 5)
        CONSTRAINT PK_Revision_56PS PRIMARY KEY (IdRevision),
        CONSTRAINT UQ_Revision_56PS_Turno UNIQUE (IdTurno),
        CONSTRAINT FK_Revision_56PS_Turno FOREIGN KEY (IdTurno)
            REFERENCES dbo.Turno_56PS (IdTurno),
        CONSTRAINT FK_Revision_56PS_Jefe FOREIGN KEY (DNI_Jefe)
            REFERENCES dbo.Usuario_56PS (DNI),
        CONSTRAINT FK_Revision_56PS_Mecanico FOREIGN KEY (DNI_Mecanico)
            REFERENCES dbo.Usuario_56PS (DNI)
    );
END
GO

IF OBJECT_ID(N'dbo.Diagnostico_56PS', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Diagnostico_56PS
    (
        IdDiagnostico        INT IDENTITY(1,1) NOT NULL,
        IdTurno              INT            NOT NULL,
        SistemaAfectado      NVARCHAR(100)  NOT NULL,
        DescripcionFallas    NVARCHAR(500)  NOT NULL,
        Urgencia             VARCHAR(10)    NOT NULL,
        RepuestosNecesarios  NVARCHAR(300)  NULL,
        Fecha                DATE           NOT NULL,
        Hora                 TIME(0)        NOT NULL,
        DNI_Mecanico         VARCHAR(8)     NOT NULL,
        TipoSolucion         VARCHAR(20)    NULL,    -- se completa en el paso 7
        DescripcionSolucion  NVARCHAR(500)  NULL,
        CONSTRAINT PK_Diagnostico_56PS PRIMARY KEY (IdDiagnostico),
        CONSTRAINT UQ_Diagnostico_56PS_Turno UNIQUE (IdTurno),
        CONSTRAINT FK_Diagnostico_56PS_Turno FOREIGN KEY (IdTurno)
            REFERENCES dbo.Turno_56PS (IdTurno),
        CONSTRAINT FK_Diagnostico_56PS_Mecanico FOREIGN KEY (DNI_Mecanico)
            REFERENCES dbo.Usuario_56PS (DNI),
        CONSTRAINT CK_Diagnostico_56PS_Urgencia CHECK (Urgencia IN ('Baja', 'Media', 'Alta', 'Critica')),
        CONSTRAINT CK_Diagnostico_56PS_Solucion CHECK (TipoSolucion IS NULL
            OR TipoSolucion IN ('Reparacion', 'Reemplazo', 'Ajuste', 'Mantenimiento'))
    );
END
GO

IF OBJECT_ID(N'dbo.RepuestoFaltante_56PS', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.RepuestoFaltante_56PS
    (
        IdFaltante     INT IDENTITY(1,1) NOT NULL,
        IdDiagnostico  INT            NOT NULL,
        Repuesto       NVARCHAR(100)  NOT NULL,
        Cantidad       INT            NOT NULL,
        Estado         VARCHAR(20)    NOT NULL CONSTRAINT DF_RepuestoFaltante_56PS_Estado DEFAULT ('PendienteCompra'),
        CONSTRAINT PK_RepuestoFaltante_56PS PRIMARY KEY (IdFaltante),
        CONSTRAINT FK_RepuestoFaltante_56PS_Diagnostico FOREIGN KEY (IdDiagnostico)
            REFERENCES dbo.Diagnostico_56PS (IdDiagnostico),
        CONSTRAINT CK_RepuestoFaltante_56PS_Cantidad CHECK (Cantidad > 0),
        CONSTRAINT CK_RepuestoFaltante_56PS_Estado CHECK (Estado IN ('PendienteCompra', 'Comprado'))
    );
END
GO

IF OBJECT_ID(N'dbo.Reparacion_56PS', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Reparacion_56PS
    (
        IdReparacion        INT IDENTITY(1,1) NOT NULL,
        IdDiagnostico       INT            NOT NULL,
        TareasRealizadas    NVARCHAR(500)  NOT NULL,
        RepuestosUtilizados NVARCHAR(300)  NULL,
        Fecha               DATE           NOT NULL,
        Hora                TIME(0)        NOT NULL,
        DNI_Mecanico        VARCHAR(8)     NOT NULL,
        PresupuestoFinal    DECIMAL(12,2)  NULL,    -- se completa en el paso 10
        FechaFinalizacion   DATETIME       NULL,
        CONSTRAINT PK_Reparacion_56PS PRIMARY KEY (IdReparacion),
        CONSTRAINT UQ_Reparacion_56PS_Diagnostico UNIQUE (IdDiagnostico),
        CONSTRAINT FK_Reparacion_56PS_Diagnostico FOREIGN KEY (IdDiagnostico)
            REFERENCES dbo.Diagnostico_56PS (IdDiagnostico),
        CONSTRAINT FK_Reparacion_56PS_Mecanico FOREIGN KEY (DNI_Mecanico)
            REFERENCES dbo.Usuario_56PS (DNI),
        CONSTRAINT CK_Reparacion_56PS_Presupuesto CHECK (PresupuestoFinal IS NULL OR PresupuestoFinal >= 0)
    );
END
GO

IF OBJECT_ID(N'dbo.Cobro_56PS', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Cobro_56PS
    (
        IdCobro            INT IDENTITY(1,1) NOT NULL,
        IdTurno            INT            NOT NULL,
        MontoTotal         DECIMAL(12,2)  NOT NULL,
        MetodoPago         VARCHAR(20)    NOT NULL,
        Fecha              DATE           NOT NULL,
        Hora               TIME(0)        NOT NULL,
        DNI_Cliente        VARCHAR(8)     NOT NULL,
        DNI_Recepcionista  VARCHAR(8)     NOT NULL,
        CONSTRAINT PK_Cobro_56PS PRIMARY KEY (IdCobro),
        CONSTRAINT UQ_Cobro_56PS_Turno UNIQUE (IdTurno),
        CONSTRAINT FK_Cobro_56PS_Turno FOREIGN KEY (IdTurno)
            REFERENCES dbo.Turno_56PS (IdTurno),
        CONSTRAINT FK_Cobro_56PS_Cliente FOREIGN KEY (DNI_Cliente)
            REFERENCES dbo.Cliente_56PS (DNI),
        CONSTRAINT FK_Cobro_56PS_Usuario FOREIGN KEY (DNI_Recepcionista)
            REFERENCES dbo.Usuario_56PS (DNI),
        CONSTRAINT CK_Cobro_56PS_Monto CHECK (MontoTotal > 0),
        CONSTRAINT CK_Cobro_56PS_Metodo CHECK (MetodoPago IN ('Efectivo', 'Debito', 'Credito', 'MercadoPago', 'Transferencia'))
    );
END
GO

/* ---------- Permisos (patentes) ---------- */

IF NOT EXISTS (SELECT 1 FROM dbo.Patentes WHERE CodigoPatente = 'P65')
    INSERT INTO dbo.Patentes (CodigoPatente, Nombre) VALUES ('P65', N'Registrar asistencia');
IF NOT EXISTS (SELECT 1 FROM dbo.Patentes WHERE CodigoPatente = 'P66')
    INSERT INTO dbo.Patentes (CodigoPatente, Nombre) VALUES ('P66', N'Registrar revisión y asignar mecánico');
IF NOT EXISTS (SELECT 1 FROM dbo.Patentes WHERE CodigoPatente = 'P67')
    INSERT INTO dbo.Patentes (CodigoPatente, Nombre) VALUES ('P67', N'Registrar diagnóstico');
IF NOT EXISTS (SELECT 1 FROM dbo.Patentes WHERE CodigoPatente = 'P68')
    INSERT INTO dbo.Patentes (CodigoPatente, Nombre) VALUES ('P68', N'Registrar reparación y finalización');
IF NOT EXISTS (SELECT 1 FROM dbo.Patentes WHERE CodigoPatente = 'P69')
    INSERT INTO dbo.Patentes (CodigoPatente, Nombre) VALUES ('P69', N'Registrar cobro');
IF NOT EXISTS (SELECT 1 FROM dbo.Patentes WHERE CodigoPatente = 'P70')
    INSERT INTO dbo.Patentes (CodigoPatente, Nombre) VALUES ('P70', N'Reporte de diagnósticos (RFN2)');
GO

/* ---------- Familias ---------- */

-- Recepcion de turnos: suma la asistencia y el cobro.
INSERT INTO dbo.FamiliaPatente (CodigoFamilia, CodigoPatente)
SELECT 'FRECEP', p.CodigoPatente
FROM dbo.Patentes p
WHERE p.CodigoPatente IN ('P65', 'P69')
  AND EXISTS (SELECT 1 FROM dbo.Familias f WHERE f.CodigoFamilia = 'FRECEP')
  AND NOT EXISTS (SELECT 1 FROM dbo.FamiliaPatente fp
                  WHERE fp.CodigoFamilia = 'FRECEP' AND fp.CodigoPatente = p.CodigoPatente);
GO

-- Reportes: suma el reporte de diagnosticos.
INSERT INTO dbo.FamiliaPatente (CodigoFamilia, CodigoPatente)
SELECT 'FREPORT', p.CodigoPatente
FROM dbo.Patentes p
WHERE p.CodigoPatente = 'P70'
  AND EXISTS (SELECT 1 FROM dbo.Familias f WHERE f.CodigoFamilia = 'FREPORT')
  AND NOT EXISTS (SELECT 1 FROM dbo.FamiliaPatente fp
                  WHERE fp.CodigoFamilia = 'FREPORT' AND fp.CodigoPatente = p.CodigoPatente);
GO

-- Nueva familia "Diagnostico y reparacion" (revision, diagnostico y reparacion).
IF NOT EXISTS (SELECT 1 FROM dbo.Familias WHERE CodigoFamilia = 'FDIAG')
    INSERT INTO dbo.Familias (Nombre, CodigoFamilia, Activa) VALUES (N'Diagnóstico y reparación', 'FDIAG', 1);
GO

INSERT INTO dbo.FamiliaPatente (CodigoFamilia, CodigoPatente)
SELECT 'FDIAG', p.CodigoPatente
FROM dbo.Patentes p
WHERE p.CodigoPatente IN ('P66', 'P67', 'P68')
  AND NOT EXISTS (SELECT 1 FROM dbo.FamiliaPatente fp
                  WHERE fp.CodigoFamilia = 'FDIAG' AND fp.CodigoPatente = p.CodigoPatente);
GO

/* ---------- Roles ---------- */

-- Jefe de taller: revision y reporte. Mecanico: diagnostico y reparacion.
INSERT INTO dbo.RolPatente (CodigoRol, CodigoPatente)
SELECT x.CodigoRol, x.CodigoPatente
FROM (VALUES ('RJEFE', 'P66'), ('RJEFE', 'P70'), ('RMEC', 'P67'), ('RMEC', 'P68')) AS x (CodigoRol, CodigoPatente)
WHERE EXISTS (SELECT 1 FROM dbo.Roles r WHERE r.CodigoRol = x.CodigoRol)
  AND NOT EXISTS (SELECT 1 FROM dbo.RolPatente rp
                  WHERE rp.CodigoRol = x.CodigoRol AND rp.CodigoPatente = x.CodigoPatente);
GO

-- El administrador (todo rol que ya tiene la familia FRECEP salvo recepcion)
-- recibe FDIAG para poder probar el RFN2 completo sin crear usuarios nuevos.
INSERT INTO dbo.RolFamilia (CodigoRol, CodigoFamilia)
SELECT rf.CodigoRol, 'FDIAG'
FROM dbo.RolFamilia rf
INNER JOIN dbo.Roles r ON r.CodigoRol = rf.CodigoRol AND r.Activo = 1
WHERE rf.CodigoFamilia = 'FRECEP'
  AND rf.CodigoRol <> 'RRECEP'
  AND NOT EXISTS (SELECT 1 FROM dbo.RolFamilia x
                  WHERE x.CodigoRol = rf.CodigoRol AND x.CodigoFamilia = 'FDIAG');
GO

/* ---------- Digitos verificadores ---------- */
-- Se registran las tablas nuevas con DV vacio; el instalador (o el boton
-- "Recalcular DV") los calcula enseguida.

IF NOT EXISTS (SELECT 1 FROM dbo.DigitoVerificador WHERE nombreTabla = 'Asistencia_56PS')
    INSERT INTO dbo.DigitoVerificador (nombreTabla, DVH, DVV) VALUES ('Asistencia_56PS', '', '');
IF NOT EXISTS (SELECT 1 FROM dbo.DigitoVerificador WHERE nombreTabla = 'Revision_56PS')
    INSERT INTO dbo.DigitoVerificador (nombreTabla, DVH, DVV) VALUES ('Revision_56PS', '', '');
IF NOT EXISTS (SELECT 1 FROM dbo.DigitoVerificador WHERE nombreTabla = 'Diagnostico_56PS')
    INSERT INTO dbo.DigitoVerificador (nombreTabla, DVH, DVV) VALUES ('Diagnostico_56PS', '', '');
IF NOT EXISTS (SELECT 1 FROM dbo.DigitoVerificador WHERE nombreTabla = 'RepuestoFaltante_56PS')
    INSERT INTO dbo.DigitoVerificador (nombreTabla, DVH, DVV) VALUES ('RepuestoFaltante_56PS', '', '');
IF NOT EXISTS (SELECT 1 FROM dbo.DigitoVerificador WHERE nombreTabla = 'Reparacion_56PS')
    INSERT INTO dbo.DigitoVerificador (nombreTabla, DVH, DVV) VALUES ('Reparacion_56PS', '', '');
IF NOT EXISTS (SELECT 1 FROM dbo.DigitoVerificador WHERE nombreTabla = 'Cobro_56PS')
    INSERT INTO dbo.DigitoVerificador (nombreTabla, DVH, DVV) VALUES ('Cobro_56PS', '', '');
GO
