using BE_96_PP;
using BE_96_PP;
using DAL_96_PP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    // RFN1 pasos 4 y 5: verificar si el vehículo existe (por patente) y, si no,
    // registrarlo asociándolo al cliente.
    public class BLL_Vehiculo_96_PP : BLL_Taller_Comun_96_PP
    {
        private const string Tabla = "Vehiculo_56PS";
        private const string Modulo = "Vehiculos";

        private readonly DAL_Vehiculo_96_PP dal = new DAL_Vehiculo_96_PP();
        private readonly DAL_Cliente_96_PP dalCliente = new DAL_Cliente_96_PP();
        private readonly DAL_Turno_96_PP dalTurno = new DAL_Turno_96_PP();

        public List<BE_Vehiculo_96_PP> ObtenerTodos()
        {
            return dal.ObtenerTodos();
        }

        public List<BE_Vehiculo_96_PP> ObtenerActivosPorCliente(string dniCliente)
        {
            if (string.IsNullOrWhiteSpace(dniCliente))
                return new List<BE_Vehiculo_96_PP>();

            return dal.ObtenerPorCliente(dniCliente.Trim()).Where(v => v.Activo).ToList();
        }

        // Paso 4 del RFN1. Devuelve null si la patente no está registrada.
        public BE_Vehiculo_96_PP BuscarPorPatente(string patente)
        {
            string normalizada = NormalizarPatente(patente);
            return string.IsNullOrEmpty(normalizada) ? null : dal.ObtenerPorPatente(normalizada);
        }

        // Paso 5 del RFN1.
        public void Alta(BE_Vehiculo_96_PP vehiculo)
        {
            Normalizar(vehiculo);
            Validar(vehiculo);

            if (dal.Existe(vehiculo.Patente))
                throw new Exception("Ya existe un vehículo registrado con esa patente.");

            vehiculo.Activo = true;
            dal.Crear(vehiculo);
            RecalcularDV(Tabla);
            RegistrarEvento(Modulo,
                $"Alta de vehículo {vehiculo.Patente} (cliente {vehiculo.Cliente.Dni})",
                Evento_96_PP.Criticidad.Bajo);
        }

        public void Modificar(BE_Vehiculo_96_PP vehiculo)
        {
            Normalizar(vehiculo);
            Validar(vehiculo);

            if (!dal.Existe(vehiculo.Patente))
                throw new Exception("El vehículo no existe.");

            dal.Modificar(vehiculo);
            RecalcularDV(Tabla);
            RegistrarEvento(Modulo, "Modificación de vehículo " + vehiculo.Patente, Evento_96_PP.Criticidad.Bajo);
        }

        public void CambiarEstadoActivo(BE_Vehiculo_96_PP vehiculo)
        {
            if (vehiculo == null)
                throw new Exception("Debe seleccionar un vehículo.");

            bool nuevoEstado = !vehiculo.Activo;

            // No se da de baja un vehículo que todavía tiene turnos por atender.
            if (!nuevoEstado && dalTurno.VehiculoTieneTurnosVigentes(vehiculo.Patente))
                throw new Exception("El vehículo tiene turnos vigentes. Cancélelos antes de darlo de baja.");

            // Para reactivar un vehículo, su dueño tiene que estar activo.
            if (nuevoEstado)
            {
                BE_Cliente_96_PP dueño = dalCliente.ObtenerPorDni(vehiculo.Cliente.Dni);
                if (dueño == null || !dueño.Activo)
                    throw new Exception("El cliente está dado de baja.");
            }

            vehiculo.Activo = nuevoEstado;
            dal.Modificar(vehiculo);
            RecalcularDV(Tabla);
            RegistrarEvento(
                Modulo,
                (vehiculo.Activo ? "Reactivación de vehículo " : "Baja de vehículo ") + vehiculo.Patente,
                Evento_96_PP.Criticidad.Medio);
        }

        // Guarda las patentes siempre igual: sin espacios ni guiones y en mayúscula
        // ("ab 123 cd" -> "AB123CD"), así la búsqueda por patente no falla por el formato.
        public static string NormalizarPatente(string patente)
        {
            if (string.IsNullOrWhiteSpace(patente))
                return null;

            return new string(patente.Where(char.IsLetterOrDigit).ToArray()).ToUpperInvariant();
        }

        private void Normalizar(BE_Vehiculo_96_PP v)
        {
            if (v == null)
                return;

            v.Patente = NormalizarPatente(v.Patente);
            v.Marca = v.Marca?.Trim();
            v.Modelo = v.Modelo?.Trim();
            v.Cedula = v.Cedula?.Trim();
        }

        private void Validar(BE_Vehiculo_96_PP v)
        {
            if (v == null)
                throw new Exception("Debe completar los datos del vehículo.");

            if (string.IsNullOrEmpty(v.Patente) || string.IsNullOrEmpty(v.Marca) ||
                string.IsNullOrEmpty(v.Modelo) || string.IsNullOrEmpty(v.Cedula))
                throw new Exception("Complete todos los campos");

            // Formatos argentinos: AAA999 (viejo) o AA999AA (Mercosur).
            if (v.Patente.Length < 6 || v.Patente.Length > 7)
                throw new Exception("La patente debe tener 6 o 7 caracteres.");

            if (v.Cliente == null)
                throw new Exception("Debe seleccionar un cliente.");

            BE_Cliente_96_PP dueño = dalCliente.ObtenerPorDni(v.Cliente.Dni);
            if (dueño == null)
                throw new Exception("El cliente no existe.");

            if (!dueño.Activo)
                throw new Exception("El cliente está dado de baja.");
        }
    }
}
