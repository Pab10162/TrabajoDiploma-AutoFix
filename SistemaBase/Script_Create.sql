/* =====================================================================
   TallerMecanicoTD - Script_Create.sql
   ---------------------------------------------------------------------
   Crea la base SistemaBase completa (tablas del SistemaBase + tablas del
   RFN1) y carga datos de prueba: roles, familias, permisos, usuarios,
   clientes, vehículos y turnos.

   Uso:
     - Automático: el instalador lo ejecuta si la base no existe
       (MenuPrincipal_56PS_Load con instancia.txt).
     - A mano: ejecutarlo completo en SSMS (F5). Después, entrar con
       "admin" y apretar "Recalcular DV" en la pantalla de reparación.

   Es idempotente: si la base o las tablas ya existen no las vuelve a
   crear, y los datos de prueba se cargan solo si la tabla Roles está vacía.

   Contraseñas: se guardan como SHA-256 en hexadecimal (igual que
   CryptoManager_56PS.Encriptar).
   ===================================================================== */

IF DB_ID(N'SistemaBase') IS NULL CREATE DATABASE [SistemaBase]
GO

USE [SistemaBase]
GO

/* =====================================================================
   1. TABLAS DEL SISTEMA BASE
   ===================================================================== */

IF OBJECT_ID(N'dbo.Idioma_56PS', N'U') IS NULL
CREATE TABLE dbo.Idioma_56PS
(
    Tipo    VARCHAR(3)    NOT NULL,
    Nombre  NVARCHAR(30)  NOT NULL,
    CONSTRAINT PK_Idioma_56PS PRIMARY KEY (Tipo)
);
GO

IF OBJECT_ID(N'dbo.Patentes', N'U') IS NULL
CREATE TABLE dbo.Patentes
(
    CodigoPatente  VARCHAR(10)   NOT NULL,
    Nombre         NVARCHAR(100) NOT NULL,
    CONSTRAINT PK_Patentes PRIMARY KEY (CodigoPatente)
);
GO

IF OBJECT_ID(N'dbo.Familias', N'U') IS NULL
CREATE TABLE dbo.Familias
(
    CodigoFamilia  VARCHAR(10)   NOT NULL,
    Nombre         NVARCHAR(100) NOT NULL,
    Activa         BIT           NOT NULL CONSTRAINT DF_Familias_Activa DEFAULT (1),
    CONSTRAINT PK_Familias PRIMARY KEY (CodigoFamilia)
);
GO

IF OBJECT_ID(N'dbo.Roles', N'U') IS NULL
CREATE TABLE dbo.Roles
(
    CodigoRol  VARCHAR(10)   NOT NULL,
    Nombre     NVARCHAR(100) NOT NULL,
    Activo     BIT           NOT NULL CONSTRAINT DF_Roles_Activo DEFAULT (1),
    CONSTRAINT PK_Roles PRIMARY KEY (CodigoRol)
);
GO

IF OBJECT_ID(N'dbo.FamiliaPatente', N'U') IS NULL
CREATE TABLE dbo.FamiliaPatente
(
    CodigoFamilia  VARCHAR(10) NOT NULL,
    CodigoPatente  VARCHAR(10) NOT NULL,
    CONSTRAINT PK_FamiliaPatente PRIMARY KEY (CodigoFamilia, CodigoPatente),
    CONSTRAINT FK_FamiliaPatente_Familia FOREIGN KEY (CodigoFamilia) REFERENCES dbo.Familias (CodigoFamilia),
    CONSTRAINT FK_FamiliaPatente_Patente FOREIGN KEY (CodigoPatente) REFERENCES dbo.Patentes (CodigoPatente)
);
GO

IF OBJECT_ID(N'dbo.FamiliaFamilia', N'U') IS NULL
CREATE TABLE dbo.FamiliaFamilia
(
    CodigoFamilia   VARCHAR(10) NOT NULL,
    CodFamiliaHija  VARCHAR(10) NOT NULL,
    CONSTRAINT PK_FamiliaFamilia PRIMARY KEY (CodigoFamilia, CodFamiliaHija),
    CONSTRAINT FK_FamiliaFamilia_Padre FOREIGN KEY (CodigoFamilia) REFERENCES dbo.Familias (CodigoFamilia),
    CONSTRAINT FK_FamiliaFamilia_Hija  FOREIGN KEY (CodFamiliaHija) REFERENCES dbo.Familias (CodigoFamilia),
    CONSTRAINT CK_FamiliaFamilia_NoSiMisma CHECK (CodigoFamilia <> CodFamiliaHija)
);
GO

