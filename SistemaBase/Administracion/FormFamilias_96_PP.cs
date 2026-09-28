using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using BE_96_PP;
using ClassLibrary2;
using Servicio;
using ClassLibrary3;
using DAL_96_PP;

namespace SistemaBase.Administracion
{
    public partial class FormFamilias_96_PP : Form, IdiomaObserver_96_PP
    {
        BLL_Familia_96_PP bllfamilia = new BLL_Familia_96_PP();
        BLL_Rol_96_PP bllrol = new BLL_Rol_96_PP();
        BLL_Patente_96_PP bllpatentes = new BLL_Patente_96_PP();
        Familia_96_PP p = new Familia_96_PP();

        public FormFamilias_96_PP()
        {
            InitializeComponent();

            cmbfa.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbpermiso.DropDownStyle = ComboBoxStyle.DropDownList;
            this.AutoScroll = true;
            MostrarFamilias();
            MostrarPermisos();
            MostrarFamiliasenDatagrid();

            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ClearSelection();

            dataGridView1.CellClick += dataGridView1_CellClick;

            SessionManager_96_PP.getInstancia().Suscribir(this);
            actualizarIdioma();
            AplicarPermisosAcciones();
        }

 
        public void actualizarIdioma()
        {
            var traductor = new BLL_Idioma_96_PP();
            traductor.Traducir(this);
        }

 
        public void OcultarColumnas()
        {
            if (dataGridView1.Columns.Contains("esfamilia"))
            {
                dataGridView1.Columns["esfamilia"].Visible = false;
            }
            if (dataGridView1.Columns.Contains("activo"))
            {
                dataGridView1.Columns["activo"].Visible = false;
            }
            if (dataGridView1.Columns.Contains("hijos"))
            {
                dataGridView1.Columns["hijos"].Visible = false;
            }
        }

        public void MostrarFamiliasenDatagrid()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = bllfamilia.ObtenerFamilias();
            OcultarColumnas();
        }

        public void MostrarFamilias()
        {
            List<Familia_96_PP> familias = bllfamilia.ObtenerFamilias();
            cmbfa.DataSource = null;
            cmbfa.DisplayMember = "Nombre";
            cmbfa.ValueMember = "Codigo";
            cmbfa.DataSource = familias;
            cmbfa.SelectedIndex = -1;
        }

        private void MostrarFamiliaEnTreeView(string codigoFamilia)
        {
            treeView1.Nodes.Clear();
            try
            {
                var familiaRaiz = bllfamilia.ObtenerFamilia(codigoFamilia);
                TreeNode nodoRaiz = new TreeNode(familiaRaiz.Nombre);
                nodoRaiz.Tag = familiaRaiz;
                AgregarNodosRecursivos(nodoRaiz, familiaRaiz.hijos);
                treeView1.Nodes.Add(nodoRaiz);
                treeView1.ExpandAll();
            }
            catch (Exception ex)
            {
                new BLL_Idioma_96_PP().MostrarMensaje(ex.Message);
            }
        }

        private void AgregarNodosRecursivos(TreeNode nodoPadre, List<Rol_96_PP> hijos)
        {
            foreach (var hijo in hijos)
            {
                TreeNode nuevoNodo = new TreeNode(hijo.Nombre);
                nuevoNodo.Tag = hijo;
                nodoPadre.Nodes.Add(nuevoNodo);

                if (hijo.esfamilia && hijo.hijos != null && hijo.hijos.Count > 0)
                {
                    AgregarNodosRecursivos(nuevoNodo, hijo.hijos);
                }
            }
        }

