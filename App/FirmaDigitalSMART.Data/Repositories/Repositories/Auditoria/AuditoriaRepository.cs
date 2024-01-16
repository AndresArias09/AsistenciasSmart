using FirmaDigitalSMART.Data.Repositories.Interfaces.Auditoria;
using FirmaDigitalSMART.Model.Models.Configuracion.Perfilamiento;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirmaDigitalSMART.Model.Models.Auditoria;
using Npgsql;
using System.Data;

namespace FirmaDigitalSMART.Data.Repositories.Repositories.Auditoria
{
    public class AuditoriaRepository : IAuditoriaRepository
    {
        private string ConnectionString;

        public AuditoriaRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected NpgsqlConnection dbConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        public async Task<bool> registrarAuditoriaEnvioEmail(EmailInfo emailInfo)
        {

            var db = dbConnection();

            var sql = @"select aud.insertar_envio_email
                    (
                        @p_email_destinatario,
                        @p_email_emisor,
                        @p_email_cc,
                        @p_email_bcc,
                        @p_asunto,
                        @p_mensaje,
                        @p_enviado,
                        @p_descripcion_error,
                        @p_usuario,
                        @p_numero_identificacion_tercero,
                        @p_pantalla,
                        @p_descripcion
                    )";

            var p = new DynamicParameters();
            p.Add("@p_email_destinatario", string.Join(", ",emailInfo.destinatarios));
            p.Add("@p_email_cc", string.Join(", ",emailInfo.cc));
            p.Add("@p_email_bcc", string.Join(", ",emailInfo.bcc));
            p.Add("@p_asunto", emailInfo.asunto);
            p.Add("@p_email_emisor", emailInfo.email_emisor);
            p.Add("@p_mensaje", emailInfo.mensaje);
            p.Add("@p_enviado", emailInfo.enviado);
            p.Add("@p_descripcion_error", emailInfo.descripcion_error);
            p.Add("@p_usuario", emailInfo.usuario);
            p.Add("@p_numero_identificacion_tercero", emailInfo.numero_identificacion_proceso);
            p.Add("@p_pantalla", emailInfo.pantalla);
            p.Add("@p_descripcion", emailInfo.descripcion);


            var result = await db.ExecuteScalarAsync(sql.ToString(), p);

            return long.Parse(result.ToString()) > 0;
        }


        public async Task<bool> registrarAuditoriaNavegacion(AuditoriaNavegacion auditoriaNavegacion)
        {
            var result = 0;
            var db = dbConnection();

            var sql = @"aud.insertar_auditoria_navegacion";

            var p = new DynamicParameters();
            p.Add("@p_useragent", auditoriaNavegacion.UserAgent.ToString());
            p.Add("@p_navegador", auditoriaNavegacion.Navegador.ToString());
            p.Add("@p_versionnavegador", auditoriaNavegacion.VersionNavegador.ToString());
            p.Add("@p_plataformanavegador", auditoriaNavegacion.PlataformaNavegador.ToString());
            p.Add("@p_urlactual", auditoriaNavegacion.UrlActual.ToString());
            p.Add("@p_idioma", auditoriaNavegacion.Idioma.ToString());
            p.Add("@p_cookieshabilitadas", auditoriaNavegacion.CookiesHabilitadas.ToString());
            p.Add("@p_anchopantalla", auditoriaNavegacion.AnchoPantalla.ToString());
            p.Add("@p_altopantalla", auditoriaNavegacion.AltoPantalla.ToString());
            p.Add("@p_profundidadcolor", auditoriaNavegacion.ProfundidadColor.ToString());
            p.Add("@p_nombreso", auditoriaNavegacion.NombreSO.ToString());
            p.Add("@p_versionso", auditoriaNavegacion.VersionSO.ToString());
            p.Add("@p_ip_address", auditoriaNavegacion.ip_address.ToString());
            p.Add("@p_usuario_accion", auditoriaNavegacion.usuario_accion.ToString());

            result = await db.ExecuteAsync(sql.ToString(), p, commandType: System.Data.CommandType.StoredProcedure);

            return result > 0;
        }

