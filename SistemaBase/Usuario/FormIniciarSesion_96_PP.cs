using BE_96_PP;
using BE_96_PP;
using BLL;
using ClassLibrary2;
using ClassLibrary3;
using Servicio;
using SistemaBase.Administracion;
using SistemaBase.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SistemaBase
{
    public partial class FormIniciarSesion_96_PP : Form, IdiomaObserver_96_PP
    {
        public FormIniciarSesion_96_PP()
        {
            InitializeComponent();
            SessionManager_96_PP.getInstancia().Suscribir(this);
            actualizarIdioma();
        }

        public void actualizarIdioma()
        {
            new BLL_Idioma_96_PP().Traducir(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SessionManager_96_PP sesion = SessionManager_96_PP.getInstancia();
            BLL_Idioma_96_PP mensajes = new BLL_Idioma_96_PP();

            if (sesion.haySesionActiva())
            {
                mensajes.MostrarMensaje("Ya inicio sesion, cierre sesion primero");
                return;
            }

            string nombreUsuario = textBox1.Text.Trim();
            string contraseña = textBox2.Text.Trim();
            if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(contraseña))
            {
                mensajes.MostrarMensaje("Complete todos los campos");
                return;
            }

            BLL_Usuario_96_PP usuarios = new BLL_Usuario_96_PP();
            if (!usuarios.existeUsuarioConEseUsername(nombreUsuario))
            {
                mensajes.MostrarMensaje("No existe usuario con ese nombre ");
                return;
            }

            Usuario_96_PP usuario = usuarios.obtenerUsuarios()
                .FirstOrDefault(item => item.NombreUsuario == nombreUsuario);
            if (usuario == null)
            {
                mensajes.MostrarMensaje("Usuario no existe");
                return;
            }

            if (!usuarios.validarUsuario(nombreUsuario, contraseña))
            {
                RegistrarIntentoFallido(usuario, usuarios, mensajes);
                return;
            }

            sesion.CambiarIdioma(usuario.Idioma ?? new Idioma_96_PP("ES"));

            if (usuario.Bloqueado)
            {
                MostrarMensajeYRestablecerIdioma(
                    mensajes,
                    "El usuario se encuentra bloqueado",
                    sesion);
                return;
            }

            if (!usuario.Activo)
            {
                MostrarMensajeYRestablecerIdioma(
                    mensajes,
                    "El usuario se encuentra inactivo",
                    sesion);
                return;
            }

            var revision = new BLL_DigitoVerificador_96_PP().Revision();
            bool hayInconsistencias = revision.tablaDVVacia || revision.tablasConError.Count > 0;
            bool puedeReparar = TieneAlgunPermiso(
                usuario.Rol,
                Permisos_96_PP.RecalcularDigitosVerificadores,
                Permisos_96_PP.RestaurarBackup);

            if (hayInconsistencias && !puedeReparar)
            {
                MostrarMensajeYRestablecerIdioma(
                    mensajes,
                    "El sistema no se encuentra disponible en estos momentos, contacte al administrador.",
                    sesion);
                return;
            }

            MenuPrincipal_96_PP menu = Application.OpenForms["MenuPrincipal_96_PP"] as MenuPrincipal_96_PP;
            sesion.iniciarSesion(usuario);

            if (hayInconsistencias)
            {
                if (revision.tablaDVVacia && revision.tablasConError.Count == 0)
                    revision.tablasConError.Add("Tabla DigitoVerificador sin registros");

                mensajes.MostrarMensaje("Se detectaron inconsistencias en la base de datos");
                if (menu != null)
                    menu.AplicarEstadoSesion(usuario.Rol);

                FormReparacion_96_PP reparacion = new FormReparacion_96_PP(revision.tablasConError);
                if (menu != null)
                {
                    reparacion.MdiParent = menu;
                    reparacion.WindowState = FormWindowState.Maximized;
                }

                reparacion.Show();
                Close();
                return;
            }

            mensajes.MostrarMensaje("Sesión iniciada, bienvenido " + usuario.NombreUsuario);
            new BLL_BitacoraEvento_96_PP().RegistrarEvento(new Evento_96_PP(
                usuario.Dni,
                DateTime.Now,
                "Usuarios",
                "Inicio de sesión",
                Evento_96_PP.Criticidad.Bajo));

            if (menu != null)
                menu.AplicarEstadoSesion(usuario.Rol);

            if (DebeCambiarContraseña(usuario.Dni))
            {
                mensajes.MostrarMensaje("Debe cambiar su contraseña antes de continuar.");
                new FormCambiarContraseña_96_PP().ShowDialog();
            }

            Close();
        }

        private void MostrarMensajeYRestablecerIdioma(
            BLL_Idioma_96_PP mensajes,
            string mensaje,
            SessionManager_96_PP sesion)
        {
            mensajes.MostrarMensaje(mensaje);
            sesion.CambiarIdioma(new Idioma_96_PP("ES"));
        }

        private void RegistrarIntentoFallido(Usuario_96_PP usuario, BLL_Usuario_96_PP usuarios, BLL_Idioma_96_PP mensajes)
        {
            mensajes.MostrarMensaje("Contraseña incorrecta");
            BLL_BitacoraEvento_96_PP bitacora = new BLL_BitacoraEvento_96_PP();
            bitacora.RegistrarEvento(new Evento_96_PP(
                usuario.Dni,
                DateTime.Now,
                "Usuarios",
                "Intento fallido",
                Evento_96_PP.Criticidad.Alto));

            int intentos = bitacora.obtenerEventos().Count(evento =>
                evento.dni == usuario.Dni &&
                evento.descripcion == "Intento fallido" &&
                evento.fecha >= DateTime.Now.AddMinutes(-5));

            if (intentos >= 3)
            {
                usuarios.bloquearUsuario(usuario.Dni);
                mensajes.MostrarMensaje("Usuario bloqueado por demasiados intentos fallidos");
            }
        }

        private bool DebeCambiarContraseña(string dniUsuario)
        {
            List<Evento_96_PP> eventos = new BLL_BitacoraEvento_96_PP().obtenerEventos();
            Evento_96_PP ultimoCambio = eventos
                .Where(evento => evento.dni == dniUsuario && evento.descripcion == "Cambio de contraseña")
                .OrderByDescending(evento => evento.fecha)
                .FirstOrDefault();

            if (ultimoCambio == null)
                return true;

            return eventos.Any(evento =>
                evento.descripcion.Contains("Desbloqueo de usuario") &&
                evento.dni.Contains(dniUsuario) &&
                evento.fecha > ultimoCambio.fecha);
        }

        private bool TieneAlgunPermiso(Rol_96_PP rol, params string[] permisos)
        {
            return rol != null && rol.TieneAlgunPermiso(permisos);
        }

        private void FormIniciarSesion_Load(object sender, EventArgs e)
        {
        }
    }
}
