using BE_96_PP;
using BLL;
using ClassLibrary2;
using Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SistemaBase.Diagnostico
{
    // RFN2 pasos 6-8: el Mecánico registra el diagnóstico, la solución propuesta y los repuestos faltantes.
    public partial class FormDiagnostico_96_PP : Form, IdiomaObserver_96_PP
    {
        private readonly BLL_Diagnostico_96_PP bllDiagnostico = new BLL_Diagnostico_96_PP();
        private readonly BLL_RepuestoFaltante_96_PP bllFaltante = new BLL_RepuestoFaltante_96_PP();
        private readonly BLL_Idioma_96_PP mensajes = new BLL_Idioma_96_PP();

        private bool puedeRegistrar;
        private BE_Diagnostico_96_PP diagnosticoActual;

        public FormDiagnostico_96_PP()
        {
            InitializeComponent();
            Tema_96_PP.Aplicar(this);
            Ayuda_96_PP.PrepararGrillaTurnos(dgvTurnos);
            Ayuda_96_PP.PrepararGrillaTurnos(dgvConDiagnostico);
            dgvFaltantes.DataBindingComplete += dgvFaltantes_DataBindingComplete;
            SessionManager_96_PP.getInstancia().Suscribir(this);
            actualizarIdioma();
        }

        public void actualizarIdioma()
        {
            mensajes.Traducir(this);
            Text = mensajes.TraducirMensaje("Diagnóstico técnico");
            CargarCombos();
            MostrarDiagnostico();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            SessionManager_96_PP.getInstancia().Desuscribir(this);
            base.OnFormClosed(e);
        }

        private void FormDiagnostico_96_PP_Load(object sender, EventArgs e)
        {
            puedeRegistrar = Ayuda_96_PP.TienePermiso(Permisos_96_PP.RegistrarDiagnostico);
            tabPrincipal.Enabled = puedeRegistrar;

            if (!puedeRegistrar)
                return;

            CargarPendientes();
            CargarConDiagnostico(null);
        }

        // ------------------------------------------------------------ combos
        private void CargarCombos()
        {
            object urgenciaPrevia = cmbUrgencia.SelectedValue;
            object tipoPrevio = cmbTipoSolucion.SelectedValue;

            var urgencias = new List<KeyValuePair<BE_Diagnostico_96_PP.NivelUrgencia, string>>
            {
                new KeyValuePair<BE_Diagnostico_96_PP.NivelUrgencia, string>(BE_Diagnostico_96_PP.NivelUrgencia.Baja, mensajes.TraducirMensaje("Baja")),
                new KeyValuePair<BE_Diagnostico_96_PP.NivelUrgencia, string>(BE_Diagnostico_96_PP.NivelUrgencia.Media, mensajes.TraducirMensaje("Media")),
                new KeyValuePair<BE_Diagnostico_96_PP.NivelUrgencia, string>(BE_Diagnostico_96_PP.NivelUrgencia.Alta, mensajes.TraducirMensaje("Alta")),
                new KeyValuePair<BE_Diagnostico_96_PP.NivelUrgencia, string>(BE_Diagnostico_96_PP.NivelUrgencia.Critica, mensajes.TraducirMensaje("Crítica"))
            };
            cmbUrgencia.DisplayMember = "Value";
            cmbUrgencia.ValueMember = "Key";
            cmbUrgencia.DataSource = urgencias;
            if (urgenciaPrevia is BE_Diagnostico_96_PP.NivelUrgencia)
                cmbUrgencia.SelectedValue = urgenciaPrevia;

            var tipos = new List<KeyValuePair<BE_Diagnostico_96_PP.TipoDeSolucion, string>>
            {
                new KeyValuePair<BE_Diagnostico_96_PP.TipoDeSolucion, string>(BE_Diagnostico_96_PP.TipoDeSolucion.Reparacion, mensajes.TraducirMensaje("Reparación")),
                new KeyValuePair<BE_Diagnostico_96_PP.TipoDeSolucion, string>(BE_Diagnostico_96_PP.TipoDeSolucion.Reemplazo, mensajes.TraducirMensaje("Reemplazo")),
                new KeyValuePair<BE_Diagnostico_96_PP.TipoDeSolucion, string>(BE_Diagnostico_96_PP.TipoDeSolucion.Ajuste, mensajes.TraducirMensaje("Ajuste")),
                new KeyValuePair<BE_Diagnostico_96_PP.TipoDeSolucion, string>(BE_Diagnostico_96_PP.TipoDeSolucion.Mantenimiento, mensajes.TraducirMensaje("Mantenimiento"))
            };
            cmbTipoSolucion.DisplayMember = "Value";
            cmbTipoSolucion.ValueMember = "Key";
            cmbTipoSolucion.DataSource = tipos;
            if (tipoPrevio is BE_Diagnostico_96_PP.TipoDeSolucion)
                cmbTipoSolucion.SelectedValue = tipoPrevio;
        }

        private string NombreUrgencia(BE_Diagnostico_96_PP.NivelUrgencia urgencia)
        {
            return mensajes.TraducirMensaje(urgencia == BE_Diagnostico_96_PP.NivelUrgencia.Critica ? "Crítica" : urgencia.ToString());
        }

        // ------------------------------------------------------------ pestaña 1: diagnóstico
        private void CargarPendientes()
        {
            dgvTurnos.DataSource = bllDiagnostico.ObtenerTurnosPendientes();
            ActualizarBotonDiagnostico();
        }

        private void dgvTurnos_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarBotonDiagnostico();
        }

        private void ActualizarBotonDiagnostico()
        {
            btnConfirmarDiagnostico.Enabled = puedeRegistrar && dgvTurnos.CurrentRow?.DataBoundItem is BE_Turno_96_PP;
        }

        private void btnConfirmarDiagnostico_Click(object sender, EventArgs e)
        {
            BE_Turno_96_PP turno = dgvTurnos.CurrentRow?.DataBoundItem as BE_Turno_96_PP;
            if (turno == null)
            {
                mensajes.MostrarMensaje("Debe seleccionar un turno.");
                return;
            }

            BE_Diagnostico_96_PP diagnostico = new BE_Diagnostico_96_PP
            {
                IdTurno = turno.IdTurno,
                SistemaAfectado = txtSistema.Text,
                DescripcionFallas = txtFallas.Text,
                Urgencia = (BE_Diagnostico_96_PP.NivelUrgencia)cmbUrgencia.SelectedValue,
                RepuestosNecesarios = txtRepuestosNecesarios.Text
            };

            try
            {
                bllDiagnostico.Alta(diagnostico);
                mensajes.MostrarMensaje("Diagnóstico registrado.");

                txtSistema.Clear();
                txtFallas.Clear();
                txtRepuestosNecesarios.Clear();

                CargarPendientes();
                CargarConDiagnostico(turno.IdTurno);
                tabPrincipal.SelectedTab = tabSolucion;
            }
            catch (Exception ex)
            {
                mensajes.MostrarMensaje(ex.Message);
            }
        }

        // ------------------------------------------------------------ pestaña 2: solución y faltantes
        private void CargarConDiagnostico(int? idTurnoASeleccionar)
        {
            dgvConDiagnostico.DataSource = bllDiagnostico.ObtenerTurnosConDiagnostico();

            if (idTurnoASeleccionar.HasValue)
                Ayuda_96_PP.SeleccionarTurno(dgvConDiagnostico, idTurnoASeleccionar.Value);

            CargarDiagnosticoSeleccionado();
        }

        private void dgvConDiagnostico_SelectionChanged(object sender, EventArgs e)
        {
            CargarDiagnosticoSeleccionado();
        }

        private void CargarDiagnosticoSeleccionado()
        {
            BE_Turno_96_PP turno = dgvConDiagnostico.CurrentRow?.DataBoundItem as BE_Turno_96_PP;
            diagnosticoActual = turno == null ? null : bllDiagnostico.ObtenerPorTurno(turno.IdTurno);

            MostrarDiagnostico();
            CargarFaltantes();
        }

        private void MostrarDiagnostico()
        {
            bool hay = diagnosticoActual != null;
            bool conSolucion = hay && diagnosticoActual.TipoSolucion.HasValue;

            lblDetalleDiag.Text = hay
                ? $"{diagnosticoActual.SistemaAfectado} ({NombreUrgencia(diagnosticoActual.Urgencia)}): {diagnosticoActual.DescripcionFallas}"
                : string.Empty;

            if (conSolucion)
            {
                cmbTipoSolucion.SelectedValue = diagnosticoActual.TipoSolucion.Value;
                txtDescSolucion.Text = diagnosticoActual.DescripcionSolucion;
            }
            else
            {
                txtDescSolucion.Clear();
            }

            cmbTipoSolucion.Enabled = hay && !conSolucion;
            txtDescSolucion.ReadOnly = !hay || conSolucion;
            btnGuardarSolucion.Enabled = hay && !conSolucion;

            txtRepuesto.Enabled = hay;
            nudCantidad.Enabled = hay;
            btnAgregarFaltante.Enabled = hay;
        }

        private void btnGuardarSolucion_Click(object sender, EventArgs e)
        {
            if (diagnosticoActual == null)
            {
                mensajes.MostrarMensaje("Debe seleccionar un turno.");
                return;
            }

            BE_Turno_96_PP turno = dgvConDiagnostico.CurrentRow?.DataBoundItem as BE_Turno_96_PP;

            try
            {
                bllDiagnostico.RegistrarSolucion(
                    diagnosticoActual.IdDiagnostico,
                    (BE_Diagnostico_96_PP.TipoDeSolucion)cmbTipoSolucion.SelectedValue,
                    txtDescSolucion.Text);

                mensajes.MostrarMensaje("Solución registrada.");
                CargarConDiagnostico(turno?.IdTurno);
            }
            catch (Exception ex)
            {
                mensajes.MostrarMensaje(ex.Message);
            }
        }

        private void CargarFaltantes()
        {
            dgvFaltantes.DataSource = diagnosticoActual == null
                ? null
                : bllFaltante.ObtenerPorDiagnostico(diagnosticoActual.IdDiagnostico);

            ActualizarBotonComprado();
        }

        private void dgvFaltantes_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            Ayuda_96_PP.OcultarColumnas(dgvFaltantes, "IdFaltante", "IdDiagnostico");
        }

        private void dgvFaltantes_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarBotonComprado();
        }

        private BE_RepuestoFaltante_96_PP FaltanteSeleccionado()
        {
            return dgvFaltantes.CurrentRow?.DataBoundItem as BE_RepuestoFaltante_96_PP;
        }

        private void ActualizarBotonComprado()
        {
            BE_RepuestoFaltante_96_PP faltante = FaltanteSeleccionado();
            btnMarcarComprado.Enabled = faltante != null
                && faltante.Estado == BE_RepuestoFaltante_96_PP.EstadoFaltante.PendienteCompra;
        }

        private void btnAgregarFaltante_Click(object sender, EventArgs e)
        {
            if (diagnosticoActual == null)
            {
                mensajes.MostrarMensaje("Debe seleccionar un turno.");
                return;
            }

            BE_RepuestoFaltante_96_PP faltante = new BE_RepuestoFaltante_96_PP
            {
                IdDiagnostico = diagnosticoActual.IdDiagnostico,
                Repuesto = txtRepuesto.Text,
                Cantidad = (int)nudCantidad.Value
            };

            try
            {
                bllFaltante.Alta(faltante);
                mensajes.MostrarMensaje("Repuesto faltante registrado.");
                txtRepuesto.Clear();
                nudCantidad.Value = nudCantidad.Minimum;
                CargarFaltantes();
            }
            catch (Exception ex)
            {
                mensajes.MostrarMensaje(ex.Message);
            }
        }

        private void btnMarcarComprado_Click(object sender, EventArgs e)
        {
            BE_RepuestoFaltante_96_PP faltante = FaltanteSeleccionado();
            if (faltante == null)
            {
                mensajes.MostrarMensaje("Debe seleccionar un repuesto.");
                return;
            }

            try
            {
                bllFaltante.MarcarComprado(faltante.IdFaltante);
                mensajes.MostrarMensaje("Repuesto marcado como comprado.");
                CargarFaltantes();
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
