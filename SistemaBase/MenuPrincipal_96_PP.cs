using BE_96_PP;
using BLL;
using ClassLibrary2;
using ClassLibrary3;
using GUI_96_PP.Administracion;
using SistemaBase.Administracion;
using SistemaBase.Usuario;
using SistemaBase.Maestros;
using SistemaBase.Turnos;
using SistemaBase.Diagnostico;
using SistemaBase.Reportes;
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
using System.IO;

namespace SistemaBase
{
    public partial class MenuPrincipal_96_PP : Form, IdiomaObserver_96_PP
    {
        private bool cierreSistemaEnCurso;

        public MenuPrincipal_96_PP()
        {
            InitializeComponent();
            Tema_96_PP.Aplicar(this);
            SessionManager_96_PP.getInstancia().Suscribir(this);
            SessionManager_96_PP.getInstancia().CierreSistemaSolicitado += CierreSistemaSolicitado;
            actualizarIdioma();
            AplicarEstadoSinSesion();
        }

        public void actualizarIdioma()
        {
            var traductor = new BLL_Idioma_96_PP();
            traductor.Traducir(this);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            SessionManager_96_PP.getInstancia().Desuscribir(this);
            SessionManager_96_PP.getInstancia().CierreSistemaSolicitado -= CierreSistemaSolicitado;
            base.OnFormClosed(e);
        }

        private void MenuPrincipal_96_PP_Load(object sender, EventArgs e)
        {
            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "instancia.txt");
            string instancia = "";

            try
            {
                if (File.Exists(ruta))
                {
                    instancia = File.ReadAllText(ruta).Trim();
                    BLL_BackUpRestore_96_PP.Instalador(instancia);
                }
            }
            catch (Exception ex)
            {
                string instanciaMensaje = string.IsNullOrWhiteSpace(instancia) ? "No configurada" : instancia;
                new BLL_Idioma_96_PP().MostrarMensaje("No se pudo inicializar la base de datos. Instancia: " + instanciaMensaje + ". Verifique que SQL Server este iniciado y que el nombre sea correcto. Detalle: " + ex.Message,
                    "Error de instalacion",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

            }
        }

