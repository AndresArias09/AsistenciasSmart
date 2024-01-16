using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaDigitalSMART.Model.Models
{
    public class Archivo
    {
        public string nombre { get; set; } = "";
        public string extension { get; set; } = "";
        public string ruta_absoluta { get; set; } = "";
        public string ruta_cliente { get; set; } = "";
    }
}
