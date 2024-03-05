using RegistroAsistenciasSMART.Model.Models.Auditoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Services.Interfaces.Auditoria
{
    /// <summary>
    /// Representa la interfaz mediante la cual se expondrán las operaciones de lógica de negocio relacionadas a auditorías
    /// </summary>
    public interface IAuditoriaService
    {
        /// <summary>
        /// Registra la auditoría de un intento de envío de un e-mail. 
        /// </summary>
        /// <param name="emailInfo">Objeto <see cref="EmailInfo"/> que contiene toda la información de un envío e-mail</param>
        /// <returns><see langword="true" /> si el registro fue exitoso, <see langword="false" /> en caso de que no</returns>
        public Task<bool> registrarAuditoriaEnvioEmail(EmailInfo emailInfo);
        /// <summary>
        /// Registra la auditoría de un evento de navegación a través del sitio web por parte de un usuario
        /// </summary>
        /// <param name="auditoriaNavegacion">Objeto <see cref="AuditoriaNavegacion"/> el cual contiene todos los detalles de un evento de navegación de un usuario a través del sitio web</param>
        /// <returns><see langword="true" /> si la inserción fue correcta, <see langword="false" /> en caso de que no</returns>
        public Task<bool> registrarAuditoriaNavegacion(AuditoriaNavegacion auditoriaNavegacion);
        /// <summary>
        /// Registra la auditoría de un evento de descarga de un archivo desde el sitio web
        /// </summary>
        /// <param name="auditoriaDescargaArchivo">Objeto <see cref="AuditoriaDescargaArchivo"/> que contiene todos los detalles de la descarga del archivo</param>
        /// <returns><see langword="true" /> si la inserción fue correcta, <see langword="false" /> en caso de que no</returns>
        public Task<bool> registrarAuditoriaDescargaArchivos(AuditoriaDescargaArchivo auditoriaDescargaArchivo);
    }
}
