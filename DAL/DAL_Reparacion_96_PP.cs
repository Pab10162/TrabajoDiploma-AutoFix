using BE_96_PP;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL_96_PP
{
    // RFN2 - Persistencia de la reparación y su informe de finalización (tabla Reparacion_56PS).
    public class DAL_Reparacion_96_PP
    {
        public int Crear(BE_Reparacion_96_PP reparacion)
        {
            string query = @"INSERT INTO dbo.Reparacion_56PS
                                 (IdDiagnostico, TareasRealizadas, RepuestosUtilizados, Fecha, Hora, DNI_Mecanico)
                             VALUES (@IdDiagnostico, @Tareas, @Repuestos, @Fecha, @Hora, @DniMecanico);
                             SELECT CAST(SCOPE_IDENTITY() AS INT);";

            SqlParameter[] parametros =
            {
                new SqlParameter("@IdDiagnostico", reparacion.IdDiagnostico),
                new SqlParameter("@Tareas", reparacion.TareasRealizadas),
                new SqlParameter("@Repuestos", (object)reparacion.RepuestosUtilizados ?? DBNull.Value),
                new SqlParameter("@Fecha", SqlDbType.Date) { Value = reparacion.Fecha.Date },
                new SqlParameter("@Hora", SqlDbType.Time) { Value = reparacion.Hora },
                new SqlParameter("@DniMecanico", reparacion.DniMecanico)
            };

            return Convert.ToInt32(DAL_Comun_96_PP.ExecuteScalar(query, parametros));
        }

        // Paso 10: informe de finalización (presupuesto final y fecha de cierre).
        public void RegistrarFinalizacion(int idReparacion, decimal presupuestoFinal, DateTime fechaFinalizacion)
        {
            DAL_Comun_96_PP.ExecuteNonQuery(
                @"UPDATE dbo.Reparacion_56PS
                  SET PresupuestoFinal = @Presupuesto, FechaFinalizacion = @FechaFin
                  WHERE IdReparacion = @Id",
                new[]
                {
                    new SqlParameter("@Presupuesto", SqlDbType.Decimal) { Precision = 12, Scale = 2, Value = presupuestoFinal },
                    new SqlParameter("@FechaFin", SqlDbType.DateTime) { Value = fechaFinalizacion },
                    new SqlParameter("@Id", idReparacion)
                });
        }

        public BE_Reparacion_96_PP ObtenerPorDiagnostico(int idDiagnostico)
        {
            DataSet ds = DAL_Comun_96_PP.ExecuteDataSet(
                @"SELECT IdReparacion, IdDiagnostico, TareasRealizadas, RepuestosUtilizados,
                         Fecha, Hora, DNI_Mecanico, PresupuestoFinal, FechaFinalizacion
                  FROM dbo.Reparacion_56PS WHERE IdDiagnostico = @Id",
                new[] { new SqlParameter("@Id", idDiagnostico) });

            if (ds.Tables[0].Rows.Count == 0)
                return null;

            DataRow fila = ds.Tables[0].Rows[0];
            return new BE_Reparacion_96_PP
            {
                IdReparacion = Convert.ToInt32(fila["IdReparacion"]),
                IdDiagnostico = Convert.ToInt32(fila["IdDiagnostico"]),
                TareasRealizadas = fila["TareasRealizadas"].ToString(),
                RepuestosUtilizados = fila["RepuestosUtilizados"] == DBNull.Value
                    ? null : fila["RepuestosUtilizados"].ToString(),
                Fecha = Convert.ToDateTime(fila["Fecha"]),
                Hora = (TimeSpan)fila["Hora"],
                DniMecanico = fila["DNI_Mecanico"].ToString(),
                PresupuestoFinal = fila["PresupuestoFinal"] == DBNull.Value
                    ? (decimal?)null : Convert.ToDecimal(fila["PresupuestoFinal"]),
                FechaFinalizacion = fila["FechaFinalizacion"] == DBNull.Value
                    ? (DateTime?)null : Convert.ToDateTime(fila["FechaFinalizacion"])
            };
        }
    }
}
