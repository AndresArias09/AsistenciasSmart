using RegistroAsistenciasSMART.Data.Repositories.Interfaces.OTP;
using RegistroAsistenciasSMART.Model.Models.OTP;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace RegistroAsistenciasSMART.Data.Repositories.Repositories.OTP
{
    public class OTPRepository : IOTPRepository
    {
        private string ConnectionString;

        public OTPRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected NpgsqlConnection dbConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        public async Task<bool> completarEstadoOTP(string id_otp_code, string estado)
        {
            var db = dbConnection();

            var result = 0;

            var sql = @"seg.actualizar_estado_otp";

            var p = new DynamicParameters();

            p.Add("@p_id_otp", Int32.Parse(id_otp_code));
            p.Add("@p_estado", estado);

            result = await db.ExecuteAsync(sql.ToString(), p, commandType: System.Data.CommandType.StoredProcedure);

            return result > 0;
        }

        public async Task<OTPModel> getUltimoOTPProceso(string numero_documento, string tipo_proceso)
        {
            var db = dbConnection();

            var sql = @"
                    SELECT 
                    id_registro_otp as id_otp_code,
                    codigo_otp as otp_code,
                    fecha_adicion,
                    fecha_validacion,
                    estado,
                    numero_identificacion_proceso as numero_documento_proceso,
                    tipo_proceso,
                    descripcion,
                    metodos_envio
                    FROM seg.registro_otp WHERE numero_identificacion_proceso = @NUMDOC
                    AND tipo_proceso = @TIPO_PROCESO
                    ORDER BY fecha_adicion DESC
                    limit 1
                ";

            var p = new DynamicParameters();
            p.Add("@NUMDOC", numero_documento);
            p.Add("@TIPO_PROCESO", tipo_proceso);

            return await db.QueryFirstOrDefaultAsync<OTPModel>(sql.ToString(), p);
        }

        public async Task<bool> insertarOTP(OTPModel otp_code)
        {
            var db = dbConnection();

            var result = 0;

            var sql = @"seg.insertar_registro_otp";

            var p = new DynamicParameters();

            p.Add("@p_codigo_otp", otp_code.otp_code);
            p.Add("@p_estado", otp_code.estado);
            p.Add("@p_numero_identificacion_proceso", otp_code.numero_documento_proceso);
            p.Add("@p_tipo_proceso", otp_code.tipo_proceso);
            p.Add("@p_descripcion", otp_code.descripcion);
            p.Add("@p_metodos_envio", otp_code.metodos_envio);

            result = await db.ExecuteAsync(sql.ToString(), p, commandType: System.Data.CommandType.StoredProcedure);

            return result > 0;
        }
    }
}
