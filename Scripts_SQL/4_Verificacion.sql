/* Solo consulta, no modifica nada. Resultado esperado:
   1) las 6 tablas del RFN2
   2) cero filas (la columna Cedula ya no existe)
   3) P60, P61, P62, P63, P65, P69 (como minimo) */
USE [SistemaBase]
GO

SELECT name AS TablasRFN2 FROM sys.tables WHERE name IN
  ('Asistencia_56PS','Revision_56PS','Diagnostico_56PS','RepuestoFaltante_56PS','Reparacion_56PS','Cobro_56PS')
ORDER BY name;

SELECT name AS ColumnaCedula FROM sys.columns
WHERE object_id = OBJECT_ID('dbo.Vehiculo_56PS') AND name = 'Cedula';

SELECT CodigoPatente AS PatentesFRECEP FROM dbo.FamiliaPatente
WHERE CodigoFamilia = 'FRECEP' ORDER BY CodigoPatente;
GO