        public void MostrarPermisos()
        {
            try
            {
                List<Patente_96_PP> patentes = bllpatentes.ObtenerPatentes();
                cmbpermiso.DataSource = null;
                cmbpermiso.DisplayMember = "Nombre";
                cmbpermiso.ValueMember = "Codigo";
                cmbpermiso.DataSource = patentes;
                cmbpermiso.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                new BLL_Idioma_96_PP().MostrarMensaje("Error en MostrarPermisos: " + ex.Message);
            }
        }

      
         private void button2_Click(object sender, EventArgs e)
        {
            p = new Familia_96_PP();
            if (cmbfa.SelectedItem == null)
            {
                new BLL_Idioma_96_PP().MostrarMensaje("Seleccionar una familia");
                return;
            }

            Familia_96_PP familiaSeleccionada = cmbfa.SelectedItem as Familia_96_PP;
            if (familiaSeleccionada == null)
            {
                new BLL_Idioma_96_PP().MostrarMensaje("Seleccionar una familia");
                return;
            }

            if (string.IsNullOrEmpty(txtcod.Text))
            {
                new BLL_Idioma_96_PP().MostrarMensaje("Ingresar el código de la familia, seleccionar del datagrid");
                return;
            }

            string codigofamilia = txtcod.Text;
            try
            {
                p = bllfamilia.ObtenerFamilia(codigofamilia);
            }
            catch (Exception ex)
            {
                new BLL_Idioma_96_PP().MostrarMensaje(ex.Message);
                return;
            }

            Familia_96_PP familia;
            try
            {
                familia = bllfamilia.ObtenerFamilia(familiaSeleccionada.Codigo);
            }
            catch (Exception ex)
            {
                new BLL_Idioma_96_PP().MostrarMensaje(ex.Message);
                return;
            }

            try
            {
                p.Agregar(familia);
                bllfamilia.AsignarPermisoAFamilia(p, familia);
                MostrarFamiliaEnTreeView(p.Codigo);
                cmbfa.SelectedIndex = -1;

                string dniUser = SessionManager_96_PP.getInstancia().getUsuarioActivo().Dni;
                Evento_96_PP ev = new Evento_96_PP(dniUser, DateTime.Now, "Familias", "Asignación de familia a familia", Evento_96_PP.Criticidad.Medio);
                new BLL_BitacoraEvento_96_PP().RegistrarEvento(ev);
            }
            catch (Exception ex)
            {
                new BLL_Idioma_96_PP().MostrarMensaje(ex.Message);
            }

            Limpiar();
        }

