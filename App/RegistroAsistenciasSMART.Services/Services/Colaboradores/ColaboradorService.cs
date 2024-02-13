using AppDemoBlazor.Model.Models.CargueMasivo;
using RegistroAsistenciasSMART.Data.Connection;
using RegistroAsistenciasSMART.Data.Repositories.Interfaces.Colaboradores;
using RegistroAsistenciasSMART.Data.Repositories.Repositories.Colaboradores;
using RegistroAsistenciasSMART.Model.Models;
using RegistroAsistenciasSMART.Model.Response;
using RegistroAsistenciasSMART.Services.Interfaces.Colaboradores;
using RegistroAsistenciasSMART.Services.Utilidades;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RegistroAsistenciasSMART.Model.Models.Colaboradores;
using Microsoft.Extensions.Configuration;
using RegistroAsistenciasSMART.Model.DTO;
using RegistroAsistenciasSMART.Services.Interfaces.Configuracion.Perfilamiento;

namespace RegistroAsistenciasSMART.Services.Services.Colaboradores
{
    public class ColaboradorService : IColaboradorService
    {
        private readonly SqlConfiguration _sqlConfiguration;
        private readonly IColaboradorRepository _colaboradorRepository;
        private readonly IUserService _userService;

        private ILogger<ColaboradorService> _logger;

        private IEnumerable<string> tipo_reporte = new List<string>()
        {
            "Entrada",
            "Salida",
            "Traslado Salida",
            "Traslado Entrada"
        };

		public ColaboradorService
        (
            SqlConfiguration sqlConfiguration,
            ILogger<ColaboradorService> logger,
            IConfiguration config,
            IUserService userService
        )
        {
            _sqlConfiguration = sqlConfiguration;

            string my_sql_connection = config.GetConnectionString("MySqlConnection");

            _colaboradorRepository = new ColaboradorRepository(my_sql_connection);
            _logger = logger;

            _userService = userService;
        }

