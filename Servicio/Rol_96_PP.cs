using System;
using System.Collections.Generic;

namespace Servicio
{
    public class Rol_96_PP
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public bool esfamilia { get; set; }
        public bool activo { get; set; }

        public virtual List<Rol_96_PP> hijos { get; set; } = new List<Rol_96_PP>();

        public virtual void Agregar(Rol_96_PP elemento)
        {
            if (Codigo == elemento.Codigo && esfamilia)
                throw new Exception("No se puede agregar una familia o rol a sí mismo.");

            if (Contiene(elemento.Codigo))
                throw new Exception("Este rol o familia ya contiene ese permiso o familia.");

            if (elemento.Contiene(Codigo))
                throw new Exception("La asignación genera una referencia circular.");

            hijos.Add(elemento);
        }

        public bool Contiene(string codigo)
        {
            if (hijos == null)
                return false;

            foreach (Rol_96_PP hijo in hijos)
            {
                if (hijo.Codigo == codigo)
                    return true;

                if (hijo.esfamilia && hijo.Contiene(codigo))
                    return true;
            }

            return false;
        }

        public bool TienePermiso(string codigo)
        {
            if (string.IsNullOrEmpty(codigo))
                return false;

            return Codigo == codigo || Contiene(codigo);
        }

        public bool TieneAlgunPermiso(params string[] codigos)
        {
            if (codigos == null)
                return false;

            foreach (string codigo in codigos)
            {
                if (TienePermiso(codigo))
                    return true;
            }

            return false;
        }

        public virtual void Eliminar(Rol_96_PP elemento)
        {
            hijos.Remove(elemento);
        }
    }
}