IF OBJECT_ID(N'dbo.RolFamilia', N'U') IS NULL
CREATE TABLE dbo.RolFamilia
(
    CodigoRol      VARCHAR(10) NOT NULL,
    CodigoFamilia  VARCHAR(10) NOT NULL,
    CONSTRAINT PK_RolFamilia PRIMARY KEY (CodigoRol, CodigoFamilia),
    CONSTRAINT FK_RolFamilia_Rol     FOREIGN KEY (CodigoRol)     REFERENCES dbo.Roles (CodigoRol),
    CONSTRAINT FK_RolFamilia_Familia FOREIGN KEY (CodigoFamilia) REFERENCES dbo.Familias (CodigoFamilia)
);
GO

IF OBJECT_ID(N'dbo.RolPatente', N'U') IS NULL
CREATE TABLE dbo.RolPatente
(
    CodigoRol      VARCHAR(10) NOT NULL,
    CodigoPatente  VARCHAR(10) NOT NULL,
    CONSTRAINT PK_RolPatente PRIMARY KEY (CodigoRol, CodigoPatente),
    CONSTRAINT FK_RolPatente_Rol     FOREIGN KEY (CodigoRol)     REFERENCES dbo.Roles (CodigoRol),
    CONSTRAINT FK_RolPatente_Patente FOREIGN KEY (CodigoPatente) REFERENCES dbo.Patentes (CodigoPatente)
);
GO

IF OBJECT_ID(N'dbo.Usuario_56PS', N'U') IS NULL
CREATE TABLE dbo.Usuario_56PS
(
    DNI            VARCHAR(8)     NOT NULL,
    Nombre         NVARCHAR(50)   NOT NULL,
    Apellido       NVARCHAR(50)   NOT NULL,
    Email          NVARCHAR(100)  NULL,
    Bloqueado      BIT            NOT NULL CONSTRAINT DF_Usuario_56PS_Bloqueado DEFAULT (0),
    NombreUsuario  NVARCHAR(50)   NOT NULL,
    Contraseña     VARCHAR(64)    NOT NULL,
    Idioma         VARCHAR(3)     NOT NULL CONSTRAINT DF_Usuario_56PS_Idioma DEFAULT ('ES'),
    Rol            VARCHAR(10)    NULL,
    Activo         BIT            NOT NULL CONSTRAINT DF_Usuario_56PS_Activo DEFAULT (1),
    CONSTRAINT PK_Usuario_56PS PRIMARY KEY (DNI),
    CONSTRAINT UQ_Usuario_56PS_NombreUsuario UNIQUE (NombreUsuario),
    CONSTRAINT FK_Usuario_56PS_Idioma FOREIGN KEY (Idioma) REFERENCES dbo.Idioma_56PS (Tipo),
    CONSTRAINT FK_Usuario_56PS_Rol    FOREIGN KEY (Rol)    REFERENCES dbo.Roles (CodigoRol)
);
GO

IF OBJECT_ID(N'dbo.Evento_56PS', N'U') IS NULL
CREATE TABLE dbo.Evento_56PS
(
    Numero       INT IDENTITY(1,1) NOT NULL,
    DNI          VARCHAR(20)    NOT NULL,
    Fecha        DATETIME       NOT NULL,
    Modulo       NVARCHAR(50)   NOT NULL,
    Descripcion  NVARCHAR(250)  NOT NULL,
    Criticidad   VARCHAR(10)    NOT NULL,
    CONSTRAINT PK_Evento_56PS PRIMARY KEY (Numero),
    CONSTRAINT CK_Evento_56PS_Criticidad CHECK (Criticidad IN ('Bajo', 'Medio', 'Alto', 'MuyAlto'))
);
GO

IF OBJECT_ID(N'dbo.DigitoVerificador', N'U') IS NULL
CREATE TABLE dbo.DigitoVerificador
(
    nombreTabla  VARCHAR(50)  NOT NULL,
    DVH          VARCHAR(64)  NULL,
    DVV          VARCHAR(64)  NULL,
    CONSTRAINT PK_DigitoVerificador PRIMARY KEY (nombreTabla)
);
GO

