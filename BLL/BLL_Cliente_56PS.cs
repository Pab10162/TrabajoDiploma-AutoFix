using BE_56_PS;
using BE_625NS;
using DAL_625NS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    // RFN1 pasos 2 y 3: verificar si el cliente existe (por DNI) y, si no, registrarlo.
    public class BLL_Cliente_56PS : BLL_Taller_Comun_56PS
    {
        private const string Tabla = "Cliente_56PS";
        private const string Modulo = "Clientes";

        private readonly DAL_Cliente_56PS dal = new DAL_Cliente_56PS();
        private readonly DAL_Turno_56PS dalTurno = new DAL_Turno_56PS();

        public List<BE_Cliente_56PS> ObtenerTodos()
        {
            return dal.ObtenerTodos();
        }

        public List<BE_Cliente_56PS> ObtenerActivos()
        {
            return dal.ObtenerTodos().Where(c => c.Activo).ToList();
        }

        // Paso 2 del RFN1. Devuelve null si el cliente no está registrado.
        public BE_Cliente_56PS BuscarPorDni(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                return null;

            return dal.ObtenerPorDni(dni.Trim());
        }

        // Paso 3 del RFN1.
        public void Alta(BE_Cliente_56PS cliente)
        {
            Normalizar(cliente);
            Validar(cliente);

            if (dal.Existe(cliente.Dni))
                throw new Exception("Ya existe un cliente registrado con ese DNI.");

            cliente.Activo = true;
            dal.Crear(cliente);
            RecalcularDV(Tabla);
            RegistrarEvento(Modulo, "Alta de cliente " + cliente.Dni, Evento_56PS.Criticidad.Bajo);
        }

        public void Modificar(BE_Cliente_56PS cliente)
        {
            Normalizar(cliente);
            Validar(cliente);

            if (!dal.Existe(cliente.Dni))
                throw new Exception("El cliente no existe.");

            dal.Modificar(cliente);
            RecalcularDV(Tabla);
            RegistrarEvento(Modulo, "Modificación de cliente " + cliente.Dni, Evento_56PS.Criticidad.Bajo);
        }

        // Baja lógica / reactivación. No se borra porque tiene vehículos y turnos asociados.
        public void CambiarEstadoActivo(BE_Cliente_56PS cliente)
        {
            if (cliente == null)
                throw new Exception("Debe seleccionar un cliente.");

            bool nuevoEstado = !cliente.Activo;

            // No se da de baja a un cliente que todavía tiene turnos por atender.
            if (!nuevoEstado && dalTurno.ClienteTieneTurnosVigentes(cliente.Dni))
                throw new Exception("El cliente tiene turnos vigentes. Cancélelos antes de darlo de baja.");

            cliente.Activo = nuevoEstado;
            dal.Modificar(cliente);
            RecalcularDV(Tabla);
            RegistrarEvento(
                Modulo,
                (cliente.Activo ? "Reactivación de cliente " : "Baja de cliente ") + cliente.Dni,
                Evento_56PS.Criticidad.Medio);
        }

        private void Normalizar(BE_Cliente_56PS c)
        {
            if (c == null)
                return;

            c.Dni = c.Dni?.Trim();
            c.Nombre = c.Nombre?.Trim();
            c.Apellido = c.Apellido?.Trim();
            c.Telefono = c.Telefono?.Trim();
            c.Direccion = c.Direccion?.Trim();
        }

        private void Validar(BE_Cliente_56PS c)
        {
            if (c == null)
                throw new Exception("Debe completar los datos del cliente.");

            if (string.IsNullOrEmpty(c.Dni) || string.IsNullOrEmpty(c.Nombre) ||
                string.IsNullOrEmpty(c.Apellido) || string.IsNullOrEmpty(c.Telefono) ||
                string.IsNullOrEmpty(c.Direccion))
                throw new Exception("Complete todos los campos");

            if (!EsSoloDigitos(c.Dni) || c.Dni.Length < 7 || c.Dni.Length > 8)
                throw new Exception("El DNI debe tener 7 u 8 dígitos numéricos.");

            if (!EsNombreValido(c.Nombre) || !EsNombreValido(c.Apellido))
                throw new Exception("El nombre y el apellido solo pueden tener letras y espacios.");

            if (!EsTelefonoValido(c.Telefono))
                throw new Exception("El teléfono debe tener entre 6 y 15 dígitos.");
        }

        // Letras (con acentos y ñ), espacios, apóstrofo y guion: "María José", "D'Angelo".
        private static bool EsNombreValido(string texto)
        {
            return texto.All(ch => char.IsLetter(ch) || ch == ' ' || ch == '\'' || ch == '-');
        }

        // Se aceptan separadores comunes (+, espacios, guiones, paréntesis),
        // pero tiene que haber entre 6 y 15 dígitos.
        private static bool EsTelefonoValido(string telefono)
        {
            if (!telefono.All(ch => char.IsDigit(ch) || " +-()".IndexOf(ch) >= 0))
                return false;

            int digitos = telefono.Count(char.IsDigit);
            return digitos >= 6 && digitos <= 15;
        }
    }
}
