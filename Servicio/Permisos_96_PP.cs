namespace Servicio
{
    public static class Permisos_96_PP
    {
        public const string ConsultarBitacora = "P02";
        public const string CambiarContrasena = "P05";
        public const string ExportarPdfBitacora = "P06";
        public const string CambiarIdioma = "P09";

        public const string ConsultarUsuarios = "P10";
        public const string CrearUsuario = "P11";
        public const string ModificarUsuario = "P12";
        public const string ActivarDesactivarUsuario = "P13";
        public const string DesbloquearUsuario = "P14";

        public const string ConsultarRoles = "P20";
        public const string CrearRol = "P21";
        public const string EliminarRol = "P22";
        public const string AsignarPatenteARol = "P23";
        public const string AsignarFamiliaARol = "P24";
        public const string QuitarPermisoDeRol = "P25";

        public const string ConsultarFamilias = "P30";
        public const string CrearFamilia = "P31";
        public const string ModificarFamilia = "P32";
        public const string EliminarFamilia = "P33";
        public const string AsignarPatenteAFamilia = "P34";
        public const string AsignarFamiliaAFamilia = "P35";
        public const string QuitarPermisoDeFamilia = "P36";

        public const string RealizarBackup = "P50";
        public const string RestaurarBackup = "P51";
        public const string RecalcularDigitosVerificadores = "P52";

        // RFN1 - Asignar turno de reparación
        public const string GestionarClientes = "P60";
        public const string GestionarVehiculos = "P61";
        public const string AsignarTurno = "P62";
        public const string ConsultarTurnos = "P63";

        public const string ReporteTurnos = "P64";

        // RFN2 - Registrar diagnóstico del vehículo
        public const string RegistrarAsistencia = "P65";
        public const string RegistrarRevision = "P66";
        public const string RegistrarDiagnostico = "P67";
        public const string RegistrarReparacion = "P68";
        public const string RegistrarCobro = "P69";

        public const string ReporteDiagnosticos = "P70";

        public static readonly string[] Maestros =
        {
            GestionarClientes,
            GestionarVehiculos
        };

        public static readonly string[] Turnos =
        {
            AsignarTurno,
            ConsultarTurnos
        };

        public static readonly string[] Diagnostico =
        {
            RegistrarAsistencia,
            RegistrarRevision,
            RegistrarDiagnostico,
            RegistrarReparacion,
            RegistrarCobro
        };

        public static readonly string[] Usuarios =
        {
            ConsultarUsuarios,
            CrearUsuario,
            ModificarUsuario,
            ActivarDesactivarUsuario,
            DesbloquearUsuario
        };

        public static readonly string[] Roles =
        {
            ConsultarRoles,
            CrearRol,
            EliminarRol,
            AsignarPatenteARol,
            AsignarFamiliaARol,
            QuitarPermisoDeRol
        };

        public static readonly string[] Familias =
        {
            ConsultarFamilias,
            CrearFamilia,
            ModificarFamilia,
            EliminarFamilia,
            AsignarPatenteAFamilia,
            AsignarFamiliaAFamilia,
            QuitarPermisoDeFamilia
        };

        public static readonly string[] Auditoria =
        {
            ConsultarBitacora,
            ExportarPdfBitacora
        };

        public static readonly string[] Administracion =
        {
            ConsultarUsuarios,
            CrearUsuario,
            ModificarUsuario,
            ActivarDesactivarUsuario,
            DesbloquearUsuario,
            ConsultarRoles,
            CrearRol,
            EliminarRol,
            AsignarPatenteARol,
            AsignarFamiliaARol,
            QuitarPermisoDeRol,
            ConsultarFamilias,
            CrearFamilia,
            ModificarFamilia,
            EliminarFamilia,
            AsignarPatenteAFamilia,
            AsignarFamiliaAFamilia,
            QuitarPermisoDeFamilia,
            ConsultarBitacora,
            ExportarPdfBitacora,
            RealizarBackup,
            RestaurarBackup,
            RecalcularDigitosVerificadores
        };
    }
}
