using BE_96_PP;
using ClassLibrary2;
using System;
using System.Windows.Forms;

namespace SistemaBase.Diagnostico
{
    // Utilidades compartidas por los formularios de la entrega 2 (RFN2).
    internal static class Ayuda_96_PP
    {
        public static bool TienePermiso(string permiso)
        {
            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            return usuario?.Rol != null && usuario.Rol.TienePermiso(permiso);
        }

        // Deja visibles solo las columnas útiles de una grilla de turnos.
        public static void PrepararGrillaTurnos(DataGridView grilla)
        {
            grilla.DataBindingComplete += GrillaTurnos_DataBindingComplete;
        }

        private static void GrillaTurnos_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView grilla = (DataGridView)sender;

            OcultarColumnas(grilla, "Cliente", "Vehiculo", "Horario", "HorarioXml", "DniMecanico");

            if (grilla.Columns.Contains("Fecha"))
                grilla.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        public static void OcultarColumnas(DataGridView grilla, params string[] columnas)
        {
            foreach (string columna in columnas)
            {
                if (grilla.Columns.Contains(columna))
                    grilla.Columns[columna].Visible = false;
            }
        }

        // Vuelve a seleccionar en la grilla el turno con ese id (después de recargar los datos).
        public static void SeleccionarTurno(DataGridView grilla, int idTurno)
        {
            DataGridViewColumn primera = grilla.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
            if (primera == null)
                return;

            foreach (DataGridViewRow fila in grilla.Rows)
            {
                BE_Turno_96_PP turno = fila.DataBoundItem as BE_Turno_96_PP;
                if (turno != null && turno.IdTurno == idTurno)
                {
                    grilla.CurrentCell = fila.Cells[primera.Index];
                    fila.Selected = true;
                    return;
                }
            }
        }

        // Resumen de una línea del turno: número, fecha, cliente y vehículo.
        public static string Describir(BE_Turno_96_PP turno)
        {
            if (turno == null)
                return string.Empty;

            string vehiculo = turno.Vehiculo == null
                ? turno.PatenteVehiculo
                : $"{turno.Vehiculo.Marca} {turno.Vehiculo.Modelo} - {turno.Vehiculo.Patente}";

            return $"#{turno.IdTurno}   {turno.Fecha:dd/MM/yyyy} {turno.HorarioTexto}   |   {turno.NombreCliente} ({turno.DniCliente})   |   {vehiculo}";
        }
    }
}
