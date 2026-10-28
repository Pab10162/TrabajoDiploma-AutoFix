namespace SistemaBase.Diagnostico
{
    partial class FormAsistencia_96_PP
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
            this.lblFecha = new System.Windows.Forms.Label();
            this.lblFechaValor = new System.Windows.Forms.Label();
            this.lblBuscar = new System.Windows.Forms.Label();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.dgvTurnos = new System.Windows.Forms.DataGridView();
            this.btnConfirmarLlegada = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnos)).BeginInit();
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
            this.lblTitulo.Tag = "rfn2AsistenciaTitulo";
            this.lblTitulo.Text = "Registrar asistencia";
            // 
            // lblFecha
            // 
            this.lblFecha.AutoSize = true;
            this.lblFecha.Location = new System.Drawing.Point(15, 52);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(40, 13);
            this.lblFecha.TabIndex = 1;
            this.lblFecha.Tag = "rfn2Fecha";
            this.lblFecha.Text = "Fecha:";
            // 
            // lblFechaValor
            // 
            this.lblFechaValor.AutoSize = true;
            this.lblFechaValor.Location = new System.Drawing.Point(70, 52);
            this.lblFechaValor.Name = "lblFechaValor";
            this.lblFechaValor.Size = new System.Drawing.Size(80, 13);
            this.lblFechaValor.TabIndex = 2;
            this.lblFechaValor.Text = "00/00/0000";
            // 
            // lblBuscar
            // 
            this.lblBuscar.AutoSize = true;
            this.lblBuscar.Location = new System.Drawing.Point(300, 52);
            this.lblBuscar.Name = "lblBuscar";
            this.lblBuscar.Size = new System.Drawing.Size(200, 13);
            this.lblBuscar.TabIndex = 3;
            this.lblBuscar.Tag = "rfn2BuscarTurno";
            this.lblBuscar.Text = "Buscar (DNI, cliente o patente)";
            // 
            // txtBuscar
            // 
            this.txtBuscar.Location = new System.Drawing.Point(520, 49);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(250, 20);
            this.txtBuscar.TabIndex = 4;
            this.txtBuscar.TextChanged += new System.EventHandler(this.txtBuscar_TextChanged);
            // 
            // dgvTurnos
            // 
            this.dgvTurnos.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom;
            this.dgvTurnos.AllowUserToAddRows = false;
            this.dgvTurnos.AllowUserToDeleteRows = false;
            this.dgvTurnos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTurnos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTurnos.Location = new System.Drawing.Point(15, 80);
            this.dgvTurnos.MultiSelect = false;
            this.dgvTurnos.Name = "dgvTurnos";
            this.dgvTurnos.ReadOnly = true;
            this.dgvTurnos.RowHeadersVisible = false;
            this.dgvTurnos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTurnos.Size = new System.Drawing.Size(870, 480);
            this.dgvTurnos.TabIndex = 5;
            this.dgvTurnos.SelectionChanged += new System.EventHandler(this.dgvTurnos_SelectionChanged);
            // 
            // btnConfirmarLlegada
            // 
            this.btnConfirmarLlegada.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.btnConfirmarLlegada.Location = new System.Drawing.Point(15, 575);
            this.btnConfirmarLlegada.Name = "btnConfirmarLlegada";
            this.btnConfirmarLlegada.Size = new System.Drawing.Size(180, 30);
            this.btnConfirmarLlegada.TabIndex = 6;
            this.btnConfirmarLlegada.Tag = "rfn2ConfirmarLlegada";
            this.btnConfirmarLlegada.Text = "Registrar llegada";
            this.btnConfirmarLlegada.UseVisualStyleBackColor = true;
            this.btnConfirmarLlegada.Click += new System.EventHandler(this.btnConfirmarLlegada_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.btnSalir.Location = new System.Drawing.Point(785, 575);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(100, 30);
            this.btnSalir.TabIndex = 7;
            this.btnSalir.Tag = "SalirButton";
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // FormAsistencia_96_PP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 620);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblFecha);
            this.Controls.Add(this.lblFechaValor);
            this.Controls.Add(this.lblBuscar);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.dgvTurnos);
            this.Controls.Add(this.btnConfirmarLlegada);
            this.Controls.Add(this.btnSalir);
            this.Name = "FormAsistencia_96_PP";
            this.Text = "Registrar asistencia";
            this.Load += new System.EventHandler(this.FormAsistencia_96_PP_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.Label lblFechaValor;
        private System.Windows.Forms.Label lblBuscar;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.DataGridView dgvTurnos;
        private System.Windows.Forms.Button btnConfirmarLlegada;
        private System.Windows.Forms.Button btnSalir;
    }
}
