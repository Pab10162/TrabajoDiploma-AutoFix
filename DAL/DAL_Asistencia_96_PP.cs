using BE_96_PP;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL_96_PP
{
    // RFN2 - Persistencia de la asistencia del cliente.
    public class DAL_Asistencia_96_PP
    {
        private const string SelectBase =
            "SELECT IdAsistencia, IdTurno, Fecha, Hora, Estado, DNI_Recepcionista FROM dbo.Asistencia_56PS";

        // Devuelve el Id generado.
        public int Crear(BE_Asistencia_96_PP asistencia)
        {
            string query = @"INSERT INTO dbo.Asistencia_56PS (IdTurno, Fecha, Hora, Estado, DNI_Recepcionista)
                             VALUES (@IdTurno, @Fecha, @Hora, @Estado, @DniRecepcionista);
                             SELECT CAST(SCOPE_IDENTITY() AS INT);";

            SqlParameter[] parametros =
            {
                new SqlParameter("@IdTurno", asistencia.IdTurno),
                new SqlParameter("@Fecha", SqlDbType.Date) { Value = asistencia.Fecha.Date },
                new SqlParameter("@Hora", SqlDbType.Time) { Value = asistencia.Hora },
                new SqlParameter("@Estado", asistencia.Estado.ToString()),
                new SqlParameter("@DniRecepcionista", asistencia.DniRecepcionista)
            };

            return Convert.ToInt32(DAL_Comun_96_PP.ExecuteScalar(query, parametros));
        }

        public BE_Asistencia_96_PP ObtenerPorTurno(int idTurno)
        {
            DataSet ds = DAL_Comun_96_PP.ExecuteDataSet(
                SelectBase + " WHERE IdTurno = @IdTurno",
                new[] { new SqlParameter("@IdTurno", idTurno) });

            if (ds.Tables[0].Rows.Count == 0)
                return null;

            DataRow fila = ds.Tables[0].Rows[0];
            return new BE_Asistencia_96_PP
            {
                IdAsistencia = Convert.ToInt32(fila["IdAsistencia"]),
                IdTurno = Convert.ToInt32(fila["IdTurno"]),
                Fecha = Convert.ToDateTime(fila["Fecha"]),
                Hora = (TimeSpan)fila["Hora"],
                Estado = (BE_Asistencia_96_PP.EstadoAsistencia)Enum.Parse(
                    typeof(BE_Asistencia_96_PP.EstadoAsistencia), fila["Estado"].ToString()),
                DniRecepcionista = fila["DNI_Recepcionista"].ToString()
            };
        }
    }
}
