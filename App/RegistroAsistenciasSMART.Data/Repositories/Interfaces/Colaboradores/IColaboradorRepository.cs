using RegistroAsistenciasSMART.Model.Models.Colaboradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Data.Repositories.Interfaces.Colaboradores
{
    public interface IColaboradorRepository
    {
        public Task<bool> insertarColaborador(Colaborador colaborador);
        public Task<bool> actualizarColaborador(Colaborador colaborador);
        public Task<bool> eliminarColaborador(string cedula);
        public Task<Colaborador> consultarColaboradorByCedula(string cedula);
        public Task<IEnumerable<Colaborador>> consultarColaboradores();
        public Task<bool> insertarRegistroAsistencia(RegistroAsistencia registro);
        public Task<IEnumerable<RegistroAsistencia>> consultarRegistrosAsistencia(FiltroAsistencia filtros);
    }
}
