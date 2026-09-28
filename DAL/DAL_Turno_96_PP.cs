using BE_96_PP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL_96_PP
{
    // RFN1 - Persistencia de turnos (tabla Turno_96_PP).
    public class DAL_Turno_96_PP
    {
        private const string SelectBase = @"
            SELECT T.IdTurno, T.Fecha, T.Horario, T.Estado, T.DNI_Mecanico,
                   V.Patente AS Veh_Patente, V.Marca AS Veh_Marca, V.Modelo AS Veh_Modelo,
                   V.Cedula AS Veh_Cedula, V.Activo AS Veh_Activo,
                   C.DNI AS Cli_DNI, C.Nombre AS Cli_Nombre, C.Apellido AS Cli_Apellido,
                   C.Telefono AS Cli_Telefono, C.Direccion AS Cli_Direccion, C.Activo AS Cli_Activo
            FROM dbo.Turno_56PS T
            INNER JOIN dbo.Vehiculo_56PS V ON V.Patente = T.Patente_Vehiculo
            INNER JOIN dbo.Cliente_56PS C ON C.DNI = T.DNI_Cliente";

        // Devuelve el Id generado (IDENTITY) para poder registrarlo en la bitácora.
        public int Crear(BE_Turno_96_PP turno)
        {
            string query = @"INSERT INTO dbo.Turno_56PS (Fecha, Horario, Estado, DNI_Cliente, Patente_Vehiculo, DNI_Mecanico)
                             VALUES (@Fecha, @Horario, @Estado, @DNI_Cliente, @Patente, @DNI_Mecanico);
                             SELECT CAST(SCOPE_IDENTITY() AS INT);";

            SqlParameter[] parametros =
            {
                new SqlParameter("@Fecha", SqlDbType.Date) { Value = turno.Fecha.Date },
                new SqlParameter("@Horario", SqlDbType.Time) { Value = turno.Horario },
                new SqlParameter("@Estado", turno.Estado.ToString()),
                new SqlParameter("@DNI_Cliente", turno.Cliente.Dni),
                new SqlParameter("@Patente", turno.Vehiculo.Patente),
                new SqlParameter("@DNI_Mecanico", (object)turno.DniMecanico ?? DBNull.Value)
            };

            return Convert.ToInt32(DAL_Comun_96_PP.ExecuteScalar(query, parametros));
        }

        public void CambiarEstado(int idTurno, BE_Turno_96_PP.EstadoTurno nuevoEstado)
        {
            DAL_Comun_96_PP.ExecuteNonQuery(
                "UPDATE dbo.Turno_56PS SET Estado = @Estado WHERE IdTurno = @Id",
                new[]
                {
                    new SqlParameter("@Estado", nuevoEstado.ToString()),
                    new SqlParameter("@Id", idTurno)
                });
        }

        public List<BE_Turno_96_PP> ObtenerTodos()
        {
            return Consultar(SelectBase + " ORDER BY T.Fecha DESC, T.Horario", null);
        }

        public List<BE_Turno_96_PP> ObtenerPorFecha(DateTime fecha)
        {
            return Consultar(
                SelectBase + " WHERE T.Fecha = @Fecha ORDER BY T.Horario",
                new[] { new SqlParameter("@Fecha", SqlDbType.Date) { Value = fecha.Date } });
        }

        public BE_Turno_96_PP ObtenerPorId(int idTurno)
        {
            List<BE_Turno_96_PP> lista = Consultar(
                SelectBase + " WHERE T.IdTurno = @Id",
                new[] { new SqlParameter("@Id", idTurno) });
            return lista.Count == 0 ? null : lista[0];
        }

        // Horarios ya tomados en una fecha (los turnos cancelados liberan el horario).
        public List<TimeSpan> ObtenerHorariosOcupados(DateTime fecha)
        {
            DataSet ds = DAL_Comun_96_PP.ExecuteDataSet(
                "SELECT Horario FROM dbo.Turno_56PS WHERE Fecha = @Fecha AND Estado <> 'Cancelado'",
                new[] { new SqlParameter("@Fecha", SqlDbType.Date) { Value = fecha.Date } });

            List<TimeSpan> ocupados = new List<TimeSpan>();
            foreach (DataRow fila in ds.Tables[0].Rows)
                ocupados.Add((TimeSpan)fila["Horario"]);
            return ocupados;
        }

        // ¿El vehículo ya tiene un turno vigente (Pendiente/Confirmado) ese día?
        public bool VehiculoTieneTurnoVigente(string patente, DateTime fecha)
        {
            object resultado = DAL_Comun_96_PP.ExecuteScalar(
                @"SELECT COUNT(*) FROM dbo.Turno_56PS
                  WHERE Patente_Vehiculo = @Patente AND Fecha = @Fecha
                    AND Estado IN ('Pendiente', 'Confirmado')",
                new[]
                {
                    new SqlParameter("@Patente", patente),
                    new SqlParameter("@Fecha", SqlDbType.Date) { Value = fecha.Date }
                });
            return Convert.ToInt32(resultado) > 0;
        }

        // ¿Tiene turnos vigentes (Pendiente o Confirmado) de hoy en adelante?
        // Se usa para no dar de baja un cliente o un vehículo con turnos por atender.
        public bool ClienteTieneTurnosVigentes(string dniCliente)
        {
            return ContarVigentes("DNI_Cliente = @Valor", dniCliente) > 0;
        }

        public bool VehiculoTieneTurnosVigentes(string patente)
        {
            return ContarVigentes("Patente_Vehiculo = @Valor", patente) > 0;
        }

        private int ContarVigentes(string condicion, string valor)
        {
            object resultado = DAL_Comun_96_PP.ExecuteScalar(
                "SELECT COUNT(*) FROM dbo.Turno_56PS WHERE " + condicion +
                " AND Fecha >= @Hoy AND Estado IN ('Pendiente', 'Confirmado')",
                new[]
                {
                    new SqlParameter("@Valor", valor),
                    new SqlParameter("@Hoy", SqlDbType.Date) { Value = DateTime.Today }
                });
            return Convert.ToInt32(resultado);
        }

        // A02 - Reporte del RFN1: turnos de un período con los datos que pide el reporte
        // (incluye teléfono del cliente, marca/modelo y el nombre del mecánico si ya se asignó).
        public List<BE_FilaReporteTurno_96_PP> ObtenerParaReporte(DateTime desde, DateTime hasta)
        {
            string query = @"
                SELECT T.IdTurno, T.Fecha, T.Horario, T.Estado,
                       C.DNI AS DniCliente, C.Apellido + ', ' + C.Nombre AS Cliente, C.Telefono,
                       V.Patente, V.Marca, V.Modelo,
                       CASE WHEN U.DNI IS NULL THEN NULL ELSE U.Apellido + ', ' + U.Nombre END AS Mecanico
                FROM dbo.Turno_56PS T
                INNER JOIN dbo.Cliente_56PS C ON C.DNI = T.DNI_Cliente
                INNER JOIN dbo.Vehiculo_56PS V ON V.Patente = T.Patente_Vehiculo
                LEFT JOIN dbo.Usuario_56PS U ON U.DNI = T.DNI_Mecanico
                WHERE T.Fecha BETWEEN @Desde AND @Hasta
                ORDER BY T.Fecha, T.Horario";

            DataSet ds = DAL_Comun_96_PP.ExecuteDataSet(query, new[]
            {
                new SqlParameter("@Desde", SqlDbType.Date) { Value = desde.Date },
                new SqlParameter("@Hasta", SqlDbType.Date) { Value = hasta.Date }
            });

            List<BE_FilaReporteTurno_96_PP> filas = new List<BE_FilaReporteTurno_96_PP>();
            foreach (DataRow fila in ds.Tables[0].Rows)
            {
                filas.Add(new BE_FilaReporteTurno_96_PP
                {
                    IdTurno = Convert.ToInt32(fila["IdTurno"]),
                    Fecha = Convert.ToDateTime(fila["Fecha"]),
                    Horario = ((TimeSpan)fila["Horario"]).ToString(@"hh\:mm"),
                    Estado = fila["Estado"].ToString(),
                    DniCliente = fila["DniCliente"].ToString(),
                    Cliente = fila["Cliente"].ToString(),
                    Telefono = fila["Telefono"].ToString(),
                    Patente = fila["Patente"].ToString(),
                    Marca = fila["Marca"].ToString(),
                    Modelo = fila["Modelo"].ToString(),
                    Mecanico = fila["Mecanico"] == DBNull.Value ? "-" : fila["Mecanico"].ToString()
                });
            }
            return filas;
        }

        private List<BE_Turno_96_PP> Consultar(string query, SqlParameter[] parametros)
        {
            DataSet ds = DAL_Comun_96_PP.ExecuteDataSet(query, parametros);
            List<BE_Turno_96_PP> lista = new List<BE_Turno_96_PP>();

            foreach (DataRow fila in ds.Tables[0].Rows)
            {
                BE_Vehiculo_96_PP vehiculo = DAL_Vehiculo_96_PP.Mapear(fila, "Veh_");

                lista.Add(new BE_Turno_96_PP(
                    Convert.ToInt32(fila["IdTurno"]),
                    Convert.ToDateTime(fila["Fecha"]),
                    (TimeSpan)fila["Horario"],
                    (BE_Turno_96_PP.EstadoTurno)Enum.Parse(typeof(BE_Turno_96_PP.EstadoTurno), fila["Estado"].ToString()),
                    vehiculo.Cliente,
                    vehiculo,
                    fila["DNI_Mecanico"] == DBNull.Value ? null : fila["DNI_Mecanico"].ToString()));
            }

            return lista;
        }
    }
}
