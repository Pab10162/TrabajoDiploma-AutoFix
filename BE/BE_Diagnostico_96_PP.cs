using System;

namespace BE_96_PP
{
    // RFN2: diagnóstico técnico del Mecánico y tipo de solución.
    public class BE_Diagnostico_96_PP
    {
        public enum NivelUrgencia
        {
            Baja,
            Media,
            Alta,
            Critica
        }

        public enum TipoDeSolucion
        {
            Reparacion,
            Reemplazo,
            Ajuste,
            Mantenimiento
        }

        public int IdDiagnostico { get; set; }
        public int IdTurno { get; set; }
        public string SistemaAfectado { get; set; }
        public string DescripcionFallas { get; set; }
        public NivelUrgencia Urgencia { get; set; }
        public string RepuestosNecesarios { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string DniMecanico { get; set; }

        public TipoDeSolucion? TipoSolucion { get; set; }
        public string DescripcionSolucion { get; set; }

        public string HoraTexto => Hora.ToString(@"hh\:mm");
    }
}
