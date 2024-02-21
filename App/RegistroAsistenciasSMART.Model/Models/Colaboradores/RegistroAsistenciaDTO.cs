using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.Models.Colaboradores
{
    public class RegistroAsistenciaDTO
    {
        public DateTime? fecha_adicion { get; set; }
        public long cedula { get; set; }
        public string nombres { get; set; } = ""; 
        public string apellidos { get; set; } = "";
        public string sede { get; set; } = "";
        public string area { get; set; } = "";
        public string tipo_reporte { get; set; } = "";
        public string correo { get; set; } = "";
        public string latitud { get; set; } = "";
        public string longitud { get; set; } = "";
        public string ip_address { get; set; } = "";
        public string jefe_inmediato { get; set; } = "";
        public string cargo { get; set; } = "";
    }
}
