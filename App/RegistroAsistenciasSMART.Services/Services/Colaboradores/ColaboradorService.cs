using AppDemoBlazor.Model.Models.CargueMasivo;
using RegistroAsistenciasSMART.Data.Connection;
using RegistroAsistenciasSMART.Data.Repositories.Interfaces.Colaboradores;
using RegistroAsistenciasSMART.Data.Repositories.Repositories.Colaboradores;
using RegistroAsistenciasSMART.Model.Models;
using RegistroAsistenciasSMART.Model.Response;
using RegistroAsistenciasSMART.Services.Interfaces.Colaboradores;
using RegistroAsistenciasSMART.Services.Utilidades;
using iTextSharp.text.log;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Services.Services.Colaboradores
{
    public class ColaboradorService : IColaboradorService
    {
        private readonly SqlConfiguration _sqlConfiguration;
        private readonly IColaboradorRepository _colaboradorRepository;

        private ILogger<ColaboradorService> _logger;

        public ColaboradorService(SqlConfiguration sqlConfiguration, ILogger<ColaboradorService> logger)
        {
            _sqlConfiguration = sqlConfiguration;
            _colaboradorRepository = new ColaboradorRepository(_sqlConfiguration.ConnectionString);
            _logger = logger;
        }
        public ResponseDTO validarColaborador(Colaborador colaborador)
        {
            if (string.IsNullOrEmpty(colaborador.cedula))
            {
                return new ResponseDTO() { estado= "ERROR", descripcion = "Debe indicar la cédula del colaborador"};
            }

            if (string.IsNullOrEmpty(colaborador.nombres))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debe indicar los nombres del colaborador" };
            }

            if (string.IsNullOrEmpty(colaborador.cargo))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debe indicar el cargo del colaborador" };
            }

            if (string.IsNullOrEmpty(colaborador.area))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debe indicar el área del colaborador" };
            }

            if (string.IsNullOrEmpty(colaborador.jefe_inmediato))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debe indicar el jefe inmediato del colaborador" };
            }

            if (string.IsNullOrEmpty(colaborador.sede))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debe indicar la sede del colaborador" };
            }

            if (string.IsNullOrEmpty(colaborador.correo))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debe indicar el correo del colaborador" };
            }

            if (string.IsNullOrEmpty(colaborador.turno))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debe indicar el turno del colaborador" };
            }

            if (string.IsNullOrEmpty(colaborador.estado))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debe indicar el estado del colaborador" };
            }

            if (string.IsNullOrEmpty(colaborador.usuario_adiciono))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debe indicar el usuario que adiciona al colaborador" };
            }

            return new ResponseDTO() { estado = "OK"};
        }

        public async Task<ResponseDTO> actualizarInfoColaborador(Colaborador colaborador)
        {
            ResponseDTO respuesta_validacion = validarColaborador(colaborador);

            if (!respuesta_validacion.estado.Equals("OK")) return respuesta_validacion;

            if(await _colaboradorRepository.actualizarColaborador(colaborador))
            {
                return new ResponseDTO() { estado = "OK", descripcion = ""};
            }
            else
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "No fue posible actualizar la información del colaborador" };
            }
        }

        public async Task<Colaborador> consultarColaboradorByCedula(string cedula)
        {
            return await _colaboradorRepository.consultarColaboradorByCedula(cedula);
        }

        public async Task<IEnumerable<Colaborador>> consultarColaboradores()
        {
            return await _colaboradorRepository.consultarColaboradores();
        }

        public async Task<ResponseDTO> insertarInfoColaborador(Colaborador colaborador)
        {
            ResponseDTO respuesta_validacion = validarColaborador(colaborador);

            Colaborador colaborador_temp = await _colaboradorRepository.consultarColaboradorByCedula(colaborador.cedula);

            if(colaborador_temp is not null)
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Ya existe un colaborador con la cédula indicada"};
            }

            if (!respuesta_validacion.estado.Equals("OK")) return respuesta_validacion;

            if (await _colaboradorRepository.insertarColaborador(colaborador))
            {
                return new ResponseDTO() { estado = "OK", descripcion = "" };
            }
            else
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "No fue posible actualizar la información del colaborador" };
            }
        }

        public async Task<ResponseDTO> cargueMasivoColaboradores(Archivo archivo_cargue, IProgress<CargueMasivoDTO> progress)
        {
            string rutaArchivo = "";

            string ruta_base = Directory.GetCurrentDirectory() + "\\ArchivosCargueMasivo\\CargueColaborador";
            if (!Directory.Exists(ruta_base)) Directory.CreateDirectory(ruta_base);

            string nombre_final = "CARGUE_MASIVO_COLABORADORES_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
            rutaArchivo = ruta_base + $"\\{nombre_final}";

            File.Copy(archivo_cargue.ruta_absoluta, rutaArchivo);

            File.Delete(archivo_cargue.ruta_absoluta);

            if (!File.Exists(rutaArchivo))
                return new ResponseDTO()
                {
                    estado = "ERROR",
                    descripcion = "Ocurrió un error al guardar el archivo"
                };

            if (UtilidadesExcel.ARCHIVO_VACIO(rutaArchivo, 0))
                return new ResponseDTO() { estado = "ERROR", descripcion = "El archivo está vacío" };

            string total_registros = UtilidadesExcel.totalRegistros(rutaArchivo, 0);

            ejecutarCargueMasivo(rutaArchivo, progress, total_registros);

            return new ResponseDTO() { estado = "OK", descripcion = "" };
        }

        private async Task ejecutarCargueMasivo(string rutaArchivoCargue, IProgress<CargueMasivoDTO> progress, string total_registros)
        {

            XSSFWorkbook LibroExcel = new XSSFWorkbook(rutaArchivoCargue);

            int contador_cargados = 0, contador_no_cargados = 0;

            string observaciones = "";

            try
            {

                try
                {
                    XSSFSheet HojaExcel = LibroExcel.GetSheetAt(0) as XSSFSheet;
                    if (HojaExcel.LastRowNum >= 1)
                    {

                        for (int i = 1; i <= HojaExcel.LastRowNum; i++)
                        {
                            string cedula = "";
                            string nombres = "";
                            string cargo = "";
                            string area = "";
                            string jefe_inmediato = "";
                            string sede = "";
                            string correo = "";
                            string turno = "";
                            string estado = "";

                            IRow fila = HojaExcel.GetRow(i);

                            try
                            {
                                int contadorCelda = -1;


                                #region Lectura datos excel

                                contadorCelda++; try
                                {
                                    ICell celda = fila.GetCell(contadorCelda);
                                    switch (celda.CellType)
                                    {
                                        case CellType.String:
                                            cedula = fila.GetCell(contadorCelda).StringCellValue;
                                            break;
                                        case CellType.Numeric:
                                            cedula = fila.GetCell(contadorCelda).NumericCellValue.ToString();
                                            break;
                                        case CellType.Blank:
                                            cedula = "";
                                            break;
                                        default:
                                            //set default
                                            break;
                                    }
                                }
                                catch (Exception exe)
                                {
                                    if (!string.IsNullOrEmpty(cedula))
                                    {

                                    }
                                }

                                contadorCelda++; try
                                {
                                    ICell celda = fila.GetCell(contadorCelda);
                                    switch (celda.CellType)
                                    {
                                        case CellType.String:
                                            nombres = fila.GetCell(contadorCelda).StringCellValue;
                                            break;
                                        case CellType.Numeric:
                                            nombres = fila.GetCell(contadorCelda).NumericCellValue.ToString();
                                            break;
                                        case CellType.Blank:
                                            nombres = "";
                                            break;
                                        default:
                                            //set default
                                            break;
                                    }
                                }
                                catch (Exception exe)
                                {
                                    if (!string.IsNullOrEmpty(nombres))
                                    {

                                    }
                                }

                                contadorCelda++; try
                                {
                                    ICell celda = fila.GetCell(contadorCelda);
                                    switch (celda.CellType)
                                    {
                                        case CellType.String:
                                            cargo = fila.GetCell(contadorCelda).StringCellValue;
                                            break;
                                        case CellType.Numeric:
                                            cargo = fila.GetCell(contadorCelda).NumericCellValue.ToString();
                                            break;
                                        case CellType.Blank:
                                            cargo = "";
                                            break;
                                        default:
                                            //set default
                                            break;
                                    }
                                }
                                catch (Exception exe)
                                {
                                    if (!string.IsNullOrEmpty(cargo))
                                    {

                                    }
                                }

                                contadorCelda++; try
                                {
                                    ICell celda = fila.GetCell(contadorCelda);
                                    switch (celda.CellType)
                                    {
                                        case CellType.String:
                                            area = fila.GetCell(contadorCelda).StringCellValue;
                                            break;
                                        case CellType.Numeric:
                                            area = fila.GetCell(contadorCelda).NumericCellValue.ToString();
                                            break;
                                        case CellType.Blank:
                                            area = "";
                                            break;
                                        default:
                                            //set default
                                            break;
                                    }
                                }
                                catch (Exception exe)
                                {
                                    if (!string.IsNullOrEmpty(area))
                                    {

                                    }
                                }

                                contadorCelda++; try
                                {
                                    ICell celda = fila.GetCell(contadorCelda);
                                    switch (celda.CellType)
                                    {
                                        case CellType.String:
                                            jefe_inmediato = fila.GetCell(contadorCelda).StringCellValue;
                                            break;
                                        case CellType.Numeric:
                                            jefe_inmediato = fila.GetCell(contadorCelda).NumericCellValue.ToString();
                                            break;
                                        case CellType.Blank:
                                            jefe_inmediato = "";
                                            break;
                                        default:
                                            //set default
                                            break;
                                    }
                                }
                                catch (Exception exe)
                                {
                                    if (!string.IsNullOrEmpty(jefe_inmediato))
                                    {

                                    }
                                }

                                contadorCelda++; try
                                {
                                    ICell celda = fila.GetCell(contadorCelda);
                                    switch (celda.CellType)
                                    {
                                        case CellType.String:
                                            sede = fila.GetCell(contadorCelda).StringCellValue;
                                            break;
                                        case CellType.Numeric:
                                            sede = fila.GetCell(contadorCelda).NumericCellValue.ToString();
                                            break;
                                        case CellType.Blank:
                                            sede = "";
                                            break;
                                        default:
                                            //set default
                                            break;
                                    }
                                }
                                catch (Exception exe)
                                {
                                    if (!string.IsNullOrEmpty(sede))
                                    {

                                    }
                                }

                                contadorCelda++; try
                                {
                                    ICell celda = fila.GetCell(contadorCelda);
                                    switch (celda.CellType)
                                    {
                                        case CellType.String:
                                            correo = fila.GetCell(contadorCelda).StringCellValue;
                                            break;
                                        case CellType.Numeric:
                                            correo = fila.GetCell(contadorCelda).NumericCellValue.ToString();
                                            break;
                                        case CellType.Blank:
                                            correo = "";
                                            break;
                                        default:
                                            //set default
                                            break;
                                    }
                                }
                                catch (Exception exe)
                                {
                                    if (!string.IsNullOrEmpty(correo))
                                    {

                                    }
                                }

                                contadorCelda++; try
                                {
                                    ICell celda = fila.GetCell(contadorCelda);
                                    switch (celda.CellType)
                                    {
                                        case CellType.String:
                                            turno = fila.GetCell(contadorCelda).StringCellValue;
                                            break;
                                        case CellType.Numeric:
                                            turno = fila.GetCell(contadorCelda).NumericCellValue.ToString();
                                            break;
                                        case CellType.Blank:
                                            turno = "";
                                            break;
                                        default:
                                            //set default
                                            break;
                                    }
                                }
                                catch (Exception exe)
                                {
                                    if (!string.IsNullOrEmpty(turno))
                                    {

                                    }
                                }

                                contadorCelda++; try
                                {
                                    ICell celda = fila.GetCell(contadorCelda);
                                    switch (celda.CellType)
                                    {
                                        case CellType.String:
                                            estado = fila.GetCell(contadorCelda).StringCellValue;
                                            break;
                                        case CellType.Numeric:
                                            estado = fila.GetCell(contadorCelda).NumericCellValue.ToString();
                                            break;
                                        case CellType.Blank:
                                            estado = "";
                                            break;
                                        default:
                                            //set default
                                            break;
                                    }
                                }
                                catch (Exception exe)
                                {
                                    if (!string.IsNullOrEmpty(estado))
                                    {

                                    }
                                }

                                #endregion


                                Colaborador colaborador = new Colaborador()
                                {
                                    area = area,
                                    cargo = cargo,
                                    cedula = cedula,
                                    correo = correo,
                                    estado = estado,
                                    jefe_inmediato = jefe_inmediato,
                                    nombres = nombres,
                                    sede = sede,
                                    turno = turno,
                                    usuario_adiciono = "Administrador"
                                };

                                ResponseDTO respuesta_validacion = validarColaborador(colaborador);

                                if (
                                    respuesta_validacion.estado.Equals("OK")
                                    )
                                {
                                    try
                                    {
                                        ResponseDTO respuesta_insercion = await insertarInfoColaborador(colaborador);

                                        if (respuesta_insercion.estado.Equals("OK"))
                                        {
                                            contador_cargados++;
                                        }
                                        else
                                        {
                                            contador_no_cargados++;
                                        }

                                        progress.Report(new CargueMasivoDTO()
                                        {
                                            estado = "EN CARGUE",
                                            total_registros = Int32.Parse(total_registros),
                                            total_registros_no_procesados = contador_no_cargados,
                                            total_registros_procesados = contador_cargados
                                        });

                                    }
                                    catch (Exception ex)
                                    {
                                        contador_no_cargados++;
                                    }
                                }
                                else
                                {
                                    contador_no_cargados++;
                                }

                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }
                }
                catch (Exception exe)
                {
                    _logger.LogError(exe, $"Error al realizar proceso de cargue de colaboradores");
                }

                observaciones = $"Se han cargado {contador_cargados} registros exitosamente, {contador_no_cargados} no pudieron cargarse";
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al realizar proceso de cague");
            }
            finally
            {
                LibroExcel.Close();
            }

            progress.Report(new CargueMasivoDTO()
            {
                estado = "FINALIZADO",
                total_registros = Int32.Parse(total_registros),
                total_registros_no_procesados = contador_no_cargados,
                total_registros_procesados = contador_cargados
            });
        }

        public async Task<bool> eliminarColaborador(string cedula)
        {
            return await _colaboradorRepository.eliminarColaborador(cedula);
        }
    }
}
