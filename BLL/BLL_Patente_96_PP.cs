using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_96_PP;
using Servicio;

namespace BLL
{
    public class BLL_Patente_96_PP
    {
        DAL_Rol_96_PP dalrol = new DAL_Rol_96_PP();

        public List<Patente_96_PP> ObtenerPatentes()
        {
            return dalrol.ObtenerPatentes();
        }
    }
}