        public async Task<bool> registrarAuditoriaDescargaArchivo(AuditoriaDescargaArchivo auditoriaDescargaArchivo)
        {
            var result = 0;
            var db = dbConnection();

            var sql = @"aud.insertar_auditoria_descarga_archivos";

            var p = new DynamicParameters();
            p.Add("@p_ruta_original", auditoriaDescargaArchivo.ruta_original);
            p.Add("@p_ruta_descargada", auditoriaDescargaArchivo.ruta_descargada);
            p.Add("@p_nombre_archivo", auditoriaDescargaArchivo.nombre_archivo);
            p.Add("@p_extension_archivo", auditoriaDescargaArchivo.extension_archivo);
            p.Add("@p_peso_archivo", auditoriaDescargaArchivo.peso_archivo);
            p.Add("@p_usuario", auditoriaDescargaArchivo.usuario);
            p.Add("@p_ip_accion", auditoriaDescargaArchivo.ip_address);

            result = await db.ExecuteAsync(sql.ToString(), p, commandType: System.Data.CommandType.StoredProcedure);

            return result > 0;
        }

        public async Task<bool> registrarAuditoriaFirma(AuditoriaTransaccionalFirma auditoria)
        {
            var result = 0;
            var db = dbConnection();

            var sql = @"aud.insertar_auditoria_transaccional";

            var p = new DynamicParameters();
            p.Add("@p_accion", auditoria.accion);
            p.Add("@p_descripcion", auditoria.descripcion);
            p.Add("@p_pantalla", auditoria.pantalla);
            p.Add("@p_ip_accion", auditoria.ip_accion);
            p.Add("@p_usuario_accion", auditoria.usuario_accion);
            p.Add("@p_id_firma", long.Parse(auditoria.id_firma));
            

            result = await db.ExecuteAsync(sql.ToString(), p, commandType: System.Data.CommandType.StoredProcedure);

            return result > 0;
        }

        public async Task<bool> registrarAuditoriaComunicacion(AuditoriaTransaccionalComunicacion auditoria)
        {
            var result = 0;
            var db = dbConnection();

            var sql = @"aud.insertar_auditoria_transaccional_comunicaciones";

            var p = new DynamicParameters();
            p.Add("@p_accion", auditoria.accion);
            p.Add("@p_descripcion", auditoria.descripcion);
            p.Add("@p_pantalla", auditoria.pantalla);
            p.Add("@p_ip_accion", auditoria.ip_accion);
            p.Add("@p_usuario_accion", auditoria.usuario_accion);
            p.Add("@p_id_comunicacion", long.Parse(auditoria.id_comunicacion));


            result = await db.ExecuteAsync(sql.ToString(), p, commandType: System.Data.CommandType.StoredProcedure);

            return result > 0;
        }

        public async Task<IEnumerable<AuditoriaTransaccionalFirma>> consultarAuditoriaFirma(long id_firma)
		{
			var db = dbConnection();

            var sql = @"SELECT 
                        id_auditoria,
                        id_firma,
                        accion,
                        descripcion,
                        pantalla,
                        ip_accion,
                        usuario_accion,
                        fecha_adicion
                        FROM aud.auditoria_transaccional
                        where id_firma = @id
                        order by fecha_adicion
                        ";

            var p = new DynamicParameters();
            p.Add("@id", id_firma);

            return await db.QueryAsync<AuditoriaTransaccionalFirma>(sql, p);
		}

        public async Task<IEnumerable<AuditoriaTransaccionalComunicacion>> consultarAuditoriaComunicacion(long id_comunicacion)
        {
            var db = dbConnection();

            var sql = @"SELECT 
                        id_auditoria,
                        id_comunicacion,
                        accion,
                        descripcion,
                        pantalla,
                        ip_accion,
                        usuario_accion,
                        fecha_adicion
                        FROM aud.auditoria_transaccional_comunicaciones
                        where id_comunicacion = @id
                        order by fecha_adicion
                        ";

            var p = new DynamicParameters();
            p.Add("@id", id_comunicacion);

            return await db.QueryAsync<AuditoriaTransaccionalComunicacion>(sql, p);
        }
    }
}
