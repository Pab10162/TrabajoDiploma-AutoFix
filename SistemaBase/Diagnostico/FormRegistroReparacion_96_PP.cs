using BE_96_PP;
using BLL;
using ClassLibrary2;
using Servicio;
using System;
using System.Windows.Forms;

namespace SistemaBase.Diagnostico
{
    // RFN2 pasos 9-10: el Mecánico registra la reparación y luego el informe de finalización.
    public partial class FormRegistroReparacion_96_PP : Form, IdiomaObserver_96_PP
    {
        private readonly BLL_Reparacion_96_PP bllReparacion = new BLL_Reparacion_96_PP();
        private readonly BLL_Diagnostico_96_PP bllDiagnostico = new BLL_Diagnostico_96_PP();
        private readonly BLL_RepuestoFaltante_96_PP bllFaltante = new BLL_RepuestoFaltante_96_PP();
        private readonly BLL_Idioma_96_PP mensajes = new BLL_Idioma_96_PP();

        private bool puedeRegistrar;

        public FormRegistroReparacion_96_PP()
        {
            InitializeComponent();
            Tema_96_PP.Aplicar(this);
            Ayuda_96_PP.PrepararGrillaTurnos(dgvTurnos);
            SessionManager_96_PP.getInstancia().Suscribir(this);
            actualizarIdioma();
        }

        public void actualizarIdioma()
        {
            mensajes.Traducir(this);
            Text = mensajes.TraducirMensaje("Registrar reparación");
            MostrarSeleccion();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            SessionManager_96_PP.getInstancia().Desuscribir(this);
            base.OnFormClosed(e);
        }

        private void FormRegistroReparacion_96_PP_Load(object sender, EventArgs e)
        {
            puedeRegistrar = Ayuda_96_PP.TienePermiso(Permisos_96_PP.RegistrarReparacion);
            grpReparacion.Enabled = puedeRegistrar;
            grpFinalizacion.Enabled = puedeRegistrar;

            if (!puedeRegistrar)
                return;

            CargarTurnos(null);
        }

        private void CargarTurnos(int? idTurnoASeleccionar)
        {
            dgvTurnos.DataSource = bllReparacion.ObtenerTurnosPendientes();

            if (idTurnoASeleccionar.HasValue)
                Ayuda_96_PP.SeleccionarTurno(dgvTurnos, idTurnoASeleccionar.Value);

            MostrarSeleccion();
        }

        private void dgvTurnos_SelectionChanged(object sender, EventArgs e)
        {
            MostrarSeleccion();
        }

        private BE_Turno_96_PP TurnoSeleccionado()
        {
            return dgvTurnos.CurrentRow?.DataBoundItem as BE_Turno_96_PP;
        }

        // Según el avance del turno habilita "registrar reparación" o "finalizar".
        private void MostrarSeleccion()
        {
            BE_Turno_96_PP turno = TurnoSeleccionado();
            BE_Diagnostico_96_PP diagnostico = turno == null ? null : bllDiagnostico.ObtenerPorTurno(turno.IdTurno);

            if (diagnostico == null)
            {
                lblDiagnostico.Text = string.Empty;
                txtTareas.Clear();
                txtRepuestosUtilizados.Clear();
                txtTareas.ReadOnly = true;
                txtRepuestosUtilizados.ReadOnly = true;
                btnConfirmarReparacion.Enabled = false;
                nudPresupuesto.Enabled = false;
                btnConfirmarFinalizacion.Enabled = false;
                return;
            }

            bool pendientes = bllFaltante.TienePendientes(diagnostico.IdDiagnostico);
            BE_Reparacion_96_PP reparacion = bllReparacion.ObtenerPorDiagnostico(diagnostico.IdDiagnostico);

            string solucion = diagnostico.TipoSolucion.HasValue
                ? $"{mensajes.TraducirMensaje(NombreSolucion(diagnostico.TipoSolucion.Value))}: {diagnostico.DescripcionSolucion}"
                : string.Empty;

            lblDiagnostico.Text = $"{diagnostico.SistemaAfectado}: {diagnostico.DescripcionFallas}\r\n{solucion}";
            if (pendientes)
                lblDiagnostico.Text += "  (" + mensajes.TraducirMensaje("Hay repuestos pendientes de compra para este diagnóstico.") + ")";

            if (reparacion == null)
            {
                txtTareas.Clear();
                txtRepuestosUtilizados.Clear();
                txtTareas.ReadOnly = false;
                txtRepuestosUtilizados.ReadOnly = false;
                btnConfirmarReparacion.Enabled = puedeRegistrar && !pendientes;
                nudPresupuesto.Enabled = false;
                btnConfirmarFinalizacion.Enabled = false;
            }
            else
            {
                txtTareas.Text = reparacion.TareasRealizadas;
                txtRepuestosUtilizados.Text = reparacion.RepuestosUtilizados;
                txtTareas.ReadOnly = true;
                txtRepuestosUtilizados.ReadOnly = true;
                btnConfirmarReparacion.Enabled = false;
                nudPresupuesto.Enabled = puedeRegistrar && !reparacion.Finalizada;
                btnConfirmarFinalizacion.Enabled = puedeRegistrar && !reparacion.Finalizada;
            }
        }

        private static string NombreSolucion(BE_Diagnostico_96_PP.TipoDeSolucion tipo)
        {
            switch (tipo)
            {
                case BE_Diagnostico_96_PP.TipoDeSolucion.Reparacion: return "Reparación";
                case BE_Diagnostico_96_PP.TipoDeSolucion.Reemplazo: return "Reemplazo";
                case BE_Diagnostico_96_PP.TipoDeSolucion.Ajuste: return "Ajuste";
                default: return "Mantenimiento";
            }
        }

        private void btnConfirmarReparacion_Click(object sender, EventArgs e)
        {
            BE_Turno_96_PP turno = TurnoSeleccionado();
            BE_Diagnostico_96_PP diagnostico = turno == null ? null : bllDiagnostico.ObtenerPorTurno(turno.IdTurno);
            if (diagnostico == null)
            {
                mensajes.MostrarMensaje("Debe seleccionar un turno.");
                return;
            }

            BE_Reparacion_96_PP reparacion = new BE_Reparacion_96_PP
            {
                IdDiagnostico = diagnostico.IdDiagnostico,
                TareasRealizadas = txtTareas.Text,
                RepuestosUtilizados = txtRepuestosUtilizados.Text
            };

            try
            {
                bllReparacion.Alta(reparacion);
                mensajes.MostrarMensaje("Reparación registrada.");
                CargarTurnos(turno.IdTurno);
            }
            catch (Exception ex)
            {
                mensajes.MostrarMensaje(ex.Message);
            }
        }

        private void btnConfirmarFinalizacion_Click(object sender, EventArgs e)
        {
            BE_Turno_96_PP turno = TurnoSeleccionado();
            BE_Diagnostico_96_PP diagnostico = turno == null ? null : bllDiagnostico.ObtenerPorTurno(turno.IdTurno);
            if (diagnostico == null)
            {
                mensajes.MostrarMensaje("Debe seleccionar un turno.");
                return;
            }

            try
            {
                bllReparacion.Finalizar(diagnostico.IdDiagnostico, nudPresupuesto.Value);
                mensajes.MostrarMensaje("Reparación finalizada.");
                nudPresupuesto.Value = 0;
                CargarTurnos(null);
            }
            catch (Exception ex)
            {
                mensajes.MostrarMensaje(ex.Message);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
