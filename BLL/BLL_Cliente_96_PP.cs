using ClassLibrary2;
using ClassLibrary3;
using System.Data;
using BE_96_PP;
using BE_96_PP;
using DAL_96_PP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    // RFN1 pasos 2 y 3: verificar si el cliente existe (por DNI) y, si no, registrarlo.
    public class BLL_Cliente_96_PP
    {
        private const string Tabla = "Cliente_56PS";
        private const string Modulo = "Clientes";

        private readonly DAL_Cliente_96_PP dal = new DAL_Cliente_96_PP();
        private readonly DAL_Turno_96_PP dalTurno = new DAL_Turno_96_PP();

        public List<BE_Cliente_96_PP> ObtenerTodos()
        {
            return dal.ObtenerTodos();
        }

        public List<BE_Cliente_96_PP> ObtenerActivos()
        {
            return dal.ObtenerTodos().Where(c => c.Activo).ToList();
        }

        // Paso 2 del RFN1. Devuelve null si el cliente no está registrado.
        public BE_Cliente_96_PP BuscarPorDni(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                return null;

            return dal.ObtenerPorDni(dni.Trim());
        }

        // Paso 3 del RFN1.
        public void Alta(BE_Cliente_96_PP cliente)
        {
            Normalizar(cliente);
            Validar(cliente);

            if (dal.Existe(cliente.Dni))
                throw new Exception("Ya existe un cliente registrado con ese DNI.");

            cliente.Activo = true;
            dal.Crear(cliente);
            RecalcularDV(Tabla);
            RegistrarEvento(Modulo, "Alta de cliente " + cliente.Dni, Evento_96_PP.Criticidad.Bajo);
        }

        public void Modificar(BE_Cliente_96_PP cliente)
        {
            Normalizar(cliente);
            Validar(cliente);

            if (!dal.Existe(cliente.Dni))
                throw new Exception("El cliente no existe.");

            dal.Modificar(cliente);
            RecalcularDV(Tabla);
            RegistrarEvento(Modulo, "Modificación de cliente " + cliente.Dni, Evento_96_PP.Criticidad.Bajo);
        }

        // Baja lógica / reactivación. No se borra porque tiene vehículos y turnos asociados.
        public void CambiarEstadoActivo(BE_Cliente_96_PP cliente)
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
                Evento_96_PP.Criticidad.Medio);
        }

        private void Normalizar(BE_Cliente_96_PP c)
        {
            if (c == null)
                return;

            c.Dni = c.Dni?.Trim();
            c.Nombre = c.Nombre?.Trim();
            c.Apellido = c.Apellido?.Trim();
            c.Telefono = c.Telefono?.Trim();
            c.Direccion = c.Direccion?.Trim();
        }

        private void Validar(BE_Cliente_96_PP c)
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
    

        private void RecalcularDV(string tabla)
        {
            DataTable registros = DAL_Comun_96_PP.ConsultarTabla(tabla);
            new BLL_DigitoVerificador_96_PP().CalcularDV(tabla, registros);
        }

        private void RegistrarEvento(string modulo, string descripcion, Evento_96_PP.Criticidad criticidad)
        {
            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            string dni = usuario?.Dni ?? "Sistema";

            new BLL_BitacoraEvento_96_PP().RegistrarEvento(
                new Evento_96_PP(dni, DateTime.Now, modulo, descripcion, criticidad));
        }

        private static bool EsSoloDigitos(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return false;

            foreach (char c in texto)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }
    }
}
