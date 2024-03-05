using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.Models.Colaboradores
{
    /// <summary>
    /// Representa la información de un registro de asistencia
    /// </summary>
    public class RegistroAsistencia
    {
        /// <summary>
        /// Identificador del registro de asistencia
        /// </summary>
        public long id_registro_asistencia { get; set; }
        /// <summary>
        /// Fecha en la que se registra el registro de asistencia
        /// </summary>
        public DateTime fecha_adicion { get; set; }
        /// <summary>
        /// Cédula del colaborador
        /// </summary>
        public long? cedula_colaborador { get; set; }
        /// <summary>
        /// Sede donde se registra la asistencia
        /// </summary>
        public string sede { get; set; } = "";
        /// <summary>
        /// Tipo de reporte del registro de asistencia
        /// </summary>
        public string tipo_reporte { get; set; } = "";
        /// <summary>
        /// Latitud desde la cual se registra la asistencia
        /// </summary>
        public string latitud { get; set; } = "";
        /// <summary>
        /// Latitud desde la cual se registra la asistencia
        /// </summary>
        public string longitud { get; set; } = "";
        /// <summary>
        /// Dirección IP desde la cual se registra la asistencia
        /// </summary>
        public string ip_address { get; set; } = "";

    }
}
