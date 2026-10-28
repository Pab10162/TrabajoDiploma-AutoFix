using BE_96_PP;
using ClassLibrary2;
using ClassLibrary3;
using DAL_96_PP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BLL
{
    // RFN2 pasos 11-12: la Recepción cobra el servicio y el turno queda Atendido.
    public class BLL_Cobro_96_PP
    {
        private const string Tabla = "Cobro_56PS";
        private const string TablaTurno = "Turno_56PS";
        private const string Modulo = "Cobro";

        private readonly DAL_Cobro_96_PP dal = new DAL_Cobro_96_PP();
        private readonly DAL_Turno_96_PP dalTurno = new DAL_Turno_96_PP();
        private readonly DAL_Diagnostico_96_PP dalDiagnostico = new DAL_Diagnostico_96_PP();
        private readonly DAL_Reparacion_96_PP dalReparacion = new DAL_Reparacion_96_PP();

        public int Alta(BE_Cobro_96_PP cobro)
        {
            if (cobro == null)
                throw new Exception("El cobro no puede ser nulo.");
            if (cobro.IdTurno <= 0)
                throw new Exception("Debe indicar el turno.");
            if (cobro.MontoTotal <= 0)
                throw new Exception("El monto total debe ser mayor a cero.");
            if (!Enum.IsDefined(typeof(BE_Cobro_96_PP.MedioPago), cobro.MetodoPago))
                throw new Exception("Debe seleccionar el medio de pago.");

            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            if (usuario == null)
                throw new Exception("No hay un usuario con sesión iniciada.");

            BE_Turno_96_PP turno = dalTurno.ObtenerPorId(cobro.IdTurno);
            if (turno == null)
                throw new Exception("El turno no existe.");
            if (turno.Estado == BE_Turno_96_PP.EstadoTurno.Cancelado || turno.Estado == BE_Turno_96_PP.EstadoTurno.Atendido)
                throw new Exception("El turno no está en un estado que permita registrar el cobro.");

            if (!ReparacionFinalizada(cobro.IdTurno))
                throw new Exception("La reparación de este turno todavía no fue finalizada.");
            if (dal.ObtenerPorTurno(cobro.IdTurno) != null)
                throw new Exception("Este turno ya tiene el cobro registrado.");

            cobro.MontoTotal = Math.Round(cobro.MontoTotal, 2);
            cobro.Fecha = DateTime.Today;
            cobro.Hora = DateTime.Now.TimeOfDay;
            cobro.DniCliente = turno.DniCliente;
            cobro.DniRecepcionista = usuario.Dni;

            try
            {
                cobro.IdCobro = dal.Crear(cobro);
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                throw new Exception("Este turno ya tiene el cobro registrado.");
            }

            // Paso 12: con el cobro registrado el turno queda Atendido.
            dalTurno.CambiarEstado(cobro.IdTurno, BE_Turno_96_PP.EstadoTurno.Atendido);

            RecalcularDV(Tabla);
            RecalcularDV(TablaTurno);
            RegistrarEvento(Modulo,
                $"Cobro #{cobro.IdCobro} - Turno #{cobro.IdTurno} - {cobro.MontoTotal:0.00} ({cobro.MetodoPago})",
                Evento_96_PP.Criticidad.Medio);

            return cobro.IdCobro;
        }

        public BE_Cobro_96_PP ObtenerPorTurno(int idTurno)
        {
            if (idTurno <= 0)
                throw new Exception("Error, id de turno no puede estar vacio");
            return dal.ObtenerPorTurno(idTurno);
        }

        // Presupuesto final de la reparación: se sugiere como monto del cobro.
        public decimal? ObtenerPresupuestoFinal(int idTurno)
        {
            BE_Diagnostico_96_PP d = dalDiagnostico.ObtenerPorTurno(idTurno);
            if (d == null) return null;
            BE_Reparacion_96_PP r = dalReparacion.ObtenerPorDiagnostico(d.IdDiagnostico);
            return r?.PresupuestoFinal;
        }

        // Turnos con la reparación finalizada y todavía sin cobro.
        public List<BE_Turno_96_PP> ObtenerTurnosPendientes()
        {
            return dalTurno.ObtenerTodos()
                .Where(t => t.Estado != BE_Turno_96_PP.EstadoTurno.Cancelado
                         && t.Estado != BE_Turno_96_PP.EstadoTurno.Atendido
                         && ReparacionFinalizada(t.IdTurno)
                         && dal.ObtenerPorTurno(t.IdTurno) == null)
                .OrderBy(t => t.Fecha).ThenBy(t => t.Horario)
                .ToList();
        }

        private bool ReparacionFinalizada(int idTurno)
        {
            BE_Diagnostico_96_PP d = dalDiagnostico.ObtenerPorTurno(idTurno);
            if (d == null) return false;
            BE_Reparacion_96_PP r = dalReparacion.ObtenerPorDiagnostico(d.IdDiagnostico);
            return r != null && r.Finalizada;
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
