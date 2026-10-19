using BE_96_PP;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL_96_PP
{
    // RFN2 - Persistencia del informe de revisión y la orden de inspección.
    public class DAL_Revision_96_PP
    {
        public int Crear(BE_Revision_96_PP revision)
        {
            string query = @"INSERT INTO dbo.Revision_56PS (IdTurno, Observaciones, Fecha, Hora, DNI_Jefe, DNI_Mecanico)
                             VALUES (@IdTurno, @Observaciones, @Fecha, @Hora, @DniJefe, @DniMecanico);
                             SELECT CAST(SCOPE_IDENTITY() AS INT);";

            SqlParameter[] parametros =
            {
                new SqlParameter("@IdTurno", revision.IdTurno),
                new SqlParameter("@Observaciones", revision.Observaciones),
                new SqlParameter("@Fecha", SqlDbType.Date) { Value = revision.Fecha.Date },
                new SqlParameter("@Hora", SqlDbType.Time) { Value = revision.Hora },
                new SqlParameter("@DniJefe", revision.DniJefe),
                new SqlParameter("@DniMecanico", revision.DniMecanico)
            };

            return Convert.ToInt32(DAL_Comun_96_PP.ExecuteScalar(query, parametros));
        }

        public BE_Revision_96_PP ObtenerPorTurno(int idTurno)
        {
            DataSet ds = DAL_Comun_96_PP.ExecuteDataSet(
                @"SELECT IdRevision, IdTurno, Observaciones, Fecha, Hora, DNI_Jefe, DNI_Mecanico
                  FROM dbo.Revision_56PS WHERE IdTurno = @IdTurno",
                new[] { new SqlParameter("@IdTurno", idTurno) });

            if (ds.Tables[0].Rows.Count == 0)
                return null;

            DataRow fila = ds.Tables[0].Rows[0];
            return new BE_Revision_96_PP
            {
                IdRevision = Convert.ToInt32(fila["IdRevision"]),
                IdTurno = Convert.ToInt32(fila["IdTurno"]),
                Observaciones = fila["Observaciones"].ToString(),
                Fecha = Convert.ToDateTime(fila["Fecha"]),
                Hora = (TimeSpan)fila["Hora"],
                DniJefe = fila["DNI_Jefe"].ToString(),
                DniMecanico = fila["DNI_Mecanico"].ToString()
            };
        }
    }
}
