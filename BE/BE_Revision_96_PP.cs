using System;

namespace BE_96_PP
{
    // RFN2: informe general de revisión del Jefe de Taller y
    // orden de inspección al Mecánico.
    public class BE_Revision_96_PP
    {
        public int IdRevision { get; set; }
        public int IdTurno { get; set; }
        public string Observaciones { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string DniJefe { get; set; }
        public string DniMecanico { get; set; }

        public string HoraTexto => Hora.ToString(@"hh\:mm");
    }
}
