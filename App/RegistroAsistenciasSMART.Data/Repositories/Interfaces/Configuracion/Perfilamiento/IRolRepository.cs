using RegistroAsistenciasSMART.Model.Models.Configuracion.Perfilamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Data.Repositories.Interfaces.Configuracion.Perfilamiento
{
    /// <summary>
    /// Interfaz mediante la cual se exponen las operacines de base de datos del modelo <see cref="Rol"/>
    /// </summary>
    public interface IRolRepository
    {
        /// <summary>
        /// Almacena la información de un nuevo rol
        /// </summary>
        /// <param name="rol">Información del rol</param>
        /// <param name="usuario">Usuario que realiza la acción</param>
        /// <returns>Identificador del nuevo rol</returns>
        public Task<long> insertarRol(Rol rol, string usuario);
        /// <summary>
        /// Almacena la información de un nuevo rol
        /// </summary>
        /// <param name="rol">Información del rol</param>
        /// <returns><see langword="true"/> si la actualización fue exitosa, <see langword="false"/> en caso de que no</returns>
        public Task<bool> actualizarRol(Rol rol);
        /// <summary>
        /// Obtiene la información de un rol en específico
        /// </summary>
        /// <param name="id_rol">Identificador del rol</param>
        /// <returns>Objeto <see cref="Rol"/></returns>
        public Task<Rol> getRol(long id_rol);
        /// <summary>
        /// Obtiene todos los módulos registrados en el sistema
        /// </summary>
        /// <returns>Lista de <see cref="Rol"/></returns>
        public Task<IEnumerable<Rol>> getRoles();
        /// <summary>
        /// Obtiene todos los módulos registrados en el sistema de manera síncrona
        /// </summary>
        /// <returns>Lista de <see cref="Rol"/></returns>
        public IEnumerable<Rol> getRolesSync();
    }
}
