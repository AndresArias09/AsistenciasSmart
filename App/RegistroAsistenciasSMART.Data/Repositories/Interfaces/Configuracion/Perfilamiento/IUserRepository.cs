using RegistroAsistenciasSMART.Model.Models.Configuracion.Perfilamiento;
using RegistroAsistenciasSMART.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Data.Repositories.Interfaces.Configuracion.Perfilamiento
{
    /// <summary>
    /// Representa la intefaz mediante la cual se expondrán las operaciones de base de datos para la entidad <see cref="Usuario"/>
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Registra la auditoría de un intento de login por un usuario en concreto
        /// </summary>
        /// <param name="usuario">Identificador del usuario que realiza el intento de login</param>
        /// <param name="descripcion">Descripción del evento de intento de login</param>
        /// <param name="ipAddress">Dirección IP desde la cual se realiza el intento de login</param>
        /// <returns>Identificador del registro de auditoría recién registrado</returns>
        public Task<string> registrarAuditoriaLogin(string usuario, string descripcion, string ipAddress);
        /// <summary>
        /// Registra la auditoría de un evento de cierre de sesión de un usuario en concreto
        /// </summary>
        /// <param name="id_auditoria">Identificador del registro de auditoría relacionado al inicio de sesión del usuario</param>
        /// <param name="usuario">Identificador del usuario</param>
        /// <param name="ip">Dirección IP desde la cual se realiza el cierre de sesión</param>
        /// <param name="motivo">Describe la motivo por el cual se realiza el cierre de sesión</param>
        /// <returns><see langword="true" /> si el registro fue exitoso, <see langword="false" /> en caso de que no</returns>
        public Task<bool> registrarAuditoriaCierreSesion(string id_auditoria, string usuario, string ip, string motivo);
        /// <summary>
        /// Almacena la información de un nuevo usuario
        /// </summary>
        /// <param name="usuario">Objeto <see cref="Usuario"/> que contiene toda la información del usuario</param>
        /// <returns>Identificador del nuevo usuario</returns>
        public Task<long> insertarUsuario(Usuario usuario);
        /// <summary>
        /// Actualiza la información de un usuario existente
        /// </summary>
        /// <param name="usuario">Objeto <see cref="Usuario"/> que contiene toda la información del usuario</param>
        /// <returns><see langword="true" /> si el registro fue exitoso, <see langword="false" /> en caso de que no</returns>
        public Task<bool> actualizarUsuario(Usuario usuario);
        /// <summary>
        /// Obtiene la información de un usuario dado su nombre de usuario
        /// </summary>
        /// <param name="usuario">Nombre del usuario del usuario</param>
        /// <returns>Objeto <see cref="Usuario"/></returns>
        public Task<Usuario> getUsuarioByUser(string usuario);
        /// <summary>
        /// Obtiene la información de un usuario dado su identificador
        /// </summary>
        /// <param name="id_usuario">Identificador del usuario</param>
        /// <returns>Objeto <see cref="Usuario"/></returns>
        public Task<Usuario> getUsuarioById(string id_usuario);
        /// <summary>
        /// Obtiene todos los usuarios registrados en el sistema
        /// </summary>
        /// <returns>Lista de <see cref="Usuario"/></returns>
        public Task<IEnumerable<Usuario>> getUsuarios();

    }
}
