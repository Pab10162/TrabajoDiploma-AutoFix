using BE_96_PP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL_96_PP
{
    public class DAL_BitacoraEventos_96_PP
    {

        public void RegistrarEvento(Evento_96_PP e)
        {
            string query = @"INSERT INTO Evento_56PS (dni, fecha, modulo, descripcion, criticidad) 
                             VALUES (@dni, @fecha, @modulo, @descripcion, @criticidad)";

            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@dni", e.dni),
                new SqlParameter("@fecha", e.fecha),
                new SqlParameter("@modulo", e.modulo),
                new SqlParameter("@descripcion", e.descripcion),
                new SqlParameter("@criticidad", e.criticidad.ToString())
            };

            DAL_Comun_96_PP.ExecuteNonQuery(query, parametros);
        }

        public List<Evento_96_PP> obtenerEventos()
        {
            string query = "SELECT numero, dni, fecha, modulo, descripcion, criticidad FROM Evento_56PS ORDER BY fecha DESC";

            DataSet ds = DAL_Comun_96_PP.ExecuteDataSet(query, null);

            List<Evento_96_PP> lista = new List<Evento_96_PP>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Evento_96_PP evento = new Evento_96_PP(
                    Convert.ToInt32(row["numero"]),
                    row["dni"].ToString(),
                    Convert.ToDateTime(row["fecha"]),
                    row["modulo"].ToString(),
                    row["descripcion"].ToString(),
                    (Evento_96_PP.Criticidad)Enum.Parse(typeof(Evento_96_PP.Criticidad), row["criticidad"].ToString())
                );

                lista.Add(evento);
            }

            return lista;
        }

    }
}
