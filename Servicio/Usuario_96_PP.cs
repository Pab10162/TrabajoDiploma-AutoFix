using System;
using Servicio;

namespace BE_96_PP
{
    [Serializable]
    public class Usuario_96_PP
    {
        public string Apellido { get; set; }
        public string Contraseña { get; set; }
        public string Dni { get; set; }
        public string Email { get; set; }
        public Rol_96_PP Rol { get; set; }
        public string Nombre { get; set; }
        public string NombreUsuario { get; set; }
        public Idioma_96_PP Idioma { get; set; }
        public bool Bloqueado { get; set; }
        public bool Activo { get; set; }

        public Usuario_96_PP()
        {
        }

        public Usuario_96_PP(string apellido, string contraseña, string dni, string email, string nombre, string nombreUsuario, Idioma_96_PP idioma, bool bloqueado, bool activo, Rol_96_PP rol)
        {
            Apellido = apellido;
            Contraseña = contraseña;
            Dni = dni;
            Email = email;
            Nombre = nombre;
            NombreUsuario = nombreUsuario;
            Idioma = idioma;
            Bloqueado = bloqueado;
            Activo = activo;
            Rol = rol;
        }
    }
}
