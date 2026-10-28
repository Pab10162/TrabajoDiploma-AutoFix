namespace SistemaBase.Diagnostico
{
    partial class FormCobro_96_PP
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
            this.grpCobro = new System.Windows.Forms.GroupBox();
            this.lblResumen = new System.Windows.Forms.Label();
            this.lblMonto = new System.Windows.Forms.Label();
            this.nudMonto = new System.Windows.Forms.NumericUpDown();
            this.lblMedioPago = new System.Windows.Forms.Label();
            this.cmbMedioPago = new System.Windows.Forms.ComboBox();
            this.btnConfirmarCobro = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMonto)).BeginInit();
            this.grpTurnos.SuspendLayout();
            this.grpCobro.SuspendLayout();
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
            this.lblTitulo.Tag = "rfn2CobroTitulo";
            this.lblTitulo.Text = "Registrar cobro";
            // 
            // grpTurnos
            // 
            this.grpTurnos.Controls.Add(this.dgvTurnos);
            this.grpTurnos.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.grpTurnos.Location = new System.Drawing.Point(12, 45);
            this.grpTurnos.Name = "grpTurnos";
            this.grpTurnos.Size = new System.Drawing.Size(876, 200);
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
            this.dgvTurnos.Size = new System.Drawing.Size(856, 170);
            this.dgvTurnos.TabIndex = 2;
            this.dgvTurnos.SelectionChanged += new System.EventHandler(this.dgvTurnos_SelectionChanged);
            // 
            // grpCobro
            // 
            this.grpCobro.Controls.Add(this.lblResumen);
            this.grpCobro.Controls.Add(this.lblMonto);
            this.grpCobro.Controls.Add(this.nudMonto);
            this.grpCobro.Controls.Add(this.lblMedioPago);
            this.grpCobro.Controls.Add(this.cmbMedioPago);
            this.grpCobro.Controls.Add(this.btnConfirmarCobro);
            this.grpCobro.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.grpCobro.Location = new System.Drawing.Point(12, 255);
            this.grpCobro.Name = "grpCobro";
            this.grpCobro.Size = new System.Drawing.Size(876, 200);
            this.grpCobro.TabIndex = 3;
            this.grpCobro.TabStop = false;
            this.grpCobro.Tag = "rfn2GrupoCobro";
            this.grpCobro.Text = "Datos del cobro";
            // 
            // lblResumen
            // 
            this.lblResumen.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.lblResumen.AutoSize = false;
            this.lblResumen.Location = new System.Drawing.Point(15, 22);
            this.lblResumen.Name = "lblResumen";
            this.lblResumen.Size = new System.Drawing.Size(846, 60);
            this.lblResumen.TabIndex = 4;
            this.lblResumen.Text = "";
            // 
            // lblMonto
            // 
            this.lblMonto.AutoSize = true;
            this.lblMonto.Location = new System.Drawing.Point(15, 98);
            this.lblMonto.Name = "lblMonto";
            this.lblMonto.Size = new System.Drawing.Size(100, 13);
            this.lblMonto.TabIndex = 5;
            this.lblMonto.Tag = "rfn2Monto";
            this.lblMonto.Text = "Monto total";
            // 
            // nudMonto
            // 
            this.nudMonto.Location = new System.Drawing.Point(130, 95);
            this.nudMonto.Name = "nudMonto";
            this.nudMonto.DecimalPlaces = 2;
            this.nudMonto.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            this.nudMonto.ThousandsSeparator = true;
            this.nudMonto.Size = new System.Drawing.Size(150, 20);
            this.nudMonto.TabIndex = 6;
            // 
            // lblMedioPago
            // 
            this.lblMedioPago.AutoSize = true;
            this.lblMedioPago.Location = new System.Drawing.Point(320, 98);
            this.lblMedioPago.Name = "lblMedioPago";
            this.lblMedioPago.Size = new System.Drawing.Size(100, 13);
            this.lblMedioPago.TabIndex = 7;
            this.lblMedioPago.Tag = "rfn2MedioPago";
            this.lblMedioPago.Text = "Medio de pago";
            // 
            // cmbMedioPago
            // 
            this.cmbMedioPago.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMedioPago.FormattingEnabled = true;
            this.cmbMedioPago.Location = new System.Drawing.Point(430, 95);
            this.cmbMedioPago.Name = "cmbMedioPago";
            this.cmbMedioPago.Size = new System.Drawing.Size(180, 21);
            this.cmbMedioPago.TabIndex = 8;
            // 
            // btnConfirmarCobro
            // 
            this.btnConfirmarCobro.Location = new System.Drawing.Point(15, 140);
            this.btnConfirmarCobro.Name = "btnConfirmarCobro";
            this.btnConfirmarCobro.Size = new System.Drawing.Size(200, 30);
            this.btnConfirmarCobro.TabIndex = 9;
            this.btnConfirmarCobro.Tag = "rfn2ConfirmarCobro";
            this.btnConfirmarCobro.Text = "Registrar cobro";
            this.btnConfirmarCobro.UseVisualStyleBackColor = true;
            this.btnConfirmarCobro.Click += new System.EventHandler(this.btnConfirmarCobro_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.btnSalir.Location = new System.Drawing.Point(785, 575);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(100, 30);
            this.btnSalir.TabIndex = 10;
            this.btnSalir.Tag = "SalirButton";
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // FormCobro_96_PP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 620);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.grpTurnos);
            this.Controls.Add(this.grpCobro);
            this.Controls.Add(this.btnSalir);
            this.Name = "FormCobro_96_PP";
            this.Text = "Registrar cobro";
            this.Load += new System.EventHandler(this.FormCobro_96_PP_Load);
            this.grpCobro.ResumeLayout(false);
            this.grpCobro.PerformLayout();
            this.grpTurnos.ResumeLayout(false);
            this.grpTurnos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMonto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.GroupBox grpTurnos;
        private System.Windows.Forms.DataGridView dgvTurnos;
        private System.Windows.Forms.GroupBox grpCobro;
        private System.Windows.Forms.Label lblResumen;
        private System.Windows.Forms.Label lblMonto;
        private System.Windows.Forms.NumericUpDown nudMonto;
        private System.Windows.Forms.Label lblMedioPago;
        private System.Windows.Forms.ComboBox cmbMedioPago;
        private System.Windows.Forms.Button btnConfirmarCobro;
        private System.Windows.Forms.Button btnSalir;
    }
}
