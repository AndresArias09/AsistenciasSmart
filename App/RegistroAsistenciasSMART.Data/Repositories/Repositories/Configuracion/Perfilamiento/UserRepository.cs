using Dapper;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using RegistroAsistenciasSMART.Data.Connection;
using RegistroAsistenciasSMART.Data.Repositories.Interfaces.Configuracion.Perfilamiento;
using RegistroAsistenciasSMART.Model.Models.Configuracion.Perfilamiento;


namespace RegistroAsistenciasSMART.Data.Repositories.Repositories.Configuracion.Perfilamiento
{
    /// <summary>
    /// Implementación de la interfaz <see cref="IUserRepository"/>
    /// </summary>
    public class UserRepository : PostgreSQLRepository, IUserRepository
    {
        public UserRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public async Task<string> registrarAuditoriaLogin(string usuario, string descripcion, string ipAddress)
        {
            var db = dbConnection();

            var sql = @"select aud.insertar_auditoria_login_usuario(@p_usuario,@p_descripcion,@p_ip_accion)";

            var p = new DynamicParameters();
            p.Add("@p_usuario", usuario);
            p.Add("@p_descripcion", descripcion);
            p.Add("@p_ip_accion", ipAddress);

            var result = await db.ExecuteScalarAsync(sql.ToString(), p);

            return result.ToString();
        }

        public async Task<long> insertarUsuario(Usuario usuario)
        {
            var db = dbConnection();

            var sql = @"select seg.insertar_info_usuario
                        (
                            @p_nombres,
                            @p_apellidos,
                            @p_email,
                            @p_usuario,
                            @p_id_rol,
                            @p_area,
                            @p_usuario_adiciono,
                            @p_cargo
                        )";

            var p = new DynamicParameters();


            p.Add("@p_nombres", usuario.nombres);
            p.Add("@p_apellidos", usuario.apellidos);
            p.Add("@p_email", usuario.email);
            p.Add("@p_usuario", usuario.usuario);
            p.Add("@p_id_rol", usuario.id_rol);
            p.Add("@p_area", usuario.area);
            p.Add("@p_usuario_adiciono", usuario.usuario_adiciono);
            p.Add("@p_cargo", usuario.cargo);

            var result = await db.ExecuteScalarAsync(sql.ToString(), p);
            return long.Parse(result.ToString());
        }

        public async Task<Usuario> getUsuarioByUser(string usuario)
        {
            var db = dbConnection();

            var sql = @"SELECT 
                        id_usuario,
                        nombres,
                        apellidos,
                        email,
                        usuario,
                        fecha_adicion,
                        estado,
                        id_rol,
                        area,
                        usuario_adiciono,
                        cargo
	                    FROM seg.usuario WHERE usuario = @p_usuario";

            var p = new DynamicParameters();
            p.Add("@p_usuario", usuario);

            return await db.QueryFirstOrDefaultAsync<Usuario>(sql.ToString(), p);
        }

        public async Task<Usuario> getUsuarioById(string id_usuario)
        {
            var db = dbConnection();

            var sql = @"SELECT 
                        id_usuario,
                        nombres,
                        apellidos,
                        email,
                        usuario,
                        fecha_adicion,
                        estado,
                        id_rol,
                        area,
                        usuario_adiciono,
                        cargo
	                    FROM seg.usuario WHERE id_usuario = @p_usuario";

            var p = new DynamicParameters();
            p.Add("@p_usuario", id_usuario);

            return await db.QueryFirstOrDefaultAsync<Usuario>(sql.ToString(), p);
        }


        public async Task<bool> registrarAuditoriaCierreSesion(string id_auditoria, string usuario, string ip, string motivo)
        {
            var result = 0;

            var db = dbConnection();

            var sql = @"seg.registrar_info_cierre_session";

            var p = new DynamicParameters();
            p.Add("@p_id_auditoria", int.Parse(id_auditoria));
            p.Add("@p_usuario_cierre_session", usuario);
            p.Add("@p_ip_cierre_session", ip);
            p.Add("@p_motivo_cierre_session", motivo);

            result = await db.ExecuteAsync(sql.ToString(), p, commandType: System.Data.CommandType.StoredProcedure);

            return result > 0;
        }

        public async Task<bool> actualizarUsuario(Usuario usuario)
        {
            var db = dbConnection();

            var sql = @"UPDATE 
                        seg.usuario
                        SET 
                        nombres=@p_nombres,
                        apellidos=@p_apellidos,
                        email=@p_email,
                        estado=@p_estado,
                        id_rol=@p_id_rol,
                        area=@p_area,
                        cargo=@p_cargo
                        WHERE usuario=@p_usuario";

            var p = new DynamicParameters();

            p.Add("@p_nombres", usuario.nombres);
            p.Add("@p_apellidos", usuario.apellidos);
            p.Add("@p_email", usuario.email);
            p.Add("@p_usuario", usuario.usuario);
            p.Add("@p_estado", usuario.estado);
            p.Add("@p_id_rol", usuario.id_rol);
            p.Add("@p_id_usuario", usuario.id_usuario);
            p.Add("@p_area", usuario.area);
            p.Add("@p_cargo", usuario.cargo);

            var result = await db.ExecuteAsync(sql.ToString(), p);
            return result > 0;
        }

        public async Task<IEnumerable<Usuario>> getUsuarios()
        {
            var db = dbConnection();

            var sql = @"SELECT 
                        id_usuario,
                        nombres,
                        apellidos,
                        email,
                        usuario,
                        fecha_adicion,
                        estado,
                        id_rol,
                        area,
                        usuario_adiciono,
                        cargo
	                    FROM seg.usuario";

            return await db.QueryAsync<Usuario>(sql.ToString());
        }
    }

}
