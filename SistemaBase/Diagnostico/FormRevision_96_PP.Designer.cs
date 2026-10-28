namespace SistemaBase.Diagnostico
{
    partial class FormRevision_96_PP
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
            this.grpRevision = new System.Windows.Forms.GroupBox();
            this.lblDetalle = new System.Windows.Forms.Label();
            this.lblMecanico = new System.Windows.Forms.Label();
            this.cmbMecanico = new System.Windows.Forms.ComboBox();
            this.lblObservaciones = new System.Windows.Forms.Label();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
            this.btnConfirmarRevision = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnos)).BeginInit();
            this.grpTurnos.SuspendLayout();
            this.grpRevision.SuspendLayout();
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
            this.lblTitulo.Tag = "rfn2RevisionTitulo";
            this.lblTitulo.Text = "Registrar revisión del vehículo";
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
            // grpRevision
            // 
            this.grpRevision.Controls.Add(this.lblDetalle);
            this.grpRevision.Controls.Add(this.lblMecanico);
            this.grpRevision.Controls.Add(this.cmbMecanico);
            this.grpRevision.Controls.Add(this.lblObservaciones);
            this.grpRevision.Controls.Add(this.txtObservaciones);
            this.grpRevision.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.grpRevision.Location = new System.Drawing.Point(12, 255);
            this.grpRevision.Name = "grpRevision";
            this.grpRevision.Size = new System.Drawing.Size(876, 300);
            this.grpRevision.TabIndex = 3;
            this.grpRevision.TabStop = false;
            this.grpRevision.Tag = "rfn2DatosRevision";
            this.grpRevision.Text = "Informe de revisión";
            // 
            // lblDetalle
            // 
            this.lblDetalle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.lblDetalle.AutoSize = false;
            this.lblDetalle.Location = new System.Drawing.Point(15, 22);
            this.lblDetalle.Name = "lblDetalle";
            this.lblDetalle.Size = new System.Drawing.Size(846, 36);
            this.lblDetalle.TabIndex = 4;
            this.lblDetalle.Text = "";
            // 
            // lblMecanico
            // 
            this.lblMecanico.AutoSize = true;
            this.lblMecanico.Location = new System.Drawing.Point(15, 75);
            this.lblMecanico.Name = "lblMecanico";
            this.lblMecanico.Size = new System.Drawing.Size(100, 13);
            this.lblMecanico.TabIndex = 5;
            this.lblMecanico.Tag = "rfn2Mecanico";
            this.lblMecanico.Text = "Mecánico asignado";
            // 
            // cmbMecanico
            // 
            this.cmbMecanico.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMecanico.FormattingEnabled = true;
            this.cmbMecanico.Location = new System.Drawing.Point(160, 72);
            this.cmbMecanico.Name = "cmbMecanico";
            this.cmbMecanico.Size = new System.Drawing.Size(350, 21);
            this.cmbMecanico.TabIndex = 6;
            // 
            // lblObservaciones
            // 
            this.lblObservaciones.AutoSize = true;
            this.lblObservaciones.Location = new System.Drawing.Point(15, 110);
            this.lblObservaciones.Name = "lblObservaciones";
            this.lblObservaciones.Size = new System.Drawing.Size(150, 13);
            this.lblObservaciones.TabIndex = 7;
            this.lblObservaciones.Tag = "rfn2Observaciones";
            this.lblObservaciones.Text = "Observaciones de la revisión";
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom;
            this.txtObservaciones.Location = new System.Drawing.Point(15, 130);
            this.txtObservaciones.MaxLength = 500;
            this.txtObservaciones.Multiline = true;
            this.txtObservaciones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.Size = new System.Drawing.Size(846, 155);
            this.txtObservaciones.TabIndex = 8;
            // 
            // btnConfirmarRevision
            // 
            this.btnConfirmarRevision.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.btnConfirmarRevision.Location = new System.Drawing.Point(15, 575);
            this.btnConfirmarRevision.Name = "btnConfirmarRevision";
            this.btnConfirmarRevision.Size = new System.Drawing.Size(180, 30);
            this.btnConfirmarRevision.TabIndex = 9;
            this.btnConfirmarRevision.Tag = "rfn2ConfirmarRevision";
            this.btnConfirmarRevision.Text = "Registrar revisión";
            this.btnConfirmarRevision.UseVisualStyleBackColor = true;
            this.btnConfirmarRevision.Click += new System.EventHandler(this.btnConfirmarRevision_Click);
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
            // FormRevision_96_PP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 620);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.grpTurnos);
            this.Controls.Add(this.grpRevision);
            this.Controls.Add(this.btnConfirmarRevision);
            this.Controls.Add(this.btnSalir);
            this.Name = "FormRevision_96_PP";
            this.Text = "Registrar revisión";
            this.Load += new System.EventHandler(this.FormRevision_96_PP_Load);
            this.grpRevision.ResumeLayout(false);
            this.grpRevision.PerformLayout();
            this.grpTurnos.ResumeLayout(false);
            this.grpTurnos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.GroupBox grpTurnos;
        private System.Windows.Forms.DataGridView dgvTurnos;
        private System.Windows.Forms.GroupBox grpRevision;
        private System.Windows.Forms.Label lblDetalle;
        private System.Windows.Forms.Label lblMecanico;
        private System.Windows.Forms.ComboBox cmbMecanico;
        private System.Windows.Forms.Label lblObservaciones;
        private System.Windows.Forms.TextBox txtObservaciones;
        private System.Windows.Forms.Button btnConfirmarRevision;
        private System.Windows.Forms.Button btnSalir;
    }
}
