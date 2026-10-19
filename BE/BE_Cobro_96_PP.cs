using System;

namespace BE_96_PP
{
    // RFN2: cobro del servicio registrado por la Recepcionista.
    public class BE_Cobro_96_PP
    {
        public enum MedioPago
        {
            Efectivo,
            Debito,
            Credito,
            MercadoPago,
            Transferencia
        }

        public int IdCobro { get; set; }
        public int IdTurno { get; set; }
        public decimal MontoTotal { get; set; }
        public MedioPago MetodoPago { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string DniCliente { get; set; }
        public string DniRecepcionista { get; set; }

        public string HoraTexto => Hora.ToString(@"hh\:mm");
    }
}
