using System;
using System.Xml.Serialization;

namespace BE_56_PS
{
    public class BE_Turno_56PS
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
        public BE_Cliente_56PS Cliente { get; set; }
        public BE_Vehiculo_56PS Vehiculo { get; set; }

        // DNI del usuario con rol Mecánico. 
        public string DniMecanico { get; set; }

        // Propiedades de solo lectura para las grillas.
        public string DniCliente => Cliente?.Dni;
        public string NombreCliente => Cliente == null ? null : $"{Cliente.Apellido}, {Cliente.Nombre}";
        public string PatenteVehiculo => Vehiculo?.Patente;
        public string HorarioTexto => Horario.ToString(@"hh\:mm");

        public BE_Turno_56PS()
        {
            Estado = EstadoTurno.Pendiente;
        }

        // Constructor para un turno nuevo 
        public BE_Turno_56PS(DateTime fecha, TimeSpan horario, BE_Cliente_56PS cliente, BE_Vehiculo_56PS vehiculo)
        {
            Fecha = fecha.Date;
            Horario = horario;
            Estado = EstadoTurno.Pendiente;
            Cliente = cliente;
            Vehiculo = vehiculo;
            DniMecanico = null;
        }

        // Constructor completo 
        public BE_Turno_56PS(int idTurno, DateTime fecha, TimeSpan horario, EstadoTurno estado,
                             BE_Cliente_56PS cliente, BE_Vehiculo_56PS vehiculo, string dniMecanico)
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
