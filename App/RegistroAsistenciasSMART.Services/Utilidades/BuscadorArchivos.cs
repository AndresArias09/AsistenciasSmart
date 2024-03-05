
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Services.Utilidades
{
    /// <summary>
    /// Expone diferentes maneras de realizar búsquedas de archivos
    /// </summary>
    public class BuscadorArchivos
    {
        /// <summary>
        /// Realiza la búsqueda de un archivo dentro de una ruta en específico dados unos patrones del nombre del archivo
        /// </summary>
        /// <param name="RUTA_DESTINO">Ruta en la cual se buscará el archivo</param>
        /// <param name="pattern">Patrón del nombre del archivo</param>
        /// <param name="final">Cadena de texto con la cual finaliza el nombre del archivo</param>
        /// <returns>Si se encuentran uno o varios archivos, retornará la ruta del archivo más reciente. En caso de no encontrar la ruta, retornará una cadena vacía</returns>
        public static string buscarRutaArchivo(string RUTA_DESTINO, string pattern, string final)
        {
            string ruta_archivo_final = "";

            try
            {
                var query = from p in Directory.GetFiles(RUTA_DESTINO).AsEnumerable()
                            where p.Contains(pattern) && p.EndsWith(final)
                            select p;

                string rutaArchivo = query.LastOrDefault().ToString();


                if (File.Exists(rutaArchivo))
                {
                    ruta_archivo_final = rutaArchivo;
                }
                else
                {

                }
            }
            catch (Exception exe)
            {
                Log.Error(exe, $"Error al buscar archivo en la ruta {RUTA_DESTINO} con el patrón {pattern}");
            }

            return ruta_archivo_final;
        }
    }
}
