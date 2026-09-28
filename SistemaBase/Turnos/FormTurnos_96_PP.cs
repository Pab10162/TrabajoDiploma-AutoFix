using BE_96_PP;
using BLL;
using ClassLibrary2;
using Servicio;
using SistemaBase.Maestros;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SistemaBase.Turnos
{
   
    public partial class FormTurnos_96_PP : Form, IdiomaObserver_96_PP
    {
        private readonly BLL_Cliente_96_PP bllCliente = new BLL_Cliente_96_PP();
        private readonly BLL_Vehiculo_96_PP bllVehiculo = new BLL_Vehiculo_96_PP();
        private readonly BLL_Turno_96_PP bllTurno = new BLL_Turno_96_PP();
        private readonly BLL_Idioma_96_PP mensajes = new BLL_Idioma_96_PP();

        private BE_Cliente_96_PP clienteActual;
        private List<TimeSpan> horariosDisponibles = new List<TimeSpan>();

        private bool puedeAsignar;
        private bool puedeConsultar;

        public FormTurnos_96_PP()
        {
            InitializeComponent();
            SessionManager_96_PP.getInstancia().Suscribir(this);
            actualizarIdioma();
        }

        public void actualizarIdioma()
        {
            mensajes.Traducir(this);
            Text = mensajes.TraducirMensaje("Turnos");
            MostrarCliente();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            SessionManager_96_PP.getInstancia().Desuscribir(this);
            base.OnFormClosed(e);
        }

        private void FormTurnos_96_PP_Load(object sender, EventArgs e)
        {
            puedeAsignar = TienePermiso(Permisos_96_PP.AsignarTurno);
            puedeConsultar = puedeAsignar || TienePermiso(Permisos_96_PP.ConsultarTurnos);

            dtpFecha.MinDate = DateTime.Today;
            dtpFecha.Value = DateTime.Today;
            dtpFiltro.Value = DateTime.Today;

            grpCliente.Enabled = puedeAsignar;
            grpVehiculo.Enabled = puedeAsignar;
            grpTurno.Enabled = puedeAsignar;
            btnImportarTurno.Enabled = puedeConsultar;

            LimpiarCliente();
            CargarHorarios();
            CargarAgenda();
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            string dni = txtDniCliente.Text.Trim();
            if (string.IsNullOrEmpty(dni))
            {
                mensajes.MostrarMensaje("Ingrese el DNI del cliente.");
                return;
            }

            BE_Cliente_96_PP encontrado = bllCliente.BuscarPorDni(dni);

            if (encontrado == null)
            {
                LimpiarCliente();
                lblDatosCliente.Text = mensajes.TraducirMensaje("El cliente no está registrado.");
                btnRegistrarCliente.Enabled = true;
                return;
            }

            if (!encontrado.Activo)
            {
                LimpiarCliente();
                lblDatosCliente.Text = mensajes.TraducirMensaje("El cliente está dado de baja.");
                return;
            }

            SeleccionarCliente(encontrado);
        }

        private void btnRegistrarCliente_Click(object sender, EventArgs e)
        {
            using (FormClientes_96_PP form = new FormClientes_96_PP(txtDniCliente.Text.Trim()))
            {
                if (form.ShowDialog(this) == DialogResult.OK && form.ClienteRegistrado != null)
                {
                    txtDniCliente.Text = form.ClienteRegistrado.Dni;
                    SeleccionarCliente(bllCliente.BuscarPorDni(form.ClienteRegistrado.Dni));
                }
            }
        }

        private void txtDniCliente_TextChanged(object sender, EventArgs e)
        {
            if (clienteActual != null && clienteActual.Dni != txtDniCliente.Text.Trim())
                LimpiarCliente();
        }

        private void SeleccionarCliente(BE_Cliente_96_PP cliente)
        {
            clienteActual = cliente;
            btnRegistrarCliente.Enabled = false;
            MostrarCliente();
            CargarVehiculos(null);
        }

        private void LimpiarCliente()
        {
            clienteActual = null;
            lblDatosCliente.Text = string.Empty;
            btnRegistrarCliente.Enabled = false;
            cmbVehiculos.DataSource = null;
            btnRegistrarVehiculo.Enabled = false;
        }

        private void MostrarCliente()
        {
            if (clienteActual == null)
                return;

            lblDatosCliente.Text =
                $"{clienteActual.Apellido}, {clienteActual.Nombre}  -  " +
                $"{mensajes.TraducirMensaje("Tel.")} {clienteActual.Telefono}  -  {clienteActual.Direccion}";
        }


        private void CargarVehiculos(string patenteASeleccionar)
        {
            cmbVehiculos.DataSource = null;
            btnRegistrarVehiculo.Enabled = clienteActual != null;

            if (clienteActual == null)
                return;

            List<BE_Vehiculo_96_PP> vehiculos = bllVehiculo.ObtenerActivosPorCliente(clienteActual.Dni);
            cmbVehiculos.DataSource = vehiculos;

            if (vehiculos.Count == 0)
            {
                mensajes.MostrarMensaje("El cliente no tiene vehículos registrados.");
                return;
            }

            if (!string.IsNullOrEmpty(patenteASeleccionar))
            {
                BE_Vehiculo_96_PP elegido = vehiculos.FirstOrDefault(v => v.Patente == patenteASeleccionar);
                if (elegido != null)
                    cmbVehiculos.SelectedItem = elegido;
            }
        }

        private void btnRegistrarVehiculo_Click(object sender, EventArgs e)
        {
            if (clienteActual == null)
            {
                mensajes.MostrarMensaje("Primero debe buscar al cliente.");
                return;
            }

            using (FormVehiculos_96_PP form = new FormVehiculos_96_PP(clienteActual.Dni, null))
            {
                if (form.ShowDialog(this) == DialogResult.OK && form.VehiculoRegistrado != null)
                    CargarVehiculos(form.VehiculoRegistrado.Patente);
            }
        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {
            CargarHorarios();
        }

        private void CargarHorarios()
        {
            horariosDisponibles = bllTurno.ObtenerHorariosDisponibles(dtpFecha.Value.Date);

            lstHorarios.DataSource = null;
            lstHorarios.DataSource = horariosDisponibles.Select(h => h.ToString(@"hh\:mm")).ToList();
            lstHorarios.ClearSelected();

            btnAsignarTurno.Enabled = puedeAsignar && horariosDisponibles.Count > 0;
        }

        private void btnAsignarTurno_Click(object sender, EventArgs e)
        {
            if (clienteActual == null)
            {
                mensajes.MostrarMensaje("Primero debe buscar al cliente.");
                return;
            }

            BE_Vehiculo_96_PP vehiculo = cmbVehiculos.SelectedItem as BE_Vehiculo_96_PP;
            if (vehiculo == null)
            {
                mensajes.MostrarMensaje("Debe seleccionar un vehículo.");
                return;
            }

            if (lstHorarios.SelectedIndex < 0)
            {
                mensajes.MostrarMensaje("Debe seleccionar un horario.");
                return;
            }

            TimeSpan horario = horariosDisponibles[lstHorarios.SelectedIndex];
            DateTime fecha = dtpFecha.Value.Date;

            try
            {
                BE_Turno_96_PP turno = bllTurno.AsignarTurno(clienteActual.Dni, vehiculo.Patente, fecha, horario);

                MessageBox.Show(
                    mensajes.TraducirMensaje("Turno asignado correctamente.") + Environment.NewLine +
                    $"#{turno.IdTurno} - {turno.Fecha:dd/MM/yyyy} {turno.HorarioTexto} - {turno.PatenteVehiculo}",
                    mensajes.TraducirMensaje("Turnos"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                chkTodasLasFechas.Checked = false;
                dtpFiltro.Value = fecha;
                CargarAgenda();
            }
            catch (Exception ex)
            {
                mensajes.MostrarMensaje(ex.Message);
            }
            finally
            {
                CargarHorarios();
            }
        }

        private void dtpFiltro_ValueChanged(object sender, EventArgs e)
        {
            CargarAgenda();
        }

        private void chkTodasLasFechas_CheckedChanged(object sender, EventArgs e)
        {
            dtpFiltro.Enabled = !chkTodasLasFechas.Checked;
            CargarAgenda();
        }

        private void CargarAgenda()
        {
            dgvTurnos.DataSource = null;

            if (!puedeConsultar)
            {
                ActualizarBotonesAgenda();
                return;
            }

            dgvTurnos.DataSource = chkTodasLasFechas.Checked
                ? bllTurno.ObtenerTodos()
                : bllTurno.ObtenerPorFecha(dtpFiltro.Value.Date);

            ActualizarBotonesAgenda();
        }

        private void dgvTurnos_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (string columna in new[] { "Cliente", "Vehiculo", "Horario" })
            {
                if (dgvTurnos.Columns.Contains(columna))
                    dgvTurnos.Columns[columna].Visible = false;
            }

            if (dgvTurnos.Columns.Contains("Fecha"))
                dgvTurnos.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void dgvTurnos_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarBotonesAgenda();
        }

        private BE_Turno_96_PP TurnoSeleccionado()
        {
            return dgvTurnos.CurrentRow?.DataBoundItem as BE_Turno_96_PP;
        }

        private void ActualizarBotonesAgenda()
        {
            BE_Turno_96_PP turno = TurnoSeleccionado();

            btnConfirmarTurno.Enabled = puedeAsignar && turno != null &&
                turno.Estado == BE_Turno_96_PP.EstadoTurno.Pendiente;

            btnCancelarTurno.Enabled = puedeAsignar && turno != null &&
                (turno.Estado == BE_Turno_96_PP.EstadoTurno.Pendiente ||
                 turno.Estado == BE_Turno_96_PP.EstadoTurno.Confirmado);

            btnExportarTurno.Enabled = puedeConsultar && turno != null;
        }

        private void btnExportarTurno_Click(object sender, EventArgs e)
        {
            BE_Turno_96_PP turno = TurnoSeleccionado();
            if (turno == null)
            {
                mensajes.MostrarMensaje("Debe seleccionar un turno de la agenda.");
                return;
            }

            using (SaveFileDialog dialogo = new SaveFileDialog())
            {
                dialogo.Filter = "Archivo XML (*.xml)|*.xml";
                dialogo.FileName = $"Turno_{turno.IdTurno}.xml";

                if (dialogo.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    bllTurno.SerializarTurno(turno, dialogo.FileName);

                    BE_Turno_96_PP verificacion = bllTurno.DeserializarTurno(dialogo.FileName);

                    MessageBox.Show(
                        mensajes.TraducirMensaje("Turno exportado correctamente a:") + Environment.NewLine +
                        dialogo.FileName + Environment.NewLine + Environment.NewLine +
                        $"#{verificacion.IdTurno} - {verificacion.Fecha:dd/MM/yyyy} {verificacion.HorarioTexto} - " +
                        $"{verificacion.NombreCliente} - {verificacion.PatenteVehiculo} - {verificacion.Estado}",
                        mensajes.TraducirMensaje("Turnos"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    mensajes.MostrarMensaje(ex.Message);
                }
            }
        }

        private void btnImportarTurno_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialogo = new OpenFileDialog())
            {
                dialogo.Filter = "Archivo XML (*.xml)|*.xml";

                if (dialogo.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    BE_Turno_96_PP turno = bllTurno.DeserializarTurno(dialogo.FileName);

                    MessageBox.Show(
                        mensajes.TraducirMensaje("Turno importado correctamente:") + Environment.NewLine +
                        $"#{turno.IdTurno} - {turno.Fecha:dd/MM/yyyy} {turno.HorarioTexto} - " +
                        $"{turno.NombreCliente} - {turno.PatenteVehiculo} - {turno.Estado}",
                        mensajes.TraducirMensaje("Turnos"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    mensajes.MostrarMensaje(mensajes.TraducirMensaje("El archivo seleccionado no es un turno válido.") + " " + ex.Message);
                }
            }
        }

        private void btnConfirmarTurno_Click(object sender, EventArgs e)
        {
            try
            {
                bllTurno.Confirmar(TurnoSeleccionado());
                mensajes.MostrarMensaje("Turno confirmado.");
                CargarAgenda();
            }
            catch (Exception ex)
            {
                mensajes.MostrarMensaje(ex.Message);
            }
        }

        private void btnCancelarTurno_Click(object sender, EventArgs e)
        {
            BE_Turno_96_PP turno = TurnoSeleccionado();
            if (turno == null)
                return;

            DialogResult respuesta = MessageBox.Show(
                mensajes.TraducirMensaje("¿Desea cancelar el turno seleccionado?"),
                mensajes.TraducirMensaje("Turnos"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
                return;

            try
            {
                bllTurno.Cancelar(turno);
                mensajes.MostrarMensaje("Turno cancelado.");
                CargarAgenda();
                CargarHorarios();   
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

        private static bool TienePermiso(string permiso)
        {
            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            return usuario?.Rol != null && usuario.Rol.TienePermiso(permiso);
        }
    }
}
