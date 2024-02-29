using RegistroAsistenciasSMART.Model.Models.Colaboradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Data.Repositories.Interfaces.Colaboradores
{
    /// <summary>
    /// Interfaz mediante la cual se definen las operaciones de base de datos para la entidad <see cref="Colaborador"/>
    /// </summary>
    public interface IColaboradorRepository
    {
        /// <summary>
        /// Realiza la inserción de un nuevo colaborador
        /// </summary>
        /// <param name="colaborador">Información del colaborador</param>
        /// <returns><see langword="true"/> si la inserción fue exitosa, <see langword="false"/> en caso contrario</returns>
        public Task<bool> insertarColaborador(Colaborador colaborador);
        /// <summary>
        /// Realiza la actualización de un colaborador existente
        /// </summary>
        /// <param name="colaborador">Información del colaborador</param>
        /// <returns><see langword="true"/> si la actualización fue exitosa, <see langword="false"/> en caso contrario</returns>
        public Task<bool> actualizarColaborador(Colaborador colaborador);
        /// <summary>
        /// Realiza la eliminación de un colaborador
        /// </summary>
        /// <param name="cedula">Cédula del colaborador</param>
        /// <returns><see langword="true"/> si la actualización fue exitosa, <see langword="false"/> en caso contrario</returns>
        public Task<bool> eliminarColaborador(long cedula);
        /// <summary>
        /// Obtiene la información de un colaborador dada su cédula
        /// </summary>
        /// <param name="cedula">Cédula del colaborador</param>
        /// <returns>Objeto <see cref="Colaborador"/></returns>
        public Task<Colaborador> consultarColaboradorByCedula(long cedula);
        /// <summary>
        /// Obtiene todos los colaboradores registrados
        /// </summary>
        /// <returns>Lista de <see cref="Colaborador"/></returns>
        public Task<IEnumerable<Colaborador>> consultarColaboradores();
        /// <summary>
        /// Inserta un nuevo registro de asistencia de un colaborador
        /// </summary>
        /// <param name="registro">Información del registro de asistencia</param>
        /// <returns><see langword="true"/> si la inserción fue exitosa, <see langword="false"/> en caso contrario</returns>
        public Task<bool> insertarRegistroAsistencia(RegistroAsistencia registro);
        /// <summary>
        /// Obtiene la información de los registros de asistencia dado un conjunto de filtros
        /// </summary>
        /// <param name="filtros">Filtros de la consulta</param>
        /// <returns>Lista de <see cref="RegistroAsistenciaDTO"/></returns>
        public Task<IEnumerable<RegistroAsistenciaDTO>> consultarRegistrosAsistencia(FiltroAsistencia filtros);
        /// <summary>
        /// Obtiene la lista de colaboradores que son jefes inmediatos
        /// </summary>
        /// <returns>Lista de <see cref="Colaborador"/></returns>
        public Task<IEnumerable<Colaborador>> consultarJefesInmediatos();
    }
}
