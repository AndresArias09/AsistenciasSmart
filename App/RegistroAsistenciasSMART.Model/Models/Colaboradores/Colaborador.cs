using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.Models.Colaboradores
{
    public class Colaborador
    {
        public long? cedula { get; set; }
        public string nombres { get; set; } = "";
        public string apellidos { get; set; } = "";
        public string cargo { get; set; } = "";
        public string area { get; set; } = "";
        public long? jefe_inmediato { get; set; }
        public string sede { get; set; } = "";
        public string correo { get; set; } = "";
        public string turno { get; set; } = "";
        public long? estado { get; set; }
        public string usuario_adiciono { get; set; } = "";
        public DateTime fecha_adicion { get; set; }
    }
}
