using System;
using System.Collections.Generic;

namespace BE_56_PS
{
    // A02 - Reporte asociado al RFN1: "Reporte de Turnos de Reparación".
    // Una fila del reporte: datos planos (texto) listos para mostrar en la grilla y en el PDF.
    public class BE_FilaReporteTurno_56PS
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

    // El reporte completo: el período pedido, las filas y los indicadores calculados por la BLL.
    public class BE_ReporteTurnos_56PS
    {
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public string EstadoFiltro { get; set; }        // null = todos
        public DateTime FechaGeneracion { get; set; }
        public string GeneradoPor { get; set; }

        public List<BE_FilaReporteTurno_56PS> Filas { get; set; } = new List<BE_FilaReporteTurno_56PS>();

        // Indicadores (se calculan sobre TODOS los turnos del período, sin importar el filtro de estado)
        public int TotalTurnos { get; set; }
        public int Pendientes { get; set; }
        public int Confirmados { get; set; }
        public int Atendidos { get; set; }
        public int Cancelados { get; set; }

        public int HorariosDelPeriodo { get; set; }     // capacidad: días x horarios por día
        public int HorariosOcupados { get; set; }       // turnos no cancelados
        public decimal PorcentajeOcupacion { get; set; }
        public decimal PorcentajeCancelacion { get; set; }

        public string DiaMasDemandado { get; set; }     // día de la semana con más turnos
        public string HorarioMasDemandado { get; set; } // franja horaria con más turnos
    }
}
