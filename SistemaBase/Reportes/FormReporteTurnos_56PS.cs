using BE_56_PS;
using BLL;
using ClassLibrary2;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Servicio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SistemaBase.Reportes
{
    // A02 - Reporte asociado al RFN1: "Reporte de Turnos de Reparación".
    // Menú Reportes -> Rep. 1 (RFN1). Pantalla de solo consulta: se eligen el período
    // y el estado, se genera el listado con sus indicadores y se puede exportar a PDF.
    public partial class FormReporteTurnos_56PS : Form, IdiomaObserver_56PS
    {
        private class OpcionEstado
        {
            public string Texto { get; set; }
            public BE_Turno_56PS.EstadoTurno? Valor { get; set; }
        }

        private readonly BLL_ReporteTurnos_56PS bllReporte = new BLL_ReporteTurnos_56PS();
        private readonly BLL_Idioma_56PS mensajes = new BLL_Idioma_56PS();

        private BE_ReporteTurnos_56PS reporteActual;

        public FormReporteTurnos_56PS()
        {
            InitializeComponent();
            SessionManager_56PS.getInstancia().Suscribir(this);
            actualizarIdioma();
        }

        public void actualizarIdioma()
        {
            mensajes.Traducir(this);
            Text = mensajes.TraducirMensaje("Reporte de turnos de reparación");
            CargarEstados();
            MostrarResumen();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            SessionManager_56PS.getInstancia().Desuscribir(this);
            base.OnFormClosed(e);
        }

        private void FormReporteTurnos_56PS_Load(object sender, EventArgs e)
        {
            // Por defecto: el mes actual.
            DateTime hoy = DateTime.Today;
            dtpDesde.Value = new DateTime(hoy.Year, hoy.Month, 1);
            dtpHasta.Value = dtpDesde.Value.AddMonths(1).AddDays(-1);

            btnExportarPdf.Enabled = false;
            MostrarResumen();
        }

        private void CargarEstados()
        {
            int seleccionado = cmbEstado.SelectedIndex;

            List<OpcionEstado> opciones = new List<OpcionEstado>
            {
                new OpcionEstado { Texto = mensajes.TraducirMensaje("Todos"), Valor = null }
            };

            foreach (BE_Turno_56PS.EstadoTurno estado in Enum.GetValues(typeof(BE_Turno_56PS.EstadoTurno)))
                opciones.Add(new OpcionEstado { Texto = mensajes.TraducirMensaje(estado.ToString()), Valor = estado });

            cmbEstado.DataSource = null;
            cmbEstado.DisplayMember = nameof(OpcionEstado.Texto);
            cmbEstado.DataSource = opciones;
            cmbEstado.SelectedIndex = seleccionado >= 0 && seleccionado < opciones.Count ? seleccionado : 0;
        }

        // ---------- Generar ----------

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                BE_Turno_56PS.EstadoTurno? estado = (cmbEstado.SelectedItem as OpcionEstado)?.Valor;

                reporteActual = bllReporte.Generar(dtpDesde.Value, dtpHasta.Value, estado);

                dgvReporte.DataSource = null;
                dgvReporte.DataSource = reporteActual.Filas;

                MostrarResumen();
                btnExportarPdf.Enabled = reporteActual.Filas.Count > 0;

                if (reporteActual.Filas.Count == 0)
                    mensajes.MostrarMensaje("No hay turnos para los filtros seleccionados.");
            }
            catch (Exception ex)
            {
                mensajes.MostrarMensaje(ex.Message);
            }
        }

        private void dgvReporte_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgvReporte.Columns.Contains("Fecha"))
                dgvReporte.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void MostrarResumen()
        {
            if (reporteActual == null)
            {
                lblResumen.Text = mensajes.TraducirMensaje("Seleccione el período y presione Generar.");
                return;
            }

            lblResumen.Text = string.Join(Environment.NewLine, LineasResumen(reporteActual));
        }

        // Mismo texto para la pantalla y para el PDF.
        private List<string> LineasResumen(BE_ReporteTurnos_56PS r)
        {
            return new List<string>
            {
                $"{Tr("Total de turnos")}: {r.TotalTurnos}   |   " +
                $"{Tr("Pendiente")}: {r.Pendientes}   {Tr("Confirmado")}: {r.Confirmados}   " +
                $"{Tr("Atendido")}: {r.Atendidos}   {Tr("Cancelado")}: {r.Cancelados}",

                $"{Tr("Ocupación de la agenda")}: {r.HorariosOcupados} / {r.HorariosDelPeriodo} " +
                $"{Tr("horarios")} ({r.PorcentajeOcupacion:0.0}%)   |   " +
                $"{Tr("Cancelaciones")}: {r.PorcentajeCancelacion:0.0}%",

                $"{Tr("Día con más turnos")}: {r.DiaMasDemandado}   |   " +
                $"{Tr("Horario con más turnos")}: {r.HorarioMasDemandado}",

                $"{Tr("Generado por")}: {r.GeneradoPor} - {r.FechaGeneracion:dd/MM/yyyy HH:mm}"
            };
        }

        // ---------- Exportar a PDF (mismo esquema que la bitácora del SistemaBase) ----------

        private void btnExportarPdf_Click(object sender, EventArgs e)
        {
            if (reporteActual == null || reporteActual.Filas.Count == 0)
            {
                mensajes.MostrarMensaje("No hay datos para exportar.");
                return;
            }

            using (SaveFileDialog dialogo = new SaveFileDialog())
            {
                dialogo.Filter = "PDF (*.pdf)|*.pdf";
                dialogo.FileName = $"ReporteTurnos_{reporteActual.Desde:yyyyMMdd}_{reporteActual.Hasta:yyyyMMdd}.pdf";

                if (dialogo.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    ExportarPdf(dialogo.FileName, reporteActual);
                    bllReporte.RegistrarExportacion(reporteActual);
                    mensajes.MostrarMensaje("PDF exportado correctamente.");
                }
                catch (Exception ex)
                {
                    mensajes.MostrarMensaje("Error al exportar PDF: " + ex.Message);
                }
            }
        }

        private void ExportarPdf(string ruta, BE_ReporteTurnos_56PS r)
        {
            iTextSharp.text.Font fTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font fNormal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Font fCabecera = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD);

            using (FileStream stream = new FileStream(ruta, FileMode.Create))
            {
                // A4 apaisado: la tabla tiene muchas columnas.
                Document pdf = new Document(PageSize.A4.Rotate(), 20f, 20f, 20f, 20f);
                PdfWriter.GetInstance(pdf, stream);
                pdf.Open();

                Paragraph titulo = new Paragraph(Tr("Reporte de turnos de reparación"), fTitulo);
                titulo.Alignment = Element.ALIGN_CENTER;
                pdf.Add(titulo);

                string estado = r.EstadoFiltro == null ? Tr("Todos") : Tr(r.EstadoFiltro);
                pdf.Add(new Paragraph(
                    $"{Tr("Período")}: {r.Desde:dd/MM/yyyy} - {r.Hasta:dd/MM/yyyy}   |   {Tr("Estado")}: {estado}", fNormal));
                pdf.Add(new Paragraph("\n"));

                // Tabla: se usan las columnas visibles de la grilla (ya traducidas).
                List<DataGridViewColumn> columnas = new List<DataGridViewColumn>();
                foreach (DataGridViewColumn c in dgvReporte.Columns)
                {
                    if (c.Visible)
                        columnas.Add(c);
                }
                columnas.Sort((a, b) => a.DisplayIndex.CompareTo(b.DisplayIndex));

                PdfPTable tabla = new PdfPTable(columnas.Count) { WidthPercentage = 100 };
                foreach (DataGridViewColumn c in columnas)
                {
                    PdfPCell celda = new PdfPCell(new Phrase(c.HeaderText, fCabecera));
                    celda.BackgroundColor = BaseColor.LIGHT_GRAY;
                    celda.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabla.AddCell(celda);
                }

                foreach (DataGridViewRow fila in dgvReporte.Rows)
                {
                    foreach (DataGridViewColumn c in columnas)
                    {
                        DataGridViewCell celda = fila.Cells[c.Index];
                        string texto = celda.Value is DateTime d ? d.ToString("dd/MM/yyyy") : celda.Value?.ToString() ?? "";
                        tabla.AddCell(new Phrase(texto, fNormal));
                    }
                }

                pdf.Add(tabla);
                pdf.Add(new Paragraph("\n"));

                pdf.Add(new Paragraph(Tr("Resumen"), fCabecera));
                foreach (string linea in LineasResumen(r))
                    pdf.Add(new Paragraph(linea, fNormal));

                pdf.Close();
            }
        }

        private string Tr(string clave)
        {
            return mensajes.TraducirMensaje(clave);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
