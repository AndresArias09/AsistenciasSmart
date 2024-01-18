using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.Models.Colaboradores
{
    public class RegistroAsistencia
    {
        public string fecha { get; set; } = "";
        public string hora { get; set; } = "";
        public string cedula { get; set; } = "";
        public string sede { get; set; } = "";
        public string reporta { get; set; } = "";
        public string correo { get; set; } = "";
        public string latitud { get; set; } = "";
        public string longitud { get; set; } = "";
        public string ip_address { get; set; } = "";
        public string email { get; set; } = "";
        public DateTime? fecha_adicion { get; set;}
    }
}
