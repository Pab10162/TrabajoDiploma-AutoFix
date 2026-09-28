using BE_625NS;
using ClassLibrary2;
using ClassLibrary3;
using DAL_625NS;
using System;
using System.Data;

namespace BLL
{
    // Comportamiento compartido por las BLL del RFN1:
    //  - recalcular los dígitos verificadores de la tabla después de cada escritura
    //  - registrar el evento en la bitácora con el usuario que tiene la sesión abierta
    // Es lo mismo que hace BLL_Usuario_56PS en el SistemaBase, pero centralizado.
    public abstract class BLL_Taller_Comun_56PS
    {
        protected void RecalcularDV(string tabla)
        {
            DataTable registros = DAL_56PS.ConsultarTabla(tabla);
            new BLL_DigitoVerificador_56PS().CalcularDV(tabla, registros);
        }

        protected void RegistrarEvento(string modulo, string descripcion, Evento_56PS.Criticidad criticidad)
        {
            var usuario = SessionManager_56PS.getInstancia().getUsuarioActivo();
            string dni = usuario?.Dni ?? "Sistema";

            new BLL_BitacoraEvento_56PS().RegistrarEvento(
                new Evento_56PS(dni, DateTime.Now, modulo, descripcion, criticidad));
        }

        protected static bool EsSoloDigitos(string texto)
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
