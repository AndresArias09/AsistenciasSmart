using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Services.Interfaces.Utilidades
{
    /// <summary>
    /// Representa la interfaz mediante la cual se expondrán diferentes operaciones de encriptación
    /// </summary>
    public interface IEncryptService
    {
        /// <summary>
        /// Realiza la encriptación de un documento PDF mediante contraseña
        /// </summary>
        /// <param name="ruta_archivo_origen">Ruta del archivo que se desea encryptar</param>
        /// <param name="ruta_archivo_final">Ruta en la cual quedará guardado el documento encriptado</param>
        /// <param name="contraseña">Contraseña que se le pondrá al documento</param>
        /// <returns><see langword="true" /> si el proceso se realizó correctamente, <see langword="false" /> en caso contrario</returns>
        public bool encriptarPDF(string ruta_archivo_origen, string ruta_archivo_final, string contraseña);
        /// <summary>
        /// Convierte un objeto diccionario de tipo <see cref="Dictionary{string, string}"/> en un string base64
        /// </summary>
        /// <param name="parametros">Diccionario el cual contiene los distintos parámetros que serán encryptados</param>
        /// <returns>Un string base64</returns>
        public string encriptarParametros(Dictionary<string, string> parametros);
        /// <summary>
        /// Convierte un string base64 previamente encriptado en un objeto diccionario de tipo <see cref="Dictionary{string, string}"/>
        /// </summary>
        /// <param name="parametrosEncriptadosBase64">String base64</param>
        /// <returns>Objeto <see cref="Dictionary{string, string}"/></returns>
        public Dictionary<string, string> desencriptarParametros(string parametrosEncriptadosBase64);
        /// <summary>
        /// Encripta un string mediante un par de claves RSA
        /// </summary>
        /// <param name="cadena">Cadena de texto que será encriptada</param>
        /// <returns>Cadena encryptada</returns>
        public string encrypCadenaRSA(string cadena);
        /// <summary>
        /// Desencripta un string previamente encriptado mediante un par de claves RSA
        /// </summary>
        /// <param name="cadena">Cadena encryptada</param>
        /// <returns>Cadena desencryptada</returns>
        public string desEncrypCadenaRSA(string cadena_encriptada);
    }
}
