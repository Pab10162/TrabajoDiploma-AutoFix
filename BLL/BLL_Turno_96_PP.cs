using ClassLibrary2;
using ClassLibrary3;
using System.Data;
using BE_96_PP;
using BE_96_PP;
using DAL_96_PP;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace BLL
{
    public class BLL_Turno_96_PP
    {
        private const string Tabla = "Turno_56PS";
        private const string Modulo = "Turnos";

        public const int HoraPrimerTurno = 8;
        public const int HoraUltimoTurno = 21;

        private readonly DAL_Turno_96_PP dal = new DAL_Turno_96_PP();
        private readonly BLL_Cliente_96_PP bllCliente = new BLL_Cliente_96_PP();
        private readonly BLL_Vehiculo_96_PP bllVehiculo = new BLL_Vehiculo_96_PP();

        public List<BE_Turno_96_PP> ObtenerTodos()
        {
            return dal.ObtenerTodos();
        }

        public List<BE_Turno_96_PP> ObtenerPorFecha(DateTime fecha)
        {
            return dal.ObtenerPorFecha(fecha);
        }

        public List<TimeSpan> ObtenerHorariosDisponibles(DateTime fecha)
        {
            List<TimeSpan> disponibles = new List<TimeSpan>();
            if (fecha.Date < DateTime.Today)
                return disponibles;

            List<TimeSpan> ocupados = dal.ObtenerHorariosOcupados(fecha);

            for (int hora = HoraPrimerTurno; hora <= HoraUltimoTurno; hora++)
            {
                TimeSpan horario = TimeSpan.FromHours(hora);

                bool yaPaso = fecha.Date == DateTime.Today && horario <= DateTime.Now.TimeOfDay;
                if (!yaPaso && !ocupados.Contains(horario))
                    disponibles.Add(horario);
            }

            return disponibles;
        }

        public BE_Turno_96_PP AsignarTurno(string dniCliente, string patente, DateTime fecha, TimeSpan horario)
        {
            BE_Cliente_96_PP cliente = bllCliente.BuscarPorDni(dniCliente);
            if (cliente == null)
                throw new Exception("El cliente no existe.");
            if (!cliente.Activo)
                throw new Exception("El cliente está dado de baja.");

            BE_Vehiculo_96_PP vehiculo = bllVehiculo.BuscarPorPatente(patente);
            if (vehiculo == null)
                throw new Exception("El vehículo no existe.");
            if (!vehiculo.Activo)
                throw new Exception("El vehículo está dado de baja.");
            if (vehiculo.Cliente.Dni != cliente.Dni)
                throw new Exception("El vehículo no pertenece al cliente seleccionado.");

            if (fecha.Date < DateTime.Today)
                throw new Exception("No puede crearse un turno con una fecha pasada");

            if (!ObtenerHorariosDisponibles(fecha).Contains(horario))
                throw new Exception("El horario seleccionado no está disponible.");

            BE_Turno_96_PP turno = new BE_Turno_96_PP(fecha, horario, cliente, vehiculo);

            try
            {
                turno.IdTurno = dal.Crear(turno);
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                throw new Exception("El horario seleccionado no está disponible.");
            }

            RecalcularDV(Tabla);
            RegistrarEvento(Modulo,
                $"Asignación de turno #{turno.IdTurno} {turno.Fecha:dd/MM/yyyy} {turno.HorarioTexto} - {vehiculo.Patente}",
                Evento_96_PP.Criticidad.Bajo);

            return turno;
        }

        public void SerializarTurno(BE_Turno_96_PP turno, string rutaArchivo)
        {
            if (turno == null)
                throw new Exception("Debe seleccionar un turno para exportar.");

            XmlSerializer serializador = new XmlSerializer(typeof(BE_Turno_96_PP));
            using (StreamWriter escritor = new StreamWriter(rutaArchivo))
            {
                serializador.Serialize(escritor, turno);
            }
        }

        public BE_Turno_96_PP DeserializarTurno(string rutaArchivo)
        {
            XmlSerializer serializador = new XmlSerializer(typeof(BE_Turno_96_PP));
            using (StreamReader lector = new StreamReader(rutaArchivo))
            {
                return (BE_Turno_96_PP)serializador.Deserialize(lector);
            }
        }

        public void Confirmar(BE_Turno_96_PP turno)
        {
            CambiarEstado(turno, BE_Turno_96_PP.EstadoTurno.Confirmado,
                          BE_Turno_96_PP.EstadoTurno.Pendiente);
        }

        public void Cancelar(BE_Turno_96_PP turno)
        {
            CambiarEstado(turno, BE_Turno_96_PP.EstadoTurno.Cancelado,
                          BE_Turno_96_PP.EstadoTurno.Pendiente, BE_Turno_96_PP.EstadoTurno.Confirmado);
        }

        private void CambiarEstado(BE_Turno_96_PP turno, BE_Turno_96_PP.EstadoTurno nuevo,
                                   params BE_Turno_96_PP.EstadoTurno[] estadosPermitidos)
        {
            if (turno == null)
                throw new Exception("Debe seleccionar un turno.");

            BE_Turno_96_PP actual = dal.ObtenerPorId(turno.IdTurno);
            if (actual == null)
                throw new Exception("El turno no existe.");

            if (!estadosPermitidos.Contains(actual.Estado))
                throw new Exception("El turno no se puede pasar a ese estado.");

            // Solo tiene sentido bloquear por fecha pasada al confirmar (no se confirma
            // un turno que ya pasó). Cancelar un turno de una fecha pasada siempre debe
            // poder hacerse: es la forma de liberar/"limpiar" pendientes viejos que
            // quedaron sin atender.
            if (nuevo == BE_Turno_96_PP.EstadoTurno.Confirmado && actual.Fecha < DateTime.Today)
                throw new Exception("No se puede confirmar un turno de una fecha pasada.");

            dal.CambiarEstado(actual.IdTurno, nuevo);
            turno.Estado = nuevo;

            RecalcularDV(Tabla);
            RegistrarEvento(Modulo,
                $"Turno #{actual.IdTurno}: {actual.Estado} -> {nuevo}",
                nuevo == BE_Turno_96_PP.EstadoTurno.Cancelado ? Evento_96_PP.Criticidad.Medio : Evento_96_PP.Criticidad.Bajo);
        }
    

        private void RecalcularDV(string tabla)
        {
            DataTable registros = DAL_Comun_96_PP.ConsultarTabla(tabla);
            new BLL_DigitoVerificador_96_PP().CalcularDV(tabla, registros);
        }

        private void RegistrarEvento(string modulo, string descripcion, Evento_96_PP.Criticidad criticidad)
        {
            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            string dni = usuario?.Dni ?? "Sistema";

            new BLL_BitacoraEvento_96_PP().RegistrarEvento(
                new Evento_96_PP(dni, DateTime.Now, modulo, descripcion, criticidad));
        }
    }
}
