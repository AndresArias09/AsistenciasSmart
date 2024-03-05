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
    /// Implementación de la interfaz <see cref="IRolRepository"/>
    ///  utilizando el motor de bases de datos <c>PostgreSQL</c>
    /// </summary>
    public class RolRepository : PostgreSQLRepository, IRolRepository
    {

        public RolRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public async Task<Rol> getRol(long id_rol)
        {
            var db = dbConnection();

            var sql = @"SELECT 
                        id_rol, 
                        nombre_rol,
                        fecha_adicion,
                        usuario_agrego
                        FROM seg.rol
                        WHERE id_rol = @ID";

            var p = new DynamicParameters();
            p.Add("@ID", id_rol);

            return await db.QueryFirstOrDefaultAsync<Rol>(sql.ToString(), p);
        }

        public async Task<IEnumerable<Rol>> getRoles()
        {
            var db = dbConnection();

            var sql = @"SELECT 
                        id_rol, 
                        nombre_rol,
                        fecha_adicion,
                        usuario_agrego
                        FROM seg.rol";

            return await db.QueryAsync<Rol>(sql.ToString());
        }

        public IEnumerable<Rol> getRolesSync()
        {
            var db = dbConnection();

            var sql = @"SELECT 
                        id_rol, 
                        nombre_rol,
                        fecha_adicion,
                        usuario_agrego
                        FROM seg.rol";

            return db.Query<Rol>(sql.ToString());
        }

        public async Task<long> insertarRol(Rol rol, string usuario)
        {
            var db = dbConnection();

            var sql = @"select seg.insertar_rol
                        (
	                        @p_nombre_rol,
	                        @p_usuario_agrego
                        );";

            var p = new DynamicParameters();
            p.Add("@p_nombre_rol", rol.nombre_rol);
            p.Add("@p_usuario_agrego", usuario);

            var result = await db.ExecuteScalarAsync(sql.ToString(), p);

            return long.Parse(result.ToString());
        }

        public async Task<bool> actualizarRol(Rol rol)
        {
            var db = dbConnection();

            var sql = @"UPDATE seg.rol
	                    SET nombre_rol=@p_nombre_rol
	                    WHERE id_rol = @p_id_rol";

            var p = new DynamicParameters();
            p.Add("@p_nombre_rol", rol.nombre_rol);
            p.Add("@p_id_rol", rol.id_rol);

            var result = await db.ExecuteAsync(sql.ToString(), p);

            return result > 0;
        }
    }
}
