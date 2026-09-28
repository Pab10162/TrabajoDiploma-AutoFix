using BE_56_PS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL_625NS
{
    // RFN1 - Persistencia de vehículos (tabla Vehiculo_56PS).
    // A diferencia de la versión anterior (que buscaba el cliente fila por fila),
    // acá el cliente se trae con un JOIN en la misma consulta.
    public class DAL_Vehiculo_56PS
    {
        private const string SelectBase = @"
            SELECT V.Patente, V.Marca, V.Modelo, V.Cedula, V.Activo,
                   C.DNI AS Cli_DNI, C.Nombre AS Cli_Nombre, C.Apellido AS Cli_Apellido,
                   C.Telefono AS Cli_Telefono, C.Direccion AS Cli_Direccion, C.Activo AS Cli_Activo
            FROM dbo.Vehiculo_56PS V
            INNER JOIN dbo.Cliente_56PS C ON C.DNI = V.DNI_Cliente";

        public void Crear(BE_Vehiculo_56PS vehiculo)
        {
            string query = @"INSERT INTO dbo.Vehiculo_56PS (Patente, Marca, Modelo, Cedula, DNI_Cliente, Activo)
                             VALUES (@Patente, @Marca, @Modelo, @Cedula, @DNI_Cliente, @Activo)";
            DAL_56PS.ExecuteNonQuery(query, Parametros(vehiculo));
        }

        public void Modificar(BE_Vehiculo_56PS vehiculo)
        {
            string query = @"UPDATE dbo.Vehiculo_56PS SET
                                Marca = @Marca,
                                Modelo = @Modelo,
                                Cedula = @Cedula,
                                DNI_Cliente = @DNI_Cliente,
                                Activo = @Activo
                             WHERE Patente = @Patente";
            DAL_56PS.ExecuteNonQuery(query, Parametros(vehiculo));
        }

        public List<BE_Vehiculo_56PS> ObtenerTodos()
        {
            return Consultar(SelectBase + " ORDER BY V.Patente", null);
        }

        public List<BE_Vehiculo_56PS> ObtenerPorCliente(string dniCliente)
        {
            return Consultar(
                SelectBase + " WHERE V.DNI_Cliente = @DNI ORDER BY V.Patente",
                new[] { new SqlParameter("@DNI", dniCliente) });
        }

        public BE_Vehiculo_56PS ObtenerPorPatente(string patente)
        {
            List<BE_Vehiculo_56PS> lista = Consultar(
                SelectBase + " WHERE V.Patente = @Patente",
                new[] { new SqlParameter("@Patente", patente) });
            return lista.Count == 0 ? null : lista[0];
        }

        public bool Existe(string patente)
        {
            object resultado = DAL_56PS.ExecuteScalar(
                "SELECT COUNT(*) FROM dbo.Vehiculo_56PS WHERE Patente = @Patente",
                new[] { new SqlParameter("@Patente", patente) });
            return Convert.ToInt32(resultado) > 0;
        }

        private List<BE_Vehiculo_56PS> Consultar(string query, SqlParameter[] parametros)
        {
            DataSet ds = DAL_56PS.ExecuteDataSet(query, parametros);
            List<BE_Vehiculo_56PS> lista = new List<BE_Vehiculo_56PS>();
            foreach (DataRow fila in ds.Tables[0].Rows)
                lista.Add(Mapear(fila));
            return lista;
        }

        private SqlParameter[] Parametros(BE_Vehiculo_56PS v)
        {
            return new[]
            {
                new SqlParameter("@Patente", v.Patente),
                new SqlParameter("@Marca", v.Marca),
                new SqlParameter("@Modelo", v.Modelo),
                new SqlParameter("@Cedula", (object)v.Cedula ?? DBNull.Value),
                new SqlParameter("@DNI_Cliente", v.Cliente.Dni),
                new SqlParameter("@Activo", v.Activo)
            };
        }

        // prefijo = "" cuando la fila viene de SelectBase; "Veh_" cuando viene del JOIN de turnos.
        internal static BE_Vehiculo_56PS Mapear(DataRow fila, string prefijo = "")
        {
            BE_Cliente_56PS cliente = DAL_Cliente_56PS.Mapear(fila, "Cli_");
            return new BE_Vehiculo_56PS(
                fila[prefijo + "Patente"].ToString(),
                fila[prefijo + "Marca"].ToString(),
                fila[prefijo + "Modelo"].ToString(),
                fila[prefijo + "Cedula"] == DBNull.Value ? null : fila[prefijo + "Cedula"].ToString(),
                cliente,
                Convert.ToBoolean(fila[prefijo + "Activo"]));
        }
    }
}
