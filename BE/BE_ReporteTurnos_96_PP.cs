using System;
using System.Collections.Generic;

namespace BE_96_PP
{
    public class BE_FilaReporteTurno_96_PP
    {
        public int IdTurno { get; set; }
        public DateTime Fecha { get; set; }
        public string Horario { get; set; }
        public string Estado { get; set; }
        public string DniCliente { get; set; }
        public string Cliente { get; set; }
        public string Telefono { get; set; }
        public string Patente { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Mecanico { get; set; }
    }

    public class BE_ReporteTurnos_96_PP
    {
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public string EstadoFiltro { get; set; }        
        public DateTime FechaGeneracion { get; set; }
        public string GeneradoPor { get; set; }

        public List<BE_FilaReporteTurno_96_PP> Filas { get; set; } = new List<BE_FilaReporteTurno_96_PP>();

        public int TotalTurnos { get; set; }
        public int Pendientes { get; set; }
        public int Confirmados { get; set; }
        public int Atendidos { get; set; }
        public int Cancelados { get; set; }

        public int HorariosDelPeriodo { get; set; }     
        public int HorariosOcupados { get; set; }       
        public decimal PorcentajeOcupacion { get; set; }
        public decimal PorcentajeCancelacion { get; set; }

        public string DiaMasDemandado { get; set; }    
        public string HorarioMasDemandado { get; set; } 
    }
}
