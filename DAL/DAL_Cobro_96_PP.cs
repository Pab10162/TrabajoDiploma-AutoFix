using BE_96_PP;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL_96_PP
{
    // RFN2 - Persistencia del cobro del servicio (tabla Cobro_56PS).
    public class DAL_Cobro_96_PP
    {
        public int Crear(BE_Cobro_96_PP cobro)
        {
            string query = @"INSERT INTO dbo.Cobro_56PS
                                 (IdTurno, MontoTotal, MetodoPago, Fecha, Hora, DNI_Cliente, DNI_Recepcionista)
                             VALUES (@IdTurno, @Monto, @Metodo, @Fecha, @Hora, @DniCliente, @DniRecepcionista);
                             SELECT CAST(SCOPE_IDENTITY() AS INT);";

            SqlParameter[] parametros =
            {
                new SqlParameter("@IdTurno", cobro.IdTurno),
                new SqlParameter("@Monto", SqlDbType.Decimal) { Precision = 12, Scale = 2, Value = cobro.MontoTotal },
                new SqlParameter("@Metodo", cobro.MetodoPago.ToString()),
                new SqlParameter("@Fecha", SqlDbType.Date) { Value = cobro.Fecha.Date },
                new SqlParameter("@Hora", SqlDbType.Time) { Value = cobro.Hora },
                new SqlParameter("@DniCliente", cobro.DniCliente),
                new SqlParameter("@DniRecepcionista", cobro.DniRecepcionista)
            };

            return Convert.ToInt32(DAL_Comun_96_PP.ExecuteScalar(query, parametros));
        }

        public BE_Cobro_96_PP ObtenerPorTurno(int idTurno)
        {
            DataSet ds = DAL_Comun_96_PP.ExecuteDataSet(
                @"SELECT IdCobro, IdTurno, MontoTotal, MetodoPago, Fecha, Hora, DNI_Cliente, DNI_Recepcionista
                  FROM dbo.Cobro_56PS WHERE IdTurno = @IdTurno",
                new[] { new SqlParameter("@IdTurno", idTurno) });

            if (ds.Tables[0].Rows.Count == 0)
                return null;

            DataRow fila = ds.Tables[0].Rows[0];
            return new BE_Cobro_96_PP
            {
                IdCobro = Convert.ToInt32(fila["IdCobro"]),
                IdTurno = Convert.ToInt32(fila["IdTurno"]),
                MontoTotal = Convert.ToDecimal(fila["MontoTotal"]),
                MetodoPago = (BE_Cobro_96_PP.MedioPago)Enum.Parse(
                    typeof(BE_Cobro_96_PP.MedioPago), fila["MetodoPago"].ToString()),
                Fecha = Convert.ToDateTime(fila["Fecha"]),
                Hora = (TimeSpan)fila["Hora"],
                DniCliente = fila["DNI_Cliente"].ToString(),
                DniRecepcionista = fila["DNI_Recepcionista"].ToString()
            };
        }
    }
}
