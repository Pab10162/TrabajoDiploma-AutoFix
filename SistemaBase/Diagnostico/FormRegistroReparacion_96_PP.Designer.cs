namespace SistemaBase.Diagnostico
{
    partial class FormRegistroReparacion_96_PP
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
            this.grpTurnos = new System.Windows.Forms.GroupBox();
            this.dgvTurnos = new System.Windows.Forms.DataGridView();
            this.grpReparacion = new System.Windows.Forms.GroupBox();
            this.lblDiagnostico = new System.Windows.Forms.Label();
            this.lblTareas = new System.Windows.Forms.Label();
            this.txtTareas = new System.Windows.Forms.TextBox();
            this.lblRepuestosUtilizados = new System.Windows.Forms.Label();
            this.txtRepuestosUtilizados = new System.Windows.Forms.TextBox();
            this.btnConfirmarReparacion = new System.Windows.Forms.Button();
            this.grpFinalizacion = new System.Windows.Forms.GroupBox();
            this.lblPresupuesto = new System.Windows.Forms.Label();
            this.nudPresupuesto = new System.Windows.Forms.NumericUpDown();
            this.btnConfirmarFinalizacion = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPresupuesto)).BeginInit();
            this.grpTurnos.SuspendLayout();
            this.grpReparacion.SuspendLayout();
            this.grpFinalizacion.SuspendLayout();
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
            this.lblTitulo.Tag = "rfn2ReparacionTitulo";
            this.lblTitulo.Text = "Registrar reparación";
            // 
            // grpTurnos
            // 
            this.grpTurnos.Controls.Add(this.dgvTurnos);
            this.grpTurnos.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.grpTurnos.Location = new System.Drawing.Point(12, 45);
            this.grpTurnos.Name = "grpTurnos";
            this.grpTurnos.Size = new System.Drawing.Size(876, 165);
            this.grpTurnos.TabIndex = 1;
            this.grpTurnos.TabStop = false;
            this.grpTurnos.Tag = "rfn2TurnosPendientes";
            this.grpTurnos.Text = "Turnos pendientes";
            // 
            // dgvTurnos
            // 
            this.dgvTurnos.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom;
            this.dgvTurnos.AllowUserToAddRows = false;
            this.dgvTurnos.AllowUserToDeleteRows = false;
            this.dgvTurnos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTurnos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTurnos.Location = new System.Drawing.Point(10, 20);
            this.dgvTurnos.MultiSelect = false;
            this.dgvTurnos.Name = "dgvTurnos";
            this.dgvTurnos.ReadOnly = true;
            this.dgvTurnos.RowHeadersVisible = false;
            this.dgvTurnos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTurnos.Size = new System.Drawing.Size(856, 135);
            this.dgvTurnos.TabIndex = 2;
            this.dgvTurnos.SelectionChanged += new System.EventHandler(this.dgvTurnos_SelectionChanged);
            // 
            // grpReparacion
            // 
            this.grpReparacion.Controls.Add(this.lblDiagnostico);
            this.grpReparacion.Controls.Add(this.lblTareas);
            this.grpReparacion.Controls.Add(this.txtTareas);
            this.grpReparacion.Controls.Add(this.lblRepuestosUtilizados);
            this.grpReparacion.Controls.Add(this.txtRepuestosUtilizados);
            this.grpReparacion.Controls.Add(this.btnConfirmarReparacion);
            this.grpReparacion.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.grpReparacion.Location = new System.Drawing.Point(12, 215);
            this.grpReparacion.Name = "grpReparacion";
            this.grpReparacion.Size = new System.Drawing.Size(876, 270);
            this.grpReparacion.TabIndex = 3;
            this.grpReparacion.TabStop = false;
            this.grpReparacion.Tag = "rfn2GrupoReparacion";
            this.grpReparacion.Text = "Reparación";
            // 
            // lblDiagnostico
            // 
            this.lblDiagnostico.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.lblDiagnostico.AutoSize = false;
            this.lblDiagnostico.Location = new System.Drawing.Point(15, 20);
            this.lblDiagnostico.Name = "lblDiagnostico";
            this.lblDiagnostico.Size = new System.Drawing.Size(846, 44);
            this.lblDiagnostico.TabIndex = 4;
            this.lblDiagnostico.Text = "";
            // 
            // lblTareas
            // 
            this.lblTareas.AutoSize = true;
            this.lblTareas.Location = new System.Drawing.Point(15, 68);
            this.lblTareas.Name = "lblTareas";
            this.lblTareas.Size = new System.Drawing.Size(150, 13);
            this.lblTareas.TabIndex = 5;
            this.lblTareas.Tag = "rfn2Tareas";
            this.lblTareas.Text = "Tareas realizadas";
            // 
            // txtTareas
            // 
            this.txtTareas.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.txtTareas.Location = new System.Drawing.Point(15, 86);
            this.txtTareas.MaxLength = 500;
            this.txtTareas.Multiline = true;
            this.txtTareas.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTareas.Name = "txtTareas";
            this.txtTareas.Size = new System.Drawing.Size(846, 70);
            this.txtTareas.TabIndex = 6;
            // 
            // lblRepuestosUtilizados
            // 
            this.lblRepuestosUtilizados.AutoSize = true;
            this.lblRepuestosUtilizados.Location = new System.Drawing.Point(15, 162);
            this.lblRepuestosUtilizados.Name = "lblRepuestosUtilizados";
            this.lblRepuestosUtilizados.Size = new System.Drawing.Size(150, 13);
            this.lblRepuestosUtilizados.TabIndex = 7;
            this.lblRepuestosUtilizados.Tag = "rfn2RepuestosUtilizados";
            this.lblRepuestosUtilizados.Text = "Repuestos utilizados";
            // 
            // txtRepuestosUtilizados
            // 
            this.txtRepuestosUtilizados.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.txtRepuestosUtilizados.Location = new System.Drawing.Point(15, 180);
            this.txtRepuestosUtilizados.MaxLength = 300;
            this.txtRepuestosUtilizados.Multiline = true;
            this.txtRepuestosUtilizados.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRepuestosUtilizados.Name = "txtRepuestosUtilizados";
            this.txtRepuestosUtilizados.Size = new System.Drawing.Size(846, 45);
            this.txtRepuestosUtilizados.TabIndex = 8;
            // 
            // btnConfirmarReparacion
            // 
            this.btnConfirmarReparacion.Location = new System.Drawing.Point(15, 233);
            this.btnConfirmarReparacion.Name = "btnConfirmarReparacion";
            this.btnConfirmarReparacion.Size = new System.Drawing.Size(200, 28);
            this.btnConfirmarReparacion.TabIndex = 9;
            this.btnConfirmarReparacion.Tag = "rfn2ConfirmarReparacion";
            this.btnConfirmarReparacion.Text = "Registrar reparación";
            this.btnConfirmarReparacion.UseVisualStyleBackColor = true;
            this.btnConfirmarReparacion.Click += new System.EventHandler(this.btnConfirmarReparacion_Click);
            // 
            // grpFinalizacion
            // 
            this.grpFinalizacion.Controls.Add(this.lblPresupuesto);
            this.grpFinalizacion.Controls.Add(this.nudPresupuesto);
            this.grpFinalizacion.Controls.Add(this.btnConfirmarFinalizacion);
            this.grpFinalizacion.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.grpFinalizacion.Location = new System.Drawing.Point(12, 493);
            this.grpFinalizacion.Name = "grpFinalizacion";
            this.grpFinalizacion.Size = new System.Drawing.Size(876, 70);
            this.grpFinalizacion.TabIndex = 10;
            this.grpFinalizacion.TabStop = false;
            this.grpFinalizacion.Tag = "rfn2GrupoFinalizacion";
            this.grpFinalizacion.Text = "Informe de finalización";
            // 
            // lblPresupuesto
            // 
            this.lblPresupuesto.AutoSize = true;
            this.lblPresupuesto.Location = new System.Drawing.Point(15, 31);
            this.lblPresupuesto.Name = "lblPresupuesto";
            this.lblPresupuesto.Size = new System.Drawing.Size(110, 13);
            this.lblPresupuesto.TabIndex = 11;
            this.lblPresupuesto.Tag = "rfn2Presupuesto";
            this.lblPresupuesto.Text = "Presupuesto final";
            // 
            // nudPresupuesto
            // 
            this.nudPresupuesto.Location = new System.Drawing.Point(135, 28);
            this.nudPresupuesto.Name = "nudPresupuesto";
            this.nudPresupuesto.DecimalPlaces = 2;
            this.nudPresupuesto.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            this.nudPresupuesto.ThousandsSeparator = true;
            this.nudPresupuesto.Size = new System.Drawing.Size(150, 20);
            this.nudPresupuesto.TabIndex = 12;
            // 
            // btnConfirmarFinalizacion
            // 
            this.btnConfirmarFinalizacion.Location = new System.Drawing.Point(305, 25);
            this.btnConfirmarFinalizacion.Name = "btnConfirmarFinalizacion";
            this.btnConfirmarFinalizacion.Size = new System.Drawing.Size(200, 28);
            this.btnConfirmarFinalizacion.TabIndex = 13;
            this.btnConfirmarFinalizacion.Tag = "rfn2ConfirmarFinalizacion";
            this.btnConfirmarFinalizacion.Text = "Finalizar reparación";
            this.btnConfirmarFinalizacion.UseVisualStyleBackColor = true;
            this.btnConfirmarFinalizacion.Click += new System.EventHandler(this.btnConfirmarFinalizacion_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.btnSalir.Location = new System.Drawing.Point(785, 575);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(100, 30);
            this.btnSalir.TabIndex = 14;
            this.btnSalir.Tag = "SalirButton";
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // FormRegistroReparacion_96_PP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 620);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.grpTurnos);
            this.Controls.Add(this.grpReparacion);
            this.Controls.Add(this.grpFinalizacion);
            this.Controls.Add(this.btnSalir);
            this.Name = "FormRegistroReparacion_96_PP";
            this.Text = "Registrar reparación";
            this.Load += new System.EventHandler(this.FormRegistroReparacion_96_PP_Load);
            this.grpFinalizacion.ResumeLayout(false);
            this.grpFinalizacion.PerformLayout();
            this.grpReparacion.ResumeLayout(false);
            this.grpReparacion.PerformLayout();
            this.grpTurnos.ResumeLayout(false);
            this.grpTurnos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPresupuesto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.GroupBox grpTurnos;
        private System.Windows.Forms.DataGridView dgvTurnos;
        private System.Windows.Forms.GroupBox grpReparacion;
        private System.Windows.Forms.Label lblDiagnostico;
        private System.Windows.Forms.Label lblTareas;
        private System.Windows.Forms.TextBox txtTareas;
        private System.Windows.Forms.Label lblRepuestosUtilizados;
        private System.Windows.Forms.TextBox txtRepuestosUtilizados;
        private System.Windows.Forms.Button btnConfirmarReparacion;
        private System.Windows.Forms.GroupBox grpFinalizacion;
        private System.Windows.Forms.Label lblPresupuesto;
        private System.Windows.Forms.NumericUpDown nudPresupuesto;
        private System.Windows.Forms.Button btnConfirmarFinalizacion;
        private System.Windows.Forms.Button btnSalir;
    }
}
