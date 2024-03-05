using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.Models.Configuracion.Perfilamiento
{
    /// <summary>
    /// Representa el modelo utilizado para describir la información de un usuario
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Identificador del usuario
        /// </summary>
        public string id_usuario { get; set; } = "";
        /// <summary>
        /// Nombres del usuario
        /// </summary>
        public string nombres { get; set; } = "";
        /// <summary>
        /// Apellidos del usuario
        /// </summary>
        public string apellidos { get; set; } = "";
        /// <summary>
        /// Nombre del usuario del usuario
        /// </summary>
        public string usuario { get; set; } = "";
        /// <summary>
        /// Dirección e-mail del usuario
        /// </summary>
        public string email { get; set; } = "";
        /// <summary>
        /// Estado actual del usuario
        /// </summary>
        public string estado { get; set; } = "";
        /// <summary>
        /// Identificador del rol que tiene asignado el usuario
        /// </summary>
        public long id_rol { get; set; }
        /// <summary>
        /// Información del rol al cual está asociado el usuario
        /// </summary>
        public Rol rol { get; set; } = new Rol();
        /// <summary>
        /// Área a la que pertenece el usuario
        /// </summary>
        public string area { get; set; } = "";
        /// <summary>
        /// Usuario que registró al usuario actual
        /// </summary>
        public string usuario_adiciono { get; set; } = "";
        /// <summary>
        /// Fecha en la que fue creado el usuario
        /// </summary>
        public DateTime fecha_adicion { get; set; }
        /// <summary>
        /// Cargo que desempeña el usuario
        /// </summary>
        public string cargo { get; set; } = "";
    }
}
