using System;
using System.Drawing;
using System.Windows.Forms;

namespace SistemaBase
{
    /// <summary>
    /// Tema visual del sistema. Una sola clase define la paleta y estiliza cualquier
    /// formulario: llamar a Tema_96_PP.Aplicar(this) justo despues de InitializeComponent().
    /// Para cambiar el look de todo el sistema alcanza con tocar los colores de abajo.
    /// </summary>
    public static class Tema_96_PP
    {
        // ------------------------- Paleta -------------------------
        public static readonly Color Fondo        = Color.FromArgb(244, 246, 249);
        public static readonly Color FondoMdi     = Color.FromArgb(232, 236, 242);
        public static readonly Color Superficie   = Color.White;
        public static readonly Color Oscuro       = Color.FromArgb(33, 41, 54);
        public static readonly Color OscuroHover  = Color.FromArgb(52, 63, 80);
        public static readonly Color Acento       = Color.FromArgb(230, 115, 25);
        public static readonly Color AcentoHover  = Color.FromArgb(205, 98, 14);
        public static readonly Color AcentoPress  = Color.FromArgb(180, 82, 8);
        public static readonly Color AcentoSuave  = Color.FromArgb(253, 233, 213);
        public static readonly Color Texto        = Color.FromArgb(38, 45, 56);
        public static readonly Color TextoSuave   = Color.FromArgb(120, 130, 145);
        public static readonly Color Borde        = Color.FromArgb(200, 206, 216);
        public static readonly Color Deshabilitado = Color.FromArgb(226, 230, 236);
        public static readonly Color FilaAlterna  = Color.FromArgb(248, 249, 251);
        public static readonly Color LineaGrilla  = Color.FromArgb(230, 234, 240);

        // ------------------------- Tipografia -------------------------
        private const string FuenteBase = "Segoe UI";
        private const string FuenteTitulo = "Segoe UI Semibold";
        private const float TamanoBase = 9F;
        private const float TamanoTituloMax = 20F;

        // Palabras que identifican la accion principal de una pantalla (boton naranja).
        private static readonly string[] AccionesPrincipales =
        {
            "aplicar", "asignar", "iniciar", "ingresar", "guardar", "ejecutar",
            "confirmar", "generar", "restaurar", "restore", "backup", "recalcular", "cambiar"
        };

        private static bool rendererGlobalAplicado;

        public static void Aplicar(Form form)
        {
            if (form == null) { return; }

            if (!rendererGlobalAplicado)
            {
                ToolStripManager.Renderer = new TemaRenderer();
                rendererGlobalAplicado = true;
            }

            form.SuspendLayout();

            form.Font = new Font(FuenteBase, TamanoBase);
            form.BackColor = Fondo;
            form.ForeColor = Texto;

            foreach (Control control in form.Controls)
            {
                Estilizar(control);
            }

            // El area gris de fondo de un formulario MDI contenedor es un control aparte.
            foreach (Control control in form.Controls)
            {
                MdiClient mdi = control as MdiClient;
                if (mdi != null) { mdi.BackColor = FondoMdi; }
            }

            form.ResumeLayout(true);
        }

        private static void Estilizar(Control control)
        {
            Button boton = control as Button;
            if (boton != null) { EstilizarBoton(boton); }

            TextBox texto = control as TextBox;
            if (texto != null) { EstilizarTexto(texto); }

            ComboBox combo = control as ComboBox;
            if (combo != null)
            {
                combo.FlatStyle = FlatStyle.Flat;
                combo.BackColor = Superficie;
            }

            DataGridView grilla = control as DataGridView;
            if (grilla != null) { EstilizarGrilla(grilla); }

            GroupBox grupo = control as GroupBox;
            if (grupo != null)
            {
                grupo.ForeColor = Oscuro;
                grupo.Font = new Font(FuenteTitulo, TamanoBase);
            }

            Label etiqueta = control as Label;
            if (etiqueta != null) { EstilizarEtiqueta(etiqueta); }

            ListBox lista = control as ListBox;
            if (lista != null)
            {
                lista.BorderStyle = BorderStyle.FixedSingle;
                lista.BackColor = Superficie;
            }

            TreeView arbol = control as TreeView;
            if (arbol != null)
            {
                arbol.BorderStyle = BorderStyle.FixedSingle;
                arbol.BackColor = Superficie;
            }

            MenuStrip menu = control as MenuStrip;
            if (menu != null) { EstilizarMenu(menu); }

            foreach (Control hijo in control.Controls)
            {
                Estilizar(hijo);
            }
        }

