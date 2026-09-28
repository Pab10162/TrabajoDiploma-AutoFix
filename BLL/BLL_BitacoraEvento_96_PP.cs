using BE_96_PP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_96_PP;
using BLL;
using System.Data;
namespace ClassLibrary3
{
    public class BLL_BitacoraEvento_96_PP
    {

        DAL_BitacoraEventos_96_PP dal = new DAL_BitacoraEventos_96_PP();

        public void RegistrarEvento(Evento_96_PP e)
        {
            dal.RegistrarEvento(e);

            DataTable dt = DAL_96_PP.ConsultarTabla("Evento_56PS");
            new BLL_DigitoVerificador_96_PP().CalcularDV("Evento_56PS", dt);

            return;
        }

        public BLL_BitacoraEvento_96_PP() { dal = new DAL_BitacoraEventos_96_PP(); }

        public List<Evento_96_PP> obtenerEventos()
        {
            return dal.obtenerEventos();
        }







    }
}
