using FirmaDigitalSMART.Model.Models.Configuracion.Perfilamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaDigitalSMART.Data.Repositories.Interfaces.Configuracion.Perfilamiento
{
    public interface IUserRepository
    {
        public Task<string> registrarAuditoriaLogin(string usuario, string descripcion, string ipAddress);
        public Task<bool> registrarAuditoriaCierreSesion(string id_auditoria, string usuario, string ip, string motivo);
        public Task<bool> insertarUsuario(Usuario usuario);
        public Task<Usuario> getUsuarioByUser(string usuario);
        public Task<Usuario> getUsuarioById(string id_usuario);
    }
}
