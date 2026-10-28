namespace SistemaBase.Maestros
{
    partial class FormClientes_96_PP
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblBuscarDni = new System.Windows.Forms.Label();
            this.txtBuscarDni = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.chkVerInactivos = new System.Windows.Forms.CheckBox();
            this.dgvClientes = new System.Windows.Forms.DataGridView();
            this.grpDatos = new System.Windows.Forms.GroupBox();
            this.lblDni = new System.Windows.Forms.Label();
            this.txtDni = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblApellido = new System.Windows.Forms.Label();
            this.txtApellido = new System.Windows.Forms.TextBox();
            this.lblTelefono = new System.Windows.Forms.Label();
            this.txtTelefono = new System.Windows.Forms.TextBox();
            this.lblDireccion = new System.Windows.Forms.Label();
            this.txtDireccion = new System.Windows.Forms.TextBox();
            this.lblModo = new System.Windows.Forms.Label();
            this.btnCrear = new System.Windows.Forms.Button();
            this.btnModificar = new System.Windows.Forms.Button();
            this.btnActivarDesactivar = new System.Windows.Forms.Button();
            this.btnAplicar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes)).BeginInit();
            this.grpDatos.SuspendLayout();
            this.SuspendLayout();
            //
            // lblBuscarDni
            //
            this.lblBuscarDni.AutoSize = true;
            this.lblBuscarDni.Location = new System.Drawing.Point(12, 15);
            this.lblBuscarDni.Name = "lblBuscarDni";
            this.lblBuscarDni.Size = new System.Drawing.Size(83, 13);
            this.lblBuscarDni.TabIndex = 0;
            this.lblBuscarDni.Tag = "buscarDniLabel";
            this.lblBuscarDni.Text = "Buscar por DNI:";
            //
            // txtBuscarDni
            //
            this.txtBuscarDni.Location = new System.Drawing.Point(110, 12);
            this.txtBuscarDni.MaxLength = 8;
            this.txtBuscarDni.Name = "txtBuscarDni";
            this.txtBuscarDni.Size = new System.Drawing.Size(150, 20);
            this.txtBuscarDni.TabIndex = 1;
            //
            // btnBuscar
            //
            this.btnBuscar.Location = new System.Drawing.Point(270, 10);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(90, 23);
            this.btnBuscar.TabIndex = 2;
            this.btnBuscar.Tag = "buscarButton";
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            //
            // chkVerInactivos
            //
            this.chkVerInactivos.AutoSize = true;
            this.chkVerInactivos.Checked = true;
            this.chkVerInactivos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkVerInactivos.Location = new System.Drawing.Point(380, 14);
            this.chkVerInactivos.Name = "chkVerInactivos";
            this.chkVerInactivos.Size = new System.Drawing.Size(114, 17);
            this.chkVerInactivos.TabIndex = 3;
            this.chkVerInactivos.Tag = "verInactivosCheckBox";
            this.chkVerInactivos.Text = "Ver dados de baja";
            this.chkVerInactivos.UseVisualStyleBackColor = true;
            this.chkVerInactivos.CheckedChanged += new System.EventHandler(this.chkVerInactivos_CheckedChanged);
            //
            // dgvClientes
            //
            this.dgvClientes.AllowUserToAddRows = false;
            this.dgvClientes.AllowUserToDeleteRows = false;
            this.dgvClientes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvClientes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvClientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClientes.Location = new System.Drawing.Point(12, 45);
            this.dgvClientes.MultiSelect = false;
            this.dgvClientes.Name = "dgvClientes";
            this.dgvClientes.ReadOnly = true;
            this.dgvClientes.RowHeadersVisible = false;
            this.dgvClientes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvClientes.Size = new System.Drawing.Size(760, 250);
            this.dgvClientes.TabIndex = 4;
            this.dgvClientes.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvClientes_DataBindingComplete);
            this.dgvClientes.SelectionChanged += new System.EventHandler(this.dgvClientes_SelectionChanged);
            //
            // grpDatos
            //
            this.grpDatos.Controls.Add(this.lblDni);
            this.grpDatos.Controls.Add(this.txtDni);
            this.grpDatos.Controls.Add(this.lblNombre);
            this.grpDatos.Controls.Add(this.txtNombre);
            this.grpDatos.Controls.Add(this.lblApellido);
            this.grpDatos.Controls.Add(this.txtApellido);
            this.grpDatos.Controls.Add(this.lblTelefono);
            this.grpDatos.Controls.Add(this.txtTelefono);
            this.grpDatos.Controls.Add(this.lblDireccion);
            this.grpDatos.Controls.Add(this.txtDireccion);
            this.grpDatos.Location = new System.Drawing.Point(12, 305);
            this.grpDatos.Name = "grpDatos";
            this.grpDatos.Size = new System.Drawing.Size(400, 190);
            this.grpDatos.TabIndex = 5;
            this.grpDatos.TabStop = false;
            this.grpDatos.Tag = "datosClienteGroup";
            this.grpDatos.Text = "Datos del cliente";
            //
            // lblDni
            //
            this.lblDni.AutoSize = true;
            this.lblDni.Location = new System.Drawing.Point(15, 30);
            this.lblDni.Name = "lblDni";
            this.lblDni.Size = new System.Drawing.Size(26, 13);
            this.lblDni.TabIndex = 0;
            this.lblDni.Tag = "DniLabel";
            this.lblDni.Text = "DNI";
            //
            // txtDni
            //
            this.txtDni.Location = new System.Drawing.Point(110, 27);
            this.txtDni.MaxLength = 8;
            this.txtDni.Name = "txtDni";
            this.txtDni.Size = new System.Drawing.Size(270, 20);
            this.txtDni.TabIndex = 1;
            //
            // lblNombre
            //
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(15, 60);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(44, 13);
            this.lblNombre.TabIndex = 2;
            this.lblNombre.Tag = "NombreLabel";
            this.lblNombre.Text = "Nombre";
            //
            // txtNombre
            //
            this.txtNombre.Location = new System.Drawing.Point(110, 57);
            this.txtNombre.MaxLength = 50;
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(270, 20);
            this.txtNombre.TabIndex = 3;
            //
            // lblApellido
            //
            this.lblApellido.AutoSize = true;
            this.lblApellido.Location = new System.Drawing.Point(15, 90);
            this.lblApellido.Name = "lblApellido";
            this.lblApellido.Size = new System.Drawing.Size(44, 13);
            this.lblApellido.TabIndex = 4;
            this.lblApellido.Tag = "ApellidoLabel";
            this.lblApellido.Text = "Apellido";
            //
            // txtApellido
            //
            this.txtApellido.Location = new System.Drawing.Point(110, 87);
            this.txtApellido.MaxLength = 50;
            this.txtApellido.Name = "txtApellido";
            this.txtApellido.Size = new System.Drawing.Size(270, 20);
            this.txtApellido.TabIndex = 5;
            //
            // lblTelefono
            //
            this.lblTelefono.AutoSize = true;
            this.lblTelefono.Location = new System.Drawing.Point(15, 120);
            this.lblTelefono.Name = "lblTelefono";
            this.lblTelefono.Size = new System.Drawing.Size(49, 13);
            this.lblTelefono.TabIndex = 6;
            this.lblTelefono.Tag = "telefonoLabel";
            this.lblTelefono.Text = "Teléfono";
            //
            // txtTelefono
            //
            this.txtTelefono.Location = new System.Drawing.Point(110, 117);
            this.txtTelefono.MaxLength = 30;
            this.txtTelefono.Name = "txtTelefono";
            this.txtTelefono.Size = new System.Drawing.Size(270, 20);
            this.txtTelefono.TabIndex = 7;
            //
            // lblDireccion
            //
            this.lblDireccion.AutoSize = true;
            this.lblDireccion.Location = new System.Drawing.Point(15, 150);
            this.lblDireccion.Name = "lblDireccion";
            this.lblDireccion.Size = new System.Drawing.Size(52, 13);
            this.lblDireccion.TabIndex = 8;
            this.lblDireccion.Tag = "direccionLabel";
            this.lblDireccion.Text = "Dirección";
            //
            // txtDireccion
            //
            this.txtDireccion.Location = new System.Drawing.Point(110, 147);
            this.txtDireccion.MaxLength = 100;
            this.txtDireccion.Name = "txtDireccion";
            this.txtDireccion.Size = new System.Drawing.Size(270, 20);
            this.txtDireccion.TabIndex = 9;
            //
            // lblModo
            //
            this.lblModo.AutoSize = true;
            this.lblModo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblModo.Location = new System.Drawing.Point(430, 312);
            this.lblModo.Name = "lblModo";
            this.lblModo.Size = new System.Drawing.Size(87, 13);
            this.lblModo.TabIndex = 6;
            this.lblModo.Text = "Modo consulta";
            //
            // btnCrear
            //
            this.btnCrear.Location = new System.Drawing.Point(430, 340);
            this.btnCrear.Name = "btnCrear";
            this.btnCrear.Size = new System.Drawing.Size(120, 28);
            this.btnCrear.TabIndex = 7;
            this.btnCrear.Tag = "CrearButton";
            this.btnCrear.Text = "Crear";
            this.btnCrear.UseVisualStyleBackColor = true;
            this.btnCrear.Click += new System.EventHandler(this.btnCrear_Click);
            //
            // btnModificar
            //
            this.btnModificar.Location = new System.Drawing.Point(430, 375);
            this.btnModificar.Name = "btnModificar";
            this.btnModificar.Size = new System.Drawing.Size(120, 28);
            this.btnModificar.TabIndex = 8;
            this.btnModificar.Tag = "ModificarButton";
            this.btnModificar.Text = "Modificar";
            this.btnModificar.UseVisualStyleBackColor = true;
            this.btnModificar.Click += new System.EventHandler(this.btnModificar_Click);
            //
            // btnActivarDesactivar
            //
            this.btnActivarDesactivar.Location = new System.Drawing.Point(430, 410);
            this.btnActivarDesactivar.Name = "btnActivarDesactivar";
            this.btnActivarDesactivar.Size = new System.Drawing.Size(120, 28);
            this.btnActivarDesactivar.TabIndex = 9;
            this.btnActivarDesactivar.Tag = "Activar/Desactivar";
            this.btnActivarDesactivar.Text = "Activar/Desactivar";
            this.btnActivarDesactivar.UseVisualStyleBackColor = true;
            this.btnActivarDesactivar.Click += new System.EventHandler(this.btnActivarDesactivar_Click);
            //
            // btnAplicar
            //
            this.btnAplicar.Location = new System.Drawing.Point(570, 340);
            this.btnAplicar.Name = "btnAplicar";
            this.btnAplicar.Size = new System.Drawing.Size(120, 28);
            this.btnAplicar.TabIndex = 10;
            this.btnAplicar.Tag = "AplicarButton";
            this.btnAplicar.Text = "Aplicar";
            this.btnAplicar.UseVisualStyleBackColor = true;
            this.btnAplicar.Click += new System.EventHandler(this.btnAplicar_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.Location = new System.Drawing.Point(570, 375);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(120, 28);
            this.btnCancelar.TabIndex = 11;
            this.btnCancelar.Tag = "CancelarButton";
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            //
            // btnSalir
            //
            this.btnSalir.Location = new System.Drawing.Point(570, 410);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(120, 28);
            this.btnSalir.TabIndex = 12;
            this.btnSalir.Tag = "SalirButton";
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            //
            // FormClientes_96_PP
            //
            this.AcceptButton = this.btnBuscar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 511);
            this.Controls.Add(this.lblBuscarDni);
            this.Controls.Add(this.txtBuscarDni);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.chkVerInactivos);
            this.Controls.Add(this.dgvClientes);
            this.Controls.Add(this.grpDatos);
            this.Controls.Add(this.lblModo);
            this.Controls.Add(this.btnCrear);
            this.Controls.Add(this.btnModificar);
            this.Controls.Add(this.btnActivarDesactivar);
            this.Controls.Add(this.btnAplicar);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnSalir);
            this.Name = "FormClientes_96_PP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "clientesTitle";
            this.Text = "Clientes";
            this.Load += new System.EventHandler(this.FormClientes_96_PP_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes)).EndInit();
            this.grpDatos.ResumeLayout(false);
            this.grpDatos.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBuscarDni;
        private System.Windows.Forms.TextBox txtBuscarDni;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.CheckBox chkVerInactivos;
        private System.Windows.Forms.DataGridView dgvClientes;
        private System.Windows.Forms.GroupBox grpDatos;
        private System.Windows.Forms.Label lblDni;
        private System.Windows.Forms.TextBox txtDni;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblApellido;
        private System.Windows.Forms.TextBox txtApellido;
        private System.Windows.Forms.Label lblTelefono;
        private System.Windows.Forms.TextBox txtTelefono;
        private System.Windows.Forms.Label lblDireccion;
        private System.Windows.Forms.TextBox txtDireccion;
        private System.Windows.Forms.Label lblModo;
        private System.Windows.Forms.Button btnCrear;
        private System.Windows.Forms.Button btnModificar;
        private System.Windows.Forms.Button btnActivarDesactivar;
        private System.Windows.Forms.Button btnAplicar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnSalir;
    }
}
