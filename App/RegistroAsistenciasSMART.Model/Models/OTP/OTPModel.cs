using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.Models.OTP
{
    /// <summary>
    /// Modelo utilizado para describir un código OTP
    /// </summary>
    public class OTPModel
    {
        /// <summary>
        /// Identificador del código OTP
        /// </summary>
        public string id_otp_code { get; set; } = string.Empty;
        /// <summary>
        /// Contenido del código OTP (normalmente de 6 dígitos) 
        /// </summary>
        public string otp_code { get; set; } = string.Empty;
        /// <summary>
        /// Estado actual del código OTP
        /// </summary>
        public string estado { get; set; } = string.Empty;
        /// <summary>
        /// Identificador del proceso relacionado al código OTP
        /// </summary>
        public string numero_documento_proceso { get; set; } = string.Empty;
        /// <summary>
        /// Tipo del proceso relacionado al código OTP
        /// </summary>
        public string tipo_proceso { get; set; } = string.Empty;
        /// <summary>
        /// Descripción del código OTP
        /// </summary>
        public string descripcion { get; set; } = string.Empty;
        /// <summary>
        /// Métodos por los cuales se ha envíado o se enviará el código OTP
        /// </summary>
        public string metodos_envio { get; set; } = string.Empty;
        /// <summary>
        /// Fecha de generación del código OTP
        /// </summary>
        public DateTime fecha_adicion { get; set; }
    }
}
