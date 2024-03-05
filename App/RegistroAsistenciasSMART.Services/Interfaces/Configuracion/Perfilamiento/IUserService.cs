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
    /// <summary>
    /// Representa la interfaz mediante la cual se exponen las operaciones de lógica de negocio para la gestión de usuarios
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Procesa una solicitud de login realizada por un usuario
        /// </summary>
        /// <param name="usuario">Objeto DTO que representa las credenciales del usuario</param>
        /// <param name="ipAddress">Dirección IP desde la cual se realiza la solicitud de login</param>
		/// <returns>Objeto <see cref="ResponseDTO"/> el cual contiene un código de respuesta y una descripción. En caso de que el proceso sea exitoso, el código de respuesta será <c>OK</c>, de lo contrario será <c>ERROR</c> y traerá la respectiva descripción del error</returns>
        public Task<ResponseDTO> loginUsuario(UserDTO usuario, string ipAddress);
        /// <summary>
        /// Registra un cierre de sesión realizado por un usuario
        /// </summary>
        /// <param name="id_auditoria">Identificador del registro de auditoría relacionado a la sesión actual del usuario</param>
        /// <param name="usuario">Identificador del usuario que realiza el cierre de sesión</param>
        /// <param name="ip">Dirección IP desde la cual se realiza el cierre de sesión</param>
        /// <param name="motivo">Motivo por el cual se genera el cierre de sesión</param>
        /// <returns><see langword="true" /> en caso de que el registro haya sido exitoso, <see langword="false" /> en caso de que no</returns>
        public Task<bool> registrarAuditoriaCierreSesion(string id_auditoria, string usuario, string ip, string motivo);
        /// <summary>
        /// Obtiene la información de un usuario dado su nombre de usuario
        /// </summary>
        /// <param name="usuario">Nombre de usuario de usuario</param>
        /// <returns>Objeto <see cref="Usuario"/></returns>
        public Task<Usuario> getUsuarioByUser(string usuario);
        /// <summary>
        /// Realiza el envío de un código OTP a un usuario para un intento de login
        /// </summary>
        /// <param name="nombre_usuario">Nombre de usuario del usuario</param>
		/// <returns>Objeto <see cref="ResponseDTO"/> el cual contiene un código de respuesta y una descripción. En caso de que el proceso sea exitoso, el código de respuesta será <c>OK</c>, de lo contrario será <c>ERROR</c> y traerá la respectiva descripción del error</returns>
        public Task<ResponseDTO> enviarCodigoOTPLogin(string nombre_usuario, string ipAccion);
        /// <summary>
        /// Valida el código OTP ingresado por el usuario con él último generado para un intento de login
        /// </summary>
        /// <param name="otp_code">Código OTP ingresado por el usuario</param>
        /// <param name="nombre_usuario">Nombre de usuario del usuario</param>
        /// <param name="ipAccion">Dirección IP desde la cual se verifica el código OTP</param>
        /// <returns>Objeto <see cref="ResponseDTO"/> el cual contiene un código de respuesta y una descripción. En caso de que el proceso sea exitoso, el código de respuesta será <c>OK</c>, de lo contrario será <c>ERROR</c> y traerá la respectiva descripción del error</returns>
        public Task<ResponseDTO> validarCodigoOTP(OTP_DTO otp_code, string nombre_usuario, string ipAccion);
        /// <summary>
        /// Registra el ingreso de un usuario al sistema
        /// </summary>
        /// <param name="usuario">Nombre de usuario del usuairo</param>
        /// <param name="ip_address">Dirección IP desde la cual se genera el ingreso</param>
        /// <param name="descripcion">Razón por la cual se procude el ingreso</param>
        public Task procesarIngreso(string usuario, string ip_address, string descripcion);
        /// <summary>
        /// Obtiene todos los usuarios registrados en el sistema
        /// </summary>
        /// <returns>Lista de <see cref="Usuario"/></returns>
        public Task<IEnumerable<Usuario>> getUsuarios();
        /// <summary>
        /// Registra un nuevo usuario en el sistema 
        /// </summary>
        /// <param name="usuario">Información del nuevo usuario</param>
        /// <param name="ipAddress">Dirección IP desde la cual se registra al usuario</param>
        /// <param name="usuarioAccion">Usuario que registra al nuevo usuario</param>
        /// <returns>Objeto <see cref="ResponseDTO"/> el cual contiene un código de respuesta y una descripción. En caso de que el proceso sea exitoso, el código de respuesta será <c>OK</c>, de lo contrario será <c>ERROR</c> y traerá la respectiva descripción del error</returns>
        public Task<ResponseDTO> insertarUsuario(Usuario usuario, string ipAddress, string usuarioAccion);
        /// <summary>
        /// Valida la información principal de un usuario
        /// </summary>
        /// <param name="usuario">Información del usuario</param>
        /// <returns>Objeto <see cref="ResponseDTO"/> el cual contiene un código de respuesta y una descripción. En caso de que el proceso sea exitoso, el código de respuesta será <c>OK</c>, de lo contrario será <c>ERROR</c> y traerá la respectiva descripción del error</returns>
        public ResponseDTO validarInfoUsuario(Usuario usuario);
        /// <summary>
        /// Consulta las direcciones IP desde las cuales es permitido conectarse al sistema
        /// </summary>
        /// <returns>Lista de <see cref="IpInfo"/></returns>
        public IEnumerable<IpInfo> consultarIpsAutorizados();

    }
}
