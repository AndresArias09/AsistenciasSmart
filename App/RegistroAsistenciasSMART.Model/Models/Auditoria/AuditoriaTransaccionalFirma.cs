using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.Models.Auditoria
{
    public class AuditoriaTransaccionalFirma
    {
        public string accion { get; set; } = "";
        public string descripcion { get; set; } = "";
        public string pantalla { get; set; } = "";
        public string ip_accion { get; set; } = "";
        public string usuario_accion { get; set; } = "";
        public string id_firma { get; set; } = "";

        public DateTime fecha_adicion { get; set; }
    }

    public class AuditoriaTransaccionalComunicacion
    {
        public string accion { get; set; } = "";
        public string descripcion { get; set; } = "";
        public string pantalla { get; set; } = "";
        public string ip_accion { get; set; } = "";
        public string usuario_accion { get; set; } = "";
        public string id_comunicacion { get; set; } = "";

        public DateTime fecha_adicion { get; set; }
    }
}
