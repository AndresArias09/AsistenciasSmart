using RegistroAsistenciasSMART.Data.Connection;
using RegistroAsistenciasSMART.Data.Repositories.Repositories.Auditoria;
using RegistroAsistenciasSMART.Data.Repositories.Interfaces.Auditoria;
using RegistroAsistenciasSMART.Services.Interfaces.Auditoria;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RegistroAsistenciasSMART.Model.Models.Auditoria;

namespace RegistroAsistenciasSMART.Services.Services.Auditoria
{
    /// <summary>
    /// Implementación de la interfaz <see cref="IAuditoriaService"/>
    /// </summary>
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
    }
}
