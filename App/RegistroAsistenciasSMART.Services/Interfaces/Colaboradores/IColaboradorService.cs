using RegistroAsistenciasSMART.Model.DTO;
using RegistroAsistenciasSMART.Model.Models;
using RegistroAsistenciasSMART.Model.Models.Colaboradores;
using RegistroAsistenciasSMART.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Services.Interfaces.Colaboradores
{
    /// <summary>
    /// Interfaz mediante la cual se definen las operaciones de lógica de negocio para la entidad <see cref="Colaborador"/>
    /// </summary>
    public interface IColaboradorService
    {
        /// <summary>
        /// Realiza la inserción de un nuevo colaborador
        /// </summary>
        /// <param name="colaborador"></param>
		/// <returns>Objeto <see cref="ResponseDTO"/> el cual contiene un código de respuesta y una descripción. En caso de que el proceso sea exitoso, el código de respuesta será <c>OK</c>, de lo contrario será <c>ERROR</c> y traerá la respectiva descripción del error</returns>
        public Task<ResponseDTO> insertarInfoColaborador(Colaborador colaborador);
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
        /// Obtiene la lista de colaboradores que son jefes inmediatos
        /// </summary>
        /// <returns>Lista de <see cref="Colaborador"/></returns>
        public Task<IEnumerable<Colaborador>> consultarJefesInmediatos();
        /// <summary>
        /// Valida la información básica de un colaborador
        /// </summary>
        /// <param name="colaborador">Información del colaborador</param>
		/// <returns>Objeto <see cref="ResponseDTO"/> el cual contiene un código de respuesta y una descripción. En caso de que el proceso sea exitoso, el código de respuesta será <c>OK</c>, de lo contrario será <c>ERROR</c> y traerá la respectiva descripción del error</returns>
        public ResponseDTO validarColaborador(Colaborador colaborador);
        /// <summary>
        /// Ejecuta un cargue masivo de colaboradores           
        /// </summary>
        /// <param name="archivo_cargue">Archivo .xlsx que contiene la información de los colaboradores a cargar</param>
        /// <param name="progress">Objeto <see cref="IProgress{CargueMasivoDTO}"/> que permite notificar de registros procesados durante el cargue</param>
        /// <param name="usuario_accion">Usuario que dispara el cargue</param>
		/// <returns>Objeto <see cref="ResponseDTO"/> el cual contiene un código de respuesta y una descripción. En caso de que el proceso sea exitoso, el código de respuesta será <c>OK</c>, de lo contrario será <c>ERROR</c> y traerá la respectiva descripción del error</returns>
        public Task<ResponseDTO> cargueMasivoColaboradores(Archivo archivo_cargue, IProgress<CargueMasivoDTO> progress, string usuario_accion);
        /// <summary>
        /// Inserta un nuevo registro de asistencia de un colaborador
        /// </summary>
        /// <param name="registro">Información del registro de asistencia</param>
		/// <returns>Objeto <see cref="ResponseDTO"/> el cual contiene un código de respuesta y una descripción. En caso de que el proceso sea exitoso, el código de respuesta será <c>OK</c>, de lo contrario será <c>ERROR</c> y traerá la respectiva descripción del error</returns>
        public Task<ResponseDTO> insertarRegistroAsistencia(RegistroAsistencia registro);
        /// <summary>
        /// Obtiene la información de los registros de asistencia dado un conjunto de filtros
        /// </summary>
        /// <param name="filtros">Filtros de la consulta</param>
        /// <returns>Lista de <see cref="RegistroAsistenciaDTO"/></returns>
        public Task<IEnumerable<RegistroAsistenciaDTO>> consultarRegistrosAsistencia(FiltroAsistencia filtros);
        /// <summary>
        /// Genera un reporte de registro de asistencias en formato .xlsx teniendo en cuenta un conjunto de filtros dados 
        /// </summary>
        /// <param name="filtros">Filtros aplicados al reporte</param>
        /// <returns>Archivo .xlsx del reporte resultante</returns>
        public Task<Archivo> generarReporteRegistroAsistencias(FiltroAsistencia filtros);
        /// <summary>
        /// Determina si un registro de asistencia se realiza dentro del horario correcto asignado al colaborador
        /// </summary>
        /// <param name="registro">Información del registro de asistencia</param>
        /// <returns><c>green</c> en caso de que el registro de asistencia esté dentro del rango horario correcto, 
        /// <c>red</c> en caso de que el registro de asistencia esté fuera del rango correcto y <c>gray</c> en caso
        /// de no tener suficiente información</returns>
        public string GetColorAsistencia(RegistroAsistenciaDTO registro);
        /// <summary>
        /// Obtiene la hora de entrada o salida según el día del registro de asistencia
        /// </summary>
        /// <param name="registro">Información del registro de asistencia</param>
        /// <returns>Hora en formato am/pm</returns>
        public string GetHorarioRegistroAsistencia(RegistroAsistenciaDTO registro);
    }
}