/* =====================================================================
   2. TABLAS DEL RFN1 (Taller mecánico)
   ===================================================================== */

IF OBJECT_ID(N'dbo.Cliente_56PS', N'U') IS NULL
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
GO

IF OBJECT_ID(N'dbo.Vehiculo_56PS', N'U') IS NULL
CREATE TABLE dbo.Vehiculo_56PS
(
    Patente      VARCHAR(7)    NOT NULL,
    Marca        NVARCHAR(50)  NOT NULL,
    Modelo       NVARCHAR(50)  NOT NULL,
    Cedula       VARCHAR(30)   NOT NULL,
    DNI_Cliente  VARCHAR(8)    NOT NULL,
    Activo       BIT           NOT NULL CONSTRAINT DF_Vehiculo_56PS_Activo DEFAULT (1),
    CONSTRAINT PK_Vehiculo_56PS PRIMARY KEY (Patente),
    CONSTRAINT FK_Vehiculo_56PS_Cliente FOREIGN KEY (DNI_Cliente) REFERENCES dbo.Cliente_56PS (DNI)
);
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
        DNI_Mecanico      VARCHAR(8)   NULL,   -- se completa en el RFN2 (Asignar mecánico)
        CONSTRAINT PK_Turno_56PS PRIMARY KEY (IdTurno),
        CONSTRAINT FK_Turno_56PS_Cliente  FOREIGN KEY (DNI_Cliente)      REFERENCES dbo.Cliente_56PS (DNI),
        CONSTRAINT FK_Turno_56PS_Vehiculo FOREIGN KEY (Patente_Vehiculo) REFERENCES dbo.Vehiculo_56PS (Patente),
        CONSTRAINT FK_Turno_56PS_Mecanico FOREIGN KEY (DNI_Mecanico)     REFERENCES dbo.Usuario_56PS (DNI),
        CONSTRAINT CK_Turno_56PS_Estado CHECK (Estado IN ('Pendiente', 'Confirmado', 'Cancelado', 'Atendido'))
    );

    -- Un solo turno vigente por fecha y horario (los cancelados liberan el lugar).
    CREATE UNIQUE INDEX UX_Turno_56PS_FechaHorario
        ON dbo.Turno_56PS (Fecha, Horario)
        WHERE Estado <> 'Cancelado';
END
GO

/* =====================================================================
   3. DATOS FIJOS: idiomas y permisos (patentes)
   ===================================================================== */

IF NOT EXISTS (SELECT 1 FROM dbo.Idioma_56PS)
INSERT INTO dbo.Idioma_56PS (Tipo, Nombre) VALUES
    ('ES',  N'Espanol'),
    ('EN',  N'English'),
    ('POR', N'Portugues');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Patentes)
INSERT INTO dbo.Patentes (CodigoPatente, Nombre) VALUES
    ('P02', N'Consultar bitácora'),
    ('P05', N'Cambiar contraseña'),
    ('P06', N'Exportar PDF de bitácora'),
    ('P09', N'Cambiar idioma'),
    ('P10', N'Consultar usuarios'),
    ('P11', N'Crear usuario'),
    ('P12', N'Modificar usuario'),
    ('P13', N'Activar/desactivar usuario'),
    ('P14', N'Desbloquear usuario'),
    ('P20', N'Consultar roles'),
    ('P21', N'Crear rol'),
    ('P22', N'Eliminar rol'),
    ('P23', N'Asignar patente a rol'),
    ('P24', N'Asignar familia a rol'),
    ('P25', N'Quitar permiso de rol'),
    ('P30', N'Consultar familias'),
    ('P31', N'Crear familia'),
    ('P32', N'Modificar familia'),
    ('P33', N'Eliminar familia'),
    ('P34', N'Asignar patente a familia'),
    ('P35', N'Asignar familia a familia'),
    ('P36', N'Quitar permiso de familia'),
    ('P50', N'Realizar backup'),
    ('P51', N'Restaurar backup'),
    ('P52', N'Recalcular dígitos verificadores'),
    ('P60', N'Gestionar clientes'),
    ('P61', N'Gestionar vehículos'),
    ('P62', N'Asignar turno'),
    ('P63', N'Consultar turnos'),
    ('P64', N'Reporte de turnos (RFN1)');
