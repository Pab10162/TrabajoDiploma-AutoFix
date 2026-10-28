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
using System.Data;

namespace BLL
{
    public class BLL_Rol_96_PP
    {
        DAL_Rol_96_PP dalrol = new DAL_Rol_96_PP();

        public void CrearRol(Rol_96_PP p)
        {
            CrearRol(p, Enumerable.Empty<Rol_96_PP>());
        }

        public void CrearRol(Rol_96_PP p, IEnumerable<Rol_96_PP> elementos)
        {
            if (p == null || string.IsNullOrWhiteSpace(p.Codigo) || string.IsNullOrWhiteSpace(p.Nombre))
                throw new Exception("Debe indicar el código y el nombre del rol.");

            List<Rol_96_PP> elementosIniciales = ValidarElementosIniciales(elementos);

            if (dalrol.VerificarExistenciaRolCodigo(p))
            {
                throw new Exception("Ya existe rol con ese codigo");
            }

            if (dalrol.VerificarExistenciaRolNombre(p))
            {
                throw new Exception("Ya existe rol con ese nombre");
            }

            dalrol.CrearRolConElementos(p, elementosIniciales);

            var user = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            Evento_96_PP evento = new Evento_96_PP(
                user.Dni,
                DateTime.Now,
                "Roles",
                "Crear rol",
                Evento_96_PP.Criticidad.Alto
            );
            new BLL_BitacoraEvento_96_PP().RegistrarEvento(evento);

            BLL_DigitoVerificador_96_PP dv = new BLL_DigitoVerificador_96_PP();
            dv.CalcularDV("Roles", DAL_Comun_96_PP.ConsultarTabla("Roles"));
            dv.CalcularDV("RolPatente", DAL_Comun_96_PP.ConsultarTabla("RolPatente"));
            dv.CalcularDV("RolFamilia", DAL_Comun_96_PP.ConsultarTabla("RolFamilia"));
        }

        private List<Rol_96_PP> ValidarElementosIniciales(IEnumerable<Rol_96_PP> elementos)
        {
            List<Rol_96_PP> elementosIniciales = elementos == null
                ? new List<Rol_96_PP>()
                : elementos.Where(elemento => elemento != null).ToList();

            if (elementosIniciales.Count == 0)
                throw new Exception("No se puede crear un rol vacío. Debe seleccionar al menos una patente o familia.");

            HashSet<string> codigos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (Rol_96_PP elemento in elementosIniciales)
            {
                if (string.IsNullOrWhiteSpace(elemento.Codigo))
                    throw new Exception("El elemento inicial no es válido.");

                string clave = elemento.esfamilia + ":" + elemento.Codigo;
                if (!codigos.Add(clave))
                    throw new Exception("No se puede asignar el mismo elemento más de una vez.");

                if (elemento.esfamilia && !dalrol.FamiliaEstaActiva(elemento.Codigo))
                    throw new Exception("La familia seleccionada no existe o se encuentra inactiva.");
            }

            return elementosIniciales;
        }

        public string ObtenerNombredeRol(string cod)
        {
            return dalrol.ObtenerNombredeRol(cod);
        }

        public Rol_96_PP ObtenerRol(string cod)
        {
            return dalrol.ObtenerRol(cod);
        }

        public Rol_96_PP ObtenerPermiso(string nombre)
        {
            return dalrol.ObtenerPermiso(nombre);
        }

        public void EliminarPermisodeRol(Rol_96_PP p, string codrol)
        {
            if (p == null || string.IsNullOrWhiteSpace(p.Codigo) || string.IsNullOrWhiteSpace(codrol))
                throw new Exception("Debe seleccionar una asignación válida.");

            dalrol.EliminarPermisodeRol(p, codrol);

            var dv = new BLL_DigitoVerificador_96_PP();
            dv.CalcularDV("RolPatente", DAL_Comun_96_PP.ConsultarTabla("RolPatente"));
            dv.CalcularDV("RolFamilia", DAL_Comun_96_PP.ConsultarTabla("RolFamilia"));
        }

        public string ObtenerCodigoRol(Rol_96_PP p)
        {
            return dalrol.ObtenerCodigoRol(p);
        }

        public void EliminarRol(Rol_96_PP rol)
        {
            if (dalrol.RolEstaActivo(rol))
            {
                if (dalrol.VerificarExistenciaRolCodigo(rol))
                {
                    if (!dalrol.VerificarAsignacion(rol))
                    {
                        rol.activo = false;
                        dalrol.EliminarRol(rol);
                        new BLL_DigitoVerificador_96_PP().CalcularDV("Roles", DAL_Comun_96_PP.ConsultarTabla("Roles"));

                        var user = SessionManager_96_PP.getInstancia().getUsuarioActivo();
                        Evento_96_PP evento = new Evento_96_PP(
                            user.Dni,
                            DateTime.Now,
                            "Roles",
                            "Eliminar Rol",
                            Evento_96_PP.Criticidad.Alto
                        );
                        new BLL_BitacoraEvento_96_PP().RegistrarEvento(evento);
                    }
                    else
                    {
                        throw new Exception("No puede borrarlo pq un usuario tiene este rol");
                    }
                }
                else
                {
                    throw new Exception("error");
                }
            }
            else
            {
                throw new Exception("error, el rol no esta activo");

            }
        }

        public void AsignarPermisosARol(Rol_96_PP p, Rol_96_PP permiso)
        {
            if (p == null || string.IsNullOrWhiteSpace(p.Codigo))
                throw new Exception("Debe seleccionar el rol de destino.");

            if (permiso == null || string.IsNullOrWhiteSpace(permiso.Codigo))
                throw new Exception("Debe seleccionar un permiso o una familia.");

            Rol_96_PP rolActual = dalrol.ObtenerRol(p.Codigo);
            if (rolActual.Contiene(permiso.Codigo))
                throw new Exception("El rol ya contiene ese elemento.");

            if (permiso.esfamilia && !dalrol.FamiliaEstaActiva(permiso.Codigo))
                throw new Exception("La familia seleccionada no existe o se encuentra inactiva.");

            dalrol.AsignarPermisosARol(p, permiso);

            var user = SessionManager_96_PP.getInstancia().getUsuarioActivo();
            Evento_96_PP evento = new Evento_96_PP(
                user.Dni,
                DateTime.Now,
                "Roles",
                "Asignar permisos a rol",
                Evento_96_PP.Criticidad.Medio
            );
            new BLL_BitacoraEvento_96_PP().RegistrarEvento(evento);
            var dv = new BLL_DigitoVerificador_96_PP();
            dv.CalcularDV("RolPatente", DAL_Comun_96_PP.ConsultarTabla("RolPatente"));
            dv.CalcularDV("RolFamilia", DAL_Comun_96_PP.ConsultarTabla("RolFamilia"));
        }

        public List<Rol_96_PP> ObtenerTodosLosRoles()
        {
            return dalrol.ObtenerTodosLosRoles();
        }
    }
}
