using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.Models.Colaboradores
{
    /// <summary>
    /// Objeto DTO que representa un registro de asistencia resultante de un reporte de asistencia de colaboradores
    /// </summary>
    public class RegistroAsistenciaDTO
    {
        /// <summary>
        /// Fecha de registro de la asistencia
        /// </summary>
        public DateTime? fecha_adicion { get; set; }
        /// <summary>
        /// Cédula del colaborador
        /// </summary>
        public long cedula { get; set; }
        /// <summary>
        /// Nombres del colaborador
        /// </summary>
        public string nombres { get; set; } = ""; 
        /// <summary>
        /// Apellidos del colaborador
        /// </summary>
        public string apellidos { get; set; } = "";
        /// <summary>
        /// Sede del colaborador
        /// </summary>
        public string sede { get; set; } = "";
        /// <summary>
        /// Área a la que pertenece el colaborador
        /// </summary>
        public string area { get; set; } = "";
        /// <summary>
        /// Tipo de reporte registrado en la asistencia
        /// </summary>
        public string tipo_reporte { get; set; } = "";
        /// <summary>
        /// Correo del colaborador
        /// </summary>
        public string correo { get; set; } = "";
        /// <summary>
        /// Latitud desde la que se registra la asistencia
        /// </summary>
        public string latitud { get; set; } = "";
        /// <summary>
        /// Longitud desde la que se registra la asistencia
        /// </summary>
        public string longitud { get; set; } = "";
        /// <summary>
        /// Dirección IP desde la que se registra la asistencia
        /// </summary>
        public string ip_address { get; set; } = "";
        /// <summary>
        /// Nombre del jefe inmediato del colaborador
        /// </summary>
        public string jefe_inmediato { get; set; } = "";
        /// <summary>
        /// Cargo del colaborador
        /// </summary>
        public string cargo { get; set; } = "";
        /// <summary>
        /// Hora de entrada del colaborador de Lunes a Viernes
        /// </summary>
        public TimeSpan? hora_entrada_lv { get; set; }
        /// <summary>
        /// Hora de salida del colaborador de Lunes a Viernes
        /// </summary>
        public TimeSpan? hora_salida_lv { get; set; }
        /// <summary>
        /// Hora de entrada del colaborador los Sábados
        /// </summary>
        public TimeSpan? hora_entrada_s { get; set; }
        /// <summary>
        /// Hora de salida del colaborador los Sábados
        /// </summary>
        public TimeSpan? hora_salida_s { get; set; }
    }
}
