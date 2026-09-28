using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_96_PP;
using Servicio;
using ClassLibrary2;
using ClassLibrary3;
using BE_96_PP;
using BE_96_PP;
using System.Data;
namespace BLL
{
    public class BLL_Familia_96_PP
    {
        DAL_Rol_96_PP dalrol = new DAL_Rol_96_PP();

        public void EliminarFamilia(Familia_96_PP familia)
        {
            if (familia == null || string.IsNullOrWhiteSpace(familia.Codigo))
                throw new Exception("Debe seleccionar una familia.");

            if (!dalrol.VerificarFamiliaEliminada(familia))
                throw new Exception("La familia no existe o ya se encuentra inactiva.");

            if (dalrol.VerificarAsignacionFamilia(familia))
                throw new Exception("No se puede eliminar la familia mientras tenga asignaciones. Primero quite sus relaciones.");

            familia.activo = false;
            dalrol.EliminarFamilia(familia);

            var user = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            Evento_96_PP evento = new Evento_96_PP(
                user.Dni,
                DateTime.Now,
                "Familias",
                "Eliminar familia",
                Evento_96_PP.Criticidad.Alto
            );

            new BLL_DigitoVerificador_96_PP().CalcularDV("Familias", DAL_96_PP.ConsultarTabla("Familias"));
            new BLL_BitacoraEvento_96_PP().RegistrarEvento(evento);
        }

        public void ModificarFamilia(Familia_96_PP f)
        {
            if (f == null || string.IsNullOrWhiteSpace(f.Codigo) || string.IsNullOrWhiteSpace(f.Nombre))
                throw new Exception("Debe indicar el código y el nombre de la familia.");

            if (dalrol.VerificarExistenciaFamiliaNombre(f))
                throw new Exception("Ya existe otra familia con ese nombre.");

            if (!dalrol.FamiliaEstaActiva(f.Codigo))
                throw new Exception("La familia no existe o se encuentra inactiva.");

            dalrol.Modificar(f);
            new BLL_DigitoVerificador_96_PP().CalcularDV("Familias", DAL_96_PP.ConsultarTabla("Familias"));
        }

        public void CrearFamilia(Familia_96_PP fam)
        {
            CrearFamilia(fam, Enumerable.Empty<Rol_96_PP>());
        }

