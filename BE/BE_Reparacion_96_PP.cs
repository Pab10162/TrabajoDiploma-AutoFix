using System;

namespace BE_96_PP
{
    // RFN2: tareas realizadas, repuestos usados e informe de finalización.
    public class BE_Reparacion_96_PP
    {
        public int IdReparacion { get; set; }
        public int IdDiagnostico { get; set; }
        public string TareasRealizadas { get; set; }
        public string RepuestosUtilizados { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string DniMecanico { get; set; }

        public decimal? PresupuestoFinal { get; set; }
        public DateTime? FechaFinalizacion { get; set; }

        public bool Finalizada => FechaFinalizacion.HasValue;
        public string HoraTexto => Hora.ToString(@"hh\:mm");
    }
}
