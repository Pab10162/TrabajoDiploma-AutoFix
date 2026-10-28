/* Da a la familia FRECEP ("Recepcion de turnos") sus patentes P60..P63.
   Sin esto no se habilitan los menus Maestros y Turnos. Es idempotente. */
USE [SistemaBase]
GO

INSERT INTO dbo.FamiliaPatente (CodigoFamilia, CodigoPatente)
SELECT 'FRECEP', p FROM (VALUES ('P60'),('P61'),('P62'),('P63')) v(p)
WHERE NOT EXISTS (SELECT 1 FROM dbo.FamiliaPatente
                  WHERE CodigoFamilia = 'FRECEP' AND CodigoPatente = v.p)
GO