GO

/* =====================================================================
   4. DATOS DE PRUEBA (solo si la base está vacía)
   ===================================================================== */

IF NOT EXISTS (SELECT 1 FROM dbo.Roles)
BEGIN
    /* ---------- Familias (composite) ---------- */
    INSERT INTO dbo.Familias (CodigoFamilia, Nombre, Activa) VALUES
        ('FPERFIL', N'Perfil de usuario',          1),
        ('FUSU',    N'Gestión de usuarios',        1),
        ('FSEG',    N'Seguridad y permisos',       1),
        ('FAUD',    N'Auditoría',                  1),
        ('FRESP',   N'Respaldos e integridad',     1),
        ('FADMIN',  N'Administración del sistema', 1),
        ('FRECEP',  N'Recepción de turnos',        1),
        ('FREPORT', N'Reportes',                   1);

    INSERT INTO dbo.FamiliaPatente (CodigoFamilia, CodigoPatente) VALUES
        ('FPERFIL', 'P05'), ('FPERFIL', 'P09'),
        ('FUSU', 'P10'), ('FUSU', 'P11'), ('FUSU', 'P12'), ('FUSU', 'P13'), ('FUSU', 'P14'),
        ('FSEG', 'P20'), ('FSEG', 'P21'), ('FSEG', 'P22'), ('FSEG', 'P23'), ('FSEG', 'P24'), ('FSEG', 'P25'),
        ('FSEG', 'P30'), ('FSEG', 'P31'), ('FSEG', 'P32'), ('FSEG', 'P33'), ('FSEG', 'P34'), ('FSEG', 'P35'), ('FSEG', 'P36'),
        ('FAUD', 'P02'), ('FAUD', 'P06'),
        ('FRESP', 'P50'), ('FRESP', 'P51'), ('FRESP', 'P52'),
        ('FRECEP', 'P60'), ('FRECEP', 'P61'), ('FRECEP', 'P62'), ('FRECEP', 'P63'),
        ('FREPORT', 'P64');

    -- Familia que contiene familias (para mostrar el patrón Composite).
    INSERT INTO dbo.FamiliaFamilia (CodigoFamilia, CodFamiliaHija) VALUES
        ('FADMIN', 'FUSU'),
        ('FADMIN', 'FSEG'),
        ('FADMIN', 'FAUD'),
        ('FADMIN', 'FRESP');

    /* ---------- Roles ---------- */
    INSERT INTO dbo.Roles (CodigoRol, Nombre, Activo) VALUES
        ('RADMIN', N'Administrador',  1),
        ('RRECEP', N'Recepcionista',  1),
        ('RJEFE',  N'Jefe de taller', 1),
        ('RMEC',   N'Mecánico',       1),
        ('RCAJ',   N'Cajero',         1);

    INSERT INTO dbo.RolFamilia (CodigoRol, CodigoFamilia) VALUES
        ('RADMIN', 'FADMIN'), ('RADMIN', 'FPERFIL'), ('RADMIN', 'FRECEP'), ('RADMIN', 'FREPORT'),
        ('RRECEP', 'FRECEP'), ('RRECEP', 'FPERFIL'),
        ('RJEFE',  'FPERFIL'), ('RJEFE', 'FREPORT'),   -- el jefe de taller ve el reporte
        ('RMEC',   'FPERFIL'),
        ('RCAJ',   'FPERFIL');

    -- Patentes sueltas asignadas directo al rol (sin familia).
    INSERT INTO dbo.RolPatente (CodigoRol, CodigoPatente) VALUES
        ('RJEFE', 'P63'),   -- el jefe de taller ve la agenda de turnos
        ('RJEFE', 'P02'),   -- y consulta la bitácora
        ('RMEC',  'P63');   -- el mecánico ve la agenda de turnos

    /* ---------- Usuarios ----------
       Usuario       Contraseña   Rol              Estado
       admin         Admin123     Administrador    activo
       admin2        Admin123     Administrador    activo (idioma inglés)
       recepcion     Recep123     Recepcionista    activo
       recepcion2    Recep123     Recepcionista    activo, primer ingreso: pide cambiar contraseña
       jefe          Jefe123      Jefe de taller   activo (agenda, bitácora y reporte)
       jefe2         Jefe123      Jefe de taller   activo (idioma portugués)
       mecanico      Meca123      Mecánico         activo
       mecanico2     Meca123      Mecánico         BLOQUEADO
       cajero        Caja123      Cajero           activo
       cajero2       Caja123      Cajero           INACTIVO
    */
    INSERT INTO dbo.Usuario_56PS (DNI, Nombre, Apellido, Email, Bloqueado, NombreUsuario, Contraseña, Idioma, Rol, Activo) VALUES
        ('30111111', N'Laura',    N'Gómez',     'admin@autofix.com',      0, 'admin',      '3b612c75a7b5048a435fb6ec81e52ff92d6d795a8b5a9c17070f6a63c97a53b2', 'ES',  'RADMIN', 1),
        ('30111112', N'Martín',   N'Suárez',    'admin2@autofix.com',     0, 'admin2',     '3b612c75a7b5048a435fb6ec81e52ff92d6d795a8b5a9c17070f6a63c97a53b2', 'EN',  'RADMIN', 1),
        ('31222221', N'Sofía',    N'Fernández', 'recepcion@autofix.com',  0, 'recepcion',  '0c01954c0f4f6bbda12d86eaecbd6a524225a0cad0fac52e6aaf4c237f7f9cbe', 'ES',  'RRECEP', 1),
        ('31222222', N'Julieta',  N'Ramos',     'recepcion2@autofix.com', 0, 'recepcion2', '0c01954c0f4f6bbda12d86eaecbd6a524225a0cad0fac52e6aaf4c237f7f9cbe', 'ES',  'RRECEP', 1),
        ('32333331', N'Carlos',   N'Méndez',    'jefe@autofix.com',       0, 'jefe',       'bba4c30ffce9d651c08c5adad6be3f582f8a3072eb92986e9be36f66fb4dbf10', 'ES',  'RJEFE',  1),
        ('32333332', N'Rodrigo',  N'Paz',       'jefe2@autofix.com',      0, 'jefe2',      'bba4c30ffce9d651c08c5adad6be3f582f8a3072eb92986e9be36f66fb4dbf10', 'POR', 'RJEFE',  1),
        ('33444441', N'Diego',    N'Torres',    'mecanico@autofix.com',   0, 'mecanico',   'db8d7aebcaa40be4b35083ec15c540d5bc3f6346398a5e724b65be1a9bad4409', 'ES',  'RMEC',   1),
        ('33444442', N'Lucas',    N'Herrera',   'mecanico2@autofix.com',  1, 'mecanico2',  'db8d7aebcaa40be4b35083ec15c540d5bc3f6346398a5e724b65be1a9bad4409', 'ES',  'RMEC',   1),
        ('34555551', N'Valeria',  N'Castro',    'cajero@autofix.com',     0, 'cajero',     '3484c59cc17b5bcdf776ab8ac6eacb4cb9fcf548a322f7ab8dca89263d4e94ca', 'ES',  'RCAJ',   1),
        ('34555552', N'Pedro',    N'Molina',    'cajero2@autofix.com',    0, 'cajero2',    '3484c59cc17b5bcdf776ab8ac6eacb4cb9fcf548a322f7ab8dca89263d4e94ca', 'ES',  'RCAJ',   0);

    -- El login obliga a cambiar la contraseña si el usuario nunca la cambió
    -- (busca un evento "Cambio de contraseña"). Se cargan esos eventos para que
    -- los usuarios de prueba entren directo; recepcion2 queda sin evento a propósito.
    INSERT INTO dbo.Evento_56PS (DNI, Fecha, Modulo, Descripcion, Criticidad)
    SELECT DNI, DATEADD(DAY, -30, GETDATE()), N'Usuarios', N'Cambio de contraseña', 'Alto'
    FROM dbo.Usuario_56PS
    WHERE NombreUsuario <> 'recepcion2';

    INSERT INTO dbo.Evento_56PS (DNI, Fecha, Modulo, Descripcion, Criticidad) VALUES
        ('33444442', DATEADD(DAY, -2, GETDATE()), N'Usuarios', N'Intento fallido', 'Alto'),
        ('33444442', DATEADD(DAY, -2, GETDATE()), N'Usuarios', N'Intento fallido', 'Alto'),
        ('33444442', DATEADD(DAY, -2, GETDATE()), N'Usuarios', N'Intento fallido', 'Alto');

    /* ---------- Clientes ---------- */
    INSERT INTO dbo.Cliente_56PS (DNI, Nombre, Apellido, Telefono, Direccion, Activo) VALUES
        ('40111222', N'Juan',      N'Pérez',     '1144556677', N'Av. Hipólito Yrigoyen 8500, Lomas de Zamora', 1),
        ('38222333', N'María',     N'López',     '1133221100', N'Laprida 1200, Lomas de Zamora',                1),
        ('35333444', N'Ricardo',   N'Díaz',      '1155667788', N'Boedo 450, Lomas de Zamora',                   1),
        ('42444555', N'Camila',    N'Acosta',    '1166778899', N'Av. Meeks 300, Temperley',                     1),
        ('29555666', N'Héctor',    N'Benítez',   '1177889900', N'Gorriti 700, Banfield',                        0);   -- dado de baja

    /* ---------- Vehículos ---------- */
    INSERT INTO dbo.Vehiculo_56PS (Patente, Marca, Modelo, Cedula, DNI_Cliente, Activo) VALUES
        ('AB123CD', N'Volkswagen', N'Gol Trend',  'CED-0001', '40111222', 1),
        ('AC456EF', N'Ford',       N'Ka',         'CED-0002', '40111222', 1),   -- Juan tiene 2 autos
        ('AD789GH', N'Chevrolet',  N'Onix',       'CED-0003', '38222333', 1),
        ('KLM123',  N'Renault',    N'Clio',       'CED-0004', '35333444', 1),   -- patente formato viejo
        ('AE321JK', N'Toyota',     N'Etios',      'CED-0005', '42444555', 1),
        ('NOP456',  N'Fiat',       N'Palio',      'CED-0006', '35333444', 0),   -- dado de baja
        ('JKL789',  N'Peugeot',    N'206',        'CED-0007', '29555666', 1);   -- dueño dado de baja

    /* ---------- Turnos (fechas relativas a hoy) ---------- */
    DECLARE @hoy DATE = CAST(GETDATE() AS DATE);

    INSERT INTO dbo.Turno_56PS (Fecha, Horario, Estado, DNI_Cliente, Patente_Vehiculo, DNI_Mecanico) VALUES
        (DATEADD(DAY, -7, @hoy), '09:00', 'Atendido',   '40111222', 'AB123CD', '33444441'),
        (DATEADD(DAY, -3, @hoy), '11:00', 'Atendido',   '38222333', 'AD789GH', '33444441'),
        (DATEADD(DAY, -1, @hoy), '10:00', 'Cancelado',  '35333444', 'KLM123',  NULL),
        (DATEADD(DAY,  1, @hoy), '09:00', 'Confirmado', '40111222', 'AC456EF', NULL),
        (DATEADD(DAY,  1, @hoy), '10:00', 'Pendiente',  '42444555', 'AE321JK', NULL),
        (DATEADD(DAY,  1, @hoy), '15:00', 'Pendiente',  '38222333', 'AD789GH', NULL),
        (DATEADD(DAY,  2, @hoy), '08:00', 'Pendiente',  '35333444', 'KLM123',  NULL),
        (DATEADD(DAY,  3, @hoy), '12:00', 'Confirmado', '40111222', 'AB123CD', NULL);
END
GO

/* =====================================================================
   5. DÍGITOS VERIFICADORES
   Se registran todas las tablas protegidas con DV vacío. El instalador
   los calcula enseguida (RecalcularDV); si el script se corre a mano,
   entrar con "admin" y apretar "Recalcular DV".
   ===================================================================== */

INSERT INTO dbo.DigitoVerificador (nombreTabla, DVH, DVV)
SELECT t.nombre, '', ''
FROM (VALUES ('Evento_56PS'), ('FamiliaFamilia'), ('FamiliaPatente'), ('Familias'),
             ('Idioma_56PS'), ('Patentes'), ('Roles'), ('RolFamilia'), ('RolPatente'),
             ('Usuario_56PS'), ('Cliente_56PS'), ('Vehiculo_56PS'), ('Turno_56PS')) AS t(nombre)
WHERE NOT EXISTS (SELECT 1 FROM dbo.DigitoVerificador dv WHERE dv.nombreTabla = t.nombre);
GO
