using BE_96_PP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL_96_PP
{
    // RFN1 - Persistencia de clientes (tabla Cliente_96_PP).
    // Usa los helpers genéricos de DAL_96_PP igual que el resto del SistemaBase.
    public class DAL_Cliente_96_PP
    {
        private const string SelectBase =
            "SELECT DNI, Nombre, Apellido, Telefono, Direccion, Activo FROM dbo.Cliente_56PS";

        public void Crear(BE_Cliente_96_PP cliente)
        {
            string query = @"INSERT INTO dbo.Cliente_56PS (DNI, Nombre, Apellido, Telefono, Direccion, Activo)
                             VALUES (@DNI, @Nombre, @Apellido, @Telefono, @Direccion, @Activo)";
            DAL_Comun_96_PP.ExecuteNonQuery(query, Parametros(cliente));
        }

        public void Modificar(BE_Cliente_96_PP cliente)
        {
            string query = @"UPDATE dbo.Cliente_56PS SET
                                Nombre = @Nombre,
                                Apellido = @Apellido,
                                Telefono = @Telefono,
                                Direccion = @Direccion,
                                Activo = @Activo
                             WHERE DNI = @DNI";
            DAL_Comun_96_PP.ExecuteNonQuery(query, Parametros(cliente));
        }

        public List<BE_Cliente_96_PP> ObtenerTodos()
        {
            DataSet ds = DAL_Comun_96_PP.ExecuteDataSet(SelectBase + " ORDER BY Apellido, Nombre", null);
            List<BE_Cliente_96_PP> lista = new List<BE_Cliente_96_PP>();
            foreach (DataRow fila in ds.Tables[0].Rows)
                lista.Add(Mapear(fila));
            return lista;
        }

        public BE_Cliente_96_PP ObtenerPorDni(string dni)
        {
            DataSet ds = DAL_Comun_96_PP.ExecuteDataSet(
                SelectBase + " WHERE DNI = @DNI",
                new[] { new SqlParameter("@DNI", dni) });

            return ds.Tables[0].Rows.Count == 0 ? null : Mapear(ds.Tables[0].Rows[0]);
        }

        public bool Existe(string dni)
        {
            object resultado = DAL_Comun_96_PP.ExecuteScalar(
                "SELECT COUNT(*) FROM dbo.Cliente_56PS WHERE DNI = @DNI",
                new[] { new SqlParameter("@DNI", dni) });
            return Convert.ToInt32(resultado) > 0;
        }

        private SqlParameter[] Parametros(BE_Cliente_96_PP c)
        {
            return new[]
            {
                new SqlParameter("@DNI", c.Dni),
                new SqlParameter("@Nombre", c.Nombre),
                new SqlParameter("@Apellido", c.Apellido),
                new SqlParameter("@Telefono", (object)c.Telefono ?? DBNull.Value),
                new SqlParameter("@Direccion", (object)c.Direccion ?? DBNull.Value),
                new SqlParameter("@Activo", c.Activo)
            };
        }

        // internal static: lo reutilizan DAL_Vehiculo y DAL_Turno cuando leen el
        // cliente dentro de un JOIN (columnas con prefijo "Cli_").
        internal static BE_Cliente_96_PP Mapear(DataRow fila, string prefijo = "")
        {
            return new BE_Cliente_96_PP(
                fila[prefijo + "DNI"].ToString(),
                fila[prefijo + "Nombre"].ToString(),
                fila[prefijo + "Apellido"].ToString(),
                fila[prefijo + "Telefono"] == DBNull.Value ? null : fila[prefijo + "Telefono"].ToString(),
                fila[prefijo + "Direccion"] == DBNull.Value ? null : fila[prefijo + "Direccion"].ToString(),
                Convert.ToBoolean(fila[prefijo + "Activo"]));
        }
    }
}
