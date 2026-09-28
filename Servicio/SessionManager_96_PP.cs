using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_96_PP;
using Servicio;

namespace ClassLibrary2
{
    public class SessionManager_96_PP : IObservable_96_PP
    {
        private static SessionManager_96_PP instancia;
        private Usuario_96_PP usuarioActivo;

        public Idioma_96_PP idiomaActual { get; private set; }
        public event EventHandler CierreSistemaSolicitado;
        private List<IdiomaObserver_96_PP> Observadores;


        private SessionManager_96_PP()
        {
            Observadores = new List<IdiomaObserver_96_PP>();
            idiomaActual = new Idioma_96_PP("ES");
        }


        public static SessionManager_96_PP getInstancia()
        {
            if (instancia == null)
                instancia = new SessionManager_96_PP();

            return instancia;
        }

        public void CambiarIdioma(Idioma_96_PP idiomaNuevo)
        {
            if (idiomaNuevo == null || string.IsNullOrWhiteSpace(idiomaNuevo.tipo))
                throw new ArgumentException("Debe indicar un idioma válido.", nameof(idiomaNuevo));

            idiomaActual = idiomaNuevo;
            Notificar();
        }

        public void Suscribir(IdiomaObserver_96_PP obs)
        {
            if (!Observadores.Contains(obs))
                Observadores.Add(obs);
        }

        public void Desuscribir(IdiomaObserver_96_PP obs)
        {
            Observadores.Remove(obs);
        }

        public void Notificar()
        {
            foreach (var obs in Observadores)
                obs.actualizarIdioma();
        }


        public void iniciarSesion(Usuario_96_PP usuario)
        {
            usuarioActivo = usuario;
        }

        public void cerrarSesion()
        {
            usuarioActivo = null;
            idiomaActual = new Idioma_96_PP("ES");
            Notificar();
        }

        public Usuario_96_PP getUsuarioActivo()
        {
            return usuarioActivo;
        }

        public bool haySesionActiva()
        {
            return usuarioActivo != null;
        }

        public void SolicitarCierreSistema()
        {
            CierreSistemaSolicitado?.Invoke(this, EventArgs.Empty);
        }

 
    }
}
