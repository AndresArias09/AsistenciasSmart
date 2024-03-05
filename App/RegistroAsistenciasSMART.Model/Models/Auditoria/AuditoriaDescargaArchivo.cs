using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.Models.Auditoria
{
    /// <summary>
    /// Representa el modelo mediante el cual se describe la información de un evento de descarga de un archivo
    /// </summary>
    public class AuditoriaDescargaArchivo
    {
        /// <summary>
        /// Ruta absoluta y original del archivo que se va a descargar
        /// </summary>
        public string ruta_original { get; set; } = "";
        /// <summary>
        /// Ruta del cliente que será finalmente descargada por el cliente
        /// </summary>
        public string ruta_descargada { get; set; } = "";
        /// <summary>
        /// Nombre del archivo descargado
        /// </summary>
        public string nombre_archivo { get; set; } = "";
        /// <summary>
        /// Extensión del archivo descargado
        /// </summary>
        public string extension_archivo { get; set; } = "";
        /// <summary>
        /// Peso del archivo descargado
        /// </summary>
        public string peso_archivo { get; set; } = "";
        /// <summary>
        /// Usuario que realiza la descarga del archivo
        /// </summary>
        public string usuario { get; set; } = "";
        /// <summary>
        /// Dirección IP desde la cual se descarga el archivo
        /// </summary>
        public string ip_address { get; set; } = "";
    }
}
