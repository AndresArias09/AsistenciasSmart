using FirmaDigitalSMART.Data.Connection;
using FirmaDigitalSMART.Data.Repositories.Repositories.Auditoria;
using FirmaDigitalSMART.Data.Repositories.Interfaces.Auditoria;
using FirmaDigitalSMART.Services.Interfaces.Auditoria;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirmaDigitalSMART.Model.Models.Configuracion.Perfilamiento;
using FirmaDigitalSMART.Model.Models.Auditoria;

namespace FirmaDigitalSMART.Services.Services.Auditoria
{
    public class AuditoriaService : IAuditoriaService
    {
        private readonly IAuditoriaRepository _auditoriaRepository;

        public AuditoriaService(SqlConfiguration sqlConfiguration)
        {
            _auditoriaRepository = new AuditoriaRepository(sqlConfiguration.ConnectionString);
        }

        public async Task<bool> registrarAuditoriaNavegacion(AuditoriaNavegacion auditoriaNavegacion)
        {
            return await _auditoriaRepository.registrarAuditoriaNavegacion(auditoriaNavegacion);
        }

        public async Task<bool> registrarAuditoriaDescargaArchivos(AuditoriaDescargaArchivo auditoriaDescargaArchivo)
        {
            return await _auditoriaRepository.registrarAuditoriaDescargaArchivo(auditoriaDescargaArchivo);
        }

        public async Task<bool> registrarAuditoriaEnvioEmail(EmailInfo emailInfo)
        {
            return await _auditoriaRepository.registrarAuditoriaEnvioEmail(emailInfo);
        }

        public async Task<bool> registrarAuditoriaFirma(AuditoriaTransaccionalFirma auditoria)
        {
            return await _auditoriaRepository.registrarAuditoriaFirma(auditoria);
        }

		public async Task<IEnumerable<AuditoriaTransaccionalFirma>> consultarAuditoriaFirma(long id_firma)
		{
            return await _auditoriaRepository.consultarAuditoriaFirma(id_firma);
		}

        public async Task<IEnumerable<AuditoriaTransaccionalComunicacion>> consultarAuditoriaComunicacion(long id_comunicacion)
        {
            return await _auditoriaRepository.consultarAuditoriaComunicacion(id_comunicacion);
        }

        public async Task<bool> registrarAuditoriaComunicacion(AuditoriaTransaccionalComunicacion auditoria)
        {
            return await _auditoriaRepository.registrarAuditoriaComunicacion(auditoria);
        }
    }
}
