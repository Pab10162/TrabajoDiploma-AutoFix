using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servicio;
using DAL_96_PP;

namespace DAL
{
    public class DAL_DigitoVerificador_96_PP
    {
        public List<DigitoVerificador_96_PP> ConsultarDV()
        {
            string query = "SELECT nombreTabla, DVH, DVV FROM DigitoVerificador";
            DataSet ds = DAL_96_PP.ExecuteDataSet(query, null);

            List<DigitoVerificador_96_PP> lista = new List<DigitoVerificador_96_PP>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                DigitoVerificador_96_PP dv = new DigitoVerificador_96_PP
                {
                    tabla = row["nombreTabla"].ToString(),
                    DVH = row["DVH"].ToString(),
                    DVV = row["DVV"].ToString()
                };
                lista.Add(dv);
            }

            return lista;
        }

         public void GuardarDV(DigitoVerificador_96_PP dv)
        {
             string queryExiste = "SELECT COUNT(*) FROM DigitoVerificador WHERE nombreTabla = @nombreTabla";
            SqlParameter[] paramExiste = { new SqlParameter("@nombreTabla", dv.tabla) };
            int existe = Convert.ToInt32(DAL_96_PP.ExecuteScalar(queryExiste, paramExiste));

             string query = existe > 0
                ? "UPDATE DigitoVerificador SET DVH = @DVH, DVV = @DVV WHERE nombreTabla = @nombreTabla"
                : "INSERT INTO DigitoVerificador (nombreTabla, DVH, DVV) VALUES (@nombreTabla, @DVH, @DVV)";

            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@nombreTabla", dv.tabla),
                new SqlParameter("@DVH", (object)dv.DVH ?? DBNull.Value),
                new SqlParameter("@DVV", (object)dv.DVV ?? DBNull.Value)
            };

            DAL_96_PP.ExecuteNonQuery(query, parametros);
        }
    }
}
