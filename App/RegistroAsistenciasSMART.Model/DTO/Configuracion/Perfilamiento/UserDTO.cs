using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.DTO.Configuracion.Perfilamiento
{
    /// <summary>
    /// Objeto DTO utilizado en los procesos de autenticación de usuarios
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// Nombre de usuario del usuario
        /// </summary>
        public string usuario { get; set; } = "";
        /// <summary>
        /// Contraseña digitada por el usuario
        /// </summary>
        public string pass { get; set; } = "";
    }
}
