using Dapper;
using Npgsql;
using RegistroAsistenciasSMART.Data.Connection;
using RegistroAsistenciasSMART.Data.Repositories.Interfaces.Configuracion.Perfilamiento;
using RegistroAsistenciasSMART.Model.Models.Configuracion.Perfilamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Data.Repositories.Repositories.Configuracion.Perfilamiento
{
    /// <summary>
    /// Implementación de la interfaz <see cref="IModuloRepository"/>
    ///  utilizando el motor de bases de datos <c>PostgreSQL</c>
    /// </summary>
    public class ModuloRepository : PostgreSQLRepository, IModuloRepository
    {
        public ModuloRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public async Task<IEnumerable<Modulo>> getModulos()
        {
            var db = dbConnection();

            var sql = @"SELECT 
                        id_modulo,
                        nombre_modulo,
                        nivel,
                        fecha_adicion
                        FROM seg.modulo
                      order by nivel";

            var p = new DynamicParameters();

            return await db.QueryAsync<Modulo>(sql.ToString());
        }

        public async Task<IEnumerable<Modulo>> getModulosRol(long id_rol)
        {
            var db = dbConnection();

            var sql = @"SELECT 
                        id_modulo,
                        nombre_modulo,
                        nivel,
                        fecha_adicion
                        FROM seg.modulo
                        where
                        id_modulo
                        in
                        (
	                        SELECT id_modulo
	                        FROM seg.rol_modulo
	                        where id_rol = @ID
                        )
                        order by nivel";

            var p = new DynamicParameters();
            p.Add("@ID", id_rol);

            return await db.QueryAsync<Modulo>(sql.ToString(), p);
        }

        public IEnumerable<Modulo> getModulosRolSync(long id_rol)
        {
            var db = dbConnection();

            var sql = @"SELECT 
                        id_modulo,
                        nombre_modulo,
                        nivel,
                        fecha_adicion
                        FROM seg.modulo
                        where id_modulo
                        in
                        (
	                        SELECT id_modulo
	                        FROM seg.rol_modulo
	                        where id_rol = @ID
                        )
                        order by nivel";

            var p = new DynamicParameters();
            p.Add("@ID", id_rol);

            return db.Query<Modulo>(sql.ToString(), p);
        }

        public IEnumerable<Modulo> getModulosSync()
        {
            var db = dbConnection();

            var sql = @"SELECT 
                        id_modulo,
                        nombre_modulo,
                        nivel,
                        fecha_adicion
                        FROM seg.modulo
                      order by nivel";

            var p = new DynamicParameters();

            return db.Query<Modulo>(sql.ToString());
        }

        public async Task<bool> insertarAsignacionRolModulo(long id_rol, long id_modulo)
        {
            var db = dbConnection();

            var sql = @"SELECT seg.insertar_asignacion_rol_modulo
                        (
                            @p_id_rol,
                            @p_id_modulo
                        );";

            var p = new DynamicParameters();
            p.Add("@p_id_rol", id_rol);
            p.Add("@p_id_modulo", id_modulo);

            var result = await db.ExecuteScalarAsync(sql, p);

            return long.Parse(result.ToString()) > 0;
        }

        public async Task<bool> limpiarAsignacionRolModulo(long id_rol)
        {
            var db = dbConnection();

            var sql = @"delete from seg.rol_modulo where id_rol = @p_id_rol";

            var p = new DynamicParameters();
            p.Add("@p_id_rol", id_rol);

            var result = await db.ExecuteAsync(sql, p);

            return result > 0;
        }
    }
}
