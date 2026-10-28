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
    // RFN2 pasos 6-7: el Mecánico registra el diagnóstico técnico y la solución propuesta.
    public class BLL_Diagnostico_96_PP
    {
        private const string Tabla = "Diagnostico_56PS";
        private const string Modulo = "Diagnostico";

        private readonly DAL_Diagnostico_96_PP dal = new DAL_Diagnostico_96_PP();
        private readonly DAL_Revision_96_PP dalRevision = new DAL_Revision_96_PP();
        private readonly DAL_Turno_96_PP dalTurno = new DAL_Turno_96_PP();
        private readonly DAL_Reparacion_96_PP dalReparacion = new DAL_Reparacion_96_PP();

        public int Alta(BE_Diagnostico_96_PP diagnostico)
        {
            if (diagnostico == null)
                throw new Exception("El diagnóstico no puede ser nulo.");
            if (diagnostico.IdTurno <= 0)
                throw new Exception("Debe indicar el turno.");
            if (string.IsNullOrWhiteSpace(diagnostico.SistemaAfectado))
                throw new Exception("Debe indicar el sistema afectado.");
            if (string.IsNullOrWhiteSpace(diagnostico.DescripcionFallas))
                throw new Exception("Debe describir las fallas encontradas.");
            if (!Enum.IsDefined(typeof(BE_Diagnostico_96_PP.NivelUrgencia), diagnostico.Urgencia))
                throw new Exception("Debe seleccionar el nivel de urgencia.");

            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            if (usuario == null)
                throw new Exception("No hay un usuario con sesión iniciada.");

            BE_Turno_96_PP turno = dalTurno.ObtenerPorId(diagnostico.IdTurno);
            if (turno == null)
                throw new Exception("El turno no existe.");
            if (turno.Estado == BE_Turno_96_PP.EstadoTurno.Cancelado || turno.Estado == BE_Turno_96_PP.EstadoTurno.Atendido)
                throw new Exception("El turno no está en un estado que permita registrar el diagnóstico.");

            BE_Revision_96_PP revision = dalRevision.ObtenerPorTurno(diagnostico.IdTurno);
            if (revision == null)
                throw new Exception("El turno todavía no tiene la revisión del Jefe de Taller.");
            if (revision.DniMecanico != usuario.Dni)
                throw new Exception("Solo el mecánico asignado a este turno puede registrar el diagnóstico.");
            if (dal.ObtenerPorTurno(diagnostico.IdTurno) != null)
                throw new Exception("Este turno ya tiene el diagnóstico registrado.");

            diagnostico.SistemaAfectado = diagnostico.SistemaAfectado.Trim();
            diagnostico.DescripcionFallas = diagnostico.DescripcionFallas.Trim();
            diagnostico.RepuestosNecesarios = string.IsNullOrWhiteSpace(diagnostico.RepuestosNecesarios)
                ? null : diagnostico.RepuestosNecesarios.Trim();
            diagnostico.Fecha = DateTime.Today;
            diagnostico.Hora = DateTime.Now.TimeOfDay;
            diagnostico.DniMecanico = usuario.Dni;

            try
            {
                diagnostico.IdDiagnostico = dal.Crear(diagnostico);
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                throw new Exception("Este turno ya tiene el diagnóstico registrado.");
            }

            RecalcularDV(Tabla);
            RegistrarEvento(Modulo,
                $"Diagnóstico #{diagnostico.IdDiagnostico} - Turno #{diagnostico.IdTurno} - Urgencia {diagnostico.Urgencia}",
                diagnostico.Urgencia == BE_Diagnostico_96_PP.NivelUrgencia.Critica
                    ? Evento_96_PP.Criticidad.Alto : Evento_96_PP.Criticidad.Bajo);

            return diagnostico.IdDiagnostico;
        }

        public void RegistrarSolucion(int idDiagnostico, BE_Diagnostico_96_PP.TipoDeSolucion tipo, string descripcion)
        {
            if (idDiagnostico <= 0)
                throw new Exception("Debe indicar el diagnóstico.");
            if (!Enum.IsDefined(typeof(BE_Diagnostico_96_PP.TipoDeSolucion), tipo))
                throw new Exception("Debe seleccionar el tipo de solución.");
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new Exception("Debe describir la solución propuesta.");

            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            if (usuario == null)
                throw new Exception("No hay un usuario con sesión iniciada.");

            BE_Diagnostico_96_PP diagnostico = dal.ObtenerPorId(idDiagnostico);
            if (diagnostico == null)
                throw new Exception("El diagnóstico no existe.");
            if (diagnostico.DniMecanico != usuario.Dni)
                throw new Exception("Solo el mecánico que registró el diagnóstico puede cargar la solución.");
            if (diagnostico.TipoSolucion != null)
                throw new Exception("Este diagnóstico ya tiene una solución registrada.");

            dal.RegistrarSolucion(idDiagnostico, tipo, descripcion.Trim());

            RecalcularDV(Tabla);
            RegistrarEvento(Modulo,
                $"Solución {tipo} - Diagnóstico #{idDiagnostico}",
                Evento_96_PP.Criticidad.Bajo);
        }

        public BE_Diagnostico_96_PP ObtenerPorTurno(int idTurno)
        {
            if (idTurno <= 0)
                throw new Exception("Error, id de turno no puede estar vacio");
            return dal.ObtenerPorTurno(idTurno);
        }

        // Turnos asignados al mecánico en sesión, con revisión y todavía sin diagnóstico.
        public List<BE_Turno_96_PP> ObtenerTurnosPendientes()
        {
            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            if (usuario == null)
                return new List<BE_Turno_96_PP>();

            return dalTurno.ObtenerTodos()
                .Where(t => t.Estado != BE_Turno_96_PP.EstadoTurno.Cancelado
                         && t.Estado != BE_Turno_96_PP.EstadoTurno.Atendido
                         && t.DniMecanico == usuario.Dni
                         && dalRevision.ObtenerPorTurno(t.IdTurno) != null
                         && dal.ObtenerPorTurno(t.IdTurno) == null)
                .OrderBy(t => t.Fecha).ThenBy(t => t.Horario)
                .ToList();
        }

        // Turnos del mecánico con diagnóstico cargado y reparación todavía no registrada:
        // en ellos se puede cargar la solución y los repuestos faltantes.
        public List<BE_Turno_96_PP> ObtenerTurnosConDiagnostico()
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

                BE_Diagnostico_96_PP d = dal.ObtenerPorTurno(t.IdTurno);
                if (d != null && dalReparacion.ObtenerPorDiagnostico(d.IdDiagnostico) == null)
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
