using FirmaDigitalSMART.Model.Models.OTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaDigitalSMART.Data.Repositories.Interfaces.OTP
{
    public interface IOTPRepository
    {
        public Task<bool> insertarOTP(OTPModel otp_code);

        public Task<OTPModel> getUltimoOTPProceso(string numero_documento, string tipo_proceso);

        public Task<bool> completarEstadoOTP(string id_otp_code, string estado); 

    }
}
