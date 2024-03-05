using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.Models.Auditoria
{
    /// <summary>
    /// Representa el modelo mediante el cual se describe un evento de navegación de un usuario dentro del sitio web
    /// </summary>
    public class AuditoriaNavegacion
    {
        /// <summary>
        /// Encabezado user-agent envíado desde el navegador hacia el servidor
        /// </summary>
        public string UserAgent { get; set; } = "";
        /// <summary>
        /// Navegador desde donde se dispara el evento
        /// </summary>
        public string Navegador { get; set; } = "";
        /// <summary>
        /// Versión del navegador desde donde se dispara el evento
        /// </summary>
        public string VersionNavegador { get; set; } = "";
        /// <summary>
        /// Plataforma/Sistema operativo del navegador desde donde se dispara el evento
        /// </summary>
        public string PlataformaNavegador { get; set; } = "";
        /// <summary>
        /// URL desde la cual se dispara el evento
        /// </summary>
        public string UrlActual { get; set; } = "";
        /// <summary>
        /// Idioma detectado en el navegador
        /// </summary>
        public string Idioma { get; set; } = "";
        /// <summary>
        /// Determina si las cookies están habilitadas o no
        /// </summary>
        public bool CookiesHabilitadas { get; set; }
        /// <summary>
        /// Indica el ancho de la pantalla detectada en el navegador
        /// </summary>
        public int AnchoPantalla { get; set; }
        /// <summary>
        /// Indica el alto de la pantalla detectada en el navegador
        /// </summary>
        public int AltoPantalla { get; set; }
        /// <summary>
        /// Profundidad del color detectado en el navegador
        /// </summary>
        public int ProfundidadColor { get; set; }
        /// <summary>
        /// Nombre del sistema operativo del cliente
        /// </summary>
        public string NombreSO { get; set; } = "";
        /// <summary>
        /// Versión del sistema operativo del cliente
        /// </summary>
        public string VersionSO { get; set; } = "";
        /// <summary>
        /// Determinar si la ubicación fue permitida o no
        /// </summary>
        public string ubicacion { get; set; } = "";
        /// <summary>
        /// Latitud detectada en el navegador
        /// </summary>
        public string Latitud { get; set; } = "";
        /// <summary>
        /// Longitud detectada en el navegador
        /// </summary>
        public string Longitud { get; set; } = "";
        /// <summary>
        /// Dirección IP desde la cual se detecta el evento
        /// </summary>
        public string ip_address { get; set; } = "";
        /// <summary>
        /// Nombre de usuario del usuario que dispara la acción
        /// </summary>
        public string usuario_accion { get; set; } = "";
        /// <summary>
        /// Rol del usuario que dispara la acción
        /// </summary>
        public string rolUsuario { get; set; } = "";
    }
}
