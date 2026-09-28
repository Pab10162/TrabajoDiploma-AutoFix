using System;

namespace BE_96_PP
{
    // RFN1 - Entidad Cliente.
    // El DNI es el identificador del cliente (paso 2 del RFN1: "verifica si el cliente
    // se encuentra registrado mediante su DNI").
    public class BE_Cliente_96_PP
    {
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }

        // Baja lógica: el cliente nunca se borra porque tiene turnos/vehículos asociados.
        public bool Activo { get; set; }

        public BE_Cliente_96_PP()
        {
            Activo = true;
        }

        public BE_Cliente_96_PP(string dni, string nombre, string apellido, string telefono, string direccion, bool activo = true)
        {
            Dni = dni;
            Nombre = nombre;
            Apellido = apellido;
            Telefono = telefono;
            Direccion = direccion;
            Activo = activo;
        }

        public override string ToString()
        {
            return $"{Dni} - {Apellido}, {Nombre}";
        }
    }
}
