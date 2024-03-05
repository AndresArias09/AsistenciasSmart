using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.Models
{
    /// <summary>
    /// Representa el modelo utilizado para describir un archivo
    /// </summary>
    public class Archivo
    {
        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public string nombre { get; set; } = "";
        /// <summary>
        /// Extensión del archivo
        /// </summary>
        public string extension { get; set; } = "";
        /// <summary>
        /// Ruta absoluta del archivo
        /// </summary>
        public string ruta_absoluta { get; set; } = "";
        /// <summary>
        /// Ruta del cliente para el archivo
        /// </summary>
        public string ruta_cliente { get; set; } = "";
    }
}
