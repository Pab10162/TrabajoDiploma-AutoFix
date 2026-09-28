using Servicio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL_96_PP
{
    public class DAL_Idioma_96_PP
    {
        public List<Idioma_96_PP> ObtenerIdiomas()
        {
            DataSet datos = DAL_Comun_96_PP.ExecuteDataSet(
                "SELECT Nombre, Tipo FROM Idioma_56PS ORDER BY Nombre",
                null);

            List<Idioma_96_PP> idiomas = new List<Idioma_96_PP>();
            foreach (DataRow fila in datos.Tables[0].Rows)
            {
                idiomas.Add(new Idioma_96_PP
                {
                    nombre = fila["Nombre"].ToString(),
                    tipo = fila["Tipo"].ToString()
                });
            }

            return idiomas;
        }

        public Idioma_96_PP ObtenerIdioma(string tipo)
        {
            DataSet datos = DAL_Comun_96_PP.ExecuteDataSet(
                "SELECT Nombre, Tipo FROM Idioma_56PS WHERE Tipo = @Tipo",
                new[] { new SqlParameter("@Tipo", tipo) });

            if (datos.Tables[0].Rows.Count == 0)
                return null;

            DataRow fila = datos.Tables[0].Rows[0];
            return new Idioma_96_PP
            {
                nombre = fila["Nombre"].ToString(),
                tipo = fila["Tipo"].ToString()
            };
        }
    }
}
