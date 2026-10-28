using BE_96_PP;
using ClassLibrary2;
using ClassLibrary3;
using DAL_96_PP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BLL
{
    // RFN2 paso 8: repuestos que el taller no tiene y deben comprarse para poder reparar.
    public class BLL_RepuestoFaltante_96_PP
    {
        private const string Tabla = "RepuestoFaltante_56PS";
        private const string Modulo = "Repuestos";

        private readonly DAL_RepuestoFaltante_96_PP dal = new DAL_RepuestoFaltante_96_PP();
        private readonly DAL_Diagnostico_96_PP dalDiagnostico = new DAL_Diagnostico_96_PP();

        public int Alta(BE_RepuestoFaltante_96_PP faltante)
        {
            if (faltante == null)
                throw new Exception("El repuesto faltante no puede ser nulo.");
            if (faltante.IdDiagnostico <= 0)
                throw new Exception("Debe indicar el diagnóstico.");
            if (string.IsNullOrWhiteSpace(faltante.Repuesto))
                throw new Exception("Debe indicar el repuesto.");
            if (faltante.Cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor a cero.");

            var usuario = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            if (usuario == null)
                throw new Exception("No hay un usuario con sesión iniciada.");

            BE_Diagnostico_96_PP diagnostico = dalDiagnostico.ObtenerPorId(faltante.IdDiagnostico);
            if (diagnostico == null)
                throw new Exception("El diagnóstico no existe.");
            if (diagnostico.DniMecanico != usuario.Dni)
                throw new Exception("Solo el mecánico que registró el diagnóstico puede informar repuestos faltantes.");

            faltante.Repuesto = faltante.Repuesto.Trim();
            faltante.Estado = BE_RepuestoFaltante_96_PP.EstadoFaltante.PendienteCompra;
            faltante.IdFaltante = dal.Crear(faltante);

            RecalcularDV(Tabla);
            RegistrarEvento(Modulo,
                $"Repuesto faltante #{faltante.IdFaltante} - {faltante.Repuesto} x{faltante.Cantidad} - Diagnóstico #{faltante.IdDiagnostico}",
                Evento_96_PP.Criticidad.Medio);

            return faltante.IdFaltante;
        }

        public void MarcarComprado(int idFaltante)
        {
            if (idFaltante <= 0)
                throw new Exception("Debe seleccionar un repuesto.");

            dal.CambiarEstado(idFaltante, BE_RepuestoFaltante_96_PP.EstadoFaltante.Comprado);

            RecalcularDV(Tabla);
            RegistrarEvento(Modulo, $"Repuesto faltante #{idFaltante} marcado como comprado", Evento_96_PP.Criticidad.Bajo);
        }

        public List<BE_RepuestoFaltante_96_PP> ObtenerPorDiagnostico(int idDiagnostico)
        {
            if (idDiagnostico <= 0)
                throw new Exception("Debe indicar el diagnóstico.");
            return dal.ObtenerPorDiagnostico(idDiagnostico);
        }

        public bool TienePendientes(int idDiagnostico)
        {
            return dal.ObtenerPorDiagnostico(idDiagnostico)
                .Any(f => f.Estado == BE_RepuestoFaltante_96_PP.EstadoFaltante.PendienteCompra);
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
    }
}
