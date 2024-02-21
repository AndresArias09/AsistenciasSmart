using RegistroAsistenciasSMART.Model.DTO;
using RegistroAsistenciasSMART.Model.Models;
using RegistroAsistenciasSMART.Model.Models.Colaboradores;
using RegistroAsistenciasSMART.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Services.Interfaces.Colaboradores
{
    public interface IColaboradorService
    {
        public Task<ResponseDTO> insertarInfoColaborador(Colaborador colaborador);
        public Task<ResponseDTO> actualizarInfoColaborador(Colaborador colaborador);
        public Task<Colaborador> consultarColaboradorByCedula(long cedula);
        public Task<IEnumerable<Colaborador>> consultarColaboradores();
        public Task<IEnumerable<Colaborador>> consultarJefesInmediatos();
        public ResponseDTO validarColaborador(Colaborador colaborador);
        public Task<bool> eliminarColaborador(long cedula);
        public Task<ResponseDTO> cargueMasivoColaboradores(Archivo archivo_cargue, IProgress<CargueMasivoDTO> progress, string usuario_accion);
        public Task<ResponseDTO> insertarRegistroAsistencia(RegistroAsistencia registro);
        public Task<IEnumerable<RegistroAsistenciaDTO>> consultarRegistrosAsistencia(FiltroAsistencia filtros);
        public Task<Archivo> generarReporteRegistroAsistencias(FiltroAsistencia filtros);
    }
}
