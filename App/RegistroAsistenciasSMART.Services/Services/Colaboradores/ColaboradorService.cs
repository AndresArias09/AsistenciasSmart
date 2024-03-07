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
using System.Text.RegularExpressions;
using RegistroAsistenciasSMART.Model.Constantes;
using iText.Kernel.Pdf.Canvas.Wmf;
using NPOI.SS.Formula.Functions;
using System.Globalization;
using DocumentFormat.OpenXml.Spreadsheet;
using static iText.Kernel.Pdf.Colorspace.PdfDeviceCs;

namespace RegistroAsistenciasSMART.Services.Services.Colaboradores
{
    public class ColaboradorService : IColaboradorService
    {
        private readonly SqlConfiguration _sqlConfiguration;
        private readonly IColaboradorRepository _colaboradorRepository;
        private readonly IUserService _userService;
        /// <summary>
        /// Lista de días de lunes a jueves
        /// </summary>
        private readonly List<DayOfWeek> l_j = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday};
        /// <summary>
        /// Lista de días Viernes
        /// </summary>
        private readonly List<DayOfWeek> v = new List<DayOfWeek> { DayOfWeek.Friday };
        /// <summary>
        /// Lista de días Sábados
        /// </summary>
        private readonly List<DayOfWeek> s = new List<DayOfWeek> { DayOfWeek.Saturday };

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

            _colaboradorRepository = new ColaboradorRepository(_sqlConfiguration.ConnectionString);
            _logger = logger;

