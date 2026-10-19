using BE_96_PP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL_96_PP
{
    // RFN2 - Persistencia de los repuestos sin stock (tabla RepuestoFaltante_56PS).
    public class DAL_RepuestoFaltante_96_PP
    {
        public int Crear(BE_RepuestoFaltante_96_PP faltante)
        {
            string query = @"INSERT INTO dbo.RepuestoFaltante_56PS (IdDiagnostico, Repuesto, Cantidad, Estado)
                             VALUES (@IdDiagnostico, @Repuesto, @Cantidad, @Estado);
                             SELECT CAST(SCOPE_IDENTITY() AS INT);";

            SqlParameter[] parametros =
            {
                new SqlParameter("@IdDiagnostico", faltante.IdDiagnostico),
                new SqlParameter("@Repuesto", faltante.Repuesto),
                new SqlParameter("@Cantidad", faltante.Cantidad),
                new SqlParameter("@Estado", faltante.Estado.ToString())
            };

            return Convert.ToInt32(DAL_Comun_96_PP.ExecuteScalar(query, parametros));
        }

        public void CambiarEstado(int idFaltante, BE_RepuestoFaltante_96_PP.EstadoFaltante nuevoEstado)
        {
            DAL_Comun_96_PP.ExecuteNonQuery(
                "UPDATE dbo.RepuestoFaltante_56PS SET Estado = @Estado WHERE IdFaltante = @Id",
                new[]
                {
                    new SqlParameter("@Estado", nuevoEstado.ToString()),
                    new SqlParameter("@Id", idFaltante)
                });
        }

        public List<BE_RepuestoFaltante_96_PP> ObtenerPorDiagnostico(int idDiagnostico)
        {
            DataSet ds = DAL_Comun_96_PP.ExecuteDataSet(
                @"SELECT IdFaltante, IdDiagnostico, Repuesto, Cantidad, Estado
                  FROM dbo.RepuestoFaltante_56PS
                  WHERE IdDiagnostico = @Id ORDER BY IdFaltante",
                new[] { new SqlParameter("@Id", idDiagnostico) });

            List<BE_RepuestoFaltante_96_PP> lista = new List<BE_RepuestoFaltante_96_PP>();
            foreach (DataRow fila in ds.Tables[0].Rows)
            {
                lista.Add(new BE_RepuestoFaltante_96_PP
                {
                    IdFaltante = Convert.ToInt32(fila["IdFaltante"]),
                    IdDiagnostico = Convert.ToInt32(fila["IdDiagnostico"]),
                    Repuesto = fila["Repuesto"].ToString(),
                    Cantidad = Convert.ToInt32(fila["Cantidad"]),
                    Estado = (BE_RepuestoFaltante_96_PP.EstadoFaltante)Enum.Parse(
                        typeof(BE_RepuestoFaltante_96_PP.EstadoFaltante), fila["Estado"].ToString())
                });
            }
            return lista;
        }
    }
}
