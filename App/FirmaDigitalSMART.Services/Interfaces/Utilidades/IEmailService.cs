using FirmaDigitalSMART.Model.Models.Auditoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaDigitalSMART.Services.Interfaces.Utilidades
{
    public interface IEmailService
    {
        public Task<bool> enviarCorreo(EmailInfo emailInfo);
    }
}
