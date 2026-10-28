using BE_96_PP;
using BLL;
using ClassLibrary2;
using Servicio;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SistemaBase.Diagnostico
{
    // RFN2 pasos 11-12: la Recepción cobra el servicio; el turno queda Atendido.
    public partial class FormCobro_96_PP : Form, IdiomaObserver_96_PP
    {
        private readonly BLL_Cobro_96_PP bll = new BLL_Cobro_96_PP();
        private readonly BLL_Idioma_96_PP mensajes = new BLL_Idioma_96_PP();

        private bool puedeRegistrar;

        public FormCobro_96_PP()
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
            Text = mensajes.TraducirMensaje("Registrar cobro");
            CargarMediosPago();
            MostrarResumen();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            SessionManager_96_PP.getInstancia().Desuscribir(this);
            base.OnFormClosed(e);
        }

        private void FormCobro_96_PP_Load(object sender, EventArgs e)
        {
            puedeRegistrar = Ayuda_96_PP.TienePermiso(Permisos_96_PP.RegistrarCobro);
            grpCobro.Enabled = puedeRegistrar;

            if (!puedeRegistrar)
                return;

            CargarTurnos();
        }

        private void CargarMediosPago()
        {
            object previo = cmbMedioPago.SelectedValue;

            var medios = new List<KeyValuePair<BE_Cobro_96_PP.MedioPago, string>>
            {
                new KeyValuePair<BE_Cobro_96_PP.MedioPago, string>(BE_Cobro_96_PP.MedioPago.Efectivo, mensajes.TraducirMensaje("Efectivo")),
                new KeyValuePair<BE_Cobro_96_PP.MedioPago, string>(BE_Cobro_96_PP.MedioPago.Debito, mensajes.TraducirMensaje("Débito")),
                new KeyValuePair<BE_Cobro_96_PP.MedioPago, string>(BE_Cobro_96_PP.MedioPago.Credito, mensajes.TraducirMensaje("Crédito")),
                new KeyValuePair<BE_Cobro_96_PP.MedioPago, string>(BE_Cobro_96_PP.MedioPago.MercadoPago, mensajes.TraducirMensaje("MercadoPago")),
                new KeyValuePair<BE_Cobro_96_PP.MedioPago, string>(BE_Cobro_96_PP.MedioPago.Transferencia, mensajes.TraducirMensaje("Transferencia"))
            };

            cmbMedioPago.DisplayMember = "Value";
            cmbMedioPago.ValueMember = "Key";
            cmbMedioPago.DataSource = medios;
            if (previo is BE_Cobro_96_PP.MedioPago)
                cmbMedioPago.SelectedValue = previo;
        }

        private void CargarTurnos()
        {
            dgvTurnos.DataSource = bll.ObtenerTurnosPendientes();
            MostrarResumen();
        }

        private void dgvTurnos_SelectionChanged(object sender, EventArgs e)
        {
            MostrarResumen();
        }

        private BE_Turno_96_PP TurnoSeleccionado()
        {
            return dgvTurnos.CurrentRow?.DataBoundItem as BE_Turno_96_PP;
        }

        private void MostrarResumen()
        {
            BE_Turno_96_PP turno = TurnoSeleccionado();
            btnConfirmarCobro.Enabled = puedeRegistrar && turno != null;

            if (turno == null)
            {
                lblResumen.Text = string.Empty;
                nudMonto.Value = 0;
                return;
            }

            decimal? presupuesto = bll.ObtenerPresupuestoFinal(turno.IdTurno);
            lblResumen.Text = Ayuda_96_PP.Describir(turno);

            if (presupuesto.HasValue)
            {
                lblResumen.Text += "\r\n" + mensajes.TraducirMensaje("Presupuesto final") + ": " + presupuesto.Value.ToString("N2");
                nudMonto.Value = Math.Min(presupuesto.Value, nudMonto.Maximum);
            }
            else
            {
                nudMonto.Value = 0;
            }
        }

        private void btnConfirmarCobro_Click(object sender, EventArgs e)
        {
            BE_Turno_96_PP turno = TurnoSeleccionado();
            if (turno == null)
            {
                mensajes.MostrarMensaje("Debe seleccionar un turno.");
                return;
            }

            BE_Cobro_96_PP cobro = new BE_Cobro_96_PP
            {
                IdTurno = turno.IdTurno,
                MontoTotal = nudMonto.Value,
                MetodoPago = (BE_Cobro_96_PP.MedioPago)cmbMedioPago.SelectedValue
            };

            try
            {
                bll.Alta(cobro);
                mensajes.MostrarMensaje("Cobro registrado. El turno quedó atendido.");
                CargarTurnos();
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
