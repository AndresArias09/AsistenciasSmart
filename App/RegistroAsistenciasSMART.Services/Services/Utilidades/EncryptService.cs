using Encriptacion;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using RegistroAsistenciasSMART.Services.Interfaces.Utilidades;
using RegistroAsistenciasSMART.Services.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace RegistroAsistenciasSMART.Services.Services.Utilidades
{
    /// <summary>
    /// Implementación de la interfaz <see cref="IEncryptService"/>
    /// </summary>
    public class EncryptService : IEncryptService
    {
        private EncriptadorUtility eu;

        public EncryptService()
        {
            eu = new EncriptadorUtility();
        }


        public string encriptarParametros(Dictionary<string, string> parametros)
        {
            return eu.encriptarParametros(parametros);
        }

        public Dictionary<string, string> desencriptarParametros(string parametrosEncriptadosBase64)
        {
            return eu.desencriptarParametros(parametrosEncriptadosBase64);
        }

        public string encrypCadenaRSA(string cadena)
        {
            return encrypCadenaRSA(cadena);
        }

        public string desEncrypCadenaRSA(string cadena_encriptada)
        {
            return desEncrypCadenaRSA(cadena_encriptada);
        }

        public bool encriptarPDF(string ruta_archivo_origen, string ruta_archivo_final, string contraseña)
        {
            //using (var input = new FileStream(ruta_archivo_origen, FileMode.Open, FileAccess.Read, FileShare.Read))
            //using (var output = new FileStream(ruta_archivo_final, FileMode.Create, FileAccess.Write, FileShare.None))
            //{
            //    var reader = new PdfReader(input);
            //    PdfEncryptor.Encrypt(reader, output, true, contraseña, contraseña, PdfWriter.ALLOW_PRINTING);
            //}

            //if (File.Exists(ruta_archivo_final)) return true;

            return false;
        }


    }
}
