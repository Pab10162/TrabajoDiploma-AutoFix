using BE_96_PP;
using ClassLibrary2;
using ClassLibrary3;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using BLL;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using Servicio;
namespace GUI_96_PP.Administracion
{
    public partial class FormBitacoraEventos_96_PP : Form, IdiomaObserver_96_PP
    {
        private List<Evento_96_PP> eventos;
        private List<Evento_96_PP> filtrados;

        public void actualizarIdioma()
        {
            var traductor = new BLL_Idioma_96_PP();
            traductor.Traducir(this);
        }

        public FormBitacoraEventos_96_PP()
        {
            InitializeComponent();
            SistemaBase.Tema_96_PP.Aplicar(this);

            eventos = new BLL_BitacoraEvento_96_PP().obtenerEventos()
                .Where(ev => ev.fecha >= DateTime.Today.AddDays(-3))
                .ToList(); filtrados = new List<Evento_96_PP>(eventos);

            dataGridView1.DataSource = filtrados;

            var user = SessionManager_96_PP.getInstancia().getUsuarioActivo();

            Evento_96_PP evento = new Evento_96_PP(
                user.Dni,
                DateTime.Now,
                "Eventos",
                "Consultó el listado de eventos",
                Evento_96_PP.Criticidad.Bajo
            );

            dateTimePicker1.MaxDate = DateTime.Today;
            dateTimePicker2.MaxDate = DateTime.Today;

             button1.Click += button1_Click;
            button2.Click += button2_Click;

            SessionManager_96_PP.getInstancia().Suscribir(this);
            actualizarIdioma();
            button3.Enabled = TienePermiso(Permisos_96_PP.ExportarPdfBitacora);
        }

        private void FormBitacoraEventos_96_PP_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(eventos.Select(ev => ev.modulo).Distinct().ToArray());

            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(Enum.GetNames(typeof(Evento_96_PP.Criticidad)));
            dateTimePicker1.Value = DateTime.Today.AddDays(-3);
            dateTimePicker2.Value = DateTime.Today;
        }



        private void button1_Click(object sender, EventArgs e)
        {
             if (dateTimePicker1.Value.Date > dateTimePicker2.Value.Date)
            {
                new BLL_Idioma_96_PP().MostrarMensaje("La fecha 'Desde' no puede ser posterior a la fecha 'Hasta'.");
                return;
            }

            var query = eventos.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(textBox1.Text))
                query = query.Where(ev => ev.dni.Contains(textBox1.Text));

            if (comboBox1.SelectedItem != null && !string.IsNullOrWhiteSpace(comboBox1.SelectedItem.ToString()))
                query = query.Where(ev => ev.modulo == comboBox1.SelectedItem.ToString());

            if (comboBox2.SelectedItem != null)
            {
                var crit = (Evento_96_PP.Criticidad)Enum.Parse(typeof(Evento_96_PP.Criticidad), comboBox2.SelectedItem.ToString());
                query = query.Where(ev => ev.criticidad == crit);
            }

            var desde = dateTimePicker1.Value.Date;
            var hasta = dateTimePicker2.Value.Date.AddDays(1).AddSeconds(-1); 

            query = query.Where(ev => ev.fecha >= desde && ev.fecha <= hasta);

            filtrados = query.ToList();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = filtrados;
        }



        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;

            dateTimePicker1.Value = eventos.Min(ev => ev.fecha);
            dateTimePicker2.Value = DateTime.Today;

            filtrados = new List<Evento_96_PP>(eventos);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = filtrados;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count == 0)
                {
                    new BLL_Idioma_96_PP().MostrarMensaje("No hay datos para exportar.");
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF (*.pdf)|*.pdf";
                saveFileDialog.FileName = "BitacoraEventos.pdf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        Document pdfDoc = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
                        PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();

                        iTextSharp.text.Font fontTitulo = new iTextSharp.text.Font(
                           iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD
                       );

                        iTextSharp.text.Font fontCabecera = new iTextSharp.text.Font(
                            iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD
                        );

                        iTextSharp.text.Font fontContenido = new iTextSharp.text.Font(
                            iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL
                        );

                        // Agregar título
                        string tituloTraducido = new BLL_Idioma_96_PP().TraducirMensaje("Bitácora de Eventos");
                        Paragraph titulo = new Paragraph(tituloTraducido, fontTitulo);
                        titulo.Alignment = Element.ALIGN_CENTER;
                        pdfDoc.Add(titulo);
                        pdfDoc.Add(new Paragraph("\n"));

                        PdfPTable pdfTable = new PdfPTable(dataGridView1.Columns.Count);
                        pdfTable.WidthPercentage = 100;

                        foreach (DataGridViewColumn column in dataGridView1.Columns)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, fontCabecera));
                            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfTable.AddCell(cell);
                        }

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    pdfTable.AddCell(new Phrase(cell.Value?.ToString() ?? "", fontContenido));
                                }
                            }
                        }

                        pdfDoc.Add(pdfTable);
                        pdfDoc.Close();
                        stream.Close();
                    }

                    new BLL_Idioma_96_PP().MostrarMensaje("PDF exportado correctamente.");

                    string a = SessionManager_96_PP.getInstancia().getUsuarioActivo().Dni;

                    Evento_96_PP ee = new Evento_96_PP(a, DateTime.Now, "Eventos", "Exportacion a pdf de evento", Evento_96_PP.Criticidad.Bajo);
                    new BLL_BitacoraEvento_96_PP().RegistrarEvento(ee);
                }
            }
            catch (Exception ex)
            {
                new BLL_Idioma_96_PP().MostrarMensaje("Error al exportar PDF: " + ex.Message);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Today.AddDays(-3);
            dateTimePicker2.Value = DateTime.Today;
        }

        private bool TienePermiso(string permiso)
        {
            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            return usuario?.Rol != null && usuario.Rol.TienePermiso(permiso);
        }
    }
}
