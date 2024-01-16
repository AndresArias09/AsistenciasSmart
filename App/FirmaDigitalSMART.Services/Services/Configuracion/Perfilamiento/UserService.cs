using FirmaDigitalSMART.Data.Connection;
using FirmaDigitalSMART.Data.Repositories.Repositories.Auditoria;
using FirmaDigitalSMART.Data.Repositories.Repositories.Configuracion.Perfilamiento;
using FirmaDigitalSMART.Data.Repositories.Interfaces.Auditoria;
using FirmaDigitalSMART.Data.Repositories.Interfaces.Configuracion.Perfilamiento;
using FirmaDigitalSMART.Model.DTO.Configuracion.Perfilamiento;
using FirmaDigitalSMART.Model.Models.Configuracion.Perfilamiento;
using FirmaDigitalSMART.Model.Response;
using FirmaDigitalSMART.Services.Interfaces.Auditoria;
using FirmaDigitalSMART.Services.Interfaces.Configuracion.Perfilamiento;
using FirmaDigitalSMART.Services.Interfaces.Utilidades;
using FirmaDigitalSMART.Services.Services.Auditoria;
using FirmaDigitalSMART.Services.Utilidades;
using iTextSharp.text.log;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FirmaDigitalSMART.Services.Services.Utilidades;
using FirmaDigitalSMART.Model.Models.OTP;
using FirmaDigitalSMART.Data.Repositories.Interfaces.OTP;
using FirmaDigitalSMART.Data.Repositories.Repositories.OTP;
using FirmaDigitalSMART.Model.DTO;
using DocumentFormat.OpenXml.Wordprocessing;
using Org.BouncyCastle.Utilities.Net;
using DocumentFormat.OpenXml.Drawing.Charts;
using OtpNet;
using DocumentFormat.OpenXml.Spreadsheet;
using NPOI.SS.Formula.Functions;
using Microsoft.AspNetCore.DataProtection;
using System.Net.Sockets;
using FirmaDigitalSMART.Model.Models.Auditoria;

namespace FirmaDigitalSMART.Services.Services.Configuracion.Perfilamiento
{
    public class UserService : IUserService
    {
        private readonly SqlConfiguration _sqlConfiguration;
        private readonly IUserRepository _userRepository;
        private readonly IAuditoriaService _auditoriaService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailSender;
        private readonly IEncryptService _encryptService;
        private readonly NavigationManager _navigationManager;
        private readonly IOTPRepository _OTPRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(SqlConfiguration sqlConfiguration, 
            IConfiguration configuration, 
            IEncryptService encrypt, 
            IEmailService emailService, 
            NavigationManager navigationManager, 
            IAuditoriaService auditoriaService,
            IEncryptService encryptService,
            ILogger<UserService> logger)
        {
            _sqlConfiguration = sqlConfiguration;
            _userRepository = new UserRepository(_sqlConfiguration.ConnectionString);
            _OTPRepository = new OTPRepository(_sqlConfiguration.ConnectionString);
            _auditoriaService = auditoriaService;
            _configuration = configuration;
            _emailSender = emailService;
            _navigationManager = navigationManager;
            _encryptService = encryptService;
            _logger = logger;
        }

        public async Task<ResponseDTO> loginUsuario(UserDTO usuario, string ipAddress)
        {
            try
            {
                if (!usuario.usuario.EndsWith("@smart.edu.co"))
                {
                    return new ResponseDTO() { estado = "ERROR",descripcion = "Usuario no autorizado"};
                }

                Usuario user = await _userRepository.getUsuarioByUser(usuario.usuario);

                if (user is null)
                {
                    await _userRepository.insertarUsuario(new Usuario()
                    {
                        usuario = usuario.usuario,
                        email = usuario.usuario
                    });
                }

                string id_auditoria = await _userRepository.registrarAuditoriaLogin(usuario.usuario, "Exitoso", ipAddress);
                return new ResponseDTO() { estado = "OK", descripcion = id_auditoria };
            }
            catch(Exception exe)
            {
                _logger.LogError(exe, $"Error al realizar login para usuario {usuario.usuario}");
                return new ResponseDTO()
                {
                    estado = "ERROR",
                    descripcion = "Ocurrió un error al realizar el login",
                };
            }
        }

