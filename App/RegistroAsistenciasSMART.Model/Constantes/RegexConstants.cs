using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.Constantes
{
    /// <summary>
    /// Contiene regex de distintos tipos
    /// </summary>
    public static class RegexConstants
    {
        /// <summary>
        /// Regex para validación de direcciones de correo eletrónico
        /// </summary>
        public const string EMAIL_REGEX = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
        /// <summary>
        /// Regex para validación de números únicamente
        /// </summary>
        public const string NUMBERS_ONLY_REGEX = @"^[0-9]*$";
    }
}
