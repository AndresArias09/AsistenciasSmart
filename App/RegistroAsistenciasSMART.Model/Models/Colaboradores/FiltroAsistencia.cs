using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.Models.Colaboradores
{
    public class FiltroAsistencia
    {
        public DateTime? fecha_desde { get; set; }
        public DateTime? fecha_hasta { get; set; }
        public string nombres { get; set; } = "";
        public string cedula { get; set; } = "";
        public string sede { get; set; } = "";
        public string area { get; set; } = "";
        public string reporta { get; set; } = "";
        public string jefe_inmediato { get; set; } = "";
        public string cargo { get; set; } = "";
    }
}
