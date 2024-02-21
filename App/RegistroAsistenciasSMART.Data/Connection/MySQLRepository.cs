using MySqlConnector;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Data.Connection
{
    public class MySQLRepository
    {
        protected private string ConnectionString;

        /// <summary>
        /// Crea un objeto <see cref="MySqlConnection"/> con el cual se gestionarán las operaciones de base de datos con el motor <c>MySQL</c>
        /// </summary>
        /// <returns>Objeto <see cref="MySqlConnection"/></returns>
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}
