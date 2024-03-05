namespace RegistroAsistenciasSMART.Web.Authentication
{
    /// <summary>
    /// Representa el modelo mediante el cual se describe una sesión de usuario dentro del sistema
    /// </summary>
    public class UserSession
    {
        /// <summary>
        /// Nombre de usuario del usuario
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// Rol del usuario
        /// </summary>
        public string Role { get; set; } = string.Empty;
        /// <summary>
        /// Identificador del usuario
        /// </summary>
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Indica si el usuario marcó o no que desea que su usuario sea recordado por el sistema para futuros ingresos
        /// </summary>
        public bool RememberMe { get; set; } = false;
        /// <summary>
        /// Dirección de ip con la cual ingresa el usuario
        /// </summary>
        public string Ip { get; set; } = string.Empty;
        /// <summary>
        /// Fecha del último login exitoso para el usuario
        /// </summary>
        public DateTime? last_login { get; set; }
        /// <summary>
        /// Hash del último password guardado en sesión
        /// </summary>
        public string hash_password { get; set; } = string.Empty;
        /// <summary>
        /// Identificador del registro de auditoría asociado al último login del usuario
        /// </summary>
        public string id_auditoria_login { get; set; } = string.Empty;
        /// <summary>
        /// Determina si el usuario tiene activado o no el doble factor de autenticación
        /// </summary>
        public bool enabled_2fa { get; set; } = false;
    }
}
