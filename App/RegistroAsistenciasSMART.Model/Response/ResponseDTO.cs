using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.Response
{
    /// <summary>
    /// Objeto DTO mediante el cual se transmite una respuesta a una solicitud
    /// </summary>
    public class ResponseDTO
    {
        /// <summary>
        /// Indica el resultado final de la solicitud
        /// </summary>
        public string estado { get; set; } = string.Empty;
        /// <summary>
        /// Indica una descripción precisa, si aplica, de lo que ha ocurrido durante la solicitud
        /// </summary>
        public string descripcion { get; set; } = string.Empty;
    }
}
