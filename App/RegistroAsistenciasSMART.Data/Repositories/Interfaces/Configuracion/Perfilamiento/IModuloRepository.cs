using RegistroAsistenciasSMART.Model.Models.Configuracion.Perfilamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Data.Repositories.Interfaces.Configuracion.Perfilamiento
{
    /// <summary>
    /// Interfaz mediante la cual se exponen las operaciones de base de datos para el modelo <see cref="Modulo"/>
    /// </summary>
    public interface IModuloRepository
    {
        /// <summary>
        /// Obtiene la lista completa de módulos existentes
        /// </summary>
        /// <returns>Lista de <see cref="Modulo"/></returns>
        public Task<IEnumerable<Modulo>> getModulos();
        /// <summary>
        /// Obtiene la lista completa de módulos existentes de manera síncrona
        /// </summary>
        /// <returns>Lista de <see cref="Modulo"/></returns>
        public IEnumerable<Modulo> getModulosSync();
        /// <summary>
        /// Obtiene la lista completa de módulos a los que un rol específico tiene acceso
        /// </summary>
        /// <param name="id_rol">Identificador del rol</param>
        /// <returns>Lista de <see cref="Modulo"/></returns>
        public Task<IEnumerable<Modulo>> getModulosRol(long id_rol);
        /// <summary>
        /// Obtiene la lista completa de módulos a los que un rol específico tiene acceso de manera síncrona
        /// </summary>
        /// <param name="id_rol">Identificador del rol</param>
        /// <returns>Lista de <see cref="Modulo"/></returns>
        public IEnumerable<Modulo> getModulosRolSync(long id_rol);
        /// <summary>
        /// Elimina todos los módulos asignados a un rol en específico
        /// </summary>
        /// <param name="id_rol">Identificador del rol</param>
        /// <returns><see langword="true"/> si la operación es exitosa, <see langword="false"/> en caso de que no</returns>
        public Task<bool> limpiarAsignacionRolModulo(long id_rol);
        /// <summary>
        /// Asigna al rol específico acceso a un módulo determinado
        /// </summary>
        /// <param name="id_rol">Identificador del rol</param>
        /// <param name="id_modulo">Identificador del módulo</param>
        /// <returns><see langword="true"/> si la operación es exitosa, <see langword="false"/> en caso de que no</returns>
        public Task<bool> insertarAsignacionRolModulo(long id_rol, long id_modulo);
    }
}
