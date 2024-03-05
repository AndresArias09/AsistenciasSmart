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
    /// Interfaz mediante la cual se expondrán las operaciones de lógica de negocio
    /// para el modelo <see cref="Rol"/>
    /// </summary>
    public interface IRolService
    {
        /// <summary>
        /// Obtiene todos los roles actuales
        /// </summary>
        /// <returns>Lista de <see cref="Rol"/></returns>
        public Task<IEnumerable<Rol>> getRoles();
        /// <summary>
        /// Obtiene todos los roles actuales de manera síncrona
        /// </summary>
        /// <returns>Lista de <see cref="Rol"/></returns>
        public IEnumerable<Rol> getRolesSync();
        /// <summary>
        /// Obtiene la información de un rol en específico
        /// </summary>
        /// <param name="id_rol">Identificador del rol</param>
        /// <returns>Objeto <see cref="Rol"/></returns>
        public Task<Rol> getRol(long id_rol);
        /// <summary>
        /// Almacena la información de un rol
        /// </summary>
        /// <param name="rol">Información del rol</param>
        /// <param name="usuario_accion">Usuario que realiza la acción</param>
        /// <param name="ipAddress">Dirección IP desde la que se realiza la acción</param>
		/// <returns>Objeto <see cref="ResponseDTO"/> el cual contiene un código de respuesta y una descripción. En caso de que el proceso sea exitoso, el código de respuesta será <c>OK</c>, de lo contrario será <c>ERROR</c> y traerá la respectiva descripción del error</returns>
        public Task<ResponseDTO> insertarRol(Rol rol, string usuario_accion, string ipAddress);
        /// <summary>
        /// Valida la información principal de un rol
        /// </summary>
        /// <param name="rol"></param>
        /// <returns>Objeto <see cref="ResponseDTO"/> el cual contiene un código de respuesta y una descripción. En caso de que el proceso sea exitoso, el código de respuesta será <c>OK</c>, de lo contrario será <c>ERROR</c> y traerá la respectiva descripción del error</returns>
        public ResponseDTO validarInformacionRol(Rol rol);
    }
}
