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
    // RFN2 pasos 9-10: el Mecánico registra la reparación y luego el informe de finalización.
    public class BLL_Reparacion_96_PP
    {
        private const string Tabla = "Reparacion_56PS";
        private const string Modulo = "Reparacion";

        private readonly DAL_Reparacion_96_PP dal = new DAL_Reparacion_96_PP();
        private readonly DAL_Diagnostico_96_PP dalDiagnostico = new DAL_Diagnostico_96_PP();
        private readonly DAL_RepuestoFaltante_96_PP dalFaltante = new DAL_RepuestoFaltante_96_PP();
        private readonly DAL_Turno_96_PP dalTurno = new DAL_Turno_96_PP();

        public int Alta(BE_Reparacion_96_PP reparacion)
        {
            if (reparacion == null)
                throw new Exception("La reparación no puede ser nula.");
            if (reparacion.IdDiagnostico <= 0)
                throw new Exception("Debe indicar el diagnóstico.");
            if (string.IsNullOrWhiteSpace(reparacion.TareasRealizadas))
                throw new Exception("Debe describir las tareas realizadas.");

            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            if (usuario == null)
                throw new Exception("No hay un usuario con sesión iniciada.");

            BE_Diagnostico_96_PP diagnostico = dalDiagnostico.ObtenerPorId(reparacion.IdDiagnostico);
            if (diagnostico == null)
                throw new Exception("El diagnóstico no existe.");
            if (diagnostico.DniMecanico != usuario.Dni)
                throw new Exception("Solo el mecánico que registró el diagnóstico puede registrar la reparación.");
            if (diagnostico.TipoSolucion == null)
                throw new Exception("El diagnóstico todavía no tiene una solución registrada.");

            BE_Turno_96_PP turno = dalTurno.ObtenerPorId(diagnostico.IdTurno);
            if (turno == null || turno.Estado == BE_Turno_96_PP.EstadoTurno.Cancelado || turno.Estado == BE_Turno_96_PP.EstadoTurno.Atendido)
                throw new Exception("El turno no está en un estado que permita registrar la reparación.");

            if (dalFaltante.ObtenerPorDiagnostico(reparacion.IdDiagnostico)
                    .Any(f => f.Estado == BE_RepuestoFaltante_96_PP.EstadoFaltante.PendienteCompra))
                throw new Exception("Hay repuestos pendientes de compra. No se puede reparar hasta recibirlos.");

            if (dal.ObtenerPorDiagnostico(reparacion.IdDiagnostico) != null)
                throw new Exception("Este diagnóstico ya tiene la reparación registrada.");

            reparacion.TareasRealizadas = reparacion.TareasRealizadas.Trim();
            reparacion.RepuestosUtilizados = string.IsNullOrWhiteSpace(reparacion.RepuestosUtilizados)
                ? null : reparacion.RepuestosUtilizados.Trim();
            reparacion.Fecha = DateTime.Today;
            reparacion.Hora = DateTime.Now.TimeOfDay;
            reparacion.DniMecanico = usuario.Dni;

            try
            {
                reparacion.IdReparacion = dal.Crear(reparacion);
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                throw new Exception("Este diagnóstico ya tiene la reparación registrada.");
            }

            RecalcularDV(Tabla);
            RegistrarEvento(Modulo,
                $"Reparación #{reparacion.IdReparacion} - Diagnóstico #{reparacion.IdDiagnostico}",
                Evento_96_PP.Criticidad.Bajo);

            return reparacion.IdReparacion;
        }

        // Paso 10: informe de finalización con el presupuesto final.
        public void Finalizar(int idDiagnostico, decimal presupuestoFinal)
        {
            if (idDiagnostico <= 0)
                throw new Exception("Debe indicar el diagnóstico.");
            if (presupuestoFinal < 0)
                throw new Exception("El presupuesto final no puede ser negativo.");

            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            if (usuario == null)
                throw new Exception("No hay un usuario con sesión iniciada.");

            BE_Reparacion_96_PP reparacion = dal.ObtenerPorDiagnostico(idDiagnostico);
            if (reparacion == null)
                throw new Exception("Este diagnóstico todavía no tiene una reparación registrada.");
            if (reparacion.DniMecanico != usuario.Dni)
                throw new Exception("Solo el mecánico que registró la reparación puede finalizarla.");
            if (reparacion.Finalizada)
                throw new Exception("La reparación ya fue finalizada.");

            dal.RegistrarFinalizacion(reparacion.IdReparacion, presupuestoFinal, DateTime.Now);

            RecalcularDV(Tabla);
            RegistrarEvento(Modulo,
                $"Finalización reparación #{reparacion.IdReparacion} - Presupuesto {presupuestoFinal:0.00}",
                Evento_96_PP.Criticidad.Medio);
        }

        public BE_Reparacion_96_PP ObtenerPorDiagnostico(int idDiagnostico)
        {
            if (idDiagnostico <= 0)
                throw new Exception("Debe indicar el diagnóstico.");
            return dal.ObtenerPorDiagnostico(idDiagnostico);
        }

        // Turnos del mecánico en sesión con solución cargada y reparación todavía sin finalizar.
        public List<BE_Turno_96_PP> ObtenerTurnosPendientes()
        {
            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            if (usuario == null)
                return new List<BE_Turno_96_PP>();

            var resultado = new List<BE_Turno_96_PP>();
            foreach (BE_Turno_96_PP t in dalTurno.ObtenerTodos())
            {
                if (t.Estado == BE_Turno_96_PP.EstadoTurno.Cancelado || t.Estado == BE_Turno_96_PP.EstadoTurno.Atendido)
                    continue;
                if (t.DniMecanico != usuario.Dni)
                    continue;

                BE_Diagnostico_96_PP d = dalDiagnostico.ObtenerPorTurno(t.IdTurno);
                if (d == null || d.TipoSolucion == null)
                    continue;

                BE_Reparacion_96_PP r = dal.ObtenerPorDiagnostico(d.IdDiagnostico);
                if (r == null || !r.Finalizada)
                    resultado.Add(t);
            }
            return resultado.OrderBy(t => t.Fecha).ThenBy(t => t.Horario).ToList();
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