        private void AbrirFormulario(Form form)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == form.GetType())
                {
                    f.Close();
                    break;
                }
            }

            form.MdiParent = this;
            form.WindowState = FormWindowState.Maximized;

            form.Show();
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FormABMUsuario_96_PP());

        }

        private void auditoriaDeEventosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FormBitacoraEventos_96_PP());

        }

        private void iniciarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FormIniciarSesion_96_PP());

        }

        public ToolStripMenuItem MenuAdministracion => usuariosToolStripMenuItem;
        public ToolStripMenuItem MenuCambiarContraseña => cambiarContraseñaToolStripMenuItem;

        public ToolStripMenuItem MenuRoles => rolesToolStripMenuItem;

        public ToolStripMenuItem MenuFamilias => familiasToolStripMenuItem;

        public ToolStripMenuItem MenuGestionRespaldo => gestionDeRespaldoToolStripMenuItem;

        public ToolStripMenuItem MenuAuditoria => auditoriaDeEventosToolStripMenuItem;

        public ToolStripMenuItem MenuCambiarIdioma => cambiarIdiomaToolStripMenuItem;

        public ToolStripMenuItem MenuIniciarSesion => iniciarSesionToolStripMenuItem;

        public ToolStripMenuItem MenuCerrarSesion => cerrarSesionToolStripMenuItem;

        public ToolStripMenuItem menuAdmin => administracionToolStripMenuItem1;

        public void AplicarEstadoSinSesion()
        {
            MenuIniciarSesion.Enabled = true;
            MenuCerrarSesion.Enabled = false;
            MenuAdministracion.Enabled = false;
            MenuCambiarContraseña.Enabled = false;
            MenuCambiarIdioma.Enabled = false;
            MenuAuditoria.Enabled = false;
            MenuRoles.Enabled = false;
            MenuFamilias.Enabled = false;
            MenuGestionRespaldo.Enabled = false;
            administracionToolStripMenuItem1.Enabled = false;

            maestrosToolStripMenuItem.Enabled = false;
            clientesToolStripMenuItem.Enabled = false;
            vehiculosToolStripMenuItem.Enabled = false;
            turnosToolStripMenuItem.Enabled = false;
            asignarTurnoToolStripMenuItem.Enabled = false;
            reportesToolStripMenuItem.Enabled = false;
            reporteTurnosToolStripMenuItem.Enabled = false;

            diagnosticoToolStripMenuItem.Enabled = false;
            registrarAsistenciaToolStripMenuItem.Enabled = false;
            registrarRevisionToolStripMenuItem.Enabled = false;
            registrarDiagnosticoToolStripMenuItem.Enabled = false;
            registrarReparacionToolStripMenuItem.Enabled = false;
            registrarCobroToolStripMenuItem.Enabled = false;
        }

        public void AplicarEstadoSesion(Rol_96_PP rol)
        {
            MenuIniciarSesion.Enabled = false;
            MenuCerrarSesion.Enabled = true;
            MenuAdministracion.Enabled = TieneAlgunPermiso(rol, Permisos_96_PP.Usuarios);
            MenuCambiarContraseña.Enabled = TienePermiso(rol, Permisos_96_PP.CambiarContrasena);
            MenuFamilias.Enabled = TieneAlgunPermiso(rol, Permisos_96_PP.Familias);
            MenuRoles.Enabled = TieneAlgunPermiso(rol, Permisos_96_PP.Roles);
            MenuAuditoria.Enabled = TieneAlgunPermiso(rol, Permisos_96_PP.Auditoria);
            MenuCambiarIdioma.Enabled = TienePermiso(rol, Permisos_96_PP.CambiarIdioma);
            MenuGestionRespaldo.Enabled = TieneAlgunPermiso(rol, Permisos_96_PP.RealizarBackup, Permisos_96_PP.RestaurarBackup);
            administracionToolStripMenuItem1.Enabled = TieneAlgunPermiso(rol, Permisos_96_PP.Administracion);

            clientesToolStripMenuItem.Enabled = TienePermiso(rol, Permisos_96_PP.GestionarClientes);
            vehiculosToolStripMenuItem.Enabled = TienePermiso(rol, Permisos_96_PP.GestionarVehiculos);
            maestrosToolStripMenuItem.Enabled = TieneAlgunPermiso(rol, Permisos_96_PP.Maestros);
            asignarTurnoToolStripMenuItem.Enabled = TieneAlgunPermiso(rol, Permisos_96_PP.Turnos);
            turnosToolStripMenuItem.Enabled = TieneAlgunPermiso(rol, Permisos_96_PP.Turnos);

            reporteTurnosToolStripMenuItem.Enabled = TienePermiso(rol, Permisos_96_PP.ReporteTurnos);
            reportesToolStripMenuItem.Enabled = TienePermiso(rol, Permisos_96_PP.ReporteTurnos);

            registrarAsistenciaToolStripMenuItem.Enabled = TienePermiso(rol, Permisos_96_PP.RegistrarAsistencia);
            registrarRevisionToolStripMenuItem.Enabled = TienePermiso(rol, Permisos_96_PP.RegistrarRevision);
            registrarDiagnosticoToolStripMenuItem.Enabled = TienePermiso(rol, Permisos_96_PP.RegistrarDiagnostico);
            registrarReparacionToolStripMenuItem.Enabled = TienePermiso(rol, Permisos_96_PP.RegistrarReparacion);
            registrarCobroToolStripMenuItem.Enabled = TienePermiso(rol, Permisos_96_PP.RegistrarCobro);
            diagnosticoToolStripMenuItem.Enabled = TieneAlgunPermiso(rol, Permisos_96_PP.Diagnostico);
        }

        private bool TienePermiso(Rol_96_PP rol, string permiso)
        {
            return rol != null && rol.TienePermiso(permiso);
        }

        private bool TieneAlgunPermiso(Rol_96_PP rol, params string[] permisos)
        {
            return rol != null && rol.TieneAlgunPermiso(permisos);
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var instance = SessionManager_96_PP.getInstancia();

            if (!instance.haySesionActiva())
            {
                new BLL_Idioma_96_PP().MostrarMensaje("Debe iniciar sesión primero");
                return;
            }


            var idiomaActual = instance.idiomaActual?.tipo;

            var idiomaOriginal = instance.getUsuarioActivo().Idioma?.tipo;

            if(idiomaActual != idiomaOriginal) 
            {
                new BLL_Usuario_96_PP().cambiarIdioma(idiomaActual, instance.getUsuarioActivo().Dni);

                Evento_96_PP evento = new Evento_96_PP(instance.getUsuarioActivo().Dni, DateTime.Now, "Usuarios", "Cambio de idioma", Evento_96_PP.Criticidad.Bajo);
                new BLL_BitacoraEvento_96_PP().RegistrarEvento(evento);

            }

            string mensajeSesionCerrada = new BLL_Idioma_96_PP().TraducirMensaje("Sesión cerrada");
            instance.cerrarSesion();
            MessageBox.Show(mensajeSesionCerrada);

            CerrarFormulariosHijos();
            AplicarEstadoSinSesion();
        }

        

        private void cambiarContraseñaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FormCambiarContraseña_96_PP());
        }

        private void administracionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void rolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FormRoles_96_PP());
        }

        private void familiasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FormFamilias_96_PP());
        }

        private void cambiarIdiomaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FormCambiarIdioma_96_PP());
        }

        private void gestionDeRespaldoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FormGestionBackUpRestore_96_PP());
        }


        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FormClientes_96_PP());
        }

        private void vehiculosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FormVehiculos_96_PP());
        }

        private void asignarTurnoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FormTurnos_96_PP());
        }

        private void registrarAsistenciaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FormAsistencia_96_PP());
        }

        private void registrarRevisionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FormRevision_96_PP());
        }

        private void registrarDiagnosticoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FormDiagnostico_96_PP());
        }

        private void registrarReparacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FormRegistroReparacion_96_PP());
        }

        private void registrarCobroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FormCobro_96_PP());
        }

        private void reporteTurnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FormReporteTurnos_96_PP());
        }

        private void CierreSistemaSolicitado(object sender, EventArgs e)
        {
            if (cierreSistemaEnCurso)
                return;

            cierreSistemaEnCurso = true;
            SessionManager_96_PP.getInstancia().cerrarSesion();

            foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
            {
                if (form != this)
                    form.Close();
            }

            AplicarEstadoSinSesion();
            Application.Exit();
        }

        private void CerrarFormulariosHijos()
        {
            foreach (Form form in this.MdiChildren.ToList())
            {
                form.Close();
            }
        }
    }
}
