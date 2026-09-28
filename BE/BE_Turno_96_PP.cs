using System;
using System.Xml.Serialization;

namespace BE_96_PP
{
    public class BE_Turno_96_PP
    {
        public enum EstadoTurno
        {
            Pendiente,
            Confirmado,
            Cancelado,
            Atendido
        }

        public int IdTurno { get; set; }
        public DateTime Fecha { get; set; }

        
        [XmlIgnore]
        public TimeSpan Horario { get; set; }

        [XmlElement("Horario")]
        public string HorarioXml
        {
            get => Horario.ToString();
            set => Horario = string.IsNullOrEmpty(value) ? TimeSpan.Zero : TimeSpan.Parse(value);
        }

        public EstadoTurno Estado { get; set; }
        public BE_Cliente_96_PP Cliente { get; set; }
        public BE_Vehiculo_96_PP Vehiculo { get; set; }

        public string DniMecanico { get; set; }

        public string DniCliente => Cliente?.Dni;
        public string NombreCliente => Cliente == null ? null : $"{Cliente.Apellido}, {Cliente.Nombre}";
        public string PatenteVehiculo => Vehiculo?.Patente;
        public string HorarioTexto => Horario.ToString(@"hh\:mm");

        public BE_Turno_96_PP()
        {
            Estado = EstadoTurno.Pendiente;
        }

        public BE_Turno_96_PP(DateTime fecha, TimeSpan horario, BE_Cliente_96_PP cliente, BE_Vehiculo_96_PP vehiculo)
        {
            Fecha = fecha.Date;
            Horario = horario;
            Estado = EstadoTurno.Pendiente;
            Cliente = cliente;
            Vehiculo = vehiculo;
            DniMecanico = null;
        }

        public BE_Turno_96_PP(int idTurno, DateTime fecha, TimeSpan horario, EstadoTurno estado,
                             BE_Cliente_96_PP cliente, BE_Vehiculo_96_PP vehiculo, string dniMecanico)
        {
            IdTurno = idTurno;
            Fecha = fecha.Date;
            Horario = horario;
            Estado = estado;
            Cliente = cliente;
            Vehiculo = vehiculo;
            DniMecanico = dniMecanico;
        }

        public override string ToString()
        {
            return $"#{IdTurno} {Fecha:dd/MM/yyyy} {HorarioTexto} - {PatenteVehiculo}";
        }
    }
}
