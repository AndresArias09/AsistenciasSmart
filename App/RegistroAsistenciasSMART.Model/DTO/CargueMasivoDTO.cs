using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.DTO
{
    /// <summary>
    /// Describe el estado de un cargue masivo
    /// </summary>
    public class CargueMasivoDTO
    {
        /// <summary>
        /// Identificador del cargue
        /// </summary>
        public long id_cargue { get; set; }
        /// <summary>
        /// Descripción actual del cargue
        /// </summary>
        public string descripcion { get; set; } = string.Empty;
        /// <summary>
        /// Estado actual del cargue
        /// </summary>
        public string estado { get; set; } = string.Empty;
        /// <summary>
        /// Total de registros en el cargue
        /// </summary>
        public int total_registros { get; set; } = 0;
        /// <summary>
        /// Total de registros procesados hasta el momento
        /// </summary>
        public int total_registros_procesados { get; set; } = 0;
        /// <summary>
        /// Total de registros que no han podido ser procesados hasta el momento
        /// </summary>
        public int total_registros_no_procesados { get; set; } = 0;
        /// <summary>
        /// Total de registros faltantes de procesar hasta el momento
        /// </summary>
        public int total_faltantes { get; set; } = 0;
        /// <summary>
        /// Lista de errores presentados en el cargues
        /// </summary>
        public List<DetalleErrorCargueMasivo> errores { get; set; } = new List<DetalleErrorCargueMasivo>();
    }
    /// <summary>
    /// Describe el detalle de un error presentado durante un cargue masivo
    /// </summary>
    public class DetalleErrorCargueMasivo
    {
        /// <summary>
        /// Número de registro que presentó el error
        /// </summary>
        public long numero_registro { get; set; } = 0;
        /// <summary>
        /// Identificador del registro que presentó el error
        /// </summary>
        public string identificador_registro { get; set; } = "";
        /// <summary>
        /// Descripción del error presentado
        /// </summary>
        public string descripcion { get; set; } = "";
    }
}