         private void button4_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            Limpiar();
        }

        public void Limpiar()
        {
            txtcod.Text = "";
            txtnomb.Text = "";
            cmbfa.SelectedIndex = -1;
            cmbpermiso.SelectedIndex = -1;
            p = new Familia_96_PP();
        }

         private void btncancelar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtcod.Text))
            {
                new BLL_Idioma_96_PP().MostrarMensaje("Ingresar el código de la familia");
                return;
            }

            Familia_96_PP f = new Familia_96_PP();
            f.Codigo = txtcod.Text;
            try
            {
                bllfamilia.EliminarFamilia(f);
                cmbfa.Items.Clear();
                MostrarFamilias();
                MostrarFamiliasenDatagrid();
                treeView1.Nodes.Clear();

                string dniUser = SessionManager_96_PP.getInstancia().getUsuarioActivo().Dni;
                Evento_96_PP ev = new Evento_96_PP(dniUser, DateTime.Now, "Familias", "Eliminación de familia", Evento_96_PP.Criticidad.Alto);
                new BLL_BitacoraEvento_96_PP().RegistrarEvento(ev);
            }
            catch (Exception ex)
            {
                new BLL_Idioma_96_PP().MostrarMensaje(ex.Message);
            }
            Limpiar();
        }

         private void button6_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                new BLL_Idioma_96_PP().MostrarMensaje("Debe seleccionar un permiso o familia para eliminar.");
                return;
            }

            TreeNode nodoseleccionado = treeView1.SelectedNode;
            if (nodoseleccionado.Parent == null)
            {
                new BLL_Idioma_96_PP().MostrarMensaje("No se puede eliminar la familia raíz desde el árbol.");
                return;
            }

            Rol_96_PP permiso = nodoseleccionado.Tag as Rol_96_PP;

            if (permiso == null)
            {
                new BLL_Idioma_96_PP().MostrarMensaje("Error: No se pudo obtener la información del elemento seleccionado.");
                return;
            }

            Familia_96_PP familiaPadre = nodoseleccionado.Parent.Tag as Familia_96_PP;
            if (familiaPadre == null)
            {
                new BLL_Idioma_96_PP().MostrarMensaje("No se pudo obtener la familia que contiene el elemento seleccionado.");
                return;
            }

            try
            {
            bllfamilia.EliminarPermisodeFamilia(permiso, familiaPadre.Codigo);
            new BLL_Idioma_96_PP().MostrarMensaje($"'{permiso.Nombre}' fue eliminado correctamente.");

            string dniUser = SessionManager_96_PP.getInstancia().getUsuarioActivo().Dni;
            Evento_96_PP ev = new Evento_96_PP(dniUser, DateTime.Now, "Familias", "Eliminación de permiso/familia de familia", Evento_96_PP.Criticidad.Medio);
            new BLL_BitacoraEvento_96_PP().RegistrarEvento(ev);


            MostrarFamiliaEnTreeView(txtcod.Text);
            }
            catch (Exception ex)
            {
                new BLL_Idioma_96_PP().MostrarMensaje(ex.Message);
            }
        }

         private void btnmodificar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtnomb.Text) || string.IsNullOrEmpty(txtcod.Text))
            {
                new BLL_Idioma_96_PP().MostrarMensaje("Ingresar el nombre y código de la familia");
                return;
            }

            Familia_96_PP f = new Familia_96_PP();
            f.Nombre = txtnomb.Text;
            f.Codigo = txtcod.Text;
            try
            {
                bllfamilia.ModificarFamilia(f);
                MostrarFamiliaEnTreeView(f.Codigo);
                MostrarFamiliasenDatagrid();
                MostrarFamilias();
                treeView1.Nodes.Clear();
                txtcod.Text = "";
                txtnomb.Text = "";

                string dniUser = SessionManager_96_PP.getInstancia().getUsuarioActivo().Dni;
                Evento_96_PP ev = new Evento_96_PP(dniUser, DateTime.Now, "Familias", "Modificación de familia", Evento_96_PP.Criticidad.Medio);
                new BLL_BitacoraEvento_96_PP().RegistrarEvento(ev);

           
            }
            catch (Exception ex)
            {
                new BLL_Idioma_96_PP().MostrarMensaje(ex.Message);
            }
        }

         private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string cod = row.Cells["Codigo"].Value.ToString();
                string nom = row.Cells["Nombre"].Value.ToString();
                txtcod.Text = cod;
                txtnomb.Text = nom;
                MostrarFamiliaEnTreeView(cod);
            }
        }

        private void FormFamilias_96_PP_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
             p = new Familia_96_PP();
            if (cmbpermiso.SelectedItem == null)
            {
                 new BLL_Idioma_96_PP().MostrarMensaje("Seleccionar un permiso");
                return;
            }

            Patente_96_PP patenteSeleccionada = cmbpermiso.SelectedItem as Patente_96_PP;
            if (patenteSeleccionada == null)
            {
                new BLL_Idioma_96_PP().MostrarMensaje("Seleccionar un permiso");
                return;
            }
            if (string.IsNullOrEmpty(txtcod.Text))
            {
                 new BLL_Idioma_96_PP().MostrarMensaje("Ingresar el código de la familia, seleccionar del datagrid");
                return;
            }

            string codigofamilia = txtcod.Text;
             try
            {
                p = bllfamilia.ObtenerFamilia(codigofamilia);
            }
            catch (Exception ex)
            {
                new BLL_Idioma_96_PP().MostrarMensaje((ex.Message));
                return;
            }
            Patente_96_PP patente = new Patente_96_PP();
            patente.Nombre = patenteSeleccionada.Nombre;
            patente.Codigo = patenteSeleccionada.Codigo;
            patente.esfamilia = false;

            try
            {
                p.Agregar(patente);

                bllfamilia.AsignarPermisoAFamilia(p, patente);
                p = bllfamilia.ObtenerFamilia(codigofamilia);
                MostrarFamiliaEnTreeView(p.Codigo);
            }
            catch (Exception ex)
            {
                new BLL_Idioma_96_PP().MostrarMensaje((ex.Message));
            }

            Limpiar();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnbuscar_Click_1(object sender, EventArgs e)
        {
            string codigo = txtcod.Text.Trim();
            string nombre = txtnomb.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(codigo))
            {
                new BLL_Idioma_96_PP().MostrarMensaje("Ingresar el nombre y código de la familia");
                return;
            }

            string pattern = @"^.{1,10}$";
            if (!Regex.IsMatch(codigo, pattern))
            {
                new BLL_Idioma_96_PP().MostrarMensaje("El código no puede tener más de 10 caracteres.");
                return;
            }

            List<Rol_96_PP> elementosIniciales = ObtenerElementosIniciales();
            if (elementosIniciales.Count == 0)
            {
                new BLL_Idioma_96_PP().MostrarMensaje("Debe seleccionar al menos un permiso o una familia para crearla.");
                return;
            }

            Familia_96_PP familiaNueva = new Familia_96_PP
            {
                Codigo = codigo,
                Nombre = nombre,
                activo = true
            };

            try
            {
                bllfamilia.CrearFamilia(familiaNueva, elementosIniciales);
            }
            catch (Exception ex)
            {
                new BLL_Idioma_96_PP().MostrarMensaje(ex.Message);
                return;
            }

            Limpiar();

            try
            {
                MostrarFamiliasenDatagrid();
                MostrarFamilias();
                MostrarFamiliaEnTreeView(codigo);
            }
            catch (Exception ex)
            {
                new BLL_Idioma_96_PP().MostrarMensaje(ex.Message);
            }
        }

        private List<Rol_96_PP> ObtenerElementosIniciales()
        {
            List<Rol_96_PP> elementos = new List<Rol_96_PP>();

            Patente_96_PP patente = cmbpermiso.SelectedItem as Patente_96_PP;
            if (patente != null)
            {
                elementos.Add(new Patente_96_PP
                {
                    Codigo = patente.Codigo,
                    Nombre = patente.Nombre,
                    esfamilia = false
                });
            }

            Familia_96_PP familia = cmbfa.SelectedItem as Familia_96_PP;
            if (familia != null)
            {
                elementos.Add(new Familia_96_PP
                {
                    Codigo = familia.Codigo,
                    Nombre = familia.Nombre,
                    esfamilia = true
                });
            }

            return elementos;
        }

        private void btnmodificar_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtnomb.Text) || string.IsNullOrEmpty(txtcod.Text))
            {
                new BLL_Idioma_96_PP().MostrarMensaje("Ingresar el nombre y código de la familia");
                return;
            }

            Familia_96_PP f = new Familia_96_PP();
            f.Nombre = txtnomb.Text;
            f.Codigo = txtcod.Text;
            try
            {
                bllfamilia.ModificarFamilia(f);
                MostrarFamiliaEnTreeView(f.Codigo);
                MostrarFamiliasenDatagrid();
                MostrarFamilias();
                treeView1.Nodes.Clear();
                txtcod.Text = "";
                txtnomb.Text = "";

                string dniUser = SessionManager_96_PP.getInstancia().getUsuarioActivo().Dni;
                Evento_96_PP ev = new Evento_96_PP(dniUser, DateTime.Now, "Familias", "Modificación de familia", Evento_96_PP.Criticidad.Medio);
                new BLL_BitacoraEvento_96_PP().RegistrarEvento(ev);

            }
            catch (Exception ex)
            {
                new BLL_Idioma_96_PP().MostrarMensaje(ex.Message);
            }
        }

        private void btncancelar_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtcod.Text))
            {
                new BLL_Idioma_96_PP().MostrarMensaje("Ingresar el código de la familia");
                return;
            }

            Familia_96_PP f = new Familia_96_PP();
            f.Codigo = txtcod.Text;
            try
            {
                bllfamilia.EliminarFamilia(f);
                MostrarFamilias();
                MostrarFamiliasenDatagrid();
                treeView1.Nodes.Clear();

                string dniUser = SessionManager_96_PP.getInstancia().getUsuarioActivo().Dni;
                Evento_96_PP ev = new Evento_96_PP(dniUser, DateTime.Now, "Familias", "Eliminación de familia", Evento_96_PP.Criticidad.Alto);
                new BLL_BitacoraEvento_96_PP().RegistrarEvento(ev);
            }
            catch (Exception ex)
            {
                new BLL_Idioma_96_PP().MostrarMensaje(ex.Message);
            }
            Limpiar();
        }

        private void AplicarPermisosAcciones()
        {
            btnbuscar.Enabled = TienePermiso(Permisos_96_PP.CrearFamilia);
            btnmodificar.Enabled = TienePermiso(Permisos_96_PP.ModificarFamilia);
            btncancelar.Enabled = TienePermiso(Permisos_96_PP.EliminarFamilia);
            button1.Enabled = TienePermiso(Permisos_96_PP.AsignarPatenteAFamilia);
            button2.Enabled = TienePermiso(Permisos_96_PP.AsignarFamiliaAFamilia);
            button6.Enabled = TienePermiso(Permisos_96_PP.QuitarPermisoDeFamilia);
        }

        private bool TienePermiso(string permiso)
        {
            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            return usuario?.Rol != null && usuario.Rol.TienePermiso(permiso);
        }
    }
}
