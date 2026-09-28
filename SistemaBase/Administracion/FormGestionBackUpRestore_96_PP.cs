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
    public partial class FormGestionBackUpRestore_96_PP : Form, IdiomaObserver_96_PP
    {
        public void actualizarIdioma()
        {
            var traductor = new BLL_Idioma_96_PP();
            traductor.Traducir(this);
        }

        public FormGestionBackUpRestore_96_PP()
        {
            InitializeComponent();

            SessionManager_96_PP.getInstancia().Suscribir(this);
            actualizarIdioma();
            button1.Enabled = TieneAlgunPermiso(Permisos_96_PP.RealizarBackup, Permisos_96_PP.RestaurarBackup);
        }

        private void FormGestionBackUpRestore_96_PP_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                new BLL_Idioma_96_PP().MostrarMensaje("Selecciona Backup o Restore antes de buscar.");
                return;
            }

            if (comboBox1.SelectedItem.ToString() == "Backup")
            {
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        textBox1.Text = folderDialog.SelectedPath;
                    }
                }
            }
            else
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Archivos de respaldo (*.bak)|*.bak|Todos los archivos (*.*)|*.*";
                    openFileDialog.Title = "Seleccionar archivo de respaldo";
                    openFileDialog.CheckFileExists = true;
                    openFileDialog.CheckPathExists = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        textBox1.Text = openFileDialog.FileName;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BLL_BackUpRestore_96_PP bll = new BLL_BackUpRestore_96_PP();

            if (comboBox1.SelectedItem == null)
            {
                new BLL_Idioma_96_PP().MostrarMensaje("Selecciona un item");
                return;
            }

            if (comboBox1.SelectedItem.ToString() == "Backup")
            {
                if (!TienePermiso(Permisos_96_PP.RealizarBackup))
                {
                    new BLL_Idioma_96_PP().MostrarMensaje("No tiene permiso para realizar backup.");
                    return;
                }

                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    try
                    {
                        bll.RealizarBackup(textBox1.Text);
                        new BLL_Idioma_96_PP().MostrarMensaje("Backup realizado con éxito.");
                        textBox1.Text = "";

                        string a = SessionManager_96_PP.getInstancia().getUsuarioActivo().Dni;

                        Evento_96_PP ee = new Evento_96_PP(a, DateTime.Now, "Base de datos", "Realizacion de backup", Evento_96_PP.Criticidad.Alto);
                        new BLL_BitacoraEvento_96_PP().RegistrarEvento(ee);
                    }
                    catch (Exception ex)
                    {
                        new BLL_Idioma_96_PP().MostrarMensaje($"Error al realizar el backup: {ex.Message}");
                    }
                }
                else
                {
                    new BLL_Idioma_96_PP().MostrarMensaje("Seleccione una ubicación para el backup.");
                }
            }
            else
            {
                if (!TienePermiso(Permisos_96_PP.RestaurarBackup))
                {
                    new BLL_Idioma_96_PP().MostrarMensaje("No tiene permiso para realizar restore.");
                    return;
                }

                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    try
                    {
                        string dniUsuario = SessionManager_96_PP.getInstancia().getUsuarioActivo().Dni;
                        bll.RealizarRestore(textBox1.Text);
                        new BLL_Idioma_96_PP().MostrarMensaje("Restauración realizada con éxito. El sistema se cerrará para volver a iniciar con la base restaurada.");

                        try
                        {
                            Evento_96_PP ee = new Evento_96_PP(dniUsuario, DateTime.Now, "Base de datos", "Realizacion de restore", Evento_96_PP.Criticidad.Alto);
                            new BLL_BitacoraEvento_96_PP().RegistrarEvento(ee);
                        }
                        catch
                        {
                        }

                        textBox1.Text = "";
                        SalirDelSistemaDespuesDeRestore();
                    }
                    catch (Exception ex)
                    {
                        new BLL_Idioma_96_PP().MostrarMensaje($"Error al restaurar la base de datos: {ex.Message}");
                    }
                }
                else
                {
                    new BLL_Idioma_96_PP().MostrarMensaje("Seleccione un archivo de restore.");
                }
            }
        }

        private bool TienePermiso(string permiso)
        {
            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            return usuario?.Rol != null && usuario.Rol.TienePermiso(permiso);
        }

        private bool TieneAlgunPermiso(params string[] permisos)
        {
            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            return usuario?.Rol != null && usuario.Rol.TieneAlgunPermiso(permisos);
        }

        private void SalirDelSistemaDespuesDeRestore()
        {
            SessionManager_96_PP.getInstancia().SolicitarCierreSistema();
        }
    }
}
