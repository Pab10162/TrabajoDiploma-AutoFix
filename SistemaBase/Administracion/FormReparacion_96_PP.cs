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

namespace SistemaBase.Administracion
{
    public partial class FormReparacion_96_PP : Form, IdiomaObserver_96_PP
    {
        BLL_BackUpRestore_96_PP bllrespaldos = new BLL_BackUpRestore_96_PP();
        BLL_Usuario_96_PP bllusuario = new BLL_Usuario_96_PP();
        BLL_DigitoVerificador_96_PP blldv = new BLL_DigitoVerificador_96_PP();
        private List<string> Errores;

        public FormReparacion_96_PP(List<string> errores)
        {
            Errores = errores;

            InitializeComponent();



            SessionManager_96_PP.getInstancia().Suscribir(this);
            actualizarIdioma();
            CargarErrores();
            btnrecalcular.Enabled = TienePermiso(Permisos_96_PP.RecalcularDigitosVerificadores);
            btnrestore.Enabled = TienePermiso(Permisos_96_PP.RestaurarBackup);
        }

        public void actualizarIdioma()
        {
            var traductor = new BLL_Idioma_96_PP();
            traductor.Traducir(this);
        }

        private void FormReparacion_96_PP_Load(object sender, EventArgs e)
        {

        }

        private void btnrecalcular_Click(object sender, EventArgs e)
        {
            try
            {
                blldv.RecalcularDV();
                var revision = blldv.Revision();
                Errores = revision.tablasConError;

                if (revision.tablaDVVacia && Errores.Count == 0)
                    Errores.Add("Tabla DigitoVerificador sin registros");

                CargarErrores();

                if (revision.tablaDVVacia || Errores.Count > 0)
                {
                    new BLL_Idioma_96_PP().MostrarMensaje("Se recalcularon los DV, pero todavia se detectan inconsistencias.");
                    return;
                }

                new BLL_Idioma_96_PP().MostrarMensaje("Se recalcularon los DV correctamente. Debe iniciar sesión nuevamente.");
                CerrarSesionYVolverAlMenu();
            }
            catch (Exception ex) { new BLL_Idioma_96_PP().MostrarMensaje(ex.Message); }
        }

        private void btnrestore_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "Archivos de respaldo (*.bak)|*.bak";
                openFile.Title = "Seleccionar archivo de respaldo";

                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    string ruta = openFile.FileName;

                    new BLL_BackUpRestore_96_PP().RealizarRestore(ruta);

                    new BLL_Idioma_96_PP().MostrarMensaje("Restauración realizada correctamente. El sistema se cerrará para volver a iniciar con la base restaurada.");

                    string aa = SessionManager_96_PP.getInstancia().getUsuarioActivo().Dni;

                    Evento_96_PP ee = new Evento_96_PP(aa, DateTime.Now, "Reparaciones", "Restauracion de respaldo realizada", Evento_96_PP.Criticidad.MuyAlto);
                    new BLL_BitacoraEvento_96_PP().RegistrarEvento(ee);

                    SessionManager_96_PP.getInstancia().SolicitarCierreSistema();

                }
                else
                {
                    new BLL_Idioma_96_PP().MostrarMensaje("Debe seleccionar un archivo de respaldo");
                }
            }
            catch (Exception ex)
            {
                new BLL_Idioma_96_PP().MostrarMensaje(ex.Message);
            }
        }

        private void btnsalir_Click(object sender, EventArgs e)
        {
            CerrarSesionYVolverAlMenu();
        }

        private bool TienePermiso(string permiso)
        {
            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            return usuario?.Rol != null && usuario.Rol.TienePermiso(permiso);
        }

        private void CargarErrores()
        {
            listBox1.Items.Clear();

            if (Errores == null || Errores.Count == 0)
            {
                listBox1.Items.Add("No se detectaron tablas con error.");
                return;
            }

            foreach (string error in Errores)
            {
                listBox1.Items.Add(error);
            }
        }

        private void CerrarSesionYVolverAlMenu()
        {
            SessionManager_96_PP.getInstancia().cerrarSesion();

            MenuPrincipal_96_PP menu = this.MdiParent as MenuPrincipal_96_PP;
            if (menu == null)
                menu = Application.OpenForms["MenuPrincipal_96_PP"] as MenuPrincipal_96_PP;

            if (menu != null)
                menu.AplicarEstadoSinSesion();

            this.Close();
        }
    }
}
