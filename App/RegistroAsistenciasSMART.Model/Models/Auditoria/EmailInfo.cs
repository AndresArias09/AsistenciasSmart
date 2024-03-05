using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.Models.Auditoria
{
    /// <summary>
    /// Representa el modelo utilizado para almacenar toda la información de un envío de e-mail
    /// </summary>
    public class EmailInfo
    {
        /// <summary>
        /// Lista de correos a los cuales se les enviará el e-mail como destinatarios principales
        /// </summary>
        public List<string> destinatarios { get; set; } = new List<string>();
        /// <summary>
        /// Lista de correos a los cuales se les enviará el e-mail como copias
        /// </summary>
        public List<string> cc { get; set; } = new List<string>();
        /// <summary>
        /// Lista de correos a los cuales se les enviará el e-mail como copias ocultas
        /// </summary>
        public List<string> bcc { get; set; } = new List<string>();
        /// <summary>
        /// Dirección e-mail que realizará el envío
        /// </summary>
        public string email_emisor { get; set; } = "";
        /// <summary>
        /// Asunto que llevará el email
        /// </summary>
        public string asunto { get; set; } = "";
        /// <summary>
        /// Mensaje (<c>HTML</c>) que llevará el e-mail
        /// </summary>
        public string mensaje { get; set; } = "";
        /// <summary>
        /// Descripción del envío del correo
        /// </summary>
        public string descripcion { get; set; } = "";
        /// <summary>
        /// Descripción de un posible error generado al momento de realizar el envío del e-mail
        /// </summary>
        public string descripcion_error { get; set; } = "";
        /// <summary>
        /// Determina si el correo pudo ser envíado o no
        /// </summary>
        public bool enviado { get; set; } = false;
        /// <summary>
        /// Usuario que dispara el envío del e-mail
        /// </summary>
        public string usuario { get; set; } = "";
        /// <summary>
        /// Pantalla desde la cual se genera el envío del e-mail
        /// </summary>
        public string pantalla { get; set; } = "";
        /// <summary>
        /// Identificador del proceso relacionado
        /// </summary>
        public string numero_identificacion_proceso { get; set; } = "";
        /// <summary>
        /// Lista de anexos <see cref="Anexo"/> que serán añadidos en el envío del e-mail
        /// </summary>
        public List<Anexo> anexos { get; set; } = new List<Anexo>();
    }

    /// <summary>
    /// Representa el modelo utilizado para los anexos dentro de un envío de e-mail
    /// </summary>
    public class Anexo
    {
        /// <summary>
        /// Ruta absoluta del archivo que será añadido como anexo
        /// </summary>
        public string ruta_anexo { get; set; } = "";
        /// <summary>
        /// Nombre con el que el anexo llegará a todos los destinatarios del e-mail
        /// </summary>
        public string nombre_archivo { get; set; } = "";
    }
}