        public void CrearFamilia(Familia_96_PP fam, IEnumerable<Rol_96_PP> elementos)
        {
            if (fam == null || string.IsNullOrWhiteSpace(fam.Codigo) || string.IsNullOrWhiteSpace(fam.Nombre))
                throw new Exception("Debe indicar el código y el nombre de la familia.");

            List<Rol_96_PP> elementosIniciales = ValidarElementosIniciales(fam, elementos);

            if (dalrol.VerificarExistenciaFamiliaCodigo(fam))
                throw new Exception("Ya existe una familia con ese código.");
            if (dalrol.VerificarExistenciaFamiliaNombre(fam))
                throw new Exception("Ya existe una familia con ese nombre.");

            try
            {
                dalrol.CrearFamiliaConElementos(fam, elementosIniciales);
                new BLL_DigitoVerificador_96_PP().CalcularDV("Familias", DAL_96_PP.ConsultarTabla("Familias"));
                new BLL_DigitoVerificador_96_PP().CalcularDV("FamiliaPatente", DAL_96_PP.ConsultarTabla("FamiliaPatente"));
                new BLL_DigitoVerificador_96_PP().CalcularDV("FamiliaFamilia", DAL_96_PP.ConsultarTabla("FamiliaFamilia"));
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

            var user = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            Evento_96_PP evento = new Evento_96_PP(
                user.Dni,
                DateTime.Now,
                "Familias",
                "Crear familia",
                Evento_96_PP.Criticidad.Alto
            );
            new BLL_BitacoraEvento_96_PP().RegistrarEvento(evento);
        }

        private List<Rol_96_PP> ValidarElementosIniciales(Familia_96_PP familia, IEnumerable<Rol_96_PP> elementos)
        {
            List<Rol_96_PP> elementosIniciales = elementos == null
                ? new List<Rol_96_PP>()
                : elementos.Where(elemento => elemento != null).ToList();

            if (elementosIniciales.Count == 0)
                throw new Exception("No se puede crear una familia vacía. Debe seleccionar al menos una patente o familia.");

            HashSet<string> codigos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (Rol_96_PP elemento in elementosIniciales)
            {
                if (string.IsNullOrWhiteSpace(elemento.Codigo))
                    throw new Exception("El elemento inicial no es válido.");

                string clave = elemento.esfamilia + ":" + elemento.Codigo;
                if (!codigos.Add(clave))
                    throw new Exception("No se puede asignar el mismo elemento más de una vez.");

                if (elemento.esfamilia)
                {
                    if (familia.Codigo == elemento.Codigo)
                        throw new Exception("Una familia no puede contenerse a sí misma.");

                    if (!dalrol.FamiliaEstaActiva(elemento.Codigo))
                        throw new Exception("La familia seleccionada no existe o se encuentra inactiva.");
                }
            }

            return elementosIniciales;
        }

        public void AsignarPermisoAFamilia(Familia_96_PP f, Rol_96_PP p)
        {
            if (f == null || string.IsNullOrWhiteSpace(f.Codigo))
                throw new Exception("Debe seleccionar la familia de destino.");

            if (p == null || string.IsNullOrWhiteSpace(p.Codigo))
                throw new Exception("Debe seleccionar un permiso o una familia.");

            if (!dalrol.FamiliaEstaActiva(f.Codigo))
                throw new Exception("La familia de destino no existe o se encuentra inactiva.");

            Familia_96_PP familiaActual = dalrol.ObtenerFamilia(f.Codigo);

            if (familiaActual.Contiene(p.Codigo))
                throw new Exception("La familia ya contiene ese elemento.");

            if (p.esfamilia)
            {
                if (!dalrol.FamiliaEstaActiva(p.Codigo))
                    throw new Exception("La familia seleccionada no existe o se encuentra inactiva.");

                if (f.Codigo == p.Codigo)
                    throw new Exception("Una familia no puede contenerse a sí misma.");

                if (dalrol.FamiliaContiene(p.Codigo, f.Codigo))
                    throw new Exception("La asignación genera un ciclo entre familias.");
            }

            dalrol.AsignarPermisoAFamilia(p, f);
            var dv = new BLL_DigitoVerificador_96_PP();
            dv.CalcularDV("FamiliaPatente", DAL_96_PP.ConsultarTabla("FamiliaPatente"));
            dv.CalcularDV("FamiliaFamilia", DAL_96_PP.ConsultarTabla("FamiliaFamilia"));

            var user = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            Evento_96_PP evento = new Evento_96_PP(
                user.Dni,
                DateTime.Now,
                "Familias",
                "Asignar permisos a familia",
                Evento_96_PP.Criticidad.Medio
            );
            new BLL_BitacoraEvento_96_PP().RegistrarEvento(evento);
        }

        public void EliminarPermisodeFamilia(Rol_96_PP p, string cod)
        {
            if (p == null || string.IsNullOrWhiteSpace(p.Codigo) || string.IsNullOrWhiteSpace(cod))
                throw new Exception("Debe seleccionar una asignación válida.");

            dalrol.EliminarPermisodeFamilia(p, cod);
            var dv = new BLL_DigitoVerificador_96_PP();
            dv.CalcularDV("FamiliaPatente", DAL_96_PP.ConsultarTabla("FamiliaPatente"));
            dv.CalcularDV("FamiliaFamilia", DAL_96_PP.ConsultarTabla("FamiliaFamilia"));
        }

        public Familia_96_PP ObtenerFamilia(string codigo)
        {
            return dalrol.ObtenerFamilia(codigo);
        }

        public List<Familia_96_PP> ObtenerFamilias()
        {
            return dalrol.ObtenerFamilias();
        }
    }
}
