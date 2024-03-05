using RegistroAsistenciasSMART.Services.Interfaces.Auditoria;
using Microsoft.JSInterop;
using Serilog;
using RegistroAsistenciasSMART.Model.Models.Auditoria;

namespace RegistroAsistenciasSMART.Web.Helpers
{
    public static class IJSRuntimeExtension
    {
        public static IAuditoriaService _auditoriaService { get; private set; }

        public static void setAuditoriaService(IAuditoriaService auditoriaService)
        {
            _auditoriaService = auditoriaService;
        }
        /// <summary>
        /// Muestra en pantalla una alerta <c>Sweet Alert</c> común
        /// </summary>
        /// <param name="titulo">Título de la alerta</param>
        /// <param name="mensaje">Mensaje de la alerta</param>
        /// <param name="tipoMensaje">Tipo de alerta, que se verá reflejada en el ícono de ésta</param>
        public static async Task SweetAlertUsual(this IJSRuntime jsRuntime, string titulo, string mensaje, TipoMensajeSweetAlert tipoMensaje)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("Swal.fire", titulo, mensaje, tipoMensaje.ToString());
            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al ejecutar javascript");
            }
        }
        /// <summary>
        /// Muestra en pantalla una alerta <c>Sweet Alert</c> en la cual se puede mostrar contenido <c>HTML</c> en el mensaje
        /// </summary>
        /// <param name="titulo">Título de la alerta</param>
        /// <param name="mensaje">Mensaje de la alerta en <c>HTML</c></param>
        /// <param name="tipoMensaje">Tipo de alerta, que se verá reflejada en el ícono de ésta</param>
        public static async Task SweetAlertHtml(this IJSRuntime jsRuntime, string titulo, string mensaje, TipoMensajeSweetAlert tipoMensaje)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("swalWithHtml", titulo, mensaje, tipoMensaje.ToString());
            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al ejecutar javascript");
            }
        }
        /// <summary>
        /// Muestra una alerta Sweet Alert comúm, con un footer
        /// </summary>
        /// <param name="titulo">Título de la alerta</param>
        /// <param name="mensaje">Mensaje de la alerta</param>
        /// <param name="footer">Contenido del footer</param>
        /// <param name="tipoMensaje">Tipo de alerta, que se verá reflejada en el ícono de ésta</param>
        public static async Task SweetAlertUsualWithFooter(this IJSRuntime jsRuntime, string titulo, string mensaje, string footer, TipoMensajeSweetAlert tipoMensaje)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("swalWithFooter", titulo, mensaje, footer, tipoMensaje.ToString());
            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al ejecutar javascript");
            }
        }

        /// <summary>
        /// Muestra una alerta Sweet Alerta de confirmación al usuario, en donde se tiene una elección SI/NO
        /// </summary>
        /// <param name="titulo">Título de la alerta</param>
        /// <param name="mensaje">Mensaje de la alerta</param>
        /// <param name="tipoMensaje">Tipo de alerta, que se verá reflejada en el ícono de ésta</param>
        public static async Task<bool> SweetAlertConfirm(this IJSRuntime jsRuntime, string titulo, string mensaje, TipoMensajeSweetAlert tipoMensaje)
        {
            try
            {
                return await jsRuntime.InvokeAsync<bool>("CustomConfirmSwal", titulo, mensaje, tipoMensaje.ToString());
            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al ejecutar javascript");
            }

            return false;
        }
        /// <summary>
        /// Muestra una alerta de confirmación al usuario, en donde se espera una acción del usuario para continuar 
        /// </summary>
        /// <param name="titulo">Título de la alerta</param>
        /// <param name="mensaje">Mensaje de la alerta</param>
        /// <param name="tipoMensaje">Tipo de alerta, que se verá reflejada en el ícono de ésta</param>
        public static async Task<bool> SweetAlertConfirmSuccess(this IJSRuntime jsRuntime, string titulo, string mensaje, TipoMensajeSweetAlert tipoMensaje)
        {
            try
            {
                return await jsRuntime.InvokeAsync<bool>("CustomConfirmSuccessSwal", titulo, mensaje, tipoMensaje.ToString());
            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al ejecutar javascript");
            }

            return false;
        }
        /// <summary>
        /// Muestra una alerta Sweet Alert de cargando
        /// </summary>
        /// <param name="titulo">Título de la alerta</param>
        /// <param name="mensaje">Mensaje de la alerta</param>
        public static async Task SweetAlertLoading(this IJSRuntime jsRuntime, string titulo, string mensaje)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("LoadingSwal", titulo, mensaje);
            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al ejecutar javascript");
            }
        }
        /// <summary>
        /// Cierra cualquier alerta Sweet Alerta en pantalla
        /// </summary>
        public static async Task SweetAlertClose(this IJSRuntime jsRuntime)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("Swal.close");
            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al ejecutar javascript");
            }
        }
        /// <summary>
        /// Realiza la descarga de un archivo al cliente
        /// </summary>
        /// <param name="ruta">Ruta absoluta del archivo que se va a descargar</param>
        /// <param name="nombre">Nombre con el que será descargado el archivo</param>
        public static async Task DescargarArchivo(this IJSRuntime jsRuntime, string ruta, string nombre)
        {
            if (!File.Exists(ruta)) throw new Exception($"La ruta especificada para el archivo para descargar {ruta} no existe");

            try
            {
                FileInfo fi = new FileInfo(ruta);
                string ruta_destino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "archivosTemporales");
                Directory.CreateDirectory(ruta_destino);

                string ruta_final = Path.Combine(ruta_destino, fi.Name);


                if (!ruta.Equals(ruta_final))
                    File.Copy(ruta, ruta_final);

                string ruta_cliente = ruta_final.Replace(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "").Replace("\\", "/");

                await jsRuntime.InvokeVoidAsync("downloadURI", ruta_cliente, nombre + fi.Extension);

                AuditoriaDescargaArchivo auditoria = new AuditoriaDescargaArchivo()
                {
                    extension_archivo = fi.Extension,
                    ip_address = await jsRuntime.InvokeAsync<string>("getIpAddress")
                    .ConfigureAwait(true),
                    nombre_archivo = nombre + fi.Extension,
                    ruta_descargada = ruta_cliente,
                    ruta_original = ruta,
                    usuario = await jsRuntime.InvokeAsync<string>("getActualUser"),
                    peso_archivo = new System.IO.FileInfo(ruta).Length.ToString()
                };

                await _auditoriaService.registrarAuditoriaDescargaArchivos(auditoria);

                if (File.Exists(ruta_final)) File.Delete(ruta_final);
            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al ejecutar javascript");
            }
        }
        /// <summary>
        /// Obtiene la dirección IP del cliente
        /// </summary>
        /// <returns>Dirección IP del cliente</returns>
        public static async Task<string> GetIpAddress(this IJSRuntime jsRuntime)
        {
            try
            {
                var ipAddress = await jsRuntime.InvokeAsync<string>("getIpAddress")
                    .ConfigureAwait(true);
                return ipAddress;
            }
            catch (Exception exe)
            {
                //If your request was blocked by CORS or some extension like uBlock Origin then you will get an exception.
                await jsRuntime.InvokeVoidAsync("console.log", $"Error al obtener IP: {exe.Message} - {exe.InnerException}");
                return string.Empty;
            }
        }
        /// <summary>
        /// Inicializa un timer de inactividad mediante javascript ayudado por una referencia <see cref="DotNetObjectReference{TValue}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dotNetObjectReference">Referencia DotNet </param>
        public static async Task InitializeInactivityTimer<T>(this IJSRuntime jsRuntime,
            DotNetObjectReference<T> dotNetObjectReference) where T : class
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("initializeInactivityTimer", dotNetObjectReference);
            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al ejecutar javascript");
            }
        }
        /// <summary>
        /// Inicializa una referencia <see cref="DotNetObjectReference{TValue}"/> para realizar un proceso de firma electrónica
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dotNetObjectReference">Referencia DotNet </param>
        public static async Task InitializeDotnetHelperFirma<T>(this IJSRuntime jsRuntime,
            DotNetObjectReference<T> dotNetObjectReference) where T : class
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("initializeDotNetHelperFirma", dotNetObjectReference);
            }
            catch (Exception exe)
            {
                Log.Error(exe, "Error al ejecutar javascript");
            }
        }
        /// <summary>
        /// Tipos de alertas disponibles para la librería <c>Sweet Alert</c>
        /// </summary>
        public enum TipoMensajeSweetAlert
        {
            question, warning, error, success, info
        }


    }
}
