using BE_96_PP;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL_96_PP
{
    // RFN2 - Persistencia del diagnóstico técnico y su solución.
    public class DAL_Diagnostico_96_PP
    {
        private const string SelectBase = @"
            SELECT IdDiagnostico, IdTurno, SistemaAfectado, DescripcionFallas, Urgencia,
                   RepuestosNecesarios, Fecha, Hora, DNI_Mecanico, TipoSolucion, DescripcionSolucion
            FROM dbo.Diagnostico_56PS";

        public int Crear(BE_Diagnostico_96_PP diagnostico)
        {
            string query = @"INSERT INTO dbo.Diagnostico_56PS
                                 (IdTurno, SistemaAfectado, DescripcionFallas, Urgencia,
                                  RepuestosNecesarios, Fecha, Hora, DNI_Mecanico)
                             VALUES (@IdTurno, @Sistema, @Fallas, @Urgencia,
                                     @Repuestos, @Fecha, @Hora, @DniMecanico);
                             SELECT CAST(SCOPE_IDENTITY() AS INT);";

            SqlParameter[] parametros =
            {
                new SqlParameter("@IdTurno", diagnostico.IdTurno),
                new SqlParameter("@Sistema", diagnostico.SistemaAfectado),
                new SqlParameter("@Fallas", diagnostico.DescripcionFallas),
                new SqlParameter("@Urgencia", diagnostico.Urgencia.ToString()),
                new SqlParameter("@Repuestos", (object)diagnostico.RepuestosNecesarios ?? DBNull.Value),
                new SqlParameter("@Fecha", SqlDbType.Date) { Value = diagnostico.Fecha.Date },
                new SqlParameter("@Hora", SqlDbType.Time) { Value = diagnostico.Hora },
                new SqlParameter("@DniMecanico", diagnostico.DniMecanico)
            };

            return Convert.ToInt32(DAL_Comun_96_PP.ExecuteScalar(query, parametros));
        }

        public void RegistrarSolucion(int idDiagnostico, BE_Diagnostico_96_PP.TipoDeSolucion tipo, string descripcion)
        {
            DAL_Comun_96_PP.ExecuteNonQuery(
                @"UPDATE dbo.Diagnostico_56PS
                  SET TipoSolucion = @Tipo, DescripcionSolucion = @Descripcion
                  WHERE IdDiagnostico = @Id",
                new[]
                {
                    new SqlParameter("@Tipo", tipo.ToString()),
                    new SqlParameter("@Descripcion", descripcion),
                    new SqlParameter("@Id", idDiagnostico)
                });
        }

        public BE_Diagnostico_96_PP ObtenerPorId(int idDiagnostico)
        {
            return Consultar(" WHERE IdDiagnostico = @Id", new SqlParameter("@Id", idDiagnostico));
        }

        public BE_Diagnostico_96_PP ObtenerPorTurno(int idTurno)
        {
            return Consultar(" WHERE IdTurno = @Id", new SqlParameter("@Id", idTurno));
        }

        private BE_Diagnostico_96_PP Consultar(string filtro, SqlParameter parametro)
        {
            DataSet ds = DAL_Comun_96_PP.ExecuteDataSet(SelectBase + filtro, new[] { parametro });
            if (ds.Tables[0].Rows.Count == 0)
                return null;

            DataRow fila = ds.Tables[0].Rows[0];
            BE_Diagnostico_96_PP diagnostico = new BE_Diagnostico_96_PP
            {
                IdDiagnostico = Convert.ToInt32(fila["IdDiagnostico"]),
                IdTurno = Convert.ToInt32(fila["IdTurno"]),
                SistemaAfectado = fila["SistemaAfectado"].ToString(),
                DescripcionFallas = fila["DescripcionFallas"].ToString(),
                Urgencia = (BE_Diagnostico_96_PP.NivelUrgencia)Enum.Parse(
                    typeof(BE_Diagnostico_96_PP.NivelUrgencia), fila["Urgencia"].ToString()),
                RepuestosNecesarios = fila["RepuestosNecesarios"] == DBNull.Value
                    ? null : fila["RepuestosNecesarios"].ToString(),
                Fecha = Convert.ToDateTime(fila["Fecha"]),
                Hora = (TimeSpan)fila["Hora"],
                DniMecanico = fila["DNI_Mecanico"].ToString()
            };

            if (fila["TipoSolucion"] != DBNull.Value)
            {
                diagnostico.TipoSolucion = (BE_Diagnostico_96_PP.TipoDeSolucion)Enum.Parse(
                    typeof(BE_Diagnostico_96_PP.TipoDeSolucion), fila["TipoSolucion"].ToString());
                diagnostico.DescripcionSolucion = fila["DescripcionSolucion"] == DBNull.Value
                    ? null : fila["DescripcionSolucion"].ToString();
            }

            return diagnostico;
        }
    }
}
