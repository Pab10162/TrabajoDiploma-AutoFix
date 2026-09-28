using BE_56_PS;
using BE_625NS;
using DAL_625NS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace BLL
{
    public class BLL_Turno_56PS : BLL_Taller_Comun_56PS
    {
        private const string Tabla = "Turno_56PS";
        private const string Modulo = "Turnos";

        public const int HoraPrimerTurno = 8;
        public const int HoraUltimoTurno = 21;

        private readonly DAL_Turno_56PS dal = new DAL_Turno_56PS();
        private readonly BLL_Cliente_56PS bllCliente = new BLL_Cliente_56PS();
        private readonly BLL_Vehiculo_56PS bllVehiculo = new BLL_Vehiculo_56PS();

        public List<BE_Turno_56PS> ObtenerTodos()
        {
            return dal.ObtenerTodos();
        }

        public List<BE_Turno_56PS> ObtenerPorFecha(DateTime fecha)
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

        public BE_Turno_56PS AsignarTurno(string dniCliente, string patente, DateTime fecha, TimeSpan horario)
        {
            BE_Cliente_56PS cliente = bllCliente.BuscarPorDni(dniCliente);
            if (cliente == null)
                throw new Exception("El cliente no existe.");
            if (!cliente.Activo)
                throw new Exception("El cliente está dado de baja.");

            BE_Vehiculo_56PS vehiculo = bllVehiculo.BuscarPorPatente(patente);
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

            if (dal.VehiculoTieneTurnoVigente(vehiculo.Patente, fecha))
                throw new Exception("El vehículo ya tiene un turno vigente para esa fecha.");

            BE_Turno_56PS turno = new BE_Turno_56PS(fecha, horario, cliente, vehiculo);

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
                Evento_56PS.Criticidad.Bajo);

            return turno;
        }

        public void SerializarTurno(BE_Turno_56PS turno, string rutaArchivo)
        {
            if (turno == null)
                throw new Exception("Debe seleccionar un turno para exportar.");

            XmlSerializer serializador = new XmlSerializer(typeof(BE_Turno_56PS));
            using (StreamWriter escritor = new StreamWriter(rutaArchivo))
            {
                serializador.Serialize(escritor, turno);
            }
        }

        public BE_Turno_56PS DeserializarTurno(string rutaArchivo)
        {
            XmlSerializer serializador = new XmlSerializer(typeof(BE_Turno_56PS));
            using (StreamReader lector = new StreamReader(rutaArchivo))
            {
                return (BE_Turno_56PS)serializador.Deserialize(lector);
            }
        }

        public void Confirmar(BE_Turno_56PS turno)
        {
            CambiarEstado(turno, BE_Turno_56PS.EstadoTurno.Confirmado,
                          BE_Turno_56PS.EstadoTurno.Pendiente);
        }

        public void Cancelar(BE_Turno_56PS turno)
        {
            CambiarEstado(turno, BE_Turno_56PS.EstadoTurno.Cancelado,
                          BE_Turno_56PS.EstadoTurno.Pendiente, BE_Turno_56PS.EstadoTurno.Confirmado);
        }

        private void CambiarEstado(BE_Turno_56PS turno, BE_Turno_56PS.EstadoTurno nuevo,
                                   params BE_Turno_56PS.EstadoTurno[] estadosPermitidos)
        {
            if (turno == null)
                throw new Exception("Debe seleccionar un turno.");

            BE_Turno_56PS actual = dal.ObtenerPorId(turno.IdTurno);
            if (actual == null)
                throw new Exception("El turno no existe.");

            if (!estadosPermitidos.Contains(actual.Estado))
                throw new Exception("El turno no se puede pasar a ese estado.");

            if (actual.Fecha < DateTime.Today)
                throw new Exception("No se puede modificar un turno de una fecha pasada.");

            dal.CambiarEstado(actual.IdTurno, nuevo);
            turno.Estado = nuevo;

            RecalcularDV(Tabla);
            RegistrarEvento(Modulo,
                $"Turno #{actual.IdTurno}: {actual.Estado} -> {nuevo}",
                nuevo == BE_Turno_56PS.EstadoTurno.Cancelado ? Evento_56PS.Criticidad.Medio : Evento_56PS.Criticidad.Bajo);
        }
    }
}
