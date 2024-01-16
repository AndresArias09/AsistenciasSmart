using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaDigitalSMART.Data.Repositories.Interfaces.Colaboradores
{
    public class Colaborador
    {
        public string cedula { get; set; } = "";
        public string nombres { get; set; } = "";
        public string cargo { get; set; } = "";
        public string area { get; set; } = "";
        public string jefe_inmediato { get; set; } = "";
        public string sede { get; set; } = "";
        public string correo { get; set; } = "";
        public string turno { get; set; } = "";
        public string estado { get; set; } = "";
        public string usuario_adiciono { get; set; } = "";
        public DateTime? fecha_adicion { get; set; }
    }
}
