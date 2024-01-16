using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.Models.Configuracion.Perfilamiento
{
    public class Usuario
    {
        public string id_usuario { get; set; } = "";
        public string nombres { get; set; } = "";
        public string apellidos { get; set; } = "";
        public string usuario { get; set; } = "";
        public string email { get; set; } = "";
        public string estado { get; set; } = "";
    }
}
