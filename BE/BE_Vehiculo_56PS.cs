using System;

namespace BE_56_PS
{
    // RFN1 - Entidad Vehículo.
    // La patente es el identificador (paso 4 del RFN1: "verifica si el vehículo se
    // encuentra registrado mediante la patente"). Cada vehículo pertenece a un cliente.
    public class BE_Vehiculo_56PS
    {
        public string Patente { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Cedula { get; set; }
        public BE_Cliente_56PS Cliente { get; set; }
        public bool Activo { get; set; }

        // Propiedades de solo lectura para mostrar el dueño en las grillas.
        public string DniCliente => Cliente?.Dni;
        public string NombreCliente => Cliente == null ? null : $"{Cliente.Apellido}, {Cliente.Nombre}";

        public BE_Vehiculo_56PS()
        {
            Activo = true;
        }

        public BE_Vehiculo_56PS(string patente, string marca, string modelo, string cedula, BE_Cliente_56PS cliente, bool activo = true)
        {
            Patente = patente;
            Marca = marca;
            Modelo = modelo;
            Cedula = cedula;
            Cliente = cliente;
            Activo = activo;
        }

        public override string ToString()
        {
            return $"{Patente} - {Marca} {Modelo}";
        }
    }
}