        public async Task<Usuario> getUsuario(string id_usuario)
        {
            return await _userRepository.getUsuarioById(id_usuario);
        }

        public async Task<Usuario> getUsuarioByUser(string usuario)
        {
            return await _userRepository.getUsuarioByUser(usuario);
        }

        public async Task<bool> registrarAuditoriaCierreSesion(string id_auditoria, string usuario, string ip, string motivo)
        {
            return await _userRepository.registrarAuditoriaCierreSesion(id_auditoria, usuario, ip, motivo);
        }

        public async Task<ResponseDTO> enviarCodigoOTPLogin(string nombre_usuario, string ipAccion)
        {
            try
            {
                Usuario user = await _userRepository.getUsuarioByUser(nombre_usuario);

                if(user is null)
                {
                    return new ResponseDTO() { estado = "ERROR", descripcion = "El usuario no existe" };
                }

                if(string.IsNullOrEmpty(user.email))
                {
                    return new ResponseDTO() { estado = "ERROR", descripcion = "El usuario no posee correo electrónico" };
                }

                Random R = new Random();
                int otp_code = R.Next(100000, 999999);

                string nombre_cliente = _configuration.GetSection("DatosAplicativo:NombreCliente").Value;
                string nombre_aplicativo = _configuration.GetSection("DatosAplicativo:NombreAplicativo").Value;

                #region Cuerpo correo

                string cuerpoCorreo = @$"<!DOCTYPE HTML
                      PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional //EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
                    <html xmlns=""http://www.w3.org/1999/xhtml"" xmlns:v=""urn:schemas-microsoft-com:vml""
                      xmlns:o=""urn:schemas-microsoft-com:office:office"">

                    <head>
                      <!--[if gte mso 9]>
                    <xml>
                      <o:OfficeDocumentSettings>
                        <o:AllowPNG/>
                        <o:PixelsPerInch>96</o:PixelsPerInch>
                      </o:OfficeDocumentSettings>
                    </xml>
                    <![endif]-->
                      <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
                      <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                      <meta name=""x-apple-disable-message-reformatting"">
                      <!--[if !mso]><!-->
                      <meta http-equiv=""X-UA-Compatible"" content=""IE=edge""><!--<![endif]-->
                      <title></title>

                      <style type=""text/css"">
                        @media only screen and (min-width: 620px) {{
                          .u-row {{
                            width: 600px !important;
                          }}

                          .u-row .u-col {{
                            vertical-align: top;
                          }}

                          .u-row .u-col-100 {{
                            width: 600px !important;
                          }}

                        }}

                        @media (max-width: 620px) {{
                          .u-row-container {{
                            max-width: 100% !important;
                            padding-left: 0px !important;
                            padding-right: 0px !important;
                          }}

                          .u-row .u-col {{
                            min-width: 320px !important;
                            max-width: 100% !important;
                            display: block !important;
                          }}

                          .u-row {{
                            width: 100% !important;
                          }}

                          .u-col {{
                            width: 100% !important;
                          }}

                          .u-col>div {{
                            margin: 0 auto;
                          }}
                        }}

                        body {{
                          margin: 0;
                          padding: 0;
                        }}

                        table,
                        tr,
                        td {{
                          vertical-align: top;
                          border-collapse: collapse;
                        }}

                        p {{
                          margin: 0;
                        }}

                        .ie-container table,
                        .mso-container table {{
                          table-layout: fixed;
                        }}

                        * {{
                          line-height: inherit;
                        }}

                        a[x-apple-data-detectors='true'] {{
                          color: inherit !important;
                          text-decoration: none !important;
                        }}

                        table,
                        td {{
                          color: #000000;
                        }}

                        #u_body a {{
                          color: #0000ee;
                          text-decoration: underline;
                        }}
                      </style>



                      <!--[if !mso]><!-->
                      <link href=""https://fonts.googleapis.com/css?family=Montserrat:400,700&display=swap"" rel=""stylesheet"" type=""text/css"">
                      <!--<![endif]-->

                    </head>

