using RegistroAsistenciasSMART.Data.Connection;
using RegistroAsistenciasSMART.Data.Repositories.Interfaces.Configuracion.Perfilamiento;
using RegistroAsistenciasSMART.Data.Repositories.Repositories.Configuracion.Perfilamiento;
using RegistroAsistenciasSMART.Model.Models.Configuracion.Perfilamiento;
using RegistroAsistenciasSMART.Model.Response;
using RegistroAsistenciasSMART.Services.Interfaces.Auditoria;
using RegistroAsistenciasSMART.Services.Interfaces.Configuracion.Perfilamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Services.Services.Configuracion.Perfilamiento
{
    /// <summary>
    /// Implementación de la interfaz <see cref="IRolService"/>
    /// </summary>
    public class RolService : IRolService
    {
        private readonly IRolRepository _rolRepository;
        private readonly IAuditoriaService _auditoriaService;
        private readonly IModuloRepository _moduloRepository;
        private readonly SqlConfiguration _sqlConfiguration;

        public RolService
        (
            SqlConfiguration sqlConfiguration,
            IAuditoriaService auditoriaService
        )
        {
            _sqlConfiguration = sqlConfiguration;
            _rolRepository = new RolRepository(_sqlConfiguration.ConnectionString);
            _moduloRepository = new ModuloRepository(_sqlConfiguration.ConnectionString);
            _auditoriaService = auditoriaService;
        }

        public async Task<Rol> getRol(long id_rol)
        {
            Rol rol = await _rolRepository.getRol(id_rol);

            if (rol is null) return rol;

            IEnumerable<Modulo> modulos = await _moduloRepository.getModulosRol(rol.id_rol);
            rol.modulos = modulos.ToList();

            return rol;
        }

        public async Task<IEnumerable<Rol>> getRoles()
        {
            IEnumerable<Rol> roles = await _rolRepository.getRoles();

            foreach (var rol in roles)
            {
                IEnumerable<Modulo> modulos = await _moduloRepository.getModulosRol(rol.id_rol);
                rol.modulos = modulos.ToList();
            }

            return roles;
        }

        public IEnumerable<Rol> getRolesSync()
        {
            IEnumerable<Rol> roles = _rolRepository.getRolesSync();

            foreach (var rol in roles)
            {
                IEnumerable<Modulo> modulos = _moduloRepository.getModulosRolSync(rol.id_rol);
                rol.modulos = modulos.ToList();
            }

            return roles;
        }

        public async Task<ResponseDTO> insertarRol(Rol rol, string usuario_accion, string ipAddress)
        {
            ResponseDTO respuesta_validacion = validarInformacionRol(rol);

            if (!respuesta_validacion.estado.Equals("OK")) return respuesta_validacion;

            Rol temp_rol = await _rolRepository.getRol(rol.id_rol);

            if (temp_rol is not null)
            {
                await _moduloRepository.limpiarAsignacionRolModulo(rol.id_rol);

                if (!await _rolRepository.actualizarRol(rol))
                {
                    return new ResponseDTO() { estado = "ERROR", descripcion = "No es posible actualizar la información del rol" };
                }
            }
            else
            {
                long id_rol = await _rolRepository.insertarRol(rol, usuario_accion);

                if (id_rol <= 0)
                {
                    return new ResponseDTO() { estado = "ERROR", descripcion = "No es posible guardar la información del rol" };
                }

                rol.id_rol = id_rol;
            }


            foreach (var modulo in rol.modulos)
            {
                await _moduloRepository.insertarAsignacionRolModulo(rol.id_rol, modulo.id_modulo);
            }

            return new ResponseDTO() { estado = "OK" };

        }

        public ResponseDTO validarInformacionRol(Rol rol)
        {
            if (string.IsNullOrEmpty(rol.nombre_rol))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debes indicar un nombre para el rol" };
            }

            return new ResponseDTO() { estado = "OK" };
        }
    }
}
