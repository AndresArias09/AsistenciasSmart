using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.Models.Colaboradores
{
    /// <summary>
    /// Representa la información básica de un colaborador
    /// </summary>
    public class Colaborador
    {
        /// <summary>
        /// Cédula del colaborador
        /// </summary>
        public long? cedula { get; set; }
        /// <summary>
        /// Nombres del colaborador
        /// </summary>
        public string nombres { get; set; } = "";
        /// <summary>
        /// Apellidos del colaborador
        /// </summary>
        public string apellidos { get; set; } = "";
        /// <summary>
        /// Cargo del colaborador
        /// </summary>
        public string cargo { get; set; } = "";
        /// <summary>
        /// Área del colaborador
        /// </summary>
        public string area { get; set; } = "";
        /// <summary>
        /// Cédula del jefe inmediato del colaborador
        /// </summary>
        public long? jefe_inmediato { get; set; }
        /// <summary>
        /// Sede principal del colaborador
        /// </summary>
        public string sede { get; set; } = "";
        /// <summary>
        /// Correo del colaborador
        /// </summary>
        public string correo { get; set; } = "";
        /// <summary>
        /// Estado del colaborador
        /// </summary>
        public long? estado { get; set; }
        /// <summary>
        /// Usuario que registró al colaborador
        /// </summary>
        public string usuario_adiciono { get; set; } = "";
        /// <summary>
        /// Fecha de registro del colaborador
        /// </summary>
        public DateTime fecha_adicion { get; set; }
        /// <summary>
        /// Hora de entrada del colaborador de Lunes a Jueves
        /// </summary>
        public TimeSpan? hora_entrada_lj { get; set; }
        /// <summary>
        /// Hora de salida del colaborador de Lunes a Jueves
        /// </summary>
        public TimeSpan? hora_salida_lj { get; set; }

        /// <summary>
        /// Hora de entrada del colaborador los Viernes
        /// </summary>
        public TimeSpan? hora_entrada_v { get; set; }
        /// <summary>
        /// Hora de salida del colaborador los Viernes
        /// </summary>
        public TimeSpan? hora_salida_v { get; set; }
        /// <summary>
        /// Hora de entrada del colaborador los Sábados
        /// </summary>
        public TimeSpan? hora_entrada_s { get; set; }
        /// <summary>
        /// Hora de salida del colaborador los Sábados
        /// </summary>
        public TimeSpan? hora_salida_s { get; set; }
        /// <summary>
        /// Observaciones sobre el colaborador
        /// </summary>
        public string observaciones { get; set; } = "";
    }
}
