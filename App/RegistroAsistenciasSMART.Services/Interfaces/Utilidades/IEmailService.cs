using RegistroAsistenciasSMART.Model.Models.Auditoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Services.Interfaces.Utilidades
{
    public interface IEmailService
    {
        /// <summary>
        /// Realiza el envío de un correo electrónico
        /// </summary>
        /// <param name="emailInfo">Objeto <see cref="EmailInfo"/> el cual contiene toda la información necesaria para realizar el envío de un correo electrónico</param>
        /// <returns><see langword="true" /> si el envío se realizó correctamente, <see langword="false" /> en caso contrario</returns>
        public Task<bool> enviarCorreo(EmailInfo emailInfo);
    }
}
