using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.Models.Colaboradores
{
    /// <summary>
    /// Representa un conjunto de filtros utilizados para consultar registros de asistencia
    /// </summary>
    public class FiltroAsistencia
    {
        /// <summary>
        /// Fecha desde la cual se consultarán los registros de asistencia
        /// </summary>
        public DateTime? fecha_desde { get; set; }
        /// <summary>
        /// Fecha hasta la cual se consultarán los registros de asistencia
        /// </summary>
        public DateTime? fecha_hasta { get; set; }
        /// <summary>
        /// Nombres del colaborador
        /// </summary>
        public string nombres { get; set; } = "";
        /// <summary>
        /// Apellidos del colaborador
        /// </summary>
        public string apellidos { get; set; } = "";
        /// <summary>
        /// Cédula del colaborador
        /// </summary>
        public long? cedula { get; set; }
        /// <summary>
        /// Sede desde la cual se registra la asistencia
        /// </summary>
        public string sede { get; set; } = "";
        /// <summary>
        /// Área del colaborador
        /// </summary>
        public string area { get; set; } = "";
        /// <summary>
        /// Tipo de reporte que se reportó en el registro de asistencia
        /// </summary>
        public string tipo_reporte { get; set; } = "";
        /// <summary>
        /// Jefe inmediato del colaborador
        /// </summary>
        public long? jefe_inmediato { get; set; }
        /// <summary>
        /// Cargo del colaborador
        /// </summary>
        public string cargo { get; set; } = "";
        /// <summary>
        /// Correo del colaborador
        /// </summary>
        public string correo { get; set; } = "";
    }
}
