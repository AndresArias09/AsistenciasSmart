using RegistroAsistenciasSMART.Model.Models.Configuracion.Perfilamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Services.Interfaces.Configuracion.Perfilamiento
{
    /// <summary>
    /// Interfaz mediante la cual se exponen las operaciones de lógica de negocio del modelo
    /// <see cref="Modulo"/>
    /// </summary>
    public interface IModuloService
    {
        /// <summary>
        /// Obtiene todos los módulos actuales
        /// </summary>
        /// <returns>Lista de <see cref="Modulo"/></returns>
        public Task<IEnumerable<Modulo>> getModulos();
        /// <summary>
        /// Obtiene todos los módulos actuales de manera síncrona
        /// </summary>
        /// <returns>Lista de <see cref="Modulo"/></returns>
        public IEnumerable<Modulo> getModulosSync();
        /// <summary>
        /// Obtiene todos los módulos asignados a un rol
        /// </summary>
        /// <param name="id_rol">Identificador del rol</param>
        /// <returns>Lista de <see cref="Modulo"/></returns>
        public Task<IEnumerable<Modulo>> getModulosRol(long id_rol);
        /// <summary>
        /// Obtiene todos los módulos asignados a un rol de manera síncrona
        /// </summary>
        /// <param name="id_rol"></param>
        /// <returns>Lista de <see cref="Modulo"/></returns>
        public IEnumerable<Modulo> getModulosRolSync(long id_rol);
    }
}
