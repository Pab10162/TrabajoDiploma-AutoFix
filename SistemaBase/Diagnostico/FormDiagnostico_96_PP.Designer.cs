namespace SistemaBase.Diagnostico
{
    partial class FormDiagnostico_96_PP
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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.tabPrincipal = new System.Windows.Forms.TabControl();
            this.tabDiagnostico = new System.Windows.Forms.TabPage();
            this.dgvTurnos = new System.Windows.Forms.DataGridView();
            this.lblSistema = new System.Windows.Forms.Label();
            this.txtSistema = new System.Windows.Forms.TextBox();
            this.lblUrgencia = new System.Windows.Forms.Label();
            this.cmbUrgencia = new System.Windows.Forms.ComboBox();
            this.lblFallas = new System.Windows.Forms.Label();
            this.txtFallas = new System.Windows.Forms.TextBox();
            this.lblRepuestosNecesarios = new System.Windows.Forms.Label();
            this.txtRepuestosNecesarios = new System.Windows.Forms.TextBox();
            this.btnConfirmarDiagnostico = new System.Windows.Forms.Button();
            this.tabSolucion = new System.Windows.Forms.TabPage();
            this.dgvConDiagnostico = new System.Windows.Forms.DataGridView();
            this.lblDetalleDiag = new System.Windows.Forms.Label();
            this.grpSolucion = new System.Windows.Forms.GroupBox();
            this.lblTipoSolucion = new System.Windows.Forms.Label();
            this.cmbTipoSolucion = new System.Windows.Forms.ComboBox();
            this.btnGuardarSolucion = new System.Windows.Forms.Button();
            this.lblDescSolucion = new System.Windows.Forms.Label();
            this.txtDescSolucion = new System.Windows.Forms.TextBox();
            this.grpFaltantes = new System.Windows.Forms.GroupBox();
            this.lblRepuesto = new System.Windows.Forms.Label();
            this.txtRepuesto = new System.Windows.Forms.TextBox();
            this.lblCantidad = new System.Windows.Forms.Label();
            this.nudCantidad = new System.Windows.Forms.NumericUpDown();
            this.btnAgregarFaltante = new System.Windows.Forms.Button();
            this.btnMarcarComprado = new System.Windows.Forms.Button();
            this.dgvFaltantes = new System.Windows.Forms.DataGridView();
            this.btnSalir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConDiagnostico)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCantidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFaltantes)).BeginInit();
            this.tabPrincipal.SuspendLayout();
            this.tabDiagnostico.SuspendLayout();
            this.tabSolucion.SuspendLayout();
            this.grpSolucion.SuspendLayout();
            this.grpFaltantes.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(15, 10);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(300, 24);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Tag = "rfn2DiagnosticoTitulo";
            this.lblTitulo.Text = "Diagnóstico técnico";
            // 
            // tabPrincipal
            // 
            this.tabPrincipal.Controls.Add(this.tabDiagnostico);
            this.tabPrincipal.Controls.Add(this.tabSolucion);
            this.tabPrincipal.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom;
            this.tabPrincipal.Location = new System.Drawing.Point(12, 45);
            this.tabPrincipal.Name = "tabPrincipal";
            this.tabPrincipal.Size = new System.Drawing.Size(876, 515);
            this.tabPrincipal.TabIndex = 1;
            // 
            // tabDiagnostico
            // 
            this.tabDiagnostico.Controls.Add(this.dgvTurnos);
            this.tabDiagnostico.Controls.Add(this.lblSistema);
            this.tabDiagnostico.Controls.Add(this.txtSistema);
            this.tabDiagnostico.Controls.Add(this.lblUrgencia);
            this.tabDiagnostico.Controls.Add(this.cmbUrgencia);
            this.tabDiagnostico.Controls.Add(this.lblFallas);
            this.tabDiagnostico.Controls.Add(this.txtFallas);
            this.tabDiagnostico.Controls.Add(this.lblRepuestosNecesarios);
            this.tabDiagnostico.Controls.Add(this.txtRepuestosNecesarios);
            this.tabDiagnostico.Controls.Add(this.btnConfirmarDiagnostico);
            this.tabDiagnostico.Location = new System.Drawing.Point(4, 22);
            this.tabDiagnostico.Name = "tabDiagnostico";
            this.tabDiagnostico.Size = new System.Drawing.Size(868, 489);
            this.tabDiagnostico.TabIndex = 2;
            this.tabDiagnostico.Tag = "rfn2TabDiagnostico";
            this.tabDiagnostico.Text = "Diagnóstico";
            this.tabDiagnostico.UseVisualStyleBackColor = true;
            this.tabDiagnostico.Padding = new System.Windows.Forms.Padding(3);
            // 
            // dgvTurnos
            // 
            this.dgvTurnos.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvTurnos.AllowUserToAddRows = false;
            this.dgvTurnos.AllowUserToDeleteRows = false;
            this.dgvTurnos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTurnos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTurnos.Location = new System.Drawing.Point(8, 8);
            this.dgvTurnos.MultiSelect = false;
            this.dgvTurnos.Name = "dgvTurnos";
            this.dgvTurnos.ReadOnly = true;
            this.dgvTurnos.RowHeadersVisible = false;
            this.dgvTurnos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTurnos.Size = new System.Drawing.Size(852, 150);
            this.dgvTurnos.TabIndex = 3;
            this.dgvTurnos.SelectionChanged += new System.EventHandler(this.dgvTurnos_SelectionChanged);
            // 
            // lblSistema
            // 
            this.lblSistema.AutoSize = true;
            this.lblSistema.Location = new System.Drawing.Point(10, 175);
            this.lblSistema.Name = "lblSistema";
            this.lblSistema.Size = new System.Drawing.Size(100, 13);
            this.lblSistema.TabIndex = 4;
            this.lblSistema.Tag = "rfn2SistemaAfectado";
            this.lblSistema.Text = "Sistema afectado";
            // 
            // txtSistema
            // 
            this.txtSistema.Location = new System.Drawing.Point(125, 172);
            this.txtSistema.MaxLength = 100;
            this.txtSistema.Name = "txtSistema";
            this.txtSistema.Size = new System.Drawing.Size(400, 20);
            this.txtSistema.TabIndex = 5;
            // 
            // lblUrgencia
            // 
            this.lblUrgencia.AutoSize = true;
            this.lblUrgencia.Location = new System.Drawing.Point(560, 175);
            this.lblUrgencia.Name = "lblUrgencia";
            this.lblUrgencia.Size = new System.Drawing.Size(50, 13);
            this.lblUrgencia.TabIndex = 6;
            this.lblUrgencia.Tag = "rfn2Urgencia";
            this.lblUrgencia.Text = "Urgencia";
            // 
            // cmbUrgencia
            // 
            this.cmbUrgencia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUrgencia.FormattingEnabled = true;
            this.cmbUrgencia.Location = new System.Drawing.Point(620, 172);
            this.cmbUrgencia.Name = "cmbUrgencia";
            this.cmbUrgencia.Size = new System.Drawing.Size(150, 21);
            this.cmbUrgencia.TabIndex = 7;
            // 
            // lblFallas
            // 
            this.lblFallas.AutoSize = true;
            this.lblFallas.Location = new System.Drawing.Point(10, 205);
            this.lblFallas.Name = "lblFallas";
            this.lblFallas.Size = new System.Drawing.Size(150, 13);
            this.lblFallas.TabIndex = 8;
            this.lblFallas.Tag = "rfn2Fallas";
            this.lblFallas.Text = "Descripción de las fallas";
            // 
            // txtFallas
            // 
            this.txtFallas.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.txtFallas.Location = new System.Drawing.Point(10, 223);
            this.txtFallas.MaxLength = 500;
            this.txtFallas.Multiline = true;
            this.txtFallas.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtFallas.Name = "txtFallas";
            this.txtFallas.Size = new System.Drawing.Size(850, 105);
            this.txtFallas.TabIndex = 9;
            // 
            // lblRepuestosNecesarios
            // 
            this.lblRepuestosNecesarios.AutoSize = true;
            this.lblRepuestosNecesarios.Location = new System.Drawing.Point(10, 338);
            this.lblRepuestosNecesarios.Name = "lblRepuestosNecesarios";
            this.lblRepuestosNecesarios.Size = new System.Drawing.Size(150, 13);
            this.lblRepuestosNecesarios.TabIndex = 10;
            this.lblRepuestosNecesarios.Tag = "rfn2RepuestosNecesarios";
            this.lblRepuestosNecesarios.Text = "Repuestos necesarios";
            // 
            // txtRepuestosNecesarios
            // 
            this.txtRepuestosNecesarios.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.txtRepuestosNecesarios.Location = new System.Drawing.Point(10, 356);
            this.txtRepuestosNecesarios.MaxLength = 300;
            this.txtRepuestosNecesarios.Multiline = true;
            this.txtRepuestosNecesarios.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRepuestosNecesarios.Name = "txtRepuestosNecesarios";
            this.txtRepuestosNecesarios.Size = new System.Drawing.Size(850, 55);
            this.txtRepuestosNecesarios.TabIndex = 11;
            // 
            // btnConfirmarDiagnostico
            // 
            this.btnConfirmarDiagnostico.Location = new System.Drawing.Point(10, 425);
            this.btnConfirmarDiagnostico.Name = "btnConfirmarDiagnostico";
            this.btnConfirmarDiagnostico.Size = new System.Drawing.Size(200, 30);
            this.btnConfirmarDiagnostico.TabIndex = 12;
            this.btnConfirmarDiagnostico.Tag = "rfn2ConfirmarDiagnostico";
            this.btnConfirmarDiagnostico.Text = "Registrar diagnóstico";
            this.btnConfirmarDiagnostico.UseVisualStyleBackColor = true;
            this.btnConfirmarDiagnostico.Click += new System.EventHandler(this.btnConfirmarDiagnostico_Click);
            // 
            // tabSolucion
            // 
            this.tabSolucion.Controls.Add(this.dgvConDiagnostico);
            this.tabSolucion.Controls.Add(this.lblDetalleDiag);
            this.tabSolucion.Controls.Add(this.grpSolucion);
            this.tabSolucion.Controls.Add(this.grpFaltantes);
            this.tabSolucion.Location = new System.Drawing.Point(4, 22);
            this.tabSolucion.Name = "tabSolucion";
            this.tabSolucion.Size = new System.Drawing.Size(868, 489);
            this.tabSolucion.TabIndex = 13;
            this.tabSolucion.Tag = "rfn2TabSolucion";
            this.tabSolucion.Text = "Solución y repuestos faltantes";
            this.tabSolucion.UseVisualStyleBackColor = true;
            this.tabSolucion.Padding = new System.Windows.Forms.Padding(3);
            // 
            // dgvConDiagnostico
            // 
            this.dgvConDiagnostico.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvConDiagnostico.AllowUserToAddRows = false;
            this.dgvConDiagnostico.AllowUserToDeleteRows = false;
            this.dgvConDiagnostico.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvConDiagnostico.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConDiagnostico.Location = new System.Drawing.Point(8, 8);
            this.dgvConDiagnostico.MultiSelect = false;
            this.dgvConDiagnostico.Name = "dgvConDiagnostico";
            this.dgvConDiagnostico.ReadOnly = true;
            this.dgvConDiagnostico.RowHeadersVisible = false;
            this.dgvConDiagnostico.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvConDiagnostico.Size = new System.Drawing.Size(852, 110);
            this.dgvConDiagnostico.TabIndex = 14;
            this.dgvConDiagnostico.SelectionChanged += new System.EventHandler(this.dgvConDiagnostico_SelectionChanged);
            // 
            // lblDetalleDiag
            // 
            this.lblDetalleDiag.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.lblDetalleDiag.AutoSize = false;
            this.lblDetalleDiag.Location = new System.Drawing.Point(10, 124);
            this.lblDetalleDiag.Name = "lblDetalleDiag";
            this.lblDetalleDiag.Size = new System.Drawing.Size(850, 36);
            this.lblDetalleDiag.TabIndex = 15;
            this.lblDetalleDiag.Text = "";
            // 
            // grpSolucion
            // 
            this.grpSolucion.Controls.Add(this.lblTipoSolucion);
            this.grpSolucion.Controls.Add(this.cmbTipoSolucion);
            this.grpSolucion.Controls.Add(this.btnGuardarSolucion);
            this.grpSolucion.Controls.Add(this.lblDescSolucion);
            this.grpSolucion.Controls.Add(this.txtDescSolucion);
            this.grpSolucion.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.grpSolucion.Location = new System.Drawing.Point(8, 165);
            this.grpSolucion.Name = "grpSolucion";
            this.grpSolucion.Size = new System.Drawing.Size(852, 150);
            this.grpSolucion.TabIndex = 16;
            this.grpSolucion.TabStop = false;
            this.grpSolucion.Tag = "rfn2GrupoSolucion";
            this.grpSolucion.Text = "Solución propuesta";
            // 
            // lblTipoSolucion
            // 
            this.lblTipoSolucion.AutoSize = true;
            this.lblTipoSolucion.Location = new System.Drawing.Point(10, 27);
            this.lblTipoSolucion.Name = "lblTipoSolucion";
            this.lblTipoSolucion.Size = new System.Drawing.Size(100, 13);
            this.lblTipoSolucion.TabIndex = 17;
            this.lblTipoSolucion.Tag = "rfn2TipoSolucion";
            this.lblTipoSolucion.Text = "Tipo de solución";
            // 
            // cmbTipoSolucion
            // 
            this.cmbTipoSolucion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoSolucion.FormattingEnabled = true;
            this.cmbTipoSolucion.Location = new System.Drawing.Point(120, 24);
            this.cmbTipoSolucion.Name = "cmbTipoSolucion";
            this.cmbTipoSolucion.Size = new System.Drawing.Size(180, 21);
            this.cmbTipoSolucion.TabIndex = 18;
            // 
            // btnGuardarSolucion
            // 
            this.btnGuardarSolucion.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnGuardarSolucion.Location = new System.Drawing.Point(640, 22);
            this.btnGuardarSolucion.Name = "btnGuardarSolucion";
            this.btnGuardarSolucion.Size = new System.Drawing.Size(200, 28);
            this.btnGuardarSolucion.TabIndex = 19;
            this.btnGuardarSolucion.Tag = "rfn2GuardarSolucion";
            this.btnGuardarSolucion.Text = "Guardar solución";
            this.btnGuardarSolucion.UseVisualStyleBackColor = true;
            this.btnGuardarSolucion.Click += new System.EventHandler(this.btnGuardarSolucion_Click);
            // 
            // lblDescSolucion
            // 
            this.lblDescSolucion.AutoSize = true;
            this.lblDescSolucion.Location = new System.Drawing.Point(10, 60);
            this.lblDescSolucion.Name = "lblDescSolucion";
            this.lblDescSolucion.Size = new System.Drawing.Size(200, 13);
            this.lblDescSolucion.TabIndex = 20;
            this.lblDescSolucion.Tag = "rfn2DescripcionSolucion";
            this.lblDescSolucion.Text = "Descripción de la solución";
            // 
            // txtDescSolucion
            // 
            this.txtDescSolucion.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.txtDescSolucion.Location = new System.Drawing.Point(10, 78);
            this.txtDescSolucion.MaxLength = 500;
            this.txtDescSolucion.Multiline = true;
            this.txtDescSolucion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescSolucion.Name = "txtDescSolucion";
            this.txtDescSolucion.Size = new System.Drawing.Size(830, 60);
            this.txtDescSolucion.TabIndex = 21;
            // 
            // grpFaltantes
            // 
            this.grpFaltantes.Controls.Add(this.lblRepuesto);
            this.grpFaltantes.Controls.Add(this.txtRepuesto);
            this.grpFaltantes.Controls.Add(this.lblCantidad);
            this.grpFaltantes.Controls.Add(this.nudCantidad);
            this.grpFaltantes.Controls.Add(this.btnAgregarFaltante);
            this.grpFaltantes.Controls.Add(this.btnMarcarComprado);
            this.grpFaltantes.Controls.Add(this.dgvFaltantes);
            this.grpFaltantes.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.grpFaltantes.Location = new System.Drawing.Point(8, 320);
            this.grpFaltantes.Name = "grpFaltantes";
            this.grpFaltantes.Size = new System.Drawing.Size(852, 165);
            this.grpFaltantes.TabIndex = 22;
            this.grpFaltantes.TabStop = false;
            this.grpFaltantes.Tag = "rfn2GrupoFaltantes";
            this.grpFaltantes.Text = "Repuestos faltantes";
            // 
            // lblRepuesto
            // 
            this.lblRepuesto.AutoSize = true;
            this.lblRepuesto.Location = new System.Drawing.Point(10, 27);
            this.lblRepuesto.Name = "lblRepuesto";
            this.lblRepuesto.Size = new System.Drawing.Size(50, 13);
            this.lblRepuesto.TabIndex = 23;
            this.lblRepuesto.Tag = "rfn2Repuesto";
            this.lblRepuesto.Text = "Repuesto";
            // 
            // txtRepuesto
            // 
            this.txtRepuesto.Location = new System.Drawing.Point(70, 24);
            this.txtRepuesto.MaxLength = 100;
            this.txtRepuesto.Name = "txtRepuesto";
            this.txtRepuesto.Size = new System.Drawing.Size(260, 20);
            this.txtRepuesto.TabIndex = 24;
            // 
            // lblCantidad
            // 
            this.lblCantidad.AutoSize = true;
            this.lblCantidad.Location = new System.Drawing.Point(345, 27);
            this.lblCantidad.Name = "lblCantidad";
            this.lblCantidad.Size = new System.Drawing.Size(50, 13);
            this.lblCantidad.TabIndex = 25;
            this.lblCantidad.Tag = "rfn2Cantidad";
            this.lblCantidad.Text = "Cantidad";
            // 
            // nudCantidad
            // 
            this.nudCantidad.Location = new System.Drawing.Point(405, 24);
            this.nudCantidad.Name = "nudCantidad";
            this.nudCantidad.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            this.nudCantidad.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudCantidad.Size = new System.Drawing.Size(70, 20);
            this.nudCantidad.TabIndex = 26;
            // 
            // btnAgregarFaltante
            // 
            this.btnAgregarFaltante.Location = new System.Drawing.Point(495, 22);
            this.btnAgregarFaltante.Name = "btnAgregarFaltante";
            this.btnAgregarFaltante.Size = new System.Drawing.Size(160, 26);
            this.btnAgregarFaltante.TabIndex = 27;
            this.btnAgregarFaltante.Tag = "rfn2AgregarFaltante";
            this.btnAgregarFaltante.Text = "Agregar faltante";
            this.btnAgregarFaltante.UseVisualStyleBackColor = true;
            this.btnAgregarFaltante.Click += new System.EventHandler(this.btnAgregarFaltante_Click);
            // 
            // btnMarcarComprado
            // 
            this.btnMarcarComprado.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnMarcarComprado.Location = new System.Drawing.Point(665, 22);
            this.btnMarcarComprado.Name = "btnMarcarComprado";
            this.btnMarcarComprado.Size = new System.Drawing.Size(175, 26);
            this.btnMarcarComprado.TabIndex = 28;
            this.btnMarcarComprado.Tag = "rfn2MarcarComprado";
            this.btnMarcarComprado.Text = "Marcar comprado";
            this.btnMarcarComprado.UseVisualStyleBackColor = true;
            this.btnMarcarComprado.Click += new System.EventHandler(this.btnMarcarComprado_Click);
            // 
            // dgvFaltantes
            // 
            this.dgvFaltantes.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvFaltantes.AllowUserToAddRows = false;
            this.dgvFaltantes.AllowUserToDeleteRows = false;
            this.dgvFaltantes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvFaltantes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFaltantes.Location = new System.Drawing.Point(10, 55);
            this.dgvFaltantes.MultiSelect = false;
            this.dgvFaltantes.Name = "dgvFaltantes";
            this.dgvFaltantes.ReadOnly = true;
            this.dgvFaltantes.RowHeadersVisible = false;
            this.dgvFaltantes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFaltantes.Size = new System.Drawing.Size(830, 100);
            this.dgvFaltantes.TabIndex = 29;
            this.dgvFaltantes.SelectionChanged += new System.EventHandler(this.dgvFaltantes_SelectionChanged);
            // 
            // btnSalir
            // 
            this.btnSalir.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.btnSalir.Location = new System.Drawing.Point(785, 575);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(100, 30);
            this.btnSalir.TabIndex = 30;
            this.btnSalir.Tag = "SalirButton";
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // FormDiagnostico_96_PP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 620);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.tabPrincipal);
            this.Controls.Add(this.btnSalir);
            this.Name = "FormDiagnostico_96_PP";
            this.Text = "Diagnóstico técnico";
            this.Load += new System.EventHandler(this.FormDiagnostico_96_PP_Load);
            this.grpFaltantes.ResumeLayout(false);
            this.grpFaltantes.PerformLayout();
            this.grpSolucion.ResumeLayout(false);
            this.grpSolucion.PerformLayout();
            this.tabSolucion.ResumeLayout(false);
            this.tabDiagnostico.ResumeLayout(false);
            this.tabPrincipal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConDiagnostico)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCantidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFaltantes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.TabControl tabPrincipal;
        private System.Windows.Forms.TabPage tabDiagnostico;
        private System.Windows.Forms.DataGridView dgvTurnos;
        private System.Windows.Forms.Label lblSistema;
        private System.Windows.Forms.TextBox txtSistema;
        private System.Windows.Forms.Label lblUrgencia;
        private System.Windows.Forms.ComboBox cmbUrgencia;
        private System.Windows.Forms.Label lblFallas;
        private System.Windows.Forms.TextBox txtFallas;
        private System.Windows.Forms.Label lblRepuestosNecesarios;
        private System.Windows.Forms.TextBox txtRepuestosNecesarios;
        private System.Windows.Forms.Button btnConfirmarDiagnostico;
        private System.Windows.Forms.TabPage tabSolucion;
        private System.Windows.Forms.DataGridView dgvConDiagnostico;
        private System.Windows.Forms.Label lblDetalleDiag;
        private System.Windows.Forms.GroupBox grpSolucion;
        private System.Windows.Forms.Label lblTipoSolucion;
        private System.Windows.Forms.ComboBox cmbTipoSolucion;
        private System.Windows.Forms.Button btnGuardarSolucion;
        private System.Windows.Forms.Label lblDescSolucion;
        private System.Windows.Forms.TextBox txtDescSolucion;
        private System.Windows.Forms.GroupBox grpFaltantes;
        private System.Windows.Forms.Label lblRepuesto;
        private System.Windows.Forms.TextBox txtRepuesto;
        private System.Windows.Forms.Label lblCantidad;
        private System.Windows.Forms.NumericUpDown nudCantidad;
        private System.Windows.Forms.Button btnAgregarFaltante;
        private System.Windows.Forms.Button btnMarcarComprado;
        private System.Windows.Forms.DataGridView dgvFaltantes;
        private System.Windows.Forms.Button btnSalir;
    }
}
