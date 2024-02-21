using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Data.Connection
{
    public class PostgreSQLRepository
    {
        protected private string ConnectionString;

        /// <summary>
        /// Crea un objeto <see cref="NpgsqlConnection"/> con el cual se gestionarán las operaciones de base de datos con el motor <c>PostgreSQL</c>
        /// </summary>
        /// <returns>Objeto <see cref="NpgsqlConnection"/></returns>
        protected NpgsqlConnection dbConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }
    }
}
