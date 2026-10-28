/* =====================================================================
   TallerMecanicoTD - Script_Migracion_QuitarCedulaVehiculo.sql
   ---------------------------------------------------------------------
   Elimina la columna Cedula de dbo.Vehiculo_56PS: no aportaba nada al
   modelo (el vehículo ya se identifica por Patente) y no la usa ninguna
   regla de negocio.

   Es idempotente: se puede ejecutar más de una vez sin romper nada
   (si la columna ya no existe, no hace nada).

   Cómo usarlo:
     1. Actualizá primero el ejecutable (recompilá con el código ya sin
        Cedula) o al menos cerrá la aplicación antes de correr esto.
     2. Ejecutá este script completo en SSMS contra la base SistemaBase.
     3. Entrá con el admin y apretá "Recalcular DV": al sacar una columna
        cambia el contenido de cada fila, así que el dígito verificador
        de Vehiculo_56PS queda desactualizado hasta que lo recalculés.
   ===================================================================== */

USE [SistemaBase]
GO

IF EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.Vehiculo_56PS') AND name = 'Cedula'
)
BEGIN
    -- Por si en algún momento se le puso un DEFAULT a la columna
    -- (Script_Create.sql actual no le pone uno, pero por las dudas).
    DECLARE @constraint NVARCHAR(200);
    SELECT @constraint = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c
        ON c.object_id = dc.parent_object_id AND c.column_id = dc.parent_column_id
    WHERE dc.parent_object_id = OBJECT_ID(N'dbo.Vehiculo_56PS') AND c.name = 'Cedula';

    IF @constraint IS NOT NULL
        EXEC('ALTER TABLE dbo.Vehiculo_56PS DROP CONSTRAINT ' + @constraint);

    ALTER TABLE dbo.Vehiculo_56PS DROP COLUMN Cedula;

    PRINT 'Columna Cedula eliminada de Vehiculo_56PS.';
END
ELSE
BEGIN
    PRINT 'Vehiculo_56PS ya no tiene la columna Cedula, no hay nada que hacer.';
END
GO

-- Deja en blanco el DV de Vehiculo_56PS: cambió el contenido de cada fila
-- (una columna menos), así que el hash viejo ya no es válido. El botón
-- "Recalcular DV" (como admin) lo vuelve a calcular con el esquema nuevo.
UPDATE dbo.DigitoVerificador
SET DVH = '', DVV = ''
WHERE nombreTabla = 'Vehiculo_56PS';
GO

PRINT 'Migración completa. Entrá con el admin y apretá "Recalcular DV" antes de seguir usando el sistema.';
GO
