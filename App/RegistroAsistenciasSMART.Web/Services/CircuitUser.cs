using RegistroAsistenciasSMART.Web.Authentication;

namespace RegistroAsistenciasSMART.Web.Services
{
    /// <summary>
    /// Modelo que describe una asigación de la sesión de usuario con el identificador de la conexión/circuito actual
    /// </summary>
    public class CircuitUser
    {
        /// <summary>
        /// Sesión del usuario actual
        /// </summary>
        public UserSession usuario { get; set; }
        /// <summary>
        /// Identificador del circuito actual
        /// </summary>
        public string CircuitId { get; set; }
    }
}
