using BE_96_PP;
using ClassLibrary2;
using Services_96_PP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_96_PP;
using System.Data;
namespace BLL
{
    public class BLL_Usuario_96_PP
    {
        private DAL_Usuario_96_PP dal = new DAL_Usuario_96_PP();




        public void cambiarContraseña(string dni, string nuevaContraseña) //si
        {
            dal.cambiarContraseña(dni, nuevaContraseña);
            DataTable dt = DAL_Comun_96_PP.ConsultarTabla("Usuario_56PS");
            new BLL_DigitoVerificador_96_PP().CalcularDV("Usuario_56PS", dt);
        }

      

        public string crearContraseña(string nombre, string dni) //si
        {
            return CryptoManager_96_PP.Encriptar(nombre + dni);
        }

        public void crearUsuario(Usuario_96_PP usuario) //si
        {
            dal.crearUsuario(usuario);
            DataTable dt = DAL_Comun_96_PP.ConsultarTabla("Usuario_56PS");
            new BLL_DigitoVerificador_96_PP().CalcularDV("Usuario_56PS", dt);
        }

        public void desbloquearUsuario(string dni) //si
        {
            dal.desbloquearUsuario(dni);


            DataTable dt = DAL_Comun_96_PP.ConsultarTabla("Usuario_56PS");
            new BLL_DigitoVerificador_96_PP().CalcularDV("Usuario_56PS", dt);
        }

        public bool iniciarSesion(string nombreUsuario, string contraseña) //si
        {
            string contraseñaEncriptada = CryptoManager_96_PP.Encriptar(contraseña);
            return dal.validarUsuario(nombreUsuario, contraseñaEncriptada);
        }

        public void modificarUsuario(Usuario_96_PP usuario) //si
        {
            dal.modificarUsuario(usuario);
            DataTable dt = DAL_Comun_96_PP.ConsultarTabla("Usuario_56PS");
            new BLL_DigitoVerificador_96_PP().CalcularDV("Usuario_56PS", dt);
        }

        public List<Usuario_96_PP> obtenerUsuarios() //si
        {
            return dal.obtenerUsuarios();
        }

        public bool validarUsuario(string nombreUsuario, string contraseña) //si
        {
            string contraseñaEncriptada = CryptoManager_96_PP.Encriptar(contraseña);
            return dal.validarUsuario(nombreUsuario, contraseñaEncriptada);
        }

        public string verificarEstado(string nombreUsuario) //si
        {
            return dal.verificarEstado(nombreUsuario);
        }


        public Usuario_96_PP obtenerUsuarioPorDni(string dni)
        {
            return dal.obtenerUsuarioPorDni(dni);
        }

  

        public bool existeUsuarioConEseUsername(string userName)
        {
            return dal.existeUsuarioConEseUsername(userName);
        }

        public void bloquearUsuario(string dni)
        {
            dal.bloquearUsuario(dni);
            DataTable dt = DAL_Comun_96_PP.ConsultarTabla("Usuario_56PS");
            new BLL_DigitoVerificador_96_PP().CalcularDV("Usuario_56PS", dt);
        }

        public void cambiarEstadoActivo(string dni)
        {
            dal.cambiarEstadoActivo(dni);
            DataTable dt = DAL_Comun_96_PP.ConsultarTabla("Usuario_56PS");
            new BLL_DigitoVerificador_96_PP().CalcularDV("Usuario_56PS", dt);
        }

        public void cambiarIdioma(string idiomaActual, string dni)
        {
            dal.cambiarIdioma(idiomaActual, dni);
            DataTable dt = DAL_Comun_96_PP.ConsultarTabla("Usuario_56PS");
            new BLL_DigitoVerificador_96_PP().CalcularDV("Usuario_56PS", dt);
        }
    }
}