                    <body class=""clean-body u_body""
                      style=""margin: 0;padding: 0;-webkit-text-size-adjust: 100%;background-color: #f0f0f0;color: #000000"">
                      <!--[if IE]><div class=""ie-container""><![endif]-->
                      <!--[if mso]><div class=""mso-container""><![endif]-->
                      <table id=""u_body""
                        style=""border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;min-width: 320px;Margin: 0 auto;background-color: #f0f0f0;width:100%""
                        cellpadding=""0"" cellspacing=""0"">
                        <tbody>
                          <tr style=""vertical-align: top"">
                            <td style=""word-break: break-word;border-collapse: collapse !important;vertical-align: top"">
                              <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td align=""center"" style=""background-color: #f0f0f0;""><![endif]-->



                              <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                                <div class=""u-row""
                                  style=""margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                                  <div
                                    style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                    <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:600px;""><tr style=""background-color: transparent;""><![endif]-->

                                    <!--[if (mso)|(IE)]><td align=""center"" width=""600"" style=""background-color: #ddffe7;width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;"" valign=""top""><![endif]-->
                                    <div class=""u-col u-col-100""
                                      style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;"">
                                      <div style=""background-color: #ddffe7;height: 100%;width: 100% !important;"">
                                        <!--[if (!mso)&(!IE)]><!-->
                                        <div
                                          style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;"">
                                          <!--<![endif]-->

                                          <table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0""
                                            cellspacing=""0"" width=""100%"" border=""0"">
                                            <tbody>
                                              <tr>
                                                <td
                                                  style=""overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:arial,helvetica,sans-serif;""
                                                  align=""left"">

                                                  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">
                                                    <tr>
                                                      <td style=""padding-right: 0px;padding-left: 0px;"" align=""center"">

                                                        <img align=""center"" border=""0"" src=""https://i.ibb.co/HKFfGgX/image-1.png""
                                                          alt=""image"" title=""image""
                                                          style=""outline: none;text-decoration: none;-ms-interpolation-mode: bicubic;clear: both;display: inline-block !important;border: none;height: auto;float: none;width: 100%;max-width: 190px;""
                                                          width=""190"" />

                                                      </td>
                                                    </tr>
                                                  </table>

                                                </td>
                                              </tr>
                                            </tbody>
                                          </table>

                                          <!--[if (!mso)&(!IE)]><!-->
                                        </div><!--<![endif]-->
                                      </div>
                                    </div>
                                    <!--[if (mso)|(IE)]></td><![endif]-->
                                    <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                                  </div>
                                </div>
                              </div>





                              <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                                <div class=""u-row""
                                  style=""margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                                  <div
                                    style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                    <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:600px;""><tr style=""background-color: transparent;""><![endif]-->

                                    <!--[if (mso)|(IE)]><td align=""center"" width=""600"" style=""background-color: #ffffff;width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"" valign=""top""><![endif]-->
                                    <div class=""u-col u-col-100""
                                      style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;"">
                                      <div
                                        style=""background-color: #ffffff;height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                        <!--[if (!mso)&(!IE)]><!-->
                                        <div
                                          style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                          <!--<![endif]-->

                                          <table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0""
                                            cellspacing=""0"" width=""100%"" border=""0"">
                                            <tbody>
                                              <tr>
                                                <td
                                                  style=""overflow-wrap:break-word;word-break:break-word;padding:30px 10px 10px;font-family:arial,helvetica,sans-serif;""
                                                  align=""left"">

                                                  <!--[if mso]><table width=""100%""><tr><td><![endif]-->
                                                  <h1
                                                    style=""margin: 0px; line-height: 140%; text-align: center; word-wrap: break-word; font-family: 'Montserrat',sans-serif; font-size: 22px; font-weight: 700;"">
                                                    <span><span>Tu código de verificación es</span></span></h1>
                                                  <!--[if mso]></td></tr></table><![endif]-->

                                                </td>
                                              </tr>
                                            </tbody>
                                          </table>

                                          <table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0""
                                            cellspacing=""0"" width=""100%"" border=""0"">
                                            <tbody>
                                              <tr>
                                                <td
                                                  style=""overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:arial,helvetica,sans-serif;""
                                                  align=""left"">

