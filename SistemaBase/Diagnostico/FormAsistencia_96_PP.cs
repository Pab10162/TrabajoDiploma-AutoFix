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
    // RFN2 paso 2: la Recepción registra la llegada del cliente con turno.
    public partial class FormAsistencia_96_PP : Form, IdiomaObserver_96_PP
    {
        private readonly BLL_Asistencia_96_PP bll = new BLL_Asistencia_96_PP();
        private readonly BLL_Idioma_96_PP mensajes = new BLL_Idioma_96_PP();

        private List<BE_Turno_96_PP> turnos = new List<BE_Turno_96_PP>();
        private bool puedeRegistrar;

        public FormAsistencia_96_PP()
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
            Text = mensajes.TraducirMensaje("Registrar asistencia");
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            SessionManager_96_PP.getInstancia().Desuscribir(this);
            base.OnFormClosed(e);
        }

        private void FormAsistencia_96_PP_Load(object sender, EventArgs e)
        {
            puedeRegistrar = Ayuda_96_PP.TienePermiso(Permisos_96_PP.RegistrarAsistencia);
            lblFechaValor.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txtBuscar.Enabled = puedeRegistrar;
            CargarTurnos();
        }

        private void CargarTurnos()
        {
            turnos = puedeRegistrar ? bll.ObtenerTurnosPendientes() : new List<BE_Turno_96_PP>();
            AplicarFiltro();
        }

        private void AplicarFiltro()
        {
            string filtro = txtBuscar.Text.Trim().ToLowerInvariant();

            dgvTurnos.DataSource = turnos
                .Where(t => filtro.Length == 0
                         || (t.DniCliente ?? string.Empty).ToLowerInvariant().Contains(filtro)
                         || (t.NombreCliente ?? string.Empty).ToLowerInvariant().Contains(filtro)
                         || (t.PatenteVehiculo ?? string.Empty).ToLowerInvariant().Contains(filtro))
                .ToList();

            ActualizarBoton();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        private void dgvTurnos_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarBoton();
        }

        private BE_Turno_96_PP TurnoSeleccionado()
        {
            return dgvTurnos.CurrentRow?.DataBoundItem as BE_Turno_96_PP;
        }

        private void ActualizarBoton()
        {
            btnConfirmarLlegada.Enabled = puedeRegistrar && TurnoSeleccionado() != null;
        }

        private void btnConfirmarLlegada_Click(object sender, EventArgs e)
        {
            BE_Turno_96_PP turno = TurnoSeleccionado();
            if (turno == null)
            {
                mensajes.MostrarMensaje("Debe seleccionar un turno.");
                return;
            }

            try
            {
                bll.Alta(new BE_Asistencia_96_PP { IdTurno = turno.IdTurno });
                mensajes.MostrarMensaje("Llegada registrada.");
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
