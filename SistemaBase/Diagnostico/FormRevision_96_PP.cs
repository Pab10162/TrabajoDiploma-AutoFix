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
    // RFN2 pasos 3-5: el Jefe de Taller revisa el vehículo, emite el informe y asigna al mecánico.
    public partial class FormRevision_96_PP : Form, IdiomaObserver_96_PP
    {
        private readonly BLL_Revision_96_PP bll = new BLL_Revision_96_PP();
        private readonly BLL_Idioma_96_PP mensajes = new BLL_Idioma_96_PP();

        private bool puedeRegistrar;

        public FormRevision_96_PP()
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
            Text = mensajes.TraducirMensaje("Registrar revisión");
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            SessionManager_96_PP.getInstancia().Desuscribir(this);
            base.OnFormClosed(e);
        }

        private void FormRevision_96_PP_Load(object sender, EventArgs e)
        {
            puedeRegistrar = Ayuda_96_PP.TienePermiso(Permisos_96_PP.RegistrarRevision);
            grpRevision.Enabled = puedeRegistrar;

            if (!puedeRegistrar)
                return;

            CargarMecanicos();
            CargarTurnos();
        }

        private void CargarMecanicos()
        {
            List<KeyValuePair<string, string>> mecanicos = bll.ObtenerMecanicos()
                .Select(m => new KeyValuePair<string, string>(m.Dni, $"{m.Apellido}, {m.Nombre} ({m.Dni})"))
                .ToList();

            cmbMecanico.DisplayMember = "Value";
            cmbMecanico.ValueMember = "Key";
            cmbMecanico.DataSource = mecanicos;
        }

        private void CargarTurnos()
        {
            dgvTurnos.DataSource = bll.ObtenerTurnosPendientes();
            MostrarDetalle();
        }

        private void dgvTurnos_SelectionChanged(object sender, EventArgs e)
        {
            MostrarDetalle();
        }

        private BE_Turno_96_PP TurnoSeleccionado()
        {
            return dgvTurnos.CurrentRow?.DataBoundItem as BE_Turno_96_PP;
        }

        private void MostrarDetalle()
        {
            BE_Turno_96_PP turno = TurnoSeleccionado();
            lblDetalle.Text = Ayuda_96_PP.Describir(turno);
            btnConfirmarRevision.Enabled = puedeRegistrar && turno != null;
        }

        private void btnConfirmarRevision_Click(object sender, EventArgs e)
        {
            BE_Turno_96_PP turno = TurnoSeleccionado();
            if (turno == null)
            {
                mensajes.MostrarMensaje("Debe seleccionar un turno.");
                return;
            }

            BE_Revision_96_PP revision = new BE_Revision_96_PP
            {
                IdTurno = turno.IdTurno,
                Observaciones = txtObservaciones.Text,
                DniMecanico = cmbMecanico.SelectedValue as string
            };

            try
            {
                bll.Alta(revision);
                mensajes.MostrarMensaje("Revisión registrada. Se asignó el mecánico al turno.");
                txtObservaciones.Clear();
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
