using FirmaDigitalSMART.Model.Models.Auditoria;
using FirmaDigitalSMART.Model.Models.Configuracion.Perfilamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaDigitalSMART.Services.Interfaces.Auditoria
{
    public interface IAuditoriaService
    {
        public Task<bool> registrarAuditoriaEnvioEmail(EmailInfo emailInfo);
        public Task<bool> registrarAuditoriaFirma(AuditoriaTransaccionalFirma auditoria);
        public Task<bool> registrarAuditoriaComunicacion(AuditoriaTransaccionalComunicacion auditoria);
        public Task<bool> registrarAuditoriaNavegacion(AuditoriaNavegacion auditoriaNavegacion);
        public Task<bool> registrarAuditoriaDescargaArchivos(AuditoriaDescargaArchivo auditoriaDescargaArchivo);
		public Task<IEnumerable<AuditoriaTransaccionalFirma>> consultarAuditoriaFirma(long id_firma);
        public Task<IEnumerable<AuditoriaTransaccionalComunicacion>> consultarAuditoriaComunicacion(long id_comunicacion);

    }
}
