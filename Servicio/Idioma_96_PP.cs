using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio
{
    public class Idioma_96_PP
    {
        public string nombre {  get; set; }

        public string tipo { get; set; }

        public Idioma_96_PP()
        {
        }

        public Idioma_96_PP(string tipo)
        {
            this.tipo = string.IsNullOrWhiteSpace(tipo) ? "ES" : tipo.Trim().ToUpperInvariant();
            nombre = ObtenerNombre(this.tipo);
        }

        private string ObtenerNombre(string tipoIdioma)
        {
            switch (tipoIdioma)
            {
                case "EN":
                    return "English";
                case "POR":
                    return "Portugues";
                default:
                    return "Espanol";
            }
        }
    }
}
