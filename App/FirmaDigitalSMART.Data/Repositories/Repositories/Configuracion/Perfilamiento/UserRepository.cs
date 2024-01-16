using FirmaDigitalSMART.Data.Repositories.Interfaces.Configuracion.Perfilamiento;
using FirmaDigitalSMART.Model.Models.Configuracion.Perfilamiento;
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

namespace FirmaDigitalSMART.Data.Repositories.Repositories.Configuracion.Perfilamiento
{
    public class UserRepository : IUserRepository
    {
        private string ConnectionString;

        public UserRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected NpgsqlConnection dbConnection()
        {
            return new NpgsqlConnection(ConnectionString);
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

        public async Task<bool> insertarUsuario(Usuario usuario)
        {
            var db = dbConnection();

            var sql = @"seg.insertar_info_usuario";

            var p = new DynamicParameters();


            p.Add("@p_nombres", usuario.nombres);
            p.Add("@p_apellidos", usuario.apellidos);
            p.Add("@p_email", usuario.email);
            p.Add("@p_usuario", usuario.usuario);


            var result = await db.ExecuteAsync(sql.ToString(), p, commandType: System.Data.CommandType.StoredProcedure);
            return result > 0;
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
                        estado
	                    FROM seg.usuario WHERE USUARIO = @USER";

            var p = new DynamicParameters();
            p.Add("@USER", usuario);

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
                        estado
	                    FROM seg.usuario WHERE ID_USUARIO = @USER";

            var p = new DynamicParameters();
            p.Add("@USER", id_usuario);

            return await db.QueryFirstOrDefaultAsync<Usuario>(sql.ToString(), p);
        }


        public async Task<bool> registrarAuditoriaCierreSesion(string id_auditoria, string usuario, string ip, string motivo)
        {
            var result = 0;

            var db = dbConnection();

            var sql = @"seg.registrar_info_cierre_session";

            var p = new DynamicParameters();
            p.Add("@p_id_auditoria", Int32.Parse(id_auditoria));
            p.Add("@p_usuario_cierre_session", usuario);
            p.Add("@p_ip_cierre_session", ip);
            p.Add("@p_motivo_cierre_session", motivo);

            result = await db.ExecuteAsync(sql.ToString(), p, commandType: System.Data.CommandType.StoredProcedure);

            return result > 0;
        }

    }

}
