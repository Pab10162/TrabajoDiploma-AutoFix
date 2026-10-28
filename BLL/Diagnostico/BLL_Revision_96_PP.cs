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
    // RFN2 pasos 3-5: el Jefe de Taller revisa el vehículo, emite el informe y asigna al mecánico.
    public class BLL_Revision_96_PP
    {
        private const string Tabla = "Revision_56PS";
        private const string TablaTurno = "Turno_56PS";
        private const string Modulo = "Revision";
        private const string RolMecanico = "RMEC";

        private readonly DAL_Revision_96_PP dal = new DAL_Revision_96_PP();
        private readonly DAL_Turno_96_PP dalTurno = new DAL_Turno_96_PP();
        private readonly DAL_Asistencia_96_PP dalAsistencia = new DAL_Asistencia_96_PP();

        public int Alta(BE_Revision_96_PP revision)
        {
            if (revision == null)
                throw new Exception("La revisión no puede ser nula.");
            if (revision.IdTurno <= 0)
                throw new Exception("Debe indicar el turno.");
            if (string.IsNullOrWhiteSpace(revision.Observaciones))
                throw new Exception("Debe ingresar las observaciones del informe de revisión.");
            if (string.IsNullOrWhiteSpace(revision.DniMecanico))
                throw new Exception("Debe seleccionar el mecánico que realizará la inspección.");

            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            if (usuario == null)
                throw new Exception("No hay un usuario con sesión iniciada.");

            BE_Turno_96_PP turno = dalTurno.ObtenerPorId(revision.IdTurno);
            if (turno == null)
                throw new Exception("El turno no existe.");
            if (turno.Estado == BE_Turno_96_PP.EstadoTurno.Cancelado || turno.Estado == BE_Turno_96_PP.EstadoTurno.Atendido)
                throw new Exception("El turno no está en un estado que permita registrar la revisión.");

            if (dalAsistencia.ObtenerPorTurno(revision.IdTurno) == null)
                throw new Exception("El cliente todavía no registró su llegada.");
            if (dal.ObtenerPorTurno(revision.IdTurno) != null)
                throw new Exception("Este turno ya tiene la revisión registrada.");

            Usuario_96_PP mecanico = new BLL_Usuario_96_PP().obtenerUsuarioPorDni(revision.DniMecanico);
            if (mecanico == null || !mecanico.Activo || mecanico.Rol == null || mecanico.Rol.Codigo != RolMecanico)
                throw new Exception("El usuario seleccionado no es un mecánico activo.");

            revision.Observaciones = revision.Observaciones.Trim();
            revision.Fecha = DateTime.Today;
            revision.Hora = DateTime.Now.TimeOfDay;
            revision.DniJefe = usuario.Dni;

            try
            {
                revision.IdRevision = dal.Crear(revision);
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                throw new Exception("Este turno ya tiene la revisión registrada.");
            }

            dalTurno.AsignarMecanico(revision.IdTurno, revision.DniMecanico);

            RecalcularDV(Tabla);
            RecalcularDV(TablaTurno);
            RegistrarEvento(Modulo,
                $"Revisión #{revision.IdRevision} - Turno #{revision.IdTurno} - Mecánico {revision.DniMecanico}",
                Evento_96_PP.Criticidad.Bajo);

            return revision.IdRevision;
        }

        public BE_Revision_96_PP ObtenerPorTurno(int idTurno)
        {
            if (idTurno <= 0)
                throw new Exception("Error, id de turno no puede estar vacio");
            return dal.ObtenerPorTurno(idTurno);
        }

        // Turnos con la llegada registrada y todavía sin revisión.
        public List<BE_Turno_96_PP> ObtenerTurnosPendientes()
        {
            return dalTurno.ObtenerTodos()
                .Where(t => t.Estado != BE_Turno_96_PP.EstadoTurno.Cancelado
                         && t.Estado != BE_Turno_96_PP.EstadoTurno.Atendido
                         && dalAsistencia.ObtenerPorTurno(t.IdTurno) != null
                         && dal.ObtenerPorTurno(t.IdTurno) == null)
                .OrderBy(t => t.Fecha).ThenBy(t => t.Horario)
                .ToList();
        }

        // Mecánicos activos y no bloqueados, para el combo de asignación.
        public List<Usuario_96_PP> ObtenerMecanicos()
        {
            return new BLL_Usuario_96_PP().obtenerUsuarios()
                .Where(u => u.Activo && !u.Bloqueado && u.Rol != null && u.Rol.Codigo == RolMecanico)
                .OrderBy(u => u.Apellido).ThenBy(u => u.Nombre)
                .ToList();
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
