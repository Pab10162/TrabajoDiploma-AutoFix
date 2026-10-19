using System;

namespace BE_96_PP
{
    
    public class BE_Vehiculo_96_PP
    {
        public string Patente { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public BE_Cliente_96_PP Cliente { get; set; }
        public bool Activo { get; set; }

        public string DniCliente => Cliente?.Dni;
        public string NombreCliente => Cliente == null ? null : $"{Cliente.Apellido}, {Cliente.Nombre}";

        public BE_Vehiculo_96_PP()
        {
            Activo = true;
        }

        public BE_Vehiculo_96_PP(string patente, string marca, string modelo, BE_Cliente_96_PP cliente, bool activo = true)
        {
            Patente = patente;
            Marca = marca;
            Modelo = modelo;
            Cliente = cliente;
            Activo = activo;
        }

        public override string ToString()
        {
            return $"{Patente} - {Marca} {Modelo}";
        }
    }
}