        // ------------------------- Botones -------------------------
        private static void EstilizarBoton(Button boton)
        {
            bool principal = EsAccionPrincipal(boton);

            boton.FlatStyle = FlatStyle.Flat;
            boton.UseVisualStyleBackColor = false;
            boton.Cursor = Cursors.Hand;
            boton.FlatAppearance.BorderSize = 1;

            AplicarColoresBoton(boton, principal);

            // Los formularios activan y desactivan botones segun el estado: se restilizan solos.
            boton.EnabledChanged += delegate (object sender, EventArgs e)
            {
                AplicarColoresBoton((Button)sender, principal);
            };
        }

        private static bool EsAccionPrincipal(Button boton)
        {
            string identificador = ((boton.Name ?? "") + " " + (boton.Text ?? "")).ToLowerInvariant();
            foreach (string palabra in AccionesPrincipales)
            {
                if (identificador.Contains(palabra)) { return true; }
            }
            return false;
        }

        private static void AplicarColoresBoton(Button boton, bool principal)
        {
            if (!boton.Enabled)
            {
                boton.BackColor = Deshabilitado;
                boton.ForeColor = TextoSuave;
                boton.FlatAppearance.BorderColor = Deshabilitado;
                boton.FlatAppearance.MouseOverBackColor = Deshabilitado;
                boton.FlatAppearance.MouseDownBackColor = Deshabilitado;
            }
            else if (principal)
            {
                boton.BackColor = Acento;
                boton.ForeColor = Color.White;
                boton.FlatAppearance.BorderColor = Acento;
                boton.FlatAppearance.MouseOverBackColor = AcentoHover;
                boton.FlatAppearance.MouseDownBackColor = AcentoPress;
            }
            else
            {
                boton.BackColor = Superficie;
                boton.ForeColor = Texto;
                boton.FlatAppearance.BorderColor = Borde;
                boton.FlatAppearance.MouseOverBackColor = AcentoSuave;
                boton.FlatAppearance.MouseDownBackColor = Color.FromArgb(248, 215, 185);
            }
        }

        // ------------------------- Texto y etiquetas -------------------------
        private static void EstilizarTexto(TextBox texto)
        {
            texto.BorderStyle = BorderStyle.FixedSingle;
            texto.BackColor = texto.ReadOnly ? Fondo : Superficie;
        }

        private static void EstilizarEtiqueta(Label etiqueta)
        {
            // Las etiquetas con una fuente propia en el diseñador (titulos, negritas) se pasan
            // a la tipografia del tema; las demas heredan la fuente del formulario.
            if (etiqueta.Font.FontFamily.Name != "Microsoft Sans Serif") { return; }

            if (etiqueta.Font.Size >= 14F)
            {
                float tamano = Math.Min(etiqueta.Font.Size, TamanoTituloMax);
                etiqueta.Font = new Font(FuenteTitulo, tamano);
                etiqueta.ForeColor = Oscuro;
            }
            else
            {
                etiqueta.Font = new Font(FuenteBase, TamanoBase, etiqueta.Font.Style);
            }
        }

