using BE_96_PP;
using BLL;
using ClassLibrary2;
using ClassLibrary3;
using Servicio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaBase.Usuario
{
    public partial class FormCambiarIdioma_96_PP : Form, IdiomaObserver_96_PP
    {
        public FormCambiarIdioma_96_PP()
        {
            InitializeComponent();
            SessionManager_96_PP.getInstancia().Suscribir(this);
            CargarIdiomas();
            button1.Enabled = TienePermiso(Permisos_96_PP.CambiarIdioma);
        }

        public void actualizarIdioma()
        {
            var traductor = new BLL_Idioma_96_PP();
            traductor.Traducir(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Idioma_96_PP idiomaSeleccionado = comboBox1.SelectedItem as Idioma_96_PP;
            if (idiomaSeleccionado == null)
            {
                new BLL_Idioma_96_PP().MostrarMensaje("Selecciona un idioma");
                return;
            }

            SessionManager_96_PP.getInstancia().CambiarIdioma(idiomaSeleccionado);
            string a = SessionManager_96_PP.getInstancia().getUsuarioActivo().Dni;
            Evento_96_PP eee = new Evento_96_PP(a, DateTime.Now, "Usuarios", "Cambio de idioma", Evento_96_PP.Criticidad.Bajo);
            new BLL_BitacoraEvento_96_PP().RegistrarEvento(eee);
        }

        private void CargarIdiomas()
        {
            List<Idioma_96_PP> idiomas = new BLL_Idioma_96_PP().ObtenerIdiomas();
            comboBox1.DataSource = idiomas;
            comboBox1.DisplayMember = "nombre";
            comboBox1.ValueMember = "tipo";

            string tipoActual = SessionManager_96_PP.getInstancia().idiomaActual?.tipo;
            int indice = idiomas.FindIndex(idioma => idioma.tipo == tipoActual);
            comboBox1.SelectedIndex = indice >= 0 ? indice : 0;
        }

        private void FormCambiarIdioma_96_PP_Load(object sender, EventArgs e)
        {

        }

        private bool TienePermiso(string permiso)
        {
            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            return usuario?.Rol != null && usuario.Rol.TienePermiso(permiso);
        }
    }
}
