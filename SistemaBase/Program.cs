using BLL;
using SistemaBase;
using System;
using System.Threading;
using System.Windows.Forms;

namespace GUI
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Red de seguridad: si algo inesperado falla en cualquier pantalla (por ejemplo,
            // se cae la conexión con SQL Server), se muestra el error y el programa sigue
            // abierto en lugar de cerrarse de golpe.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += ErrorNoControlado;

            Application.Run(new MenuPrincipal_56PS());
        }

        private static void ErrorNoControlado(object sender, ThreadExceptionEventArgs e)
        {
            new BLL_Idioma_56PS().MostrarMensaje(
                "Error: " + e.Exception.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
