using RegistroAsistenciasSMART.Model.Models;
using Microsoft.AspNetCore.Components.Forms;
using Serilog;

namespace RegistroAsistenciasSMART.Web.Helpers
{
    /// <summary>
    /// Realiza tareas de gestión de archivos del lado del cliente
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// Dado una lista de tipo <see cref="IReadOnlyList{IBrowserFile}"/>, cual proviene de un componente <see cref="InputFile"/> en el que un usuario puede subir uno o varios archivos, devuelve una lista de <see cref="Archivo"/> que contiene la información de todos los archivos seleccionados por el usuario. Cada uno de los archivos es guardado en una ruta temporal ubicada en el <c>wwwroot</c>
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<Archivo>> getTempFiles(IReadOnlyList<IBrowserFile> files)
        {
            List<Archivo> archivos = new List<Archivo>();

            string directorio_temp = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "archivosTemporales");

            Directory.CreateDirectory(directorio_temp);

            foreach (var file in files)
            {
                Archivo archivo = new Archivo();

                Stream stream = file.OpenReadStream(999999999);
                string ext = Path.GetExtension(file.Name);
                string nombreArchivo = Path.GetFileNameWithoutExtension(file.Name);
                string nombre_final = nombreArchivo + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "_" + Guid.NewGuid().ToString("N") + ext;
                var path = Path.Combine(directorio_temp, nombre_final);
                FileStream fs = File.Create(path);
                try
                {
                    await stream.CopyToAsync(fs);

                    archivo.ruta_absoluta = path;
                    archivo.ruta_cliente = path.Replace(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "");
                    archivo.extension = ext;
                    archivo.nombre = nombreArchivo;

                    archivos.Add(archivo);
                }
                catch (Exception exe)
                {
                    Log.Error(exe, $"Error al guardar archivo");
                }
                finally
                {
                    stream.Close();
                    fs.Close();
                }
            }

            return archivos;
        }
    }
}
