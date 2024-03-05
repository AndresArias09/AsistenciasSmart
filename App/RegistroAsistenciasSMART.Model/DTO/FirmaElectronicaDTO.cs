using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.DTO
{
    /// <summary>
    /// Objeto DTO utilizado en los procesos de firma electrónica
    /// </summary>
    public class FirmaElectronicaDTO
    {
        /// <summary>
        /// Ruta del archivo que será firmado
        /// </summary>
        public string ruta_original { get; set; } = string.Empty;
        /// <summary>
        /// Ruta final en la que se guardará el archivo ya firmado (final)
        /// </summary>
        public string ruta_final { get; set; } = string.Empty;
        /// <summary>
        /// Identificador del proceso asociado
        /// </summary>
        public string numero_documento_proceso { get; set; } = string.Empty;
        /// <summary>
        /// Descripción del proceso de firma
        /// </summary>
        public string descripcion { get; set; } = string.Empty;
        /// <summary>
        /// Usuario relacionado al proceso de firma
        /// </summary>
        public string usuario { get; set; } = string.Empty;
        /// <summary>
        /// Dirección IP desde donde se realizará la firma
        /// </summary>
        public string ip_accion { get; set; } = string.Empty;
        /// <summary>
        /// Estado de la firma
        /// </summary>
        public bool estado_firma { get; set; } = false;
    }
}