                                                  <!--[if mso]><style>.v-button {{background: transparent !important;}}</style><![endif]-->
                                                  <div align=""center"">
                                                    <!--[if mso]><v:roundrect xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:w=""urn:schemas-microsoft-com:office:word"" href=""https://unlayer.com"" style=""height:42px; v-text-anchor:middle; width:216px;"" arcsize=""0%""  strokecolor=""#000000"" strokeweight=""2px"" fillcolor=""#ffffff""><w:anchorlock/><center style=""color:#000000;""><![endif]-->
                                                    <a href=""https://unlayer.com"" target=""_blank"" class=""v-button""
                                                      style=""box-sizing: border-box;display: inline-block;text-decoration: none;-webkit-text-size-adjust: none;text-align: center;color: #000000; background-color: #ffffff; border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px; width:38%; max-width:100%; overflow-wrap: break-word; word-break: break-word; word-wrap:break-word; mso-border-alt: none;border-top-color: #000000; border-top-style: solid; border-top-width: 2px; border-left-color: #000000; border-left-style: solid; border-left-width: 2px; border-right-color: #000000; border-right-style: solid; border-right-width: 2px; border-bottom-color: #000000; border-bottom-style: solid; border-bottom-width: 2px;font-size: 18px;"">
                                                      <span style=""display:block;padding:10px 20px;line-height:120%;"">"+otp_code+@"</span>
                                                    </a>
                                                    <!--[if mso]></center></v:roundrect><![endif]-->
                                                  </div>

                                                </td>
                                              </tr>
                                            </tbody>
                                          </table>

                                          <table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0""
                                            cellspacing=""0"" width=""100%"" border=""0"">
                                            <tbody>
                                              <tr>
                                                <td
                                                  style=""overflow-wrap:break-word;word-break:break-word;padding:30px 10px 10px;font-family:arial,helvetica,sans-serif;""
                                                  align=""left"">

                                                  <div
                                                    style=""font-size: 14px; line-height: 140%; text-align: center; word-wrap: break-word;"">
                                                    <p style=""line-height: 140%;"">Usa este código para completar tu inicio de sesión en el aplicativo "+nombre_aplicativo+@".</p>
                                                    <p style=""line-height: 140%;"">El código es válido por 5 minutos</p>
                                                  </div>

                                                </td>
                                              </tr>
                                            </tbody>
                                          </table>

                                          <!--[if (!mso)&(!IE)]><!-->
                                        </div><!--<![endif]-->
                                      </div>
                                    </div>
                                    <!--[if (mso)|(IE)]></td><![endif]-->
                                    <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                                  </div>
                                </div>
                              </div>





                              <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                                <div class=""u-row""
                                  style=""margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                                  <div
                                    style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                    <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:600px;""><tr style=""background-color: transparent;""><![endif]-->

                                    <!--[if (mso)|(IE)]><td align=""center"" width=""600"" style=""width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"" valign=""top""><![endif]-->
                                    <div class=""u-col u-col-100""
                                      style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;"">
                                      <div
                                        style=""height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                        <!--[if (!mso)&(!IE)]><!-->
                                        <div
                                          style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                          <!--<![endif]-->

                                          <table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0""
                                            cellspacing=""0"" width=""100%"" border=""0"">
                                            <tbody>
                                              <tr>
                                                <td
                                                  style=""overflow-wrap:break-word;word-break:break-word;padding:10px 0px 0px;font-family:arial,helvetica,sans-serif;""
                                                  align=""left"">

                                                  <table height=""0px"" align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%""
                                                    style=""border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;border-top: 1px solid #000000;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%"">
                                                    <tbody>
                                                      <tr style=""vertical-align: top"">
                                                        <td
                                                          style=""word-break: break-word;border-collapse: collapse !important;vertical-align: top;font-size: 0px;line-height: 0px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%"">
                                                          <span>&#160;</span>
                                                        </td>
                                                      </tr>
                                                    </tbody>
                                                  </table>

                                                </td>
                                              </tr>
                                            </tbody>
                                          </table>

                                          <table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0""
                                            cellspacing=""0"" width=""100%"" border=""0"">
                                            <tbody>
                                              <tr>
                                                <td
                                                  style=""overflow-wrap:break-word;word-break:break-word;padding:0px 0px 10px;font-family:arial,helvetica,sans-serif;""
                                                  align=""left"">

