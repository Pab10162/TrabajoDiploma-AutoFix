namespace SistemaBase.Turnos
{
    partial class FormTurnos_96_PP
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
            this.grpCliente = new System.Windows.Forms.GroupBox();
            this.lblDniCliente = new System.Windows.Forms.Label();
            this.txtDniCliente = new System.Windows.Forms.TextBox();
            this.btnBuscarCliente = new System.Windows.Forms.Button();
            this.btnRegistrarCliente = new System.Windows.Forms.Button();
            this.lblDatosCliente = new System.Windows.Forms.Label();
            this.grpVehiculo = new System.Windows.Forms.GroupBox();
            this.lblVehiculo = new System.Windows.Forms.Label();
            this.cmbVehiculos = new System.Windows.Forms.ComboBox();
            this.btnRegistrarVehiculo = new System.Windows.Forms.Button();
            this.grpTurno = new System.Windows.Forms.GroupBox();
            this.lblFecha = new System.Windows.Forms.Label();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.lblHorario = new System.Windows.Forms.Label();
            this.lstHorarios = new System.Windows.Forms.ListBox();
            this.btnAsignarTurno = new System.Windows.Forms.Button();
            this.lblAgenda = new System.Windows.Forms.Label();
            this.dtpFiltro = new System.Windows.Forms.DateTimePicker();
            this.chkTodasLasFechas = new System.Windows.Forms.CheckBox();
            this.dgvTurnos = new System.Windows.Forms.DataGridView();
            this.btnConfirmarTurno = new System.Windows.Forms.Button();
            this.btnCancelarTurno = new System.Windows.Forms.Button();
            this.btnExportarTurno = new System.Windows.Forms.Button();
            this.btnImportarTurno = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnos)).BeginInit();
            this.grpCliente.SuspendLayout();
            this.grpVehiculo.SuspendLayout();
            this.grpTurno.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpCliente
            // 
            this.grpCliente.Controls.Add(this.lblDniCliente);
            this.grpCliente.Controls.Add(this.txtDniCliente);
            this.grpCliente.Controls.Add(this.btnBuscarCliente);
            this.grpCliente.Controls.Add(this.btnRegistrarCliente);
            this.grpCliente.Controls.Add(this.lblDatosCliente);
            this.grpCliente.Location = new System.Drawing.Point(12, 10);
            this.grpCliente.Name = "grpCliente";
            this.grpCliente.Size = new System.Drawing.Size(470, 110);
            this.grpCliente.TabIndex = 0;
            this.grpCliente.TabStop = false;
            this.grpCliente.Tag = "clienteGroup";
            this.grpCliente.Text = "1. Cliente";
            // 
            // lblDniCliente
            // 
            this.lblDniCliente.AutoSize = true;
            this.lblDniCliente.Location = new System.Drawing.Point(15, 28);
            this.lblDniCliente.Name = "lblDniCliente";
            this.lblDniCliente.Size = new System.Drawing.Size(26, 13);
            this.lblDniCliente.TabIndex = 0;
            this.lblDniCliente.Tag = "DniLabel";
            this.lblDniCliente.Text = "DNI";
            // 
            // txtDniCliente
            // 
            this.txtDniCliente.MaxLength = 8;
            this.txtDniCliente.Location = new System.Drawing.Point(60, 25);
            this.txtDniCliente.Name = "txtDniCliente";
            this.txtDniCliente.Size = new System.Drawing.Size(140, 20);
            this.txtDniCliente.TabIndex = 1;
            this.txtDniCliente.TextChanged += new System.EventHandler(this.txtDniCliente_TextChanged);
            // 
            // btnBuscarCliente
            // 
            this.btnBuscarCliente.Location = new System.Drawing.Point(210, 23);
            this.btnBuscarCliente.Name = "btnBuscarCliente";
            this.btnBuscarCliente.Size = new System.Drawing.Size(85, 23);
            this.btnBuscarCliente.TabIndex = 2;
            this.btnBuscarCliente.Tag = "buscarButton";
            this.btnBuscarCliente.Text = "Buscar";
            this.btnBuscarCliente.UseVisualStyleBackColor = true;
            this.btnBuscarCliente.Click += new System.EventHandler(this.btnBuscarCliente_Click);
            // 
            // btnRegistrarCliente
            // 
            this.btnRegistrarCliente.Location = new System.Drawing.Point(305, 23);
            this.btnRegistrarCliente.Name = "btnRegistrarCliente";
            this.btnRegistrarCliente.Size = new System.Drawing.Size(150, 23);
            this.btnRegistrarCliente.TabIndex = 3;
            this.btnRegistrarCliente.Tag = "registrarClienteButton";
            this.btnRegistrarCliente.Text = "Registrar cliente";
            this.btnRegistrarCliente.UseVisualStyleBackColor = true;
            this.btnRegistrarCliente.Click += new System.EventHandler(this.btnRegistrarCliente_Click);
            // 
            // lblDatosCliente
            // 
            this.lblDatosCliente.AutoSize = false;
            this.lblDatosCliente.Location = new System.Drawing.Point(15, 60);
            this.lblDatosCliente.Name = "lblDatosCliente";
            this.lblDatosCliente.Size = new System.Drawing.Size(440, 40);
            this.lblDatosCliente.TabIndex = 4;
            this.lblDatosCliente.Text = "";
            // 
            // grpVehiculo
            // 
            this.grpVehiculo.Controls.Add(this.lblVehiculo);
            this.grpVehiculo.Controls.Add(this.cmbVehiculos);
            this.grpVehiculo.Controls.Add(this.btnRegistrarVehiculo);
            this.grpVehiculo.Location = new System.Drawing.Point(12, 125);
            this.grpVehiculo.Name = "grpVehiculo";
            this.grpVehiculo.Size = new System.Drawing.Size(470, 90);
            this.grpVehiculo.TabIndex = 1;
            this.grpVehiculo.TabStop = false;
            this.grpVehiculo.Tag = "vehiculoGroup";
            this.grpVehiculo.Text = "2. Vehículo";
            // 
            // lblVehiculo
            // 
            this.lblVehiculo.AutoSize = true;
            this.lblVehiculo.Location = new System.Drawing.Point(15, 33);
            this.lblVehiculo.Name = "lblVehiculo";
            this.lblVehiculo.Size = new System.Drawing.Size(50, 13);
            this.lblVehiculo.TabIndex = 0;
            this.lblVehiculo.Tag = "vehiculoLabel";
            this.lblVehiculo.Text = "Vehículo";
            // 
            // cmbVehiculos
            // 
            this.cmbVehiculos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVehiculos.FormattingEnabled = true;
            this.cmbVehiculos.Location = new System.Drawing.Point(80, 30);
            this.cmbVehiculos.Name = "cmbVehiculos";
            this.cmbVehiculos.Size = new System.Drawing.Size(215, 21);
            this.cmbVehiculos.TabIndex = 1;
            // 
            // btnRegistrarVehiculo
            // 
            this.btnRegistrarVehiculo.Location = new System.Drawing.Point(305, 28);
            this.btnRegistrarVehiculo.Name = "btnRegistrarVehiculo";
            this.btnRegistrarVehiculo.Size = new System.Drawing.Size(150, 23);
            this.btnRegistrarVehiculo.TabIndex = 2;
            this.btnRegistrarVehiculo.Tag = "registrarVehiculoButton";
            this.btnRegistrarVehiculo.Text = "Registrar vehículo";
            this.btnRegistrarVehiculo.UseVisualStyleBackColor = true;
            this.btnRegistrarVehiculo.Click += new System.EventHandler(this.btnRegistrarVehiculo_Click);
            // 
            // grpTurno
            // 
            this.grpTurno.Controls.Add(this.lblFecha);
            this.grpTurno.Controls.Add(this.dtpFecha);
            this.grpTurno.Controls.Add(this.lblHorario);
            this.grpTurno.Controls.Add(this.lstHorarios);
            this.grpTurno.Controls.Add(this.btnAsignarTurno);
            this.grpTurno.Location = new System.Drawing.Point(500, 10);
            this.grpTurno.Name = "grpTurno";
            this.grpTurno.Size = new System.Drawing.Size(280, 205);
            this.grpTurno.TabIndex = 2;
            this.grpTurno.TabStop = false;
            this.grpTurno.Tag = "turnoGroup";
            this.grpTurno.Text = "3. Turno";
            // 
            // lblFecha
            // 
            this.lblFecha.AutoSize = true;
            this.lblFecha.Location = new System.Drawing.Point(15, 28);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(37, 13);
            this.lblFecha.TabIndex = 0;
            this.lblFecha.Tag = "fechaLabel";
            this.lblFecha.Text = "Fecha";
            // 
            // dtpFecha
            // 
            this.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFecha.Location = new System.Drawing.Point(80, 25);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(180, 20);
            this.dtpFecha.TabIndex = 1;
            this.dtpFecha.ValueChanged += new System.EventHandler(this.dtpFecha_ValueChanged);
            // 
            // lblHorario
            // 
            this.lblHorario.AutoSize = true;
            this.lblHorario.Location = new System.Drawing.Point(15, 58);
            this.lblHorario.Name = "lblHorario";
            this.lblHorario.Size = new System.Drawing.Size(41, 13);
            this.lblHorario.TabIndex = 2;
            this.lblHorario.Tag = "horarioLabel";
            this.lblHorario.Text = "Horario";
            // 
            // lstHorarios
            // 
            this.lstHorarios.FormattingEnabled = true;
            this.lstHorarios.Location = new System.Drawing.Point(80, 55);
            this.lstHorarios.Name = "lstHorarios";
            this.lstHorarios.Size = new System.Drawing.Size(180, 95);
            this.lstHorarios.TabIndex = 3;
            // 
            // btnAsignarTurno
            // 
            this.btnAsignarTurno.Location = new System.Drawing.Point(80, 162);
            this.btnAsignarTurno.Name = "btnAsignarTurno";
            this.btnAsignarTurno.Size = new System.Drawing.Size(180, 30);
            this.btnAsignarTurno.TabIndex = 4;
            this.btnAsignarTurno.Tag = "asignarTurnoButton";
            this.btnAsignarTurno.Text = "Asignar turno";
            this.btnAsignarTurno.UseVisualStyleBackColor = true;
            this.btnAsignarTurno.Click += new System.EventHandler(this.btnAsignarTurno_Click);
            // 
            // lblAgenda
            // 
            this.lblAgenda.AutoSize = true;
            this.lblAgenda.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblAgenda.Location = new System.Drawing.Point(12, 232);
            this.lblAgenda.Name = "lblAgenda";
            this.lblAgenda.Size = new System.Drawing.Size(105, 13);
            this.lblAgenda.TabIndex = 3;
            this.lblAgenda.Tag = "agendaLabel";
            this.lblAgenda.Text = "Agenda de turnos";
            // 
            // dtpFiltro
            // 
            this.dtpFiltro.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFiltro.Location = new System.Drawing.Point(150, 228);
            this.dtpFiltro.Name = "dtpFiltro";
            this.dtpFiltro.Size = new System.Drawing.Size(120, 20);
            this.dtpFiltro.TabIndex = 4;
            this.dtpFiltro.ValueChanged += new System.EventHandler(this.dtpFiltro_ValueChanged);
            // 
            // chkTodasLasFechas
            // 
            this.chkTodasLasFechas.AutoSize = true;
            this.chkTodasLasFechas.Location = new System.Drawing.Point(285, 230);
            this.chkTodasLasFechas.Name = "chkTodasLasFechas";
            this.chkTodasLasFechas.Size = new System.Drawing.Size(108, 17);
            this.chkTodasLasFechas.TabIndex = 5;
            this.chkTodasLasFechas.Tag = "todasLasFechasCheckBox";
            this.chkTodasLasFechas.Text = "Todas las fechas";
            this.chkTodasLasFechas.UseVisualStyleBackColor = true;
            this.chkTodasLasFechas.CheckedChanged += new System.EventHandler(this.chkTodasLasFechas_CheckedChanged);
            // 
            // dgvTurnos
            // 
            this.dgvTurnos.AllowUserToAddRows = false;
            this.dgvTurnos.AllowUserToDeleteRows = false;
            this.dgvTurnos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTurnos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTurnos.MultiSelect = false;
            this.dgvTurnos.ReadOnly = true;
            this.dgvTurnos.RowHeadersVisible = false;
            this.dgvTurnos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTurnos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTurnos.Location = new System.Drawing.Point(12, 255);
            this.dgvTurnos.Name = "dgvTurnos";
            this.dgvTurnos.Size = new System.Drawing.Size(768, 200);
            this.dgvTurnos.TabIndex = 6;
            this.dgvTurnos.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvTurnos_DataBindingComplete);
            this.dgvTurnos.SelectionChanged += new System.EventHandler(this.dgvTurnos_SelectionChanged);
            // 
            // btnConfirmarTurno
            // 
            this.btnConfirmarTurno.Location = new System.Drawing.Point(12, 465);
            this.btnConfirmarTurno.Name = "btnConfirmarTurno";
            this.btnConfirmarTurno.Size = new System.Drawing.Size(150, 28);
            this.btnConfirmarTurno.TabIndex = 7;
            this.btnConfirmarTurno.Tag = "confirmarTurnoButton";
            this.btnConfirmarTurno.Text = "Confirmar turno";
            this.btnConfirmarTurno.UseVisualStyleBackColor = true;
            this.btnConfirmarTurno.Click += new System.EventHandler(this.btnConfirmarTurno_Click);
            // 
            // btnCancelarTurno
            // 
            this.btnCancelarTurno.Location = new System.Drawing.Point(172, 465);
            this.btnCancelarTurno.Name = "btnCancelarTurno";
            this.btnCancelarTurno.Size = new System.Drawing.Size(150, 28);
            this.btnCancelarTurno.TabIndex = 8;
            this.btnCancelarTurno.Tag = "cancelarTurnoButton";
            this.btnCancelarTurno.Text = "Cancelar turno";
            this.btnCancelarTurno.UseVisualStyleBackColor = true;
            this.btnCancelarTurno.Click += new System.EventHandler(this.btnCancelarTurno_Click);
            //
            // btnExportarTurno
            //
            this.btnExportarTurno.Location = new System.Drawing.Point(330, 465);
            this.btnExportarTurno.Name = "btnExportarTurno";
            this.btnExportarTurno.Size = new System.Drawing.Size(140, 28);
            this.btnExportarTurno.TabIndex = 9;
            this.btnExportarTurno.Tag = "exportarTurnoButton";
            this.btnExportarTurno.Text = "Exportar turno (XML)";
            this.btnExportarTurno.UseVisualStyleBackColor = true;
            this.btnExportarTurno.Click += new System.EventHandler(this.btnExportarTurno_Click);
            //
            // btnImportarTurno
            //
            this.btnImportarTurno.Location = new System.Drawing.Point(480, 465);
            this.btnImportarTurno.Name = "btnImportarTurno";
            this.btnImportarTurno.Size = new System.Drawing.Size(140, 28);
            this.btnImportarTurno.TabIndex = 10;
            this.btnImportarTurno.Tag = "importarTurnoButton";
            this.btnImportarTurno.Text = "Importar turno (XML)";
            this.btnImportarTurno.UseVisualStyleBackColor = true;
            this.btnImportarTurno.Click += new System.EventHandler(this.btnImportarTurno_Click);
            //
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(630, 465);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(150, 28);
            this.btnSalir.TabIndex = 11;
            this.btnSalir.Tag = "SalirButton";
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // FormTurnos_96_PP
            // 
            this.Text = "Turnos";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 506);
            this.Controls.Add(this.grpCliente);
            this.Controls.Add(this.grpVehiculo);
            this.Controls.Add(this.grpTurno);
            this.Controls.Add(this.lblAgenda);
            this.Controls.Add(this.dtpFiltro);
            this.Controls.Add(this.chkTodasLasFechas);
            this.Controls.Add(this.dgvTurnos);
            this.Controls.Add(this.btnConfirmarTurno);
            this.Controls.Add(this.btnCancelarTurno);
            this.Controls.Add(this.btnExportarTurno);
            this.Controls.Add(this.btnImportarTurno);
            this.Controls.Add(this.btnSalir);
            this.Name = "FormTurnos_96_PP";
            this.Load += new System.EventHandler(this.FormTurnos_96_PP_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnos)).EndInit();
            this.grpCliente.ResumeLayout(false);
            this.grpCliente.PerformLayout();
            this.grpVehiculo.ResumeLayout(false);
            this.grpVehiculo.PerformLayout();
            this.grpTurno.ResumeLayout(false);
            this.grpTurno.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpCliente;
        private System.Windows.Forms.Label lblDniCliente;
        private System.Windows.Forms.TextBox txtDniCliente;
        private System.Windows.Forms.Button btnBuscarCliente;
        private System.Windows.Forms.Button btnRegistrarCliente;
        private System.Windows.Forms.Label lblDatosCliente;
        private System.Windows.Forms.GroupBox grpVehiculo;
        private System.Windows.Forms.Label lblVehiculo;
        private System.Windows.Forms.ComboBox cmbVehiculos;
        private System.Windows.Forms.Button btnRegistrarVehiculo;
        private System.Windows.Forms.GroupBox grpTurno;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Label lblHorario;
        private System.Windows.Forms.ListBox lstHorarios;
        private System.Windows.Forms.Button btnAsignarTurno;
        private System.Windows.Forms.Label lblAgenda;
        private System.Windows.Forms.DateTimePicker dtpFiltro;
        private System.Windows.Forms.CheckBox chkTodasLasFechas;
        private System.Windows.Forms.DataGridView dgvTurnos;
        private System.Windows.Forms.Button btnConfirmarTurno;
        private System.Windows.Forms.Button btnCancelarTurno;
        private System.Windows.Forms.Button btnExportarTurno;
        private System.Windows.Forms.Button btnImportarTurno;
        private System.Windows.Forms.Button btnSalir;
    }
}
