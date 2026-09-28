using BE_96_PP;
using BLL;
using ClassLibrary2;
using ClassLibrary3;
using Services_96_PP;
using Servicio;
using System;
using System.Windows.Forms;

namespace SistemaBase.Usuario
{
    public partial class FormCambiarContraseña_96_PP : Form, IdiomaObserver_96_PP
    {
        public FormCambiarContraseña_96_PP()
        {
            InitializeComponent();
            actualizarIdioma();
            button1.Enabled = TienePermiso(Permisos_96_PP.CambiarContrasena);
        }

        public void actualizarIdioma()
        {
            new BLL_Idioma_96_PP().Traducir(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string actual = textBox1.Text.Trim();
            string nueva = textBox2.Text.Trim();
            string confirmacion = textBox3.Text.Trim();
            BLL_Idioma_96_PP mensajes = new BLL_Idioma_96_PP();

            if (string.IsNullOrEmpty(actual) || string.IsNullOrEmpty(nueva) || string.IsNullOrEmpty(confirmacion))
            {
                mensajes.MostrarMensaje("Debe completar todos los campos.");
                return;
            }

            if (nueva != confirmacion)
            {
                mensajes.MostrarMensaje("La nueva contraseña no coincide con la confirmación.");
                return;
            }

            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            if (usuario == null)
            {
                mensajes.MostrarMensaje("No hay sesión activa.");
                return;
            }

            if (usuario.Contraseña != CryptoManager_96_PP.Encriptar(actual))
            {
                mensajes.MostrarMensaje("La contraseña actual es incorrecta.");
                return;
            }

            if (nueva == actual)
            {
                mensajes.MostrarMensaje("La contraseña nueva no puede ser igual a la actual");
                return;
            }

            string nuevaHash = CryptoManager_96_PP.Encriptar(nueva);
            try
            {
                new BLL_Usuario_96_PP().cambiarContraseña(usuario.Dni, nuevaHash);
                usuario.Contraseña = nuevaHash;
                mensajes.MostrarMensaje("La contraseña se cambió correctamente.");

                new BLL_BitacoraEvento_96_PP().RegistrarEvento(new Evento_96_PP(
                    usuario.Dni,
                    DateTime.Now,
                    "Usuarios",
                    "Cambio de contraseña",
                    Evento_96_PP.Criticidad.Alto));

                Close();
            }
            catch (Exception ex)
            {
                mensajes.MostrarMensaje("Error: " + ex.Message);
            }
        }

        private void FormCambiarContraseña_96_PP_Load(object sender, EventArgs e)
        {
        }

        private bool TienePermiso(string permiso)
        {
            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            return usuario?.Rol != null && usuario.Rol.TienePermiso(permiso);
        }
    }
}
