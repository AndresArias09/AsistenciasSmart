using RegistroAsistenciasSMART.Model.Models.OTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Data.Repositories.Interfaces.OTP
{
    /// <summary>
    /// Representa la intefaz mediante la cual se expondrán las operaciones de base de datos relacionadas a OTP
    /// </summary>
    public interface IOTPRepository
    {
        /// <summary>
        /// Registra la información inicial de un código OTP generado
        /// </summary>
        /// <param name="otp_code">Objeto <see cref="OTPModel"/> que contiene la información inicial de un código OTP</param>
        /// <returns><see langword="true" /> si el registro fue exitoso, <see langword="false" /> en caso de que no</returns>
        public Task<bool> insertarOTP(OTPModel otp_code);
        /// <summary>
        /// Obtiene el último código OTP generado para un proceso y un identificador en específico
        /// </summary>
        /// <param name="numero_documento">Identificador del proceso para el cual se generó el código OTP</param>
        /// <param name="tipo_proceso">Tipo de proceso para el cual se generó el código OTP</param>
        /// <returns>Objeto <see cref="OTPModel"/></returns>
        public Task<OTPModel> getUltimoOTPProceso(string numero_documento, string tipo_proceso);
        /// <summary>
        /// Completa un código OTP, actualizando su estado y fecha de verificación
        /// </summary>
        /// <param name="id_otp_code">Identificador del código OTP</param>
        /// <param name="estado">Estado en el que quedará el código OTP</param>
        /// <returns><see langword="true" /> si la actualización fue exitosa, <see langword="false" /> en caso de que no</returns>
        public Task<bool> completarEstadoOTP(string id_otp_code, string estado);

    }
}
