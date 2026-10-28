using System;

namespace BE_96_PP
{
    // RFN2 paso 2: la Recepcionista registra la llegada del cliente a su turno.
    public class BE_Asistencia_96_PP
    {
        public enum EstadoAsistencia
        {
            Presente
        }

        public int IdAsistencia { get; set; }
        public int IdTurno { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public EstadoAsistencia Estado { get; set; }
        public string DniRecepcionista { get; set; }

        public string HoraTexto => Hora.ToString(@"hh\:mm");

        public BE_Asistencia_96_PP()
        {
            Estado = EstadoAsistencia.Presente;
        }
    }
}
