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

        public FormABMUsuario_96_PP()
        {
            InitializeComponent();

            CargarRoles();

            listaGeneral = new BLL_Usuario_96_PP().obtenerUsuarios().Where(e => e.Dni != SessionManager_96_PP.getInstancia().getUsuarioActivo().Dni).ToList();

            dataGridView1.DataSource = listaGeneral;


            deshabilitarBotonCancelar();
            deshabilitarBotonAplicar();


            //ActualizarIdioma_96_PP();

            comboBox1.SelectedItem = 0;

            actualizar();
            ConfigurarColumnasRolEIdioma();

            ActualizarMensajeModo();

            dataGridView1.Columns["Contraseña"].Visible = false;
            dataGridView1.Columns["Email"].Visible = false;
            dataGridView1.Columns["Bloqueado"].Visible = false;
            dataGridView1.Columns["Activo"].Visible = false;
            dataGridView1.Columns["Idioma"].Visible = false;
            dataGridView1.Columns["Rol"].Visible = false;
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



            DataGridViewRow fila = dataGridView1.SelectedRows[0];


            string codigoRol = ((Usuario_96_PP)fila.DataBoundItem).Rol?.Codigo;
            if (!string.IsNullOrEmpty(codigoRol))
            {
                var items = comboBox1.DataSource as List<Rol_96_PP>;
                int idx = items?.FindIndex(p => p.Codigo == codigoRol) ?? -1;
                if (idx >= 0) comboBox1.SelectedIndex = idx;
            }

            // Cargar datos al formulario
            textBox1.Text = fila.Cells["DNI"].Value.ToString();
            textBox3.Text = fila.Cells["Nombre"].Value.ToString();
            textBox2.Text = fila.Cells["Apellido"].Value.ToString();
            textBox4.Text = fila.Cells["Email"].Value.ToString();
            checkBox1.Checked = true;
            checkBox2.Checked = Convert.ToBoolean(fila.Cells["Bloqueado"].Value);

            if (checkBox2.Checked)
                checkBox1.Checked = false;

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



            //if (modo == "desbloquear")
            //{
            //    try
            //    {

            //        DataGridViewRow fila = dataGridView1.SelectedRows[0];


            //        string dni = fila.Cells["DNI"].Value.ToString();

            //        Usuario_96_PP usuarioADesbloquear = (Usuario_96_PP)new BLL_Usuario_96_PP().obtenerUsuarioPorDni(dni); // to do

            //        if (!usuarioADesbloquear.Bloqueado)
            //        {
            //            MessageBox.Show("No puede desbloquear un usuario que no se encuentra bloqueado");
            //            return;
            //        }

            //        new BLL_Usuario_96_PP().desbloquearUsuario(dni);

            //        MessageBox.Show($"Usuario {usuarioADesbloquear.NombreUsuario} desbloqueado con exito");


            //    }
            //    catch
            //    {
            //        MessageBox.Show("Debe seleccionar un usuario para poder desbloquearlo");
            //    }


            //}

            if (modo == "modificar")
            {

                string dni = textBox1.Text;
                string apellido = textBox2.Text;
                string nombre = textBox3.Text;
                string email = textBox4.Text;
                bool activo = checkBox1.Checked;
                bool bloqueado = checkBox2.Checked;

                DataGridViewRow fila = dataGridView1.SelectedRows[0];

                textBox1.Text = fila.Cells["DNI"].Value.ToString();
                textBox3.Text = fila.Cells["Nombre"].Value.ToString();
                textBox2.Text = fila.Cells["Apellido"].Value.ToString();
                textBox4.Text = fila.Cells["Email"].Value.ToString();
                checkBox1.Checked = true;
                checkBox2.Checked = Convert.ToBoolean(fila.Cells["Bloqueado"].Value);






                if (nombre.Length < 2 || apellido.Length < 2 || dni.Length < 2)
                {
                    new BLL_Idioma_96_PP().MostrarMensaje("Nombre, Apellido y DNI deben tener al menos 2 caracteres.");
                    return;
                }
                string nombreUsuario = fila.Cells["NombreUsuario"].Value.ToString();

                if (activo == bloqueado)
                {

                    new BLL_Idioma_96_PP().MostrarMensaje("Solo puede tener un estado, bloqueado y activo no pueden tener el mismo valor");
                    return;
                }




                Usuario_96_PP user = new Usuario_96_PP(
                    apellido: apellido,
                    bloqueado: bloqueado,
                    contraseña: fila.Cells["Contraseña"].Value.ToString(), //la contraseña no se puede cambiar desde este abm
                    dni: dni,
                    email: email,
                    nombre: nombre,
                    nombreUsuario: nombreUsuario,
                    activo: activo,
                    idioma: ((Usuario_96_PP)fila.DataBoundItem).Idioma,
rol: (Rol_96_PP)comboBox1.SelectedItem);








                BLL.BLL_Usuario_96_PP a = new BLL.BLL_Usuario_96_PP();
                a.modificarUsuario(user);

                actualizar();
                LimpiarCampos();


                new BLL_Idioma_96_PP().MostrarMensaje("Usuario modificado");


                var currentUser = SessionManager_96_PP.getInstancia().getUsuarioActivo();

                Evento_96_PP evento = new Evento_96_PP(currentUser.Dni, DateTime.Now, "Usuarios", "Modificación de usuario", Evento_96_PP.Criticidad.Medio);

                new BLL_BitacoraEvento_96_PP().RegistrarEvento(evento);
        

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

          
            label2.Text = "Numero de usuarios: " + bl.obtenerUsuarios().Count();


            listaGeneral = new BLL_Usuario_96_PP().obtenerUsuarios().Where(e => e.Dni != SessionManager_96_PP.getInstancia().getUsuarioActivo().Dni).ToList();

            dataGridView1.DataSource = listaGeneral;

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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButton1.Checked)
            {
                radioButton2.Checked = true;
            }
            else
            {
                radioButton2.Checked = false;
                dataGridView1.DataSource = listaGeneral.Where(x => x.Activo == true).ToList(); //mostramos solo los usuarios activos, no bloqueados

            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButton2.Checked)
            {
            }
            else
            {
                radioButton1.Checked = false;
                radioButton3.Checked = false;
                dataGridView1.DataSource = listaGeneral;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {

                radioButton1.Checked= false;
                radioButton2.Checked= false;

                dataGridView1.DataSource = listaGeneral.Where(x => x.Bloqueado);


            }
            else
            {
                dataGridView1.DataSource= listaGeneral;
            }
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
