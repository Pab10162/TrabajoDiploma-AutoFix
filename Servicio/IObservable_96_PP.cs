using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio
{
    public interface IObservable_96_PP
    {
        void Suscribir(IdiomaObserver_96_PP obs);
        void Desuscribir(IdiomaObserver_96_PP obs);
        void Notificar();
    }
}
