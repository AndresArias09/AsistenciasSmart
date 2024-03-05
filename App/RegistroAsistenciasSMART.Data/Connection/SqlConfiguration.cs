using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Data.Connection
{
    /// <summary>
    /// Representa una configuración para la conexión a un motor de base de datos SQL
    /// </summary>
    public class SqlConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString">Cadena de conexión</param>
        public SqlConfiguration(string connectionString) => ConnectionString = connectionString;
        /// <summary>
        /// Cadena de conexión al motor de base de datos SQL
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
