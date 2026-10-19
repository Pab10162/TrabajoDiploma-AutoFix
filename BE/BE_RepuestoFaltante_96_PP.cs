namespace BE_96_PP
{
    // RFN2: repuesto necesario que no tiene stock.
    public class BE_RepuestoFaltante_96_PP
    {
        public enum EstadoFaltante
        {
            PendienteCompra,
            Comprado
        }

        public int IdFaltante { get; set; }
        public int IdDiagnostico { get; set; }
        public string Repuesto { get; set; }
        public int Cantidad { get; set; }
        public EstadoFaltante Estado { get; set; }

        public BE_RepuestoFaltante_96_PP()
        {
            Estado = EstadoFaltante.PendienteCompra;
        }
    }
}
