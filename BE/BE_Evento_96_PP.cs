using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_96_PP
{
    public class BE_Evento_96_PP
    {

        public enum Criticidad
        {
            Bajo,
            Medio,
            Alto,
            MuyAlto
        }



        public int numero { get; set; }

        public string dni{ get; set; }

        public DateTime fecha{ get; set; }

        public string modulo{ get; set; }

        public string descripcion{ get; set; }

        public Criticidad criticidad{ get; set; }

        public BE_Evento_96_PP(int numero_96_PP, string dni_96_PP, DateTime fecha_96_PP, string modulo_96_PP, string descripcion_96_PP, Criticidad criticidad_96_PP)
        {
            this.numero = numero_96_PP;
            this.dni= dni_96_PP;
            this.fecha= fecha_96_PP;
            this.modulo= modulo_96_PP;
            this.descripcion= descripcion_96_PP;
            this.criticidad= criticidad_96_PP;
        }

        public BE_Evento_96_PP(string dni_96_PP, DateTime fecha_96_PP, string modulo_96_PP, string descripcion_96_PP, Criticidad criticidad_96_PP)
        {

            this.dni= dni_96_PP;
            this.fecha= fecha_96_PP;
            this.modulo= modulo_96_PP;
            this.descripcion= descripcion_96_PP;
            this.criticidad= criticidad_96_PP;
        }

    }
}
