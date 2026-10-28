using BE_96_PP;
using ClassLibrary2;
using ClassLibrary3;
using DAL;
using DAL_96_PP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BLL
{
    public class BLL_Asistencia_96_PP
    {
        private const string Tabla = "Asistencia_56PS";
        private const string Modulo = "Asistencia";
        private readonly DAL_Asistencia_96_PP dAL_Asistencia_96_PP = new DAL_Asistencia_96_PP();
        private readonly DAL_Turno_96_PP dalTurno = new DAL_Turno_96_PP();
        
        public int Alta(BE_Asistencia_96_PP asistencia)
        {
            if (asistencia == null)
                throw new Exception("La asistencia no puede ser nula.");

            if (asistencia.IdTurno <= 0)
                throw new Exception("Debe indicar el turno.");

            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            if (usuario == null)
                throw new Exception("No hay un usuario con sesión iniciada.");

            BE_Turno_96_PP turno = dalTurno.ObtenerPorId(asistencia.IdTurno);
            if (turno == null)
                throw new Exception("El turno no existe.");

            if (turno.Estado != BE_Turno_96_PP.EstadoTurno.Confirmado &&
                turno.Estado != BE_Turno_96_PP.EstadoTurno.Pendiente)
                throw new Exception("El turno no está en un estado que permita registrar la llegada.");
            
            if (turno.Fecha.Date != DateTime.Today)
                throw new Exception("No pueden registrarse asistencias de fechas distintas a la actual");
            
            BE_Asistencia_96_PP existente = dAL_Asistencia_96_PP.ObtenerPorTurno(asistencia.IdTurno);
            if (existente != null)
                throw new Exception("Este turno ya tiene la asistencia registrada.");

            asistencia.Fecha = DateTime.Today;
            asistencia.Hora = DateTime.Now.TimeOfDay;
            asistencia.Estado = BE_Asistencia_96_PP.EstadoAsistencia.Presente;
            asistencia.DniRecepcionista = usuario.Dni;

            try
            {
                asistencia.IdAsistencia = dAL_Asistencia_96_PP.Crear(asistencia);
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                throw new Exception("Este turno ya tiene la asistencia registrada.");
            }

            RecalcularDV(Tabla);
            RegistrarEvento(Modulo,
                $"Asistencia #{asistencia.IdAsistencia} - Turno #{asistencia.IdTurno}",
                Evento_96_PP.Criticidad.Bajo);

            return asistencia.IdAsistencia;
        }

        public BE_Asistencia_96_PP ObtenerPorTurno(int idTurno)
        {
            if (idTurno <= 0)
            {
                throw new Exception("Error, id de turno no puede estar vacio");
            }
            return dAL_Asistencia_96_PP.ObtenerPorTurno(idTurno);
        }

        // Turnos de hoy (Pendiente o Confirmado) cuya llegada todavía no se registró.
        public List<BE_Turno_96_PP> ObtenerTurnosPendientes()
        {
            return dalTurno.ObtenerPorFecha(DateTime.Today)
                .Where(t => (t.Estado == BE_Turno_96_PP.EstadoTurno.Confirmado ||
                             t.Estado == BE_Turno_96_PP.EstadoTurno.Pendiente)
                         && dAL_Asistencia_96_PP.ObtenerPorTurno(t.IdTurno) == null)
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
