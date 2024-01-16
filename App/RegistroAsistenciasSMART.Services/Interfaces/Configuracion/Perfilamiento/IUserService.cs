using RegistroAsistenciasSMART.Model.DTO;
using RegistroAsistenciasSMART.Model.DTO.Configuracion.Perfilamiento;
using RegistroAsistenciasSMART.Model.Models.Configuracion.Perfilamiento;
using RegistroAsistenciasSMART.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Services.Interfaces.Configuracion.Perfilamiento
{
    public interface IUserService
    {
        public Task<ResponseDTO> loginUsuario(UserDTO usuario, string ipAddress);
        public Task<bool> registrarAuditoriaCierreSesion(string id_auditoria, string usuario, string ip, string motivo);
        public Task<Usuario> getUsuario(string id_usuario);
        public Task<Usuario> getUsuarioByUser(string usuario);
        public Task<ResponseDTO> enviarCodigoOTPLogin(string nombre_usuario, string ipAccion);
        public Task<ResponseDTO> validarCodigoOTP(OTP_DTO otp_code, string nombre_usuario, string ipAccion);
        public Task procesarIngreso(string usuario, string ip_address, string descripcion);
    }
}