                                                  <table height=""0px"" align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%""
                                                    style=""border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;border-top: 1px solid #000000;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%"">
                                                    <tbody>
                                                      <tr style=""vertical-align: top"">
                                                        <td
                                                          style=""word-break: break-word;border-collapse: collapse !important;vertical-align: top;font-size: 0px;line-height: 0px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%"">
                                                          <span>&#160;</span>
                                                        </td>
                                                      </tr>
                                                    </tbody>
                                                  </table>

                                                </td>
                                              </tr>
                                            </tbody>
                                          </table>

                                          <table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0""
                                            cellspacing=""0"" width=""100%"" border=""0"">
                                            <tbody>
                                              <tr>
                                                <td
                                                  style=""overflow-wrap:break-word;word-break:break-word;padding:20px 50px 10px;font-family:arial,helvetica,sans-serif;""
                                                  align=""left"">

                                                  <div
                                                    style=""font-size: 14px; color: #000000; line-height: 140%; text-align: center; word-wrap: break-word;"">
                                                    <p style=""font-size: 14px; line-height: 140%;""> SMART TRAINING SOCIETY - Todos los derechos reservados
                                                    </p>
                                                  </div>

                                                </td>
                                              </tr>
                                            </tbody>
                                          </table>

                                          <!--[if (!mso)&(!IE)]><!-->
                                        </div><!--<![endif]-->
                                      </div>
                                    </div>
                                    <!--[if (mso)|(IE)]></td><![endif]-->
                                    <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                                  </div>
                                </div>
                              </div>



                              <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
                            </td>
                          </tr>
                        </tbody>
                      </table>
                      <!--[if mso]></div><![endif]-->
                      <!--[if IE]></div><![endif]-->
                    </body>

                    </html>";

                #endregion

                EmailInfo emailInfo = new EmailInfo();

                emailInfo.asunto = $"{nombre_aplicativo} - Verificación de Identidad";
                emailInfo.pantalla = "Login";
                emailInfo.mensaje = cuerpoCorreo;
                emailInfo.descripcion = "Verificación OTP para Login";
                emailInfo.destinatarios.Add(user.email);
                emailInfo.numero_identificacion_proceso = user.email;
                emailInfo.usuario = user.email;

                bool resultado = await _emailSender.enviarCorreo(emailInfo);

                await _OTPRepository.insertarOTP(new OTPModel()
                {
                    otp_code = otp_code.ToString(),
                    descripcion = "Validación OTP Login",
                    estado = "Envíado",
                    metodos_envio = "email",
                    numero_documento_proceso = user.usuario,
                    tipo_proceso = "Login"
                });

                if (resultado)
                {
                    return new ResponseDTO() { estado = "OK", descripcion = "" };
                }
            }
            catch (Exception exe)
            {
                _logger.LogError(exe, $"Error al enviar codigo OTP login usuario {nombre_usuario}");
            }

            return new ResponseDTO() { estado = "ERROR", descripcion = "No ha sido posible completar esta operación" };
        }

        public async Task<ResponseDTO> validarCodigoOTP(OTP_DTO otp_code, string nombre_usuario, string ipAccion)
        {
            Usuario user = await _userRepository.getUsuarioByUser(nombre_usuario);

            if (user is null)
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "El usuario no existe" };
            }

            OTPModel lastOTP = await _OTPRepository.getUltimoOTPProceso(nombre_usuario, "Login");

            if (lastOTP is null) return new ResponseDTO() { estado = "ERROR", descripcion = "No se encontró último código OTP" };

            string OTPCliente = otp_code.C1 +
                                otp_code.C2 +
                                otp_code.C3 +
                                otp_code.C4 +
                                otp_code.C5 +
                                otp_code.C6
                                ;

            if (!lastOTP.otp_code.Equals(OTPCliente))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "Código no válido" };
            }

            //OTP mayor a 5 minutos
            if ((DateTime.Now - lastOTP.fecha_adicion).TotalSeconds > (5 * 60))
            {
                return new ResponseDTO() { estado = "ERROR", descripcion = "El código ha expirado, por favor solicite uno nuevo" };
            }

            await _OTPRepository.completarEstadoOTP(lastOTP.id_otp_code, "Verificado");

            return new ResponseDTO() { estado = "OK", descripcion = "" };
        }

        public async Task procesarIngreso(string usuario, string ip_address, string descripcion)
        {
            await _userRepository.registrarAuditoriaLogin(usuario, descripcion, ip_address);
        }
    }
}
