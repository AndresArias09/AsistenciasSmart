using RegistroAsistenciasSMART.Data.Connection;
using RegistroAsistenciasSMART.Services.Interfaces.Utilidades;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RegistroAsistenciasSMART.Data.Repositories.Interfaces.Auditoria;
using RegistroAsistenciasSMART.Data.Repositories.Repositories.Auditoria;
using System.Net.Mime;
using RegistroAsistenciasSMART.Services.Interfaces.Auditoria;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;
using RegistroAsistenciasSMART.Model.Models.Auditoria;

namespace RegistroAsistenciasSMART.Services.Services.Utilidades
{
    /// <summary>
    /// Implementación de la interfaz <see cref="IEmailService"/>
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly IAuditoriaService _auditoriaService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IConfiguration configuration,
            IAuditoriaService auditoriaService,
            ILogger<EmailService> logger)
        {
            _auditoriaService = auditoriaService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> enviarCorreo(EmailInfo emailInfo)
        {
            if (emailInfo.destinatarios.Count <= 0) return false;

            if (emailInfo.destinatarios.All(d => string.IsNullOrEmpty(d))) return false;

            try
            {
                string email_from = _configuration.GetSection("AppSettings:EmailFrom").Value;
                string email_bcc = _configuration.GetSection("AppSettings:EmailBCC").Value;
                string email_host = _configuration.GetSection("AppSettings:EmailHost").Value;
                string email_port = _configuration.GetSection("AppSettings:EmailPort").Value;
                string email_ssl = _configuration.GetSection("AppSettings:EmailSSLEnabled").Value;
                string email_pass = _configuration.GetSection("AppSettings:EmailPass").Value;

                emailInfo.email_emisor = email_from;

                MailMessage emailsend = new MailMessage();
                emailsend.From = new MailAddress(email_from);

                foreach (var d in emailInfo.destinatarios)
                {
                    if (!string.IsNullOrEmpty(d))
                    {
                        emailsend.To.Add(new MailAddress(d));
                    }
                }

                foreach (var cc in emailInfo.cc)
                {
                    if (!string.IsNullOrEmpty(cc))
                    {
                        emailsend.CC.Add(new MailAddress(cc));
                    }
                }

                foreach (var bcc in emailInfo.bcc)
                {
                    if (!string.IsNullOrEmpty(bcc))
                    {
                        emailsend.Bcc.Add(new MailAddress(bcc));
                    }
                }

                if (!string.IsNullOrEmpty(email_bcc))
                {
                    emailsend.Bcc.Add(new MailAddress(email_bcc));
                }

                emailsend.Subject = emailInfo.asunto;
                emailsend.IsBodyHtml = true;
                emailsend.Body = emailInfo.mensaje;
                emailsend.IsBodyHtml = true;
                emailsend.Priority = MailPriority.Normal;

                if (emailInfo.anexos.Count > 0)
                {
                    foreach (var anexo in emailInfo.anexos)
                    {
                        if (File.Exists(anexo.ruta_anexo))
                        {
                            Attachment attachment = new Attachment(anexo.ruta_anexo, MediaTypeNames.Application.Octet);

                            if (!string.IsNullOrEmpty(anexo.nombre_archivo))
                            {
                                attachment.ContentDisposition.FileName = $"{anexo.nombre_archivo}{Path.GetExtension(anexo.ruta_anexo)}";
                            }

                            emailsend.Attachments.Add(attachment);
                        }
                    }
                }

                SmtpClient smtp = new SmtpClient(email_host);
                smtp.Host = email_host;
                smtp.Port = int.Parse(email_port);
                smtp.EnableSsl = Convert.ToBoolean(email_ssl);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(email_from, email_pass);

                //INTENTOS DE ENVIO DE CORREO
                Boolean enviado = false;
                int intentos = 0;
                while (!enviado && intentos < 3)
                {
                    try
                    {
                        smtp.Send(emailsend);
                        enviado = true;
                        emailsend.Dispose();

                        emailInfo.enviado = true;
                    }
                    catch (Exception exe)
                    {
                        _logger.LogError(exe, $"Error al enviar correo");
                        emailInfo.descripcion_error = $"{exe.Message} {exe.InnerException}";
                        emailInfo.enviado = false;
                    }

                    await _auditoriaService.registrarAuditoriaEnvioEmail(emailInfo);

                    Thread.Sleep(5000);
                    intentos++;
                }
            }
            catch (Exception exe)
            {
                _logger.LogError(exe, $"Error al enviar correo");
            }

            return emailInfo.enviado;
        }
    }
}
