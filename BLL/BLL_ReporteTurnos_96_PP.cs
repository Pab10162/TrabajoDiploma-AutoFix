using BE_96_PP;
using BE_96_PP;
using ClassLibrary2;
using DAL_96_PP;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BLL
{
    // A02 - Reporte asociado al RFN1: "Reporte de Turnos de Reparación".
    // Es de solo lectura: no modifica datos. Arma el listado del período y calcula
    // los indicadores que sirven para planificar (ocupación, cancelaciones, demanda).
    public class BLL_ReporteTurnos_96_PP : BLL_Taller_Comun_96_PP
    {
        private const int MaximoDiasPorReporte = 366;

        private readonly DAL_Turno_96_PP dal = new DAL_Turno_96_PP();

        // estado = null para "todos los estados".
        public BE_ReporteTurnos_96_PP Generar(DateTime desde, DateTime hasta, BE_Turno_96_PP.EstadoTurno? estado)
        {
            desde = desde.Date;
            hasta = hasta.Date;

            if (desde > hasta)
                throw new Exception("La fecha desde no puede ser mayor que la fecha hasta.");

            if ((hasta - desde).TotalDays + 1 > MaximoDiasPorReporte)
                throw new Exception("El período del reporte no puede superar un año.");

            List<BE_FilaReporteTurno_96_PP> todas = dal.ObtenerParaReporte(desde, hasta);

            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();

            BE_ReporteTurnos_96_PP reporte = new BE_ReporteTurnos_96_PP
            {
                Desde = desde,
                Hasta = hasta,
                EstadoFiltro = estado?.ToString(),
                FechaGeneracion = DateTime.Now,
                GeneradoPor = usuario == null ? "-" : $"{usuario.NombreUsuario} ({usuario.Apellido}, {usuario.Nombre})",
                Filas = estado == null
                    ? todas
                    : todas.Where(f => f.Estado == estado.ToString()).ToList()
            };

            CalcularIndicadores(reporte, todas);

            RegistrarEvento("Reportes",
                $"Reporte de turnos {desde:dd/MM/yyyy} - {hasta:dd/MM/yyyy}" + (estado == null ? "" : $" ({estado})"),
                Evento_96_PP.Criticidad.Bajo);

            return reporte;
        }

        public void RegistrarExportacion(BE_ReporteTurnos_96_PP reporte)
        {
            RegistrarEvento("Reportes",
                $"Exportación a PDF del reporte de turnos {reporte.Desde:dd/MM/yyyy} - {reporte.Hasta:dd/MM/yyyy}",
                Evento_96_PP.Criticidad.Bajo);
        }

        // Los indicadores se calculan sobre todos los turnos del período (no sobre el
        // filtro de estado), porque describen cómo se usó la agenda del taller.
        private void CalcularIndicadores(BE_ReporteTurnos_96_PP r, List<BE_FilaReporteTurno_96_PP> todas)
        {
            r.TotalTurnos = todas.Count;
            r.Pendientes = todas.Count(f => f.Estado == nameof(BE_Turno_96_PP.EstadoTurno.Pendiente));
            r.Confirmados = todas.Count(f => f.Estado == nameof(BE_Turno_96_PP.EstadoTurno.Confirmado));
            r.Atendidos = todas.Count(f => f.Estado == nameof(BE_Turno_96_PP.EstadoTurno.Atendido));
            r.Cancelados = todas.Count(f => f.Estado == nameof(BE_Turno_96_PP.EstadoTurno.Cancelado));

            int dias = (int)(r.Hasta - r.Desde).TotalDays + 1;
            int horariosPorDia = BLL_Turno_96_PP.HoraUltimoTurno - BLL_Turno_96_PP.HoraPrimerTurno + 1;

            r.HorariosDelPeriodo = dias * horariosPorDia;
            r.HorariosOcupados = r.TotalTurnos - r.Cancelados;

            r.PorcentajeOcupacion = r.HorariosDelPeriodo == 0
                ? 0
                : Math.Round(100m * r.HorariosOcupados / r.HorariosDelPeriodo, 1);

            r.PorcentajeCancelacion = r.TotalTurnos == 0
                ? 0
                : Math.Round(100m * r.Cancelados / r.TotalTurnos, 1);

            List<BE_FilaReporteTurno_96_PP> vigentes = todas
                .Where(f => f.Estado != nameof(BE_Turno_96_PP.EstadoTurno.Cancelado))
                .ToList();

            CultureInfo es = new CultureInfo("es-AR");

            r.DiaMasDemandado = vigentes.Count == 0
                ? "-"
                : vigentes.GroupBy(f => f.Fecha.DayOfWeek)
                          .OrderByDescending(g => g.Count()).ThenBy(g => g.Key)
                          .Select(g => $"{es.DateTimeFormat.GetDayName(g.Key)} ({g.Count()})")
                          .First();

            r.HorarioMasDemandado = vigentes.Count == 0
                ? "-"
                : vigentes.GroupBy(f => f.Horario)
                          .OrderByDescending(g => g.Count()).ThenBy(g => g.Key)
                          .Select(g => $"{g.Key} ({g.Count()})")
                          .First();
        }
    }
}