        // ------------------------- Grillas -------------------------
        private static void EstilizarGrilla(DataGridView grilla)
        {
            grilla.BorderStyle = BorderStyle.None;
            grilla.BackgroundColor = Superficie;
            grilla.GridColor = LineaGrilla;
            grilla.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grilla.RowHeadersVisible = false;
            grilla.EnableHeadersVisualStyles = false;

            grilla.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grilla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grilla.ColumnHeadersHeight = 32;
            grilla.ColumnHeadersDefaultCellStyle.BackColor = Oscuro;
            grilla.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grilla.ColumnHeadersDefaultCellStyle.SelectionBackColor = Oscuro;
            grilla.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            grilla.ColumnHeadersDefaultCellStyle.Font = new Font(FuenteTitulo, TamanoBase);
            grilla.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grilla.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 0, 0);

            grilla.RowTemplate.Height = 26;
            grilla.DefaultCellStyle.BackColor = Superficie;
            grilla.DefaultCellStyle.ForeColor = Texto;
            grilla.DefaultCellStyle.SelectionBackColor = AcentoSuave;
            grilla.DefaultCellStyle.SelectionForeColor = Texto;
            grilla.DefaultCellStyle.Padding = new Padding(4, 0, 0, 0);
            grilla.AlternatingRowsDefaultCellStyle.BackColor = FilaAlterna;
        }

        // ------------------------- Menu -------------------------
        private static void EstilizarMenu(MenuStrip menu)
        {
            menu.Renderer = new TemaRenderer();
            menu.BackColor = Oscuro;
            menu.ForeColor = Color.White;
            menu.Font = new Font(FuenteBase, 9.5F);
            menu.Padding = new Padding(8, 4, 0, 4);

            foreach (ToolStripItem item in menu.Items)
            {
                EstilizarItemMenu(item, true);
            }
        }

        private static void EstilizarItemMenu(ToolStripItem item, bool nivelSuperior)
        {
            item.ForeColor = nivelSuperior ? Color.White : Texto;

            ToolStripMenuItem itemMenu = item as ToolStripMenuItem;
            if (itemMenu == null) { return; }

            itemMenu.DropDown.BackColor = Superficie;
            itemMenu.DropDown.Font = new Font(FuenteBase, TamanoBase);

            foreach (ToolStripItem sub in itemMenu.DropDownItems)
            {
                EstilizarItemMenu(sub, false);
            }
        }

        // ------------------------- Renderer del menu -------------------------
        private class TemaColores : ProfessionalColorTable
        {
            public TemaColores() { UseSystemColors = false; }

            public override Color MenuStripGradientBegin { get { return Oscuro; } }
            public override Color MenuStripGradientEnd { get { return Oscuro; } }
            public override Color ToolStripDropDownBackground { get { return Superficie; } }
            public override Color ImageMarginGradientBegin { get { return Superficie; } }
            public override Color ImageMarginGradientMiddle { get { return Superficie; } }
            public override Color ImageMarginGradientEnd { get { return Superficie; } }
            public override Color MenuBorder { get { return Borde; } }
            public override Color MenuItemBorder { get { return Color.Transparent; } }
            public override Color SeparatorDark { get { return Borde; } }
            public override Color SeparatorLight { get { return Superficie; } }
        }

        private class TemaRenderer : ToolStripProfessionalRenderer
        {
            public TemaRenderer() : base(new TemaColores())
            {
                RoundedEdges = false;
            }

            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                ToolStripItem item = e.Item;
                if (!item.Enabled || !(item.Selected || item.Pressed)) { return; }

                Color relleno = item.IsOnDropDown ? AcentoSuave : OscuroHover;
                Rectangle area = new Rectangle(Point.Empty, item.Size);
                if (item.IsOnDropDown) { area.Inflate(-2, 0); }

                using (SolidBrush pincel = new SolidBrush(relleno))
                {
                    e.Graphics.FillRectangle(pincel, area);
                }
            }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                if (!e.Item.Enabled)
                {
                    e.TextColor = e.Item.IsOnDropDown ? TextoSuave : Color.FromArgb(125, 135, 150);
                }
                base.OnRenderItemText(e);
            }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                // La barra de menu superior va sin borde; los desplegables conservan el suyo.
                if (e.ToolStrip is MenuStrip) { return; }
                base.OnRenderToolStripBorder(e);
            }
        }
    }
}
