using BE_96_PP;
using BE_96_PP;
using BLL;
using ClassLibrary2;
using ClassLibrary3;
using DAL_96_PP;
using Services_96_PP;
using Servicio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaBase.Administracion
{
    public partial class FormABMUsuario_96_PP : Form, IdiomaObserver_96_PP
    {
        string modo = "";


        public void actualizarIdioma()
        {
            var traductor = new BLL_Idioma_96_PP();
            traductor.Traducir(this);
            ActualizarMensajeModo();
        }

        private void ActualizarMensajeModo()
        {
            string mensaje;
            switch (modo)
            {
                case "crear":
                    mensaje = "Mensaje: \nModo crear";
                    break;
                case "modificar":
                    mensaje = "Mensaje: \nModo modificar";
                    break;
                case "activar/desactivar":
                    mensaje = "Mensaje: \nModo activar/desactivar";
                    break;
                case "desbloquear":
                    mensaje = "Mensaje: \nModo desbloquear";
                    break;
                default:
                    mensaje = "Mensaje: \nModo consulta";
                    break;
            }

            textBox5.Text = new BLL_Idioma_96_PP().TraducirMensaje(mensaje);
        }

        private void VolverAModoConsulta()
        {
            modo = "";
            ActualizarMensajeModo();
            deshabilitarBotonAplicar();
            deshabilitarBotonCancelar();
            AplicarPermisosAcciones();
        }

        List<Usuario_96_PP> listaGeneral;

        // Usuario que se esta editando en modo "modificar". Se guarda al presionar
        // Modificar para no depender de la fila seleccionada al momento de Aplicar
        // (si en el medio se cambiaba el filtro o la seleccion, se guardaban la
        // contraseña y el nombre de usuario de otra persona).
        Usuario_96_PP usuarioEnEdicion;

        public FormABMUsuario_96_PP()
        {
            InitializeComponent();

            CargarRoles();

            deshabilitarBotonCancelar();
            deshabilitarBotonAplicar();

            // Filtro inicial: Todos. Se marca antes de cargar la grilla para que
            // actualizar() sepa que filtro aplicar.
            radioButton2.Checked = true;

            actualizar();

            ActualizarMensajeModo();

            SessionManager_96_PP.getInstancia().Suscribir(this);

            actualizarIdioma();
            AplicarPermisosAcciones();
        }

        private void ConfigurarColumnasRolEIdioma()
        {
            if (!dataGridView1.Columns.Contains("NombreRol"))
            {
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "NombreRol",
                    HeaderText = "Rol",
                    ReadOnly = true,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                });
            }

            if (!dataGridView1.Columns.Contains("NombreIdioma"))
            {
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "NombreIdioma",
                    HeaderText = "Idioma",
                    ReadOnly = true,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                });
            }

            dataGridView1.CellFormatting -= dataGridView1_CellFormatting;
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            string nombreColumna = dataGridView1.Columns[e.ColumnIndex].Name;
            if (nombreColumna != "NombreRol" && nombreColumna != "NombreIdioma")
                return;

            Usuario_96_PP usuario = dataGridView1.Rows[e.RowIndex].DataBoundItem as Usuario_96_PP;
            if (usuario == null)
                return;

            e.Value = nombreColumna == "NombreRol"
                ? usuario.Rol?.Nombre ?? string.Empty
                : usuario.Idioma?.nombre ?? string.Empty;
            e.FormattingApplied = true;
        }

        void deshabilitarBotonAplicar()
        {
            button5.Enabled = false;
        }


        void habilitarBotonAplicar()
        {
            button5.Enabled = true;
        }

        void CargarRoles()
        {
            try
            {
                var bll = new BLL_Rol_96_PP();
                List<Rol_96_PP> roles = bll.ObtenerTodosLosRoles()
                                                .Where(p => p.activo)
                                                .ToList();

                comboBox1.DataSource = roles;
                comboBox1.DisplayMember = "Nombre";
                comboBox1.ValueMember = "Codigo";

                if (roles.Count > 0)
                    comboBox1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                new BLL_Idioma_96_PP().MostrarMensaje("Error al cargar roles: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            modo = "crear";

            ActualizarMensajeModo();

            habilitarBotonAplicar();
            habilitarBotonCancelar();

            deshabilitarBotonActivarDesactivar();
            deshabilitarBotonDesbloquear();
            deshabilitarBotonModificar();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                new BLL_Idioma_96_PP().MostrarMensaje("Debe seleccionar un usuario de la lista.");
                return;
            }



            usuarioEnEdicion = dataGridView1.SelectedRows[0].DataBoundItem as Usuario_96_PP;
            if (usuarioEnEdicion == null)
            {
                new BLL_Idioma_96_PP().MostrarMensaje("Debe seleccionar un usuario de la lista.");
                return;
            }

            string codigoRol = usuarioEnEdicion.Rol?.Codigo;
            if (!string.IsNullOrEmpty(codigoRol))
            {
                var items = comboBox1.DataSource as List<Rol_96_PP>;
                int idx = items?.FindIndex(p => p.Codigo == codigoRol) ?? -1;
                if (idx >= 0) comboBox1.SelectedIndex = idx;
            }

            // Cargar datos al formulario con el estado REAL del usuario
            // (antes Activo se cargaba siempre en true, o en false si estaba bloqueado,
            // y al aplicar se guardaba ese valor inventado).
            textBox1.Text = usuarioEnEdicion.Dni;
            textBox3.Text = usuarioEnEdicion.Nombre;
            textBox2.Text = usuarioEnEdicion.Apellido;
            textBox4.Text = usuarioEnEdicion.Email ?? string.Empty;
            checkBox1.Checked = usuarioEnEdicion.Activo;
            checkBox2.Checked = usuarioEnEdicion.Bloqueado;

            textBox1.Enabled = false;

            HabilitarCampos(true);

            modo = "modificar";

            ActualizarMensajeModo();

            habilitarBotonAplicar();
            habilitarBotonCancelar();

            deshabilitarBotonActivarDesactivar();
            deshabilitarBotonCrear();
            deshabilitarBotonDesbloquear();
            deshabilitarBotonModificar();

        }

        private void button5_Click(object sender, EventArgs e)
        {

            if(modo == "desbloquear")
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {


                    new BLL_Idioma_96_PP().MostrarMensaje("Debe seleccionar un usuario de la tabla");
                    return;
                }

                var fila = dataGridView1.SelectedRows[0];

                string dni = fila.Cells["DNI"].Value.ToString();
                bool bloqueadoActual = Convert.ToBoolean(fila.Cells["Bloqueado"].Value);

                var bll = new BLL_Usuario_96_PP();

                if (bloqueadoActual)
                {
                    bll.desbloquearUsuario(dni);
                }
                else
                {
                    new BLL_Idioma_96_PP().MostrarMensaje("No puede desbloquear a un usuario que no se encuentra bloqueado");
                    return;
                }

                new BLL_Idioma_96_PP().MostrarMensaje("Estado actualizado con éxito.");
                actualizar(); // Esto refresca la lista

                var user = SessionManager_96_PP.getInstancia().getUsuarioActivo();

                Evento_96_PP evento = new Evento_96_PP(
                    user.Dni,
                    DateTime.Now,
                    "Usuarios",
                    $"Desbloqueo de usuario DNI {dni}",
                    Evento_96_PP.Criticidad.Medio
                );

                new BLL_BitacoraEvento_96_PP().RegistrarEvento(evento);

            }


            if (modo == "activar/desactivar")
            {
                if (dataGridView1.SelectedRows.Count == 0) {


                    new BLL_Idioma_96_PP().MostrarMensaje("Debe seleccionar un usuario de la tabla");
                    return;
                }

                var fila = dataGridView1.SelectedRows[0];

                string dni = fila.Cells["DNI"].Value.ToString();
                bool ActivoActual = Convert.ToBoolean(fila.Cells["Activo"].Value);

                  var bll = new BLL_Usuario_96_PP();
                // Invertimos: si está bloqueado, lo desbloqueamos (false) y viceversa


                bll.cambiarEstadoActivo(dni);

                    new BLL_Idioma_96_PP().MostrarMensaje("Estado actualizado con éxito.");
                    actualizar(); // Esto refresca la lista

                var user = SessionManager_96_PP.getInstancia().getUsuarioActivo();

                string accion = ActivoActual ? "Desactivación" : "Activación";

                Evento_96_PP evento = new Evento_96_PP(
                    user.Dni,
                    DateTime.Now,
                    "Usuarios",
                    $"{accion} de usuario DNI {dni}",
                    Evento_96_PP.Criticidad.Alto
                );

                new BLL_BitacoraEvento_96_PP().RegistrarEvento(evento);
               
            }

            if (modo == "modificar")
            {

                if (usuarioEnEdicion == null)
                {
                    new BLL_Idioma_96_PP().MostrarMensaje("Debe seleccionar un usuario de la lista.");
                    return;
                }

                string dni = usuarioEnEdicion.Dni;
                string apellido = textBox2.Text.Trim();
                string nombre = textBox3.Text.Trim();
                string email = textBox4.Text.Trim();
                bool activo = checkBox1.Checked;
                bool bloqueado = checkBox2.Checked;

                if (nombre.Length < 2 || apellido.Length < 2 || dni.Length < 2)
                {
                    new BLL_Idioma_96_PP().MostrarMensaje("Nombre, Apellido y DNI deben tener al menos 2 caracteres.");
                    return;
                }

                if (comboBox1.SelectedItem == null)
                {
                    new BLL_Idioma_96_PP().MostrarMensaje("Debe seleccionar un rol", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Se quito la regla "activo y bloqueado no pueden ser iguales": el propio
                // sistema genera esos estados (el login bloquea dejando Activo = 1 y
                // Act./Desact. deja Bloqueado = 0), asi que esos usuarios no se podian modificar.

                Usuario_96_PP user = new Usuario_96_PP(
                    apellido: apellido,
                    bloqueado: bloqueado,
                    contraseña: usuarioEnEdicion.Contraseña, //la contraseña no se puede cambiar desde este abm
                    dni: dni,
                    email: email,
                    nombre: nombre,
                    nombreUsuario: usuarioEnEdicion.NombreUsuario,
                    activo: activo,
                    idioma: usuarioEnEdicion.Idioma,
                    rol: (Rol_96_PP)comboBox1.SelectedItem);

                try
                {
                    new BLL_Usuario_96_PP().modificarUsuario(user);
                }
                catch (Exception ex)
                {
                    new BLL_Idioma_96_PP().MostrarMensaje($"Error al modificar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                actualizar();
                LimpiarCampos();

                new BLL_Idioma_96_PP().MostrarMensaje("Usuario modificado");

                var currentUser = SessionManager_96_PP.getInstancia().getUsuarioActivo();

                Evento_96_PP evento = new Evento_96_PP(currentUser.Dni, DateTime.Now, "Usuarios", "Modificación de usuario", Evento_96_PP.Criticidad.Medio);

                new BLL_BitacoraEvento_96_PP().RegistrarEvento(evento);

                usuarioEnEdicion = null;
            }

            if(modo == "crear")
            {

                try
                {
                    // Validaciones básicas
                    if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                        string.IsNullOrWhiteSpace(textBox2.Text) ||
                        string.IsNullOrWhiteSpace(textBox3.Text) ||
                        string.IsNullOrWhiteSpace(textBox4.Text))
                    {
                        new BLL_Idioma_96_PP().MostrarMensaje("Todos los campos son obligatorios", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (comboBox1.SelectedItem == null)
                    {
                        new BLL_Idioma_96_PP().MostrarMensaje("Debe seleccionar un rol", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // El nombre de usuario se arma con Substring de 2 caracteres de cada campo:
                    // con menos de 2 caracteres el Substring tiraba excepcion.
                    if (textBox1.Text.Trim().Length < 2 || textBox2.Text.Trim().Length < 2 || textBox3.Text.Trim().Length < 2)
                    {
                        new BLL_Idioma_96_PP().MostrarMensaje("Nombre, Apellido y DNI deben tener al menos 2 caracteres.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(textBox4.Text))
                    {
                        new BLL_Idioma_96_PP().MostrarMensaje("Debe ingresar un email");
                        return;
                    }

                    if(checkBox1.Checked == checkBox2.Checked)
                    {
                        new BLL_Idioma_96_PP().MostrarMensaje("Los botones de activo y bloqueado no pueden ser iguales"); return;
                    }


                    string dni = textBox1.Text;

                    if (new BLL_Usuario_96_PP().obtenerUsuarios().Any(ee => ee.Dni == dni))
                    {
                        new BLL_Idioma_96_PP().MostrarMensaje("No se puede crear un usuario con ese DNI.");
                        return;
                    }


                    // Crear nombre de usuario, es la combinacion de los primeros dos digitos de nombre + apellido + dni ultimos 2 digitos
                    string nombreUsuario = textBox3.Text.Substring(0, 2) +
                                           textBox2.Text.Substring(0, 2) +
                                           textBox1.Text.Substring(textBox1.Text.Length - 2);

              

                    // Crear objeto Usuario
                    var usuario = new Usuario_96_PP(
                        apellido: textBox2.Text,
                        bloqueado: checkBox2.Checked,
                        contraseña: CryptoManager_96_PP.Encriptar(textBox1.Text + textBox2.Text),
                        dni: textBox1.Text,
                        email: textBox4.Text,
                        nombre: textBox3.Text,
                        nombreUsuario: nombreUsuario,
                        idioma: new Idioma_96_PP("ES"),
                        activo: checkBox1.Checked,
                        rol: (Rol_96_PP)comboBox1.SelectedItem
                        );


                    // Guardar usuario
                    var usuarioBLL = new BLL_Usuario_96_PP();
                    usuarioBLL.crearUsuario(usuario);



                    new BLL_Idioma_96_PP().MostrarMensaje("Usuario creado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    string aa = SessionManager_96_PP.getInstancia().getUsuarioActivo().Dni;

                    Evento_96_PP eee = new Evento_96_PP(aa, DateTime.Now, "Usuarios", "Creacion de usuario", Evento_96_PP.Criticidad.Bajo);
                    new BLL_BitacoraEvento_96_PP().RegistrarEvento(eee);

       

                }
                catch (Exception ex)
                {
                    new BLL_Idioma_96_PP().MostrarMensaje($"Error al crear el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                actualizar();
                LimpiarCampos();
             }


            VolverAModoConsulta();
        }

        void habilitarBotonCancelar()
        {
            button6.Enabled = true;
        }

        void deshabilitarBotonCancelar()
        {
            button6.Enabled = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            usuarioEnEdicion = null;

            new BLL_Idioma_96_PP().MostrarMensaje("Operacion cancelada");

            VolverAModoConsulta();

        }


        private void LimpiarCampos()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            textBox1.Enabled = true;
        }


        void actualizar()
        {
            BLL_Usuario_96_PP bl = new BLL_Usuario_96_PP
             ();

          
            List<Usuario_96_PP> todos = bl.obtenerUsuarios();

            label2.Text = "Numero de usuarios: " + todos.Count;

            string dniActual = SessionManager_96_PP.getInstancia().getUsuarioActivo().Dni;
            listaGeneral = todos.Where(e => e.Dni != dniActual).ToList();

            // Antes se asignaba siempre la lista completa, ignorando el filtro marcado.
            AplicarFiltro();
        }

        // Muestra en la grilla la lista segun el radio button marcado.
        // Siempre con .ToList(): el DataGridView necesita un IList como DataSource.
        private void AplicarFiltro()
        {
            if (listaGeneral == null)
                return;

            if (radioButton1.Checked)
                dataGridView1.DataSource = listaGeneral.Where(x => x.Activo && !x.Bloqueado).ToList();
            else if (radioButton3.Checked)
                dataGridView1.DataSource = listaGeneral.Where(x => x.Bloqueado).ToList();
            else
                dataGridView1.DataSource = listaGeneral;

            ConfigurarColumnasRolEIdioma();
            OcultarColumnas();
        }

        private void OcultarColumnas()
        {
            foreach (string columna in new[] { "Contraseña", "Email", "Bloqueado", "Activo", "Idioma", "Rol" })
            {
                if (dataGridView1.Columns.Contains(columna))
                    dataGridView1.Columns[columna].Visible = false;
            }
        }

        //private void CargarRoles()
        ////{
        ////    var rolBLL = new BLLRol_96_PP();
        //    var roles = rolBLL.ObtenerRolesSimples96_PP();
        //    comboBox1.DataSource = roles;
        //    comboBox1.DisplayMember = "Nombre_96_PP";
        //    comboBox1.ValueMember = "Nombre_96_PP";
        //}



        private void HabilitarCampos(bool habilitar)
        {
            textBox1.Enabled = habilitar;
            textBox2.Enabled = habilitar;
            textBox3.Enabled = habilitar;
            textBox4.Enabled = habilitar;

            checkBox1.Enabled = habilitar;
            checkBox2.Enabled = habilitar;

            button5.Enabled = habilitar;
        }


        void habilitarBotonCrear()
        {
            button1.Enabled = TienePermiso(Permisos_96_PP.CrearUsuario);
        }

        void deshabilitarBotonCrear()
        {
            button1.Enabled = false;
        }

        void habilitarBotonModificar()
        {
            button3.Enabled = TienePermiso(Permisos_96_PP.ModificarUsuario);
        }

        void deshabilitarBotonModificar()
        {
            button3 .Enabled = false;
        }

        void habilitarBotonActivarDesactivar()
        {
            btnActivarDesactivar.Enabled = TienePermiso(Permisos_96_PP.ActivarDesactivarUsuario);
        }

        void deshabilitarBotonActivarDesactivar()
        {
            btnActivarDesactivar.Enabled= false;
        }

        void habilitarBotonDesbloquear()
        {
            button2.Enabled = TienePermiso(Permisos_96_PP.DesbloquearUsuario);
            
        }

        void deshabilitarBotonDesbloquear()
        {
            button2.Enabled = false;
        }




        private void FormABMUsuario_96_PP_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnActivarDesactivar_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count == 0)
            {
                new BLL_Idioma_96_PP().MostrarMensaje("Debe seleccionar un usuario de la lista.");
                return;
            }



            modo = "activar/desactivar";

            ActualizarMensajeModo();

            habilitarBotonAplicar();
            habilitarBotonCancelar();
            deshabilitarBotonModificar();
            deshabilitarBotonCrear();
            deshabilitarBotonActivarDesactivar();
            deshabilitarBotonDesbloquear();


        }

        // Los tres radio buttons estan en el mismo contenedor, asi que WinForms ya los
        // hace excluyentes solo. Antes cada handler destildaba/tildaba a los otros a mano
        // y eso disparaba los CheckedChanged en cadena (ej: estando en "Activos" y
        // tocando "Bloqueados" terminaba marcado "Todos"). Ahora cada uno solo filtra
        // cuando queda marcado.
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                AplicarFiltro();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                AplicarFiltro();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
                AplicarFiltro();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count == 0)
            {


                new BLL_Idioma_96_PP().MostrarMensaje("Debe seleccionar un usuario de la tabla");
                return;
            }

            modo = "desbloquear";

            ActualizarMensajeModo();

            habilitarBotonAplicar();
            habilitarBotonCancelar();

            deshabilitarBotonDesbloquear();
            deshabilitarBotonCrear();
            deshabilitarBotonActivarDesactivar();
            deshabilitarBotonModificar();


        }

        private void AplicarPermisosAcciones()
        {
            habilitarBotonCrear();
            habilitarBotonModificar();
            habilitarBotonActivarDesactivar();
            habilitarBotonDesbloquear();
        }

        private bool TienePermiso(string permiso)
        {
            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            return usuario?.Rol != null && usuario.Rol.TienePermiso(permiso);
        }
    }
}
