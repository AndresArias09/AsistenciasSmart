using RegistroAsistenciasSMART.Data.Connection;
using RegistroAsistenciasSMART.Data.Repositories.Interfaces.Configuracion.Perfilamiento;
using RegistroAsistenciasSMART.Model.Models.Configuracion.Perfilamiento;
using RegistroAsistenciasSMART.Services.Interfaces.Configuracion.Perfilamiento;
using RegistroAsistenciasSMART.Data.Repositories.Repositories.Configuracion.Perfilamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Services.Services.Configuracion.Perfilamiento
{
    /// <summary>
    /// Implementación de la interfaz <see cref="IModuloService"/>
    /// </summary>
    public class ModuloService : IModuloService
    {
        private readonly IModuloRepository _moduloRepository;

        public ModuloService(SqlConfiguration sqlConfiguration)
        {
            _moduloRepository = new ModuloRepository(sqlConfiguration.ConnectionString);
        }

        public async Task<IEnumerable<Modulo>> getModulos()
        {
            return await _moduloRepository.getModulos();
        }

        public async Task<IEnumerable<Modulo>> getModulosRol(long id_rol)
        {
            return await _moduloRepository.getModulosRol(id_rol);
        }

        public IEnumerable<Modulo> getModulosRolSync(long id_rol)
        {
            return _moduloRepository.getModulosRolSync(id_rol);
        }

        public IEnumerable<Modulo> getModulosSync()
        {
            return _moduloRepository.getModulosSync();
        }

    }
}
