using AppDemoBlazor.Model.Models.CargueMasivo;
using FirmaDigitalSMART.Data.Repositories.Interfaces.Colaboradores;
using FirmaDigitalSMART.Model.Models;
using FirmaDigitalSMART.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaDigitalSMART.Services.Interfaces.Colaboradores
{
    public interface IColaboradorService
    {
        public Task<ResponseDTO> insertarInfoColaborador(Colaborador colaborador);
        public Task<ResponseDTO> actualizarInfoColaborador(Colaborador colaborador);
        public Task<Colaborador> consultarColaboradorByCedula(string cedula);
        public Task<IEnumerable<Colaborador>> consultarColaboradores();
        public ResponseDTO validarColaborador(Colaborador colaborador);
        public Task<bool> eliminarColaborador(string cedula);
        public Task<ResponseDTO> cargueMasivoColaboradores(Archivo archivo_cargue, IProgress<CargueMasivoDTO> progress);
    }
}
