using BE_96_PP;
using BLL;
using ClassLibrary2;
using Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SistemaBase.Maestros
{
    // RFN1 pasos 4 y 5: la recepcionista busca el vehículo por patente y, si no está
    // registrado, lo da de alta asociándolo al cliente. Igual que FormClientes_96_PP,
    // se puede abrir desde el menú Maestros o desde la pantalla de turnos.
    public partial class FormVehiculos_96_PP : Form, IdiomaObserver_96_PP
    {
        private enum Modo { Consulta, Crear, Modificar }

        private readonly BLL_Vehiculo_96_PP bllVehiculo = new BLL_Vehiculo_96_PP();
        private readonly BLL_Cliente_96_PP bllCliente = new BLL_Cliente_96_PP();
        private readonly BLL_Idioma_96_PP mensajes = new BLL_Idioma_96_PP();

        private Modo modo = Modo.Consulta;
        private List<BE_Vehiculo_96_PP> vehiculos = new List<BE_Vehiculo_96_PP>();

        // Cliente dueño que se está cargando en el formulario (alta o modificación).
        private BE_Cliente_96_PP clienteDelFormulario;

        // Datos que llegan desde Turnos.
        private readonly string dniClienteInicial;
        private readonly string patenteInicial;

        public BE_Vehiculo_96_PP VehiculoRegistrado { get; private set; }

        public FormVehiculos_96_PP() : this(null, null)
        {
        }

        public FormVehiculos_96_PP(string dniCliente, string patente)
        {
            InitializeComponent();
            Tema_96_PP.Aplicar(this);
            dniClienteInicial = dniCliente;
            patenteInicial = patente;

            dgvVehiculos.CellFormatting += dgvVehiculos_CellFormatting;

            SessionManager_96_PP.getInstancia().Suscribir(this);
            actualizarIdioma();
        }

        public void actualizarIdioma()
        {
            mensajes.Traducir(this);
            Text = mensajes.TraducirMensaje("Vehiculos");
            ActualizarModo();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            SessionManager_96_PP.getInstancia().Desuscribir(this);
            base.OnFormClosed(e);
        }

        private void FormVehiculos_96_PP_Load(object sender, EventArgs e)
        {
            CargarGrilla();
            VolverAModoConsulta();

            if (!string.IsNullOrWhiteSpace(dniClienteInicial))
            {
                IniciarAlta(patenteInicial);
                txtDniCliente.Text = dniClienteInicial;
                BuscarCliente();
            }
        }

        // ---------- Grilla ----------

        private void CargarGrilla()
        {
            vehiculos = bllVehiculo.ObtenerTodos();
            MostrarVehiculos();
        }

        private void MostrarVehiculos()
        {
            IEnumerable<BE_Vehiculo_96_PP> lista = chkVerInactivos.Checked ? vehiculos : vehiculos.Where(v => v.Activo);

            dgvVehiculos.DataSource = null;
            dgvVehiculos.DataSource = lista.ToList();
        }

        private void dgvVehiculos_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // El objeto Cliente completo no se muestra; se ven DniCliente y NombreCliente.
            if (dgvVehiculos.Columns.Contains("Cliente"))
                dgvVehiculos.Columns["Cliente"].Visible = false;
        }

        private void dgvVehiculos_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarBotonEstado();

            if (modo != Modo.Consulta)
                return;

            BE_Vehiculo_96_PP vehiculo = VehiculoSeleccionado();
            if (vehiculo != null)
                MostrarEnCampos(vehiculo);
        }

        private BE_Vehiculo_96_PP VehiculoSeleccionado()
        {
            return dgvVehiculos.CurrentRow?.DataBoundItem as BE_Vehiculo_96_PP;
        }

        private void chkVerInactivos_CheckedChanged(object sender, EventArgs e)
        {
            MostrarVehiculos();
        }

        // ---------- Búsqueda por patente (paso 4 del RFN1) ----------

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string patente = txtBuscarPatente.Text;
            if (string.IsNullOrWhiteSpace(patente))
            {
                MostrarVehiculos();
                return;
            }

            BE_Vehiculo_96_PP encontrado = bllVehiculo.BuscarPorPatente(patente);
            if (encontrado != null)
            {
                if (!encontrado.Activo && !chkVerInactivos.Checked)
                    chkVerInactivos.Checked = true;

                SeleccionarEnGrilla(encontrado.Patente);
                MostrarEnCampos(encontrado);
                return;
            }

            // Paso 5: no está registrado -> se ofrece darlo de alta.
            DialogResult respuesta = MessageBox.Show(
                mensajes.TraducirMensaje("El vehículo no está registrado. ¿Desea registrarlo?"),
                mensajes.TraducirMensaje("Vehiculos"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
                IniciarAlta(BLL_Vehiculo_96_PP.NormalizarPatente(patente));
        }

        private void SeleccionarEnGrilla(string patente)
        {
            foreach (DataGridViewRow fila in dgvVehiculos.Rows)
            {
                if (fila.DataBoundItem is BE_Vehiculo_96_PP v && v.Patente == patente)
                {
                    dgvVehiculos.CurrentCell = fila.Cells.Cast<DataGridViewCell>().First(celda => celda.Visible);
                    return;
                }
            }
        }

        // ---------- Cliente dueño ----------

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            BuscarCliente();
        }

        private void BuscarCliente()
        {
            clienteDelFormulario = bllCliente.BuscarPorDni(txtDniCliente.Text);

            if (clienteDelFormulario == null)
            {
                lblNombreCliente.Text = mensajes.TraducirMensaje("El cliente no existe.");
                return;
            }

            if (!clienteDelFormulario.Activo)
            {
                lblNombreCliente.Text = mensajes.TraducirMensaje("El cliente está dado de baja.");
                clienteDelFormulario = null;
                return;
            }

            lblNombreCliente.Text = $"{clienteDelFormulario.Apellido}, {clienteDelFormulario.Nombre}";
        }

        // ---------- Botones ----------

        private void btnCrear_Click(object sender, EventArgs e)
        {
            IniciarAlta(null);
        }

        private void IniciarAlta(string patente)
        {
            modo = Modo.Crear;
            LimpiarCampos();
            txtPatente.Text = patente ?? string.Empty;
            HabilitarCampos(true, patenteEditable: true);
            ActualizarModo();
            (string.IsNullOrEmpty(patente) ? (Control)txtPatente : txtMarca).Focus();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            BE_Vehiculo_96_PP vehiculo = VehiculoSeleccionado();
            if (vehiculo == null)
            {
                mensajes.MostrarMensaje("Debe seleccionar un vehículo.");
                return;
            }

            modo = Modo.Modificar;
            MostrarEnCampos(vehiculo);
            HabilitarCampos(true, patenteEditable: false);
            ActualizarModo();
        }

        private void btnActivarDesactivar_Click(object sender, EventArgs e)
        {
            BE_Vehiculo_96_PP vehiculo = VehiculoSeleccionado();
            if (vehiculo == null)
            {
                mensajes.MostrarMensaje("Debe seleccionar un vehículo.");
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                mensajes.TraducirMensaje(vehiculo.Activo ? "¿Desea dar de baja al vehículo?" : "¿Desea reactivar al vehículo?"),
                mensajes.TraducirMensaje("Vehiculos"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
                return;

            try
            {
                bllVehiculo.CambiarEstadoActivo(vehiculo);
                mensajes.MostrarMensaje(vehiculo.Activo ? "Vehículo reactivado." : "Vehículo dado de baja.");
                CargarGrilla();
                SeleccionarEnGrilla(vehiculo.Patente);
            }
            catch (Exception ex)
            {
                mensajes.MostrarMensaje("Error: " + ex.Message);
            }
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            try
            {
                if (clienteDelFormulario == null)
                {
                    mensajes.MostrarMensaje("Debe seleccionar un cliente.");
                    return;
                }

                if (modo == Modo.Crear)
                {
                    BE_Vehiculo_96_PP nuevo = LeerCampos();
                    bllVehiculo.Alta(nuevo);
                    mensajes.MostrarMensaje("Vehículo registrado correctamente.");
                    VehiculoRegistrado = nuevo;

                    if (Modal)
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                        return;
                    }

                    CargarGrilla();
                    VolverAModoConsulta();
                    SeleccionarEnGrilla(nuevo.Patente);
                }
                else if (modo == Modo.Modificar)
                {
                    BE_Vehiculo_96_PP vehiculo = VehiculoSeleccionado();
                    if (vehiculo == null)
                        return;

                    BE_Vehiculo_96_PP modificado = LeerCampos();
                    modificado.Patente = vehiculo.Patente;   // la patente es la clave
                    modificado.Activo = vehiculo.Activo;

                    bllVehiculo.Modificar(modificado);
                    mensajes.MostrarMensaje("Vehículo modificado correctamente.");
                    CargarGrilla();
                    VolverAModoConsulta();
                    SeleccionarEnGrilla(modificado.Patente);
                }
            }
            catch (Exception ex)
            {
                mensajes.MostrarMensaje(ex.Message);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (Modal)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            VolverAModoConsulta();
            BE_Vehiculo_96_PP vehiculo = VehiculoSeleccionado();
            if (vehiculo != null)
                MostrarEnCampos(vehiculo);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        // ---------- Helpers de pantalla ----------

        private void VolverAModoConsulta()
        {
            modo = Modo.Consulta;
            HabilitarCampos(false, patenteEditable: false);
            ActualizarModo();
        }

        private void ActualizarModo()
        {
            string texto;
            switch (modo)
            {
                case Modo.Crear: texto = "Modo crear"; break;
                case Modo.Modificar: texto = "Modo modificar"; break;
                default: texto = "Modo consulta"; break;
            }
            lblModo.Text = mensajes.TraducirMensaje(texto);

            bool consulta = modo == Modo.Consulta;
            bool puedeGestionar = TienePermiso(Permisos_96_PP.GestionarVehiculos);

            btnCrear.Enabled = consulta && puedeGestionar;
            btnModificar.Enabled = consulta && puedeGestionar;
            btnActivarDesactivar.Enabled = consulta && puedeGestionar;
            ActualizarBotonEstado();
            btnAplicar.Enabled = !consulta && puedeGestionar;
            btnCancelar.Enabled = !consulta;
            btnBuscar.Enabled = consulta;
            txtBuscarPatente.Enabled = consulta;
            dgvVehiculos.Enabled = consulta;
        }

        private void HabilitarCampos(bool habilitar, bool patenteEditable)
        {
            txtPatente.ReadOnly = !(habilitar && patenteEditable);
            txtMarca.ReadOnly = !habilitar;
            txtModelo.ReadOnly = !habilitar;
            txtDniCliente.ReadOnly = !habilitar;
            btnBuscarCliente.Enabled = habilitar;
        }

        private void MostrarEnCampos(BE_Vehiculo_96_PP v)
        {
            txtPatente.Text = v.Patente;
            txtMarca.Text = v.Marca;
            txtModelo.Text = v.Modelo;
            txtDniCliente.Text = v.Cliente?.Dni;   // primero el texto (dispara TextChanged)...
            clienteDelFormulario = v.Cliente;      // ...después el cliente
            lblNombreCliente.Text = v.NombreCliente;
        }

        // Si la recepcionista cambia el DNI a mano, el cliente anterior deja de valer
        // hasta que vuelva a apretar "Buscar".
        private void txtDniCliente_TextChanged(object sender, EventArgs e)
        {
            if (clienteDelFormulario != null && clienteDelFormulario.Dni != txtDniCliente.Text.Trim())
            {
                clienteDelFormulario = null;
                lblNombreCliente.Text = string.Empty;
            }
        }

        private void LimpiarCampos()
        {
            txtPatente.Clear();
            txtMarca.Clear();
            txtModelo.Clear();
            txtDniCliente.Clear();
            lblNombreCliente.Text = string.Empty;
            clienteDelFormulario = null;
        }

        private BE_Vehiculo_96_PP LeerCampos()
        {
            return new BE_Vehiculo_96_PP(
                txtPatente.Text,
                txtMarca.Text,
                txtModelo.Text,
                clienteDelFormulario);
        }

        // El botón dice lo que va a hacer con el registro seleccionado.
        private void ActualizarBotonEstado()
        {
            BE_Vehiculo_96_PP seleccionado = VehiculoSeleccionado();
            if (seleccionado == null)
                btnActivarDesactivar.Text = mensajes.TraducirMensaje("Activar/Desactivar");
            else
                btnActivarDesactivar.Text = mensajes.TraducirMensaje(seleccionado.Activo ? "Dar de baja" : "Reactivar");
        }

        // Los registros dados de baja se ven en gris.
        private void dgvVehiculos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvVehiculos.Rows.Count)
                return;

            if (dgvVehiculos.Rows[e.RowIndex].DataBoundItem is BE_Vehiculo_96_PP item && !item.Activo)
            {
                e.CellStyle.ForeColor = System.Drawing.Color.Gray;
                e.CellStyle.SelectionForeColor = System.Drawing.Color.LightGray;
            }
        }

        private static bool TienePermiso(string permiso)
        {
            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            return usuario?.Rol != null && usuario.Rol.TienePermiso(permiso);
        }
    }
}