            _userService = userService;
        }

        private void limpiarInfoColaborador(Colaborador colaborador)
        {
            colaborador.area = colaborador.area.Trim();
            colaborador.usuario_adiciono = colaborador.usuario_adiciono.Trim();
            colaborador.cargo = colaborador.cargo.Trim();
            colaborador.correo = colaborador.correo.Trim();
            colaborador.sede = colaborador.sede.Trim();
            colaborador.nombres = colaborador.nombres.Trim();
            colaborador.apellidos = colaborador.apellidos.Trim();

            colaborador.sede = colaborador.sede.ToUpper();
            colaborador.area = colaborador.area.ToUpper();
        }
        private void limpiarInfoRegistroAsistencia(RegistroAsistencia registro)
        {
            registro.tipo_reporte = registro.tipo_reporte.Trim();
            registro.sede = registro.sede.Trim();
        }
        public ResponseDTO validarColaborador(Colaborador colaborador)
        {
            if (colaborador.cedula is null)
            {
                return new ResponseDTO() { estado= "ERROR", descripcion = "Debes indicar la cédula del colaborador"};
            }

            if (string.IsNullOrEmpty(colaborador.nombres))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debes indicar los nombres del colaborador" };
            }

            if (string.IsNullOrEmpty(colaborador.apellidos))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debes indicar los apellidos del colaborador" };
            }

            if (string.IsNullOrEmpty(colaborador.cargo))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debes indicar el cargo del colaborador" };
            }

            if (string.IsNullOrEmpty(colaborador.area))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debes indicar el área del colaborador" };
            }

            if (string.IsNullOrEmpty(colaborador.sede))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debes indicar la sede del colaborador" };
            }

            if (string.IsNullOrEmpty(colaborador.correo))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debes indicar el correo del colaborador" };
            }

            if (!Regex.Match(colaborador.correo, RegexConstants.EMAIL_REGEX).Success)
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = $"Formato de correo incorrecto para el correo electrónico del colaborador" };
            }

            if (colaborador.estado is null)
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debes indicar el estado del colaborador" };
            }

            if (string.IsNullOrEmpty(colaborador.usuario_adiciono))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debes indicar el usuario que adiciona al colaborador" };
            }

            return new ResponseDTO() { estado = "OK"};
        }
        public async Task<Colaborador> consultarColaboradorByCedula(long cedula)
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

            if (!respuesta_validacion.estado.Equals("OK")) return respuesta_validacion;

            Colaborador colaborador_temp = await _colaboradorRepository.consultarColaboradorByCedula(colaborador.cedula.GetValueOrDefault());

            bool nuevo_colaborador = colaborador_temp is null;

            bool respuesta = false;


            Colaborador jefe = await _colaboradorRepository.consultarColaboradorByCedula(colaborador.jefe_inmediato.GetValueOrDefault());

            if (jefe is null) colaborador.jefe_inmediato = null;


            if (nuevo_colaborador)
            {
                colaborador.estado = 1;
                respuesta = await _colaboradorRepository.insertarColaborador(colaborador);
            }
            else
            {
                respuesta = await _colaboradorRepository.actualizarColaborador(colaborador);
            }

            if (respuesta)
            {
                return new ResponseDTO() { estado = "OK", descripcion = "" };
            }
            else
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "No fue posible guardar la información del colaborador" };
            }
        }
        public async Task<ResponseDTO> cargueMasivoColaboradores(Archivo archivo_cargue, IProgress<CargueMasivoDTO> progress, string usuario_accion)
        {
            string rutaArchivo = "";

            string ruta_base = Path.Combine(Directory.GetCurrentDirectory(), "ArchivosCargueMasivo", "CargueColaborador");
            Directory.CreateDirectory(ruta_base);

            string nombre_final = "CARGUE_MASIVO_COLABORADORES_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";

            rutaArchivo = Path.Combine(ruta_base, nombre_final);

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
                XSSFSheet HojaExcel = LibroExcel.GetSheetAt(0) as XSSFSheet;
                if (HojaExcel.LastRowNum >= 1)
                {
                    string cedula = "";
                    string nombres = "";
                    string apellidos = "";
                    string cargo = "";
                    string area = "";
                    string jefe_inmediato = "";
                    string sede = "";
                    string correo = "";
                    DateTime? hora_entrada_lj = null;
                    DateTime? hora_salida_lj = null;
                    DateTime? hora_entrada_v = null;
                    DateTime? hora_salida_v = null;
                    DateTime? hora_entrada_s = null;
                    DateTime? hora_salida_s = null;

                    for (int i = 1; i <= HojaExcel.LastRowNum; i++)
                    {
                        cedula = "";
                        nombres = "";
                        apellidos = "";
                        cargo = "";
                        area = "";
                        jefe_inmediato = "";
                        sede = "";
                        correo = "";
                        hora_entrada_lj = null;
                        hora_salida_lj = null;
                        hora_entrada_v = null;
                        hora_salida_v = null;
                        hora_entrada_s = null;
                        hora_salida_s = null;

                        IRow fila = HojaExcel.GetRow(i);

                        try
                        {
                            int contadorCelda = -1;

                            #region Lectura datos excel

                            contadorCelda++;
                            cedula = UtilidadesExcel.getCellValue(contadorCelda,fila);

                            contadorCelda++;
                            nombres = UtilidadesExcel.getCellValue(contadorCelda, fila);

                            contadorCelda++;
                            apellidos = UtilidadesExcel.getCellValue(contadorCelda, fila);

                            contadorCelda++;
                            cargo = UtilidadesExcel.getCellValue(contadorCelda, fila);

                            contadorCelda++;
                            area = UtilidadesExcel.getCellValue(contadorCelda, fila);

                            contadorCelda++;
                            jefe_inmediato = UtilidadesExcel.getCellValue(contadorCelda, fila);

                            contadorCelda++;
                            sede = UtilidadesExcel.getCellValue(contadorCelda, fila);

                            contadorCelda++;
                            correo = UtilidadesExcel.getCellValue(contadorCelda, fila);

                            contadorCelda++;
                            hora_entrada_lj = UtilidadesExcel.getCellDateValue(contadorCelda, fila);

                            contadorCelda++;
                            hora_salida_lj = UtilidadesExcel.getCellDateValue(contadorCelda, fila);

                            contadorCelda++;
                            hora_entrada_v = UtilidadesExcel.getCellDateValue(contadorCelda, fila);

                            contadorCelda++;
                            hora_salida_v = UtilidadesExcel.getCellDateValue(contadorCelda, fila);

                            contadorCelda++;
                            hora_entrada_s = UtilidadesExcel.getCellDateValue(contadorCelda, fila);

                            contadorCelda++;
                            hora_salida_s = UtilidadesExcel.getCellDateValue(contadorCelda, fila);

                            #endregion

                            Colaborador colaborador = new Colaborador()
                            {
                                nombres = nombres,
                                apellidos = apellidos,
                                cargo = cargo,
                                area = area,
                                sede = sede,
                                correo = correo,
                                usuario_adiciono = usuario
                            };

                            #region Conversión de datos

                            //Cédula

                            long cedula_value = 0;

                            if (!long.TryParse(cedula, out cedula_value))
                            {
                                colaborador.cedula = null;
                            }
                            else
                            {
                                colaborador.cedula = cedula_value;
                            }

                            //Cédula del jefe inmediato

                            long cedula_jefe_inmediato_value = 0;

                            if (!long.TryParse(jefe_inmediato, out cedula_jefe_inmediato_value))
                            {
                                colaborador.jefe_inmediato = null;
                            }
                            else
                            {
                                colaborador.jefe_inmediato = cedula_jefe_inmediato_value;
                            }

                            #endregion

                            colaborador.cedula = cedula_value;
                            colaborador.jefe_inmediato = cedula_jefe_inmediato_value;
                            colaborador.hora_entrada_lj = hora_entrada_lj is null ? null : hora_entrada_lj.GetValueOrDefault().TimeOfDay;
                            colaborador.hora_salida_lj = hora_salida_lj is null ? null : hora_salida_lj.GetValueOrDefault().TimeOfDay;
                            colaborador.hora_entrada_v = hora_entrada_v is null ? null : hora_entrada_v.GetValueOrDefault().TimeOfDay;
                            colaborador.hora_salida_v = hora_salida_v is null ? null : hora_salida_v.GetValueOrDefault().TimeOfDay;
                            colaborador.hora_entrada_s = hora_entrada_s is null ? null : hora_entrada_s.GetValueOrDefault().TimeOfDay;
                            colaborador.hora_salida_s = hora_salida_s is null ? null : hora_salida_s.GetValueOrDefault().TimeOfDay;
                            

                            colaborador.estado = 1;

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
                                        identificador_registro = colaborador.cedula.GetValueOrDefault().ToString(),
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
                                    identificador_registro = colaborador.cedula.GetValueOrDefault().ToString()
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

        private ResponseDTO validarRegistroAsistencia(RegistroAsistencia registro)
        {
            if (registro.cedula_colaborador is null)
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debes indicar la cédula para el registro"};
            }

            if (string.IsNullOrEmpty(registro.tipo_reporte))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Debes indicar el tipo de reporte para el registro" };
            }

            if (!tipo_reporte.Contains(registro.tipo_reporte))
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

            ResponseDTO respuesta_validacion = validarRegistroAsistencia(registro);

            if (!respuesta_validacion.estado.Equals("OK"))
            {
                return respuesta_validacion;
            }

            if (registro.tipo_reporte.Equals("Traslado Salida") || registro.tipo_reporte.Equals("Traslado Entrada"))
            {
                FiltroAsistencia filtros = new FiltroAsistencia()
                {
                    cedula = registro.cedula_colaborador,
                    fecha_desde = DateTime.Now.Date,
                    tipo_reporte = "Entrada"
                };

                IEnumerable<RegistroAsistenciaDTO> registros_entrada = await _colaboradorRepository.consultarRegistrosAsistencia(filtros);

                if (registros_entrada.Count() <= 0)
                {
                    return new ResponseDTO() { estado = "ERROR", descripcion = "No se ha registrado una entrada hoy para el colaborador indicado"};
                }
            }

            Colaborador colaborador = await _colaboradorRepository.consultarColaboradorByCedula(registro.cedula_colaborador.GetValueOrDefault());

            if(colaborador is null)
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Tu cédula no está registrada como colaborador de SMART"};
            }


            if( await _colaboradorRepository.insertarRegistroAsistencia(registro))
            {
                string saludo = "Bienvenido";

                if (registro.tipo_reporte.ToLower().Contains("entrada"))
                {
                    saludo = "Bienvenido/a,";
                }
                else if(registro.tipo_reporte.ToLower().Contains("salida"))
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
        public async Task<IEnumerable<RegistroAsistenciaDTO>> consultarRegistrosAsistencia(FiltroAsistencia filtros)
        {
            return await _colaboradorRepository.consultarRegistrosAsistencia(filtros);
        }

        public async Task<Archivo> generarReporteRegistroAsistencias(FiltroAsistencia filtros)
        {

            IEnumerable<RegistroAsistenciaDTO> asistencias = await consultarRegistrosAsistencia(filtros);

            string ruta_base = Path.Combine(Directory.GetCurrentDirectory(), "Reportes");
            
            Directory.CreateDirectory(ruta_base);

            string ruta_reporte = Path.Combine(ruta_base, $"ReporteAsistencias_{DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss tt")}.xlsx");

            IWorkbook workbook;
            workbook = new XSSFWorkbook();

            //Estilo para las celdas de encabezado
            byte[] verde = new byte[3] { 163, 189, 49 };
            NPOI.XSSF.UserModel.XSSFCellStyle boldStyle = UtilidadesExcel.createStyle(verde, workbook);


            //Estilo para las celdas rojo
            byte[] rojo = new byte[3] { 201, 18, 18 };
            NPOI.XSSF.UserModel.XSSFCellStyle boldStyle_rojo = UtilidadesExcel.createStyle(rojo, workbook);


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
            celda.SetCellValue("Horario");
            celda.CellStyle = boldStyle;
            column++;
            cantidadColumnas++;

            celda = encabezado.CreateCell(column);
            celda.SetCellValue("Estado");
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
            celda.SetCellValue("Nombres");
            celda.CellStyle = boldStyle;
            column++;
            cantidadColumnas++;

            celda = encabezado.CreateCell(column);
            celda.SetCellValue("Apellidos");
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
                column = 0;
                ROW++;
                IRow fila = sheet.CreateRow(ROW);

                celda = fila.CreateCell(column);
                celda.SetCellValue(asistencia.fecha_adicion?.ToString("dddd, dd MMMM yyyy", new CultureInfo("es-ES")));
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(asistencia.fecha_adicion?.ToString("hh:mm:ss tt"));
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(asistencia.tipo_reporte);
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(GetHorarioRegistroAsistencia(asistencia));
                column++;

                string c = GetColorAsistencia(asistencia);

                celda = fila.CreateCell(column);

                if (c.Equals("green")) celda.CellStyle = boldStyle;
                else if (c.Equals("red")) celda.CellStyle = boldStyle_rojo;

                celda.SetCellValue("");
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(asistencia.sede);
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(asistencia.cedula);
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(asistencia.nombres);
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(asistencia.apellidos);
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(asistencia.correo);
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(asistencia.cargo);
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(asistencia.jefe_inmediato);
                column++;

                celda = fila.CreateCell(column);
                celda.SetCellValue(asistencia.area);
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

        public string GetHorarioRegistroAsistencia(RegistroAsistenciaDTO registro)
        {
            if (!registro.tipo_reporte.Equals("Entrada") && !registro.tipo_reporte.Equals("Salida"))
            {
                return "";
            }

            TimeSpan? hora = null;

            DayOfWeek dia = registro.fecha_adicion.GetValueOrDefault().DayOfWeek;

            if (l_j.Contains(dia)) //Lunes a jueves
            {
                if (registro.tipo_reporte.Equals("Entrada"))
                {
                    hora = registro.hora_entrada_lj;
                }
                else if (registro.tipo_reporte.Equals("Salida"))
                {
                    hora = registro.hora_salida_lj;
                }
            }
            else if (v.Contains(dia)) //Viernes
            {
                if (registro.tipo_reporte.Equals("Entrada"))
                {
                    hora = registro.hora_entrada_v;
                }
                else if (registro.tipo_reporte.Equals("Salida"))
                {
                    hora = registro.hora_salida_v;
                }
            }
            else if (s.Contains(dia)) //Sábados
            {
                if (registro.tipo_reporte.Equals("Entrada"))
                {
                    hora = registro.hora_entrada_s;
                }
                else if (registro.tipo_reporte.Equals("Salida"))
                {
                    hora = registro.hora_salida_s;
                }
            }
            else
            {
                return "";
            }

            if (hora is null) return "";

            return DateTime.Now.Date.Add(hora.GetValueOrDefault()).ToString("hh:mm tt");

        }

        public string GetColorAsistencia(RegistroAsistenciaDTO registro)
        {
            if (!registro.tipo_reporte.Equals("Entrada") && !registro.tipo_reporte.Equals("Salida"))
            {
                return "gray";
            }

            TimeSpan? hora = null;

            DayOfWeek dia = registro.fecha_adicion.GetValueOrDefault().DayOfWeek;

            if (l_j.Contains(dia)) //Lunes a jueves
            {
                if (registro.tipo_reporte.Equals("Entrada"))
                {
                    hora = registro.hora_entrada_lj;
                }
                else if (registro.tipo_reporte.Equals("Salida"))
                {
                    hora = registro.hora_salida_lj;
                }
            }
            else if (v.Contains(dia)) //Viernes
            {
                if (registro.tipo_reporte.Equals("Entrada"))
                {
                    hora = registro.hora_entrada_v;
                }
                else if (registro.tipo_reporte.Equals("Salida"))
                {
                    hora = registro.hora_salida_v;
                }
            }
            else if (s.Contains(dia)) //Sábados
            {
                if (registro.tipo_reporte.Equals("Entrada"))
                {
                    hora = registro.hora_entrada_s;
                }
                else if (registro.tipo_reporte.Equals("Salida"))
                {
                    hora = registro.hora_salida_s;
                }
            }
            else
            {
                return "gray";
            }

            if (hora is null) return "gray";

            TimeSpan hora_registro = registro.fecha_adicion.GetValueOrDefault().TimeOfDay;

            if (registro.tipo_reporte.Equals("Entrada"))
            {
                DateTime hora_maxima_llegada = DateTime.Now.Date.Add(hora.GetValueOrDefault()).AddMinutes(5);

                if (hora_registro > hora_maxima_llegada.TimeOfDay)
                {
                    return "red";
                }
                else
                {
                    return "green";
                }
            }
            else if (registro.tipo_reporte.Equals("Salida"))
            {
                if (hora_registro < hora)
                {
                    return "red";
                }
                else
                {
                    return "green";
                }
            }

            return "gray";

        }

        public async Task<IEnumerable<Colaborador>> consultarJefesInmediatos()
        {
            return await _colaboradorRepository.consultarJefesInmediatos();
        }
    }
}
