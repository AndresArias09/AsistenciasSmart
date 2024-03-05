using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.Models.Configuracion.Perfilamiento
{
    /// <summary>
    /// Modelo mediante el cual se representa la información de un módulo
    /// </summary>
    public class Modulo
    {
        /// <summary>
        /// Identificador del módulo
        /// </summary>
        public long id_modulo { get; set; }
        /// <summary>
        /// Nombre del módulo
        /// </summary>
        public string nombre_modulo { get; set; } = "";
        /// <summary>
        /// Nivel del módulo
        /// </summary>
        public string nivel { get; set; } = "";
        /// <summary>
        /// Fecha en la que se creó el módulo
        /// </summary>
        public DateTime fecha_adicion { get; set; }
    }
}
