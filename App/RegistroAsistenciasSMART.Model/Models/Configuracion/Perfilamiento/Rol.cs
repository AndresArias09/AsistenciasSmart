using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.Models.Configuracion.Perfilamiento
{
    /// <summary>
    /// Modulo mediante el cual se representa la información de un rol
    /// </summary>
    public class Rol
    {
        /// <summary>
        /// Identificador del rol
        /// </summary>
        public long id_rol { get; set; }
        /// <summary>
        /// Nombre del rol
        /// </summary>
        public string nombre_rol { get; set; } = string.Empty;
        /// <summary>
        /// Fecha de creación del rol
        /// </summary>
        public DateTime fecha_adicion { get; set; }
        /// <summary>
        /// Lista de <see cref="Modulo"/> a los que el rol tiene acceso
        /// </summary>
        public List<Modulo> modulos { get; set; } = new List<Modulo>();
    }
}
