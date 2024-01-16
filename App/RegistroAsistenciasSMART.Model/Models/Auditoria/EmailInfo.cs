using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.Models.Auditoria
{
    public class EmailInfo
    {
        public List<string> destinatarios { get; set; } = new List<string>();
        public List<string> cc { get; set; } = new List<string>();
        public List<string> bcc { get; set; } = new List<string>();
        public string email_emisor { get; set; } = "";
        public string asunto { get; set; } = "";
        public string mensaje { get; set; } = "";
        public string descripcion { get; set; } = "";
        public string descripcion_error { get; set; } = "";
        public bool enviado { get; set; } = false;
        public string usuario { get; set; } = "";
        public string pantalla { get; set; } = "";
        public string numero_identificacion_proceso { get; set; } = "";
        public List<string> anexos { get; set; } = new List<string>();
    }
}
