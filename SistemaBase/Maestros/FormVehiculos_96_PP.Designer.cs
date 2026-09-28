namespace SistemaBase.Maestros
{
    partial class FormVehiculos_96_PP
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
            this.lblBuscarPatente = new System.Windows.Forms.Label();
            this.txtBuscarPatente = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.chkVerInactivos = new System.Windows.Forms.CheckBox();
            this.dgvVehiculos = new System.Windows.Forms.DataGridView();
            this.grpDatos = new System.Windows.Forms.GroupBox();
            this.lblPatente = new System.Windows.Forms.Label();
            this.txtPatente = new System.Windows.Forms.TextBox();
            this.lblMarca = new System.Windows.Forms.Label();
            this.txtMarca = new System.Windows.Forms.TextBox();
            this.lblModelo = new System.Windows.Forms.Label();
            this.txtModelo = new System.Windows.Forms.TextBox();
            this.lblCedula = new System.Windows.Forms.Label();
            this.txtCedula = new System.Windows.Forms.TextBox();
            this.lblDniCliente = new System.Windows.Forms.Label();
            this.txtDniCliente = new System.Windows.Forms.TextBox();
            this.btnBuscarCliente = new System.Windows.Forms.Button();
            this.lblNombreCliente = new System.Windows.Forms.Label();
            this.lblModo = new System.Windows.Forms.Label();
            this.btnCrear = new System.Windows.Forms.Button();
            this.btnModificar = new System.Windows.Forms.Button();
            this.btnActivarDesactivar = new System.Windows.Forms.Button();
            this.btnAplicar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVehiculos)).BeginInit();
            this.grpDatos.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblBuscarPatente
            // 
            this.lblBuscarPatente.AutoSize = true;
            this.lblBuscarPatente.Location = new System.Drawing.Point(12, 15);
            this.lblBuscarPatente.Name = "lblBuscarPatente";
            this.lblBuscarPatente.Size = new System.Drawing.Size(100, 13);
            this.lblBuscarPatente.TabIndex = 0;
            this.lblBuscarPatente.Tag = "buscarPatenteLabel";
            this.lblBuscarPatente.Text = "Buscar por patente:";
            // 
            // txtBuscarPatente
            // 
            this.txtBuscarPatente.MaxLength = 10;
            this.txtBuscarPatente.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBuscarPatente.Location = new System.Drawing.Point(130, 12);
            this.txtBuscarPatente.Name = "txtBuscarPatente";
            this.txtBuscarPatente.Size = new System.Drawing.Size(140, 20);
            this.txtBuscarPatente.TabIndex = 1;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(280, 10);
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
            this.chkVerInactivos.Location = new System.Drawing.Point(390, 14);
            this.chkVerInactivos.Name = "chkVerInactivos";
            this.chkVerInactivos.Size = new System.Drawing.Size(114, 17);
            this.chkVerInactivos.TabIndex = 3;
            this.chkVerInactivos.Tag = "verInactivosCheckBox";
            this.chkVerInactivos.Text = "Ver dados de baja";
            this.chkVerInactivos.UseVisualStyleBackColor = true;
            this.chkVerInactivos.CheckedChanged += new System.EventHandler(this.chkVerInactivos_CheckedChanged);
            // 
            // dgvVehiculos
            // 
            this.dgvVehiculos.AllowUserToAddRows = false;
            this.dgvVehiculos.AllowUserToDeleteRows = false;
            this.dgvVehiculos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvVehiculos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVehiculos.MultiSelect = false;
            this.dgvVehiculos.ReadOnly = true;
            this.dgvVehiculos.RowHeadersVisible = false;
            this.dgvVehiculos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVehiculos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvVehiculos.Location = new System.Drawing.Point(12, 45);
            this.dgvVehiculos.Name = "dgvVehiculos";
            this.dgvVehiculos.Size = new System.Drawing.Size(760, 250);
            this.dgvVehiculos.TabIndex = 4;
            this.dgvVehiculos.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvVehiculos_DataBindingComplete);
            this.dgvVehiculos.SelectionChanged += new System.EventHandler(this.dgvVehiculos_SelectionChanged);
            // 
            // grpDatos
            // 
            this.grpDatos.Controls.Add(this.lblPatente);
            this.grpDatos.Controls.Add(this.txtPatente);
            this.grpDatos.Controls.Add(this.lblMarca);
            this.grpDatos.Controls.Add(this.txtMarca);
            this.grpDatos.Controls.Add(this.lblModelo);
            this.grpDatos.Controls.Add(this.txtModelo);
            this.grpDatos.Controls.Add(this.lblCedula);
            this.grpDatos.Controls.Add(this.txtCedula);
            this.grpDatos.Controls.Add(this.lblDniCliente);
            this.grpDatos.Controls.Add(this.txtDniCliente);
            this.grpDatos.Controls.Add(this.btnBuscarCliente);
            this.grpDatos.Controls.Add(this.lblNombreCliente);
            this.grpDatos.Location = new System.Drawing.Point(12, 305);
            this.grpDatos.Name = "grpDatos";
            this.grpDatos.Size = new System.Drawing.Size(400, 200);
            this.grpDatos.TabIndex = 5;
            this.grpDatos.TabStop = false;
            this.grpDatos.Tag = "datosVehiculoGroup";
            this.grpDatos.Text = "Datos del vehículo";
            // 
            // lblPatente
            // 
            this.lblPatente.AutoSize = true;
            this.lblPatente.Location = new System.Drawing.Point(15, 30);
            this.lblPatente.Name = "lblPatente";
            this.lblPatente.Size = new System.Drawing.Size(44, 13);
            this.lblPatente.TabIndex = 0;
            this.lblPatente.Tag = "patenteLabel";
            this.lblPatente.Text = "Patente";
            // 
            // txtPatente
            // 
            this.txtPatente.MaxLength = 10;
            this.txtPatente.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPatente.Location = new System.Drawing.Point(110, 27);
            this.txtPatente.Name = "txtPatente";
            this.txtPatente.Size = new System.Drawing.Size(270, 20);
            this.txtPatente.TabIndex = 1;
            // 
            // lblMarca
            // 
            this.lblMarca.AutoSize = true;
            this.lblMarca.Location = new System.Drawing.Point(15, 60);
            this.lblMarca.Name = "lblMarca";
            this.lblMarca.Size = new System.Drawing.Size(37, 13);
            this.lblMarca.TabIndex = 2;
            this.lblMarca.Tag = "marcaLabel";
            this.lblMarca.Text = "Marca";
            // 
            // txtMarca
            // 
            this.txtMarca.MaxLength = 50;
            this.txtMarca.Location = new System.Drawing.Point(110, 57);
            this.txtMarca.Name = "txtMarca";
            this.txtMarca.Size = new System.Drawing.Size(270, 20);
            this.txtMarca.TabIndex = 3;
            // 
            // lblModelo
            // 
            this.lblModelo.AutoSize = true;
            this.lblModelo.Location = new System.Drawing.Point(15, 90);
            this.lblModelo.Name = "lblModelo";
            this.lblModelo.Size = new System.Drawing.Size(42, 13);
            this.lblModelo.TabIndex = 4;
            this.lblModelo.Tag = "modeloLabel";
            this.lblModelo.Text = "Modelo";
            // 
            // txtModelo
            // 
            this.txtModelo.MaxLength = 50;
            this.txtModelo.Location = new System.Drawing.Point(110, 87);
            this.txtModelo.Name = "txtModelo";
            this.txtModelo.Size = new System.Drawing.Size(270, 20);
            this.txtModelo.TabIndex = 5;
            // 
            // lblCedula
            // 
            this.lblCedula.AutoSize = true;
            this.lblCedula.Location = new System.Drawing.Point(15, 120);
            this.lblCedula.Name = "lblCedula";
            this.lblCedula.Size = new System.Drawing.Size(40, 13);
            this.lblCedula.TabIndex = 6;
            this.lblCedula.Tag = "cedulaLabel";
            this.lblCedula.Text = "Cédula";
            // 
            // txtCedula
            // 
            this.txtCedula.MaxLength = 30;
            this.txtCedula.Location = new System.Drawing.Point(110, 117);
            this.txtCedula.Name = "txtCedula";
            this.txtCedula.Size = new System.Drawing.Size(270, 20);
            this.txtCedula.TabIndex = 7;
            // 
            // lblDniCliente
            // 
            this.lblDniCliente.AutoSize = true;
            this.lblDniCliente.Location = new System.Drawing.Point(15, 150);
            this.lblDniCliente.Name = "lblDniCliente";
            this.lblDniCliente.Size = new System.Drawing.Size(62, 13);
            this.lblDniCliente.TabIndex = 8;
            this.lblDniCliente.Tag = "dniClienteLabel";
            this.lblDniCliente.Text = "DNI cliente";
            // 
            // txtDniCliente
            // 
            this.txtDniCliente.MaxLength = 8;
            this.txtDniCliente.Location = new System.Drawing.Point(110, 147);
            this.txtDniCliente.Name = "txtDniCliente";
            this.txtDniCliente.Size = new System.Drawing.Size(170, 20);
            this.txtDniCliente.TabIndex = 9;
            this.txtDniCliente.TextChanged += new System.EventHandler(this.txtDniCliente_TextChanged);
            // 
            // btnBuscarCliente
            // 
            this.btnBuscarCliente.Location = new System.Drawing.Point(290, 145);
            this.btnBuscarCliente.Name = "btnBuscarCliente";
            this.btnBuscarCliente.Size = new System.Drawing.Size(90, 23);
            this.btnBuscarCliente.TabIndex = 10;
            this.btnBuscarCliente.Tag = "buscarButton";
            this.btnBuscarCliente.Text = "Buscar";
            this.btnBuscarCliente.UseVisualStyleBackColor = true;
            this.btnBuscarCliente.Click += new System.EventHandler(this.btnBuscarCliente_Click);
            // 
            // lblNombreCliente
            // 
            this.lblNombreCliente.AutoSize = false;
            this.lblNombreCliente.Location = new System.Drawing.Point(110, 175);
            this.lblNombreCliente.Name = "lblNombreCliente";
            this.lblNombreCliente.Size = new System.Drawing.Size(270, 13);
            this.lblNombreCliente.TabIndex = 11;
            this.lblNombreCliente.Text = "";
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
            // FormVehiculos_96_PP
            // 
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Vehículos";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 521);
            this.Controls.Add(this.lblBuscarPatente);
            this.Controls.Add(this.txtBuscarPatente);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.chkVerInactivos);
            this.Controls.Add(this.dgvVehiculos);
            this.Controls.Add(this.grpDatos);
            this.Controls.Add(this.lblModo);
            this.Controls.Add(this.btnCrear);
            this.Controls.Add(this.btnModificar);
            this.Controls.Add(this.btnActivarDesactivar);
            this.Controls.Add(this.btnAplicar);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnSalir);
            this.Name = "FormVehiculos_96_PP";
            this.Load += new System.EventHandler(this.FormVehiculos_96_PP_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVehiculos)).EndInit();
            this.grpDatos.ResumeLayout(false);
            this.grpDatos.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBuscarPatente;
        private System.Windows.Forms.TextBox txtBuscarPatente;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.CheckBox chkVerInactivos;
        private System.Windows.Forms.DataGridView dgvVehiculos;
        private System.Windows.Forms.GroupBox grpDatos;
        private System.Windows.Forms.Label lblPatente;
        private System.Windows.Forms.TextBox txtPatente;
        private System.Windows.Forms.Label lblMarca;
        private System.Windows.Forms.TextBox txtMarca;
        private System.Windows.Forms.Label lblModelo;
        private System.Windows.Forms.TextBox txtModelo;
        private System.Windows.Forms.Label lblCedula;
        private System.Windows.Forms.TextBox txtCedula;
        private System.Windows.Forms.Label lblDniCliente;
        private System.Windows.Forms.TextBox txtDniCliente;
        private System.Windows.Forms.Button btnBuscarCliente;
        private System.Windows.Forms.Label lblNombreCliente;
        private System.Windows.Forms.Label lblModo;
        private System.Windows.Forms.Button btnCrear;
        private System.Windows.Forms.Button btnModificar;
        private System.Windows.Forms.Button btnActivarDesactivar;
        private System.Windows.Forms.Button btnAplicar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnSalir;
    }
}