        private void limpiarInfoColaborador(Colaborador colaborador)
        {
            colaborador.area = colaborador.area.Trim();
            colaborador.turno = colaborador.turno.Trim();
            colaborador.usuario_adiciono = colaborador.usuario_adiciono.Trim();
            colaborador.cargo = colaborador.cargo.Trim();
            colaborador.cedula = colaborador.cedula.Trim();
            colaborador.correo = colaborador.correo.Trim();
            colaborador.estado = colaborador.estado.Trim();
            colaborador.sede = colaborador.sede.Trim();
            colaborador.jefe_inmediato = colaborador.jefe_inmediato.Trim();
            colaborador.nombres = colaborador.nombres.Trim();
        }
        private void limpiarInfoRegistroAsistencia(RegistroAsistencia registro)
        {
            registro.reporta = registro.reporta.Trim();
            registro.hora = registro.hora.Trim();
            registro.fecha = registro.fecha.Trim();
            registro.correo = registro.correo.Trim();
            registro.cedula = registro.cedula.Trim();
            registro.sede = registro.sede.Trim();
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
            limpiarInfoColaborador(colaborador);

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
            limpiarInfoColaborador(colaborador);

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
        public async Task<ResponseDTO> cargueMasivoColaboradores(Archivo archivo_cargue, IProgress<CargueMasivoDTO> progress, string usuario_accion)
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

            ejecutarCargueMasivo(rutaArchivo, progress, total_registros, usuario_accion);

            return new ResponseDTO() { estado = "OK", descripcion = "" };
        }
        private async Task ejecutarCargueMasivo(string rutaArchivoCargue, IProgress<CargueMasivoDTO> progress, string total_registros, string usuario)
        {

            XSSFWorkbook LibroExcel = new XSSFWorkbook(rutaArchivoCargue);

            int contador_cargados = 0, contador_no_cargados = 0;

            CargueMasivoDTO cargue = new CargueMasivoDTO();

            try
            {
                try
                {
                    XSSFSheet HojaExcel = LibroExcel.GetSheetAt(0) as XSSFSheet;
                    if (HojaExcel.LastRowNum >= 1)
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

                        for (int i = 1; i <= HojaExcel.LastRowNum; i++)
                        {
                            cedula = "";
                            nombres = "";
                            cargo = "";
                            area = "";
                            jefe_inmediato = "";
                            sede = "";
                            correo = "";
                            turno = "";
                            estado = "";

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
                                    usuario_adiciono = usuario
                                };

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

                                        cargue.errores.Add(new DetalleErrorCargueMasivo()
                                        {
                                            numero_registro = i,
                                            descripcion = respuesta_insercion.descripcion,
                                            identificador_registro = colaborador.cedula
                                        });
                                    }
                                }
                                catch (Exception exe)
                                {
                                    contador_no_cargados++;

                                    cargue.errores.Add(new DetalleErrorCargueMasivo()
                                    {
                                        numero_registro = i,
                                        descripcion = "Ocurrió un error al procesar el registro",
                                        identificador_registro = colaborador.cedula
                                    });

                                    _logger.LogError(exe, $"Error al realizar proceso de cargue de colaboradores");
                                }
                                finally
                                {
                                    cargue.estado = "EN CARGUE";
                                    cargue.total_registros = Int32.Parse(total_registros);
                                    cargue.total_registros_no_procesados = contador_no_cargados;
                                    cargue.total_registros_procesados = contador_cargados;
                                    cargue.total_faltantes = (Int32.Parse(total_registros) - contador_cargados - contador_no_cargados);

                                    progress.Report(cargue);
                                }
                            }
                            catch (Exception exe)
                            {
                                _logger.LogError(exe, $"Error al realizar proceso de cargue de colaboradores");
                            }
                        }
                    }
                }
                catch (Exception exe)
                {
                    _logger.LogError(exe, $"Error al realizar proceso de cargue de colaboradores");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error al realizar proceso de cague");
            }
            finally
            {
                LibroExcel.Close();
            }

            cargue.estado = "FINALIZADO";
            cargue.total_registros = Int32.Parse(total_registros);
            cargue.total_registros_no_procesados = contador_no_cargados;
            cargue.total_registros_procesados = contador_cargados;
            cargue.total_faltantes = (Int32.Parse(total_registros) - contador_cargados - contador_no_cargados);

            progress.Report(cargue);
        }
        public async Task<bool> eliminarColaborador(string cedula)
        {
            return await _colaboradorRepository.eliminarColaborador(cedula);
        }
        private ResponseDTO validarRegistroAsistencia(RegistroAsistencia registro)
        {
            if (string.IsNullOrEmpty(registro.cedula))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debes indicar la cédula para el registro"};
            }

            if (string.IsNullOrEmpty(registro.fecha))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debes indicar la fecha para el registro" };
            }

            if (string.IsNullOrEmpty(registro.hora))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debes indicar la hora para el registro" };
            }

            if (string.IsNullOrEmpty(registro.reporta))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debes indicar el tipo de reporte para el registro" };
            }

            if (!tipo_reporte.Contains(registro.reporta))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Tipo de registro no válido" };
            }

            if (string.IsNullOrEmpty(registro.sede))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debes indicar la sede para el registro" };
            }

            if (string.IsNullOrEmpty(registro.ip_address))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debes indicar la dirección IP para el registro" };
            }

            if (string.IsNullOrEmpty(registro.latitud))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debes indicar la ubicación para el registro" };
            }

            if (string.IsNullOrEmpty(registro.longitud))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debes indicar la ubicación para el registro" };
            }

            return new ResponseDTO() { estado = "OK"};
        }
		public async Task<ResponseDTO> insertarRegistroAsistencia(RegistroAsistencia registro)
        {
            limpiarInfoRegistroAsistencia(registro);

            IEnumerable<IpInfo> ips = _userService.consultarIpsAutorizados();

            if (!ips.Any(ip => ip.ip_address.Equals(registro.ip_address)))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "No te encuentras dentro de la red SMART"};
            }

            DateTime fecha_actual = DateTime.Now;

            registro.fecha = fecha_actual.ToString("yyyy-MM-dd");
            registro.hora = fecha_actual.ToString("HH:mm:ss");

            ResponseDTO respuesta_validacion = validarRegistroAsistencia(registro);

            if (!respuesta_validacion.estado.Equals("OK"))
            {
                return respuesta_validacion;
            }

            Colaborador colaborador = await _colaboradorRepository.consultarColaboradorByCedula(registro.cedula);

            if(colaborador is null)
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Tu cédula no está registrada como colaborador de SMART"};
            }

            registro.correo = colaborador.correo;

            if( await _colaboradorRepository.insertarRegistroAsistencia(registro))
            {
                string saludo = "Bienvenido";

                if (registro.reporta.ToLower().Contains("entrada"))
                {
                    saludo = "Bienvenido/a,";
                }
                else if(registro.reporta.ToLower().Contains("salida"))
                {
                    saludo = "Que tengas un buen día,";
                }

                return new ResponseDTO() { estado = "OK", descripcion = $"{saludo} {colaborador.nombres}"};
            }
            else
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "No fue posible guardar la información del registro"};
            }
        }
        public async Task<IEnumerable<RegistroAsistencia>> consultarRegistrosAsistencia(FiltroAsistencia filtros)
        {
            return await _colaboradorRepository.consultarRegistrosAsistencia(filtros);
        }

        public async Task<Archivo> generarReporteRegistroAsistencias(FiltroAsistencia filtros)
        {
            var tAsistencias = _colaboradorRepository.consultarRegistrosAsistencia(filtros);
            var tColaboradores = _colaboradorRepository.consultarColaboradores();

            await Task.WhenAll(tAsistencias, tColaboradores);

            IEnumerable<RegistroAsistencia> asistencias = await tAsistencias;
            IEnumerable<Colaborador> colaboradores = await tColaboradores;

            string ruta_base = Directory.GetCurrentDirectory() + "\\Reportes";
            if (!Directory.Exists(ruta_base)) Directory.CreateDirectory(ruta_base);

            string ruta_reporte = ruta_base + $"\\ReporteAsistencias_{DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss tt")}.xlsx";

            IWorkbook workbook;
            workbook = new XSSFWorkbook();

            //Estilo para las celdas de encabezado
            byte[] rgb = new byte[3] { 163, 189, 49 };
            NPOI.XSSF.UserModel.XSSFColor color = new NPOI.XSSF.UserModel.XSSFColor(rgb);

            NPOI.XSSF.UserModel.XSSFCellStyle boldStyle = (NPOI.XSSF.UserModel.XSSFCellStyle)workbook.CreateCellStyle();
            boldStyle.SetFillForegroundColor(color);
            boldStyle.FillPattern = FillPattern.SolidForeground;
            boldStyle.Alignment = HorizontalAlignment.Center;
            boldStyle.VerticalAlignment = VerticalAlignment.Center;
            IFont font = workbook.CreateFont();
            font.Color = NPOI.SS.UserModel.IndexedColors.White.Index;
            boldStyle.SetFont(font);

            #region Pestaña 1 - Asistencias

            ISheet sheet = workbook.CreateSheet("Asistencias");

            int ROW = 0;
            int column = 0;

            int cantidadColumnas = 0;

            #region Encabezado

            IRow encabezado = sheet.CreateRow(ROW);

            ICell celda;

            celda = encabezado.CreateCell(column);
            celda.SetCellValue("Fecha");
            celda.CellStyle = boldStyle;
            column++;
            cantidadColumnas++;

            celda = encabezado.CreateCell(column);
            celda.SetCellValue("Hora");
            celda.CellStyle = boldStyle;
            column++;
            cantidadColumnas++;

            celda = encabezado.CreateCell(column);
            celda.SetCellValue("Registro");
            celda.CellStyle = boldStyle;
            column++;
            cantidadColumnas++;

            celda = encabezado.CreateCell(column);
            celda.SetCellValue("Sede");
            celda.CellStyle = boldStyle;
            column++;
            cantidadColumnas++;

            celda = encabezado.CreateCell(column);
            celda.SetCellValue("Cédula");
            celda.CellStyle = boldStyle;
            column++;
            cantidadColumnas++;

            celda = encabezado.CreateCell(column);
            celda.SetCellValue("Nombre");
            celda.CellStyle = boldStyle;
            column++;
            cantidadColumnas++;

            celda = encabezado.CreateCell(column);
            celda.SetCellValue("Correo institucional");
            celda.CellStyle = boldStyle;
            column++;
            cantidadColumnas++;

            celda = encabezado.CreateCell(column);
            celda.SetCellValue("Cargo");
            celda.CellStyle = boldStyle;
            column++;
            cantidadColumnas++;

            celda = encabezado.CreateCell(column);
            celda.SetCellValue("Líder");
            celda.CellStyle = boldStyle;
            column++;
            cantidadColumnas++;

            celda = encabezado.CreateCell(column);
            celda.SetCellValue("Subproceso");
            celda.CellStyle = boldStyle;
            column++;
            cantidadColumnas++;

            celda = encabezado.CreateCell(column);
            celda.SetCellValue("Longitud");
            celda.CellStyle = boldStyle;
            column++;
            cantidadColumnas++;

            celda = encabezado.CreateCell(column);
            celda.SetCellValue("Latitud");
            celda.CellStyle = boldStyle;
            column++;
            cantidadColumnas++;

            celda = encabezado.CreateCell(column);
            celda.SetCellValue("Dirección IP");
            celda.CellStyle = boldStyle;
            column++;
            cantidadColumnas++;

            #endregion

            foreach (var asistencia in asistencias)
            {
                Colaborador colaborador = colaboradores.FirstOrDefault(c => c.cedula.Equals(asistencia.cedula));

                column = 0;
                ROW++;
                IRow fila = sheet.CreateRow(ROW);

                celda = fila.CreateCell(column);
                celda.SetCellValue(asistencia.fecha_adicion?.ToString("dd/MM/yyyy"));
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(asistencia.hora);
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(asistencia.reporta);
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(asistencia.sede);
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(asistencia.cedula);
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(colaborador?.nombres);
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(colaborador?.correo);
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(colaborador?.cargo);
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(colaborador?.jefe_inmediato);
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(colaborador?.area);
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(asistencia.longitud);
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(asistencia.latitud);
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(asistencia.ip_address);
                column++;

            }


            for (int i = 0; i < cantidadColumnas; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            #endregion

            using (FileStream fsx = new FileStream(ruta_reporte, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fsx);
            }
            workbook.Close();

            return new Archivo()
            {
                ruta_absoluta = ruta_reporte
            };
        }
    }
}
