using RegistroAsistenciasSMART.Model.Models;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Components.Forms;
using Serilog;

namespace RegistroAsistenciasSMART.Web.Helpers
{
    public class FileHelper
    {
        public static async Task<IEnumerable<Archivo>> getTempFiles(IReadOnlyList<IBrowserFile> files)
        {
            List<Archivo> archivos = new List<Archivo>();

            string directorio_temp = Directory.GetCurrentDirectory() + "\\wwwroot\\archivosTemporales";

            if (!Directory.Exists(directorio_temp))
            {
                Directory.CreateDirectory(directorio_temp);
            }

            foreach (var file in files)
            {
                Archivo archivo = new Archivo();

                Stream stream = file.OpenReadStream(999999999);
                string ext = Path.GetExtension(file.Name);
                string nombreArchivo = Path.GetFileNameWithoutExtension(file.Name);
                string nombre_final = nombreArchivo + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "_" + Guid.NewGuid().ToString("N") + ext;
                var path = directorio_temp + $"\\{nombre_final}";
                FileStream fs = File.Create(path);
                try
                {
                    await stream.CopyToAsync(fs);

                    archivo.ruta_absoluta = path;
                    archivo.ruta_cliente = path.Replace(Directory.GetCurrentDirectory() + "\\wwwroot", "");
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
