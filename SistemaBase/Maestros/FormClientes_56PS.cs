using BE_56_PS;
using BLL;
using ClassLibrary2;
using Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SistemaBase.Maestros
{
    public partial class FormClientes_56PS : Form, IdiomaObserver_56PS
    {
        private enum Modo { Consulta, Crear, Modificar }

        private readonly BLL_Cliente_56PS bllCliente = new BLL_Cliente_56PS();
        private readonly BLL_Idioma_56PS mensajes = new BLL_Idioma_56PS();

        private Modo modo = Modo.Consulta;
        private List<BE_Cliente_56PS> clientes = new List<BE_Cliente_56PS>();

        // Si viene desde Turnos: DNI que hay que registrar.
        private readonly string dniARegistrar;

        // Cliente dado de alta en esta pantalla (lo lee FormTurnos_56PS).
        public BE_Cliente_56PS ClienteRegistrado { get; private set; }

        public FormClientes_56PS() : this(null)
        {
        }

        public FormClientes_56PS(string dniARegistrar)
        {
            InitializeComponent();
            this.dniARegistrar = dniARegistrar;

            dgvClientes.CellFormatting += dgvClientes_CellFormatting;

            SessionManager_56PS.getInstancia().Suscribir(this);
            actualizarIdioma();
        }

        public void actualizarIdioma()
        {
            mensajes.Traducir(this);
            Text = mensajes.TraducirMensaje("Clientes");
            ActualizarModo();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            SessionManager_56PS.getInstancia().Desuscribir(this);
            base.OnFormClosed(e);
        }

        private void FormClientes_56PS_Load(object sender, EventArgs e)
        {
            CargarGrilla();
            VolverAModoConsulta();

            if (!string.IsNullOrWhiteSpace(dniARegistrar))
            {
                IniciarAlta(dniARegistrar);
            }
        }


        private void CargarGrilla()
        {
            clientes = bllCliente.ObtenerTodos();
            MostrarClientes(clientes);
        }

        private void MostrarClientes(IEnumerable<BE_Cliente_56PS> lista)
        {
            IEnumerable<BE_Cliente_56PS> filtrada = chkVerInactivos.Checked ? lista : lista.Where(c => c.Activo);

            dgvClientes.DataSource = null;
            dgvClientes.DataSource = filtrada.ToList();
        }

        private void dgvClientes_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarBotonEstado();

            if (modo != Modo.Consulta)
                return;

            BE_Cliente_56PS cliente = ClienteSeleccionado();
            if (cliente != null)
                MostrarEnCampos(cliente);
        }

        private BE_Cliente_56PS ClienteSeleccionado()
        {
            return dgvClientes.CurrentRow?.DataBoundItem as BE_Cliente_56PS;
        }

        private void chkVerInactivos_CheckedChanged(object sender, EventArgs e)
        {
            MostrarClientes(clientes);
        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string dni = txtBuscarDni.Text.Trim();
            if (string.IsNullOrEmpty(dni))
            {
                MostrarClientes(clientes);
                return;
            }

            BE_Cliente_56PS encontrado = bllCliente.BuscarPorDni(dni);
            if (encontrado != null)
            {
                if (!encontrado.Activo && !chkVerInactivos.Checked)
                    chkVerInactivos.Checked = true;

                SeleccionarEnGrilla(encontrado.Dni);
                MostrarEnCampos(encontrado);
                return;
            }

            // Paso 3: no está registrado -> se ofrece darlo de alta.
            DialogResult respuesta = MessageBox.Show(
                mensajes.TraducirMensaje("El cliente no está registrado. ¿Desea registrarlo?"),
                mensajes.TraducirMensaje("Clientes"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
                IniciarAlta(dni);
        }

        private void SeleccionarEnGrilla(string dni)
        {
            foreach (DataGridViewRow fila in dgvClientes.Rows)
            {
                if (fila.DataBoundItem is BE_Cliente_56PS c && c.Dni == dni)
                {
                    dgvClientes.CurrentCell = fila.Cells.Cast<DataGridViewCell>().First(celda => celda.Visible);
                    return;
                }
            }
        }


        private void btnCrear_Click(object sender, EventArgs e)
        {
            IniciarAlta(null);
        }

        private void IniciarAlta(string dni)
        {
            modo = Modo.Crear;
            LimpiarCampos();
            txtDni.Text = dni ?? string.Empty;
            HabilitarCampos(true, dniEditable: true);
            ActualizarModo();
            (string.IsNullOrEmpty(dni) ? (Control)txtDni : txtNombre).Focus();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            BE_Cliente_56PS cliente = ClienteSeleccionado();
            if (cliente == null)
            {
                mensajes.MostrarMensaje("Debe seleccionar un cliente.");
                return;
            }

            modo = Modo.Modificar;
            MostrarEnCampos(cliente);
            HabilitarCampos(true, dniEditable: false);
            ActualizarModo();
        }

        private void btnActivarDesactivar_Click(object sender, EventArgs e)
        {
            BE_Cliente_56PS cliente = ClienteSeleccionado();
            if (cliente == null)
            {
                mensajes.MostrarMensaje("Debe seleccionar un cliente.");
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                mensajes.TraducirMensaje(cliente.Activo ? "¿Desea dar de baja al cliente?" : "¿Desea reactivar al cliente?"),
                mensajes.TraducirMensaje("Clientes"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
                return;

            try
            {
                bllCliente.CambiarEstadoActivo(cliente);
                mensajes.MostrarMensaje(cliente.Activo ? "Cliente reactivado." : "Cliente dado de baja.");
                CargarGrilla();
                SeleccionarEnGrilla(cliente.Dni);
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
                if (modo == Modo.Crear)
                {
                    BE_Cliente_56PS nuevo = LeerCampos();
                    bllCliente.Alta(nuevo);
                    mensajes.MostrarMensaje("Cliente registrado correctamente.");
                    ClienteRegistrado = nuevo;

                    if (Modal)
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                        return;
                    }

                    CargarGrilla();
                    VolverAModoConsulta();
                    SeleccionarEnGrilla(nuevo.Dni);
                }
                else if (modo == Modo.Modificar)
                {
                    BE_Cliente_56PS cliente = ClienteSeleccionado();
                    if (cliente == null)
                        return;

                    BE_Cliente_56PS modificado = LeerCampos();
                    modificado.Dni = cliente.Dni;          
                    modificado.Activo = cliente.Activo;

                    bllCliente.Modificar(modificado);
                    mensajes.MostrarMensaje("Cliente modificado correctamente.");
                    CargarGrilla();
                    VolverAModoConsulta();
                    SeleccionarEnGrilla(modificado.Dni);
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
            BE_Cliente_56PS cliente = ClienteSeleccionado();
            if (cliente != null)
                MostrarEnCampos(cliente);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void VolverAModoConsulta()
        {
            modo = Modo.Consulta;
            HabilitarCampos(false, dniEditable: false);
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
            bool puedeGestionar = TienePermiso(Permisos_56PS.GestionarClientes);

            btnCrear.Enabled = consulta && puedeGestionar;
            btnModificar.Enabled = consulta && puedeGestionar;
            btnActivarDesactivar.Enabled = consulta && puedeGestionar;
            ActualizarBotonEstado();
            btnAplicar.Enabled = !consulta && puedeGestionar;
            btnCancelar.Enabled = !consulta;
            btnBuscar.Enabled = consulta;
            txtBuscarDni.Enabled = consulta;
            dgvClientes.Enabled = consulta;
        }

        private void HabilitarCampos(bool habilitar, bool dniEditable)
        {
            txtDni.ReadOnly = !(habilitar && dniEditable);
            txtNombre.ReadOnly = !habilitar;
            txtApellido.ReadOnly = !habilitar;
            txtTelefono.ReadOnly = !habilitar;
            txtDireccion.ReadOnly = !habilitar;
        }

        private void MostrarEnCampos(BE_Cliente_56PS c)
        {
            txtDni.Text = c.Dni;
            txtNombre.Text = c.Nombre;
            txtApellido.Text = c.Apellido;
            txtTelefono.Text = c.Telefono;
            txtDireccion.Text = c.Direccion;
        }

        private void LimpiarCampos()
        {
            txtDni.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
        }

        private BE_Cliente_56PS LeerCampos()
        {
            return new BE_Cliente_56PS(
                txtDni.Text,
                txtNombre.Text,
                txtApellido.Text,
                txtTelefono.Text,
                txtDireccion.Text);
        }

        private void dgvClientes_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Columnas visibles: DNI, Apellido, Nombre, Teléfono, Dirección, Activo.
            if (dgvClientes.Columns.Contains("Dni")) dgvClientes.Columns["Dni"].DisplayIndex = 0;
            if (dgvClientes.Columns.Contains("Apellido")) dgvClientes.Columns["Apellido"].DisplayIndex = 1;
        }

        // El botón dice lo que va a hacer con el registro seleccionado.
        private void ActualizarBotonEstado()
        {
            BE_Cliente_56PS seleccionado = ClienteSeleccionado();
            if (seleccionado == null)
                btnActivarDesactivar.Text = mensajes.TraducirMensaje("Activar/Desactivar");
            else
                btnActivarDesactivar.Text = mensajes.TraducirMensaje(seleccionado.Activo ? "Dar de baja" : "Reactivar");
        }

        // Los registros dados de baja se ven en gris.
        private void dgvClientes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvClientes.Rows.Count)
                return;

            if (dgvClientes.Rows[e.RowIndex].DataBoundItem is BE_Cliente_56PS item && !item.Activo)
            {
                e.CellStyle.ForeColor = System.Drawing.Color.Gray;
                e.CellStyle.SelectionForeColor = System.Drawing.Color.LightGray;
            }
        }

        private static bool TienePermiso(string permiso)
        {
            var usuario = SessionManager_56PS.getInstancia().getUsuarioActivo();
            return usuario?.Rol != null && usuario.Rol.TienePermiso(permiso);
        }
    }
}
