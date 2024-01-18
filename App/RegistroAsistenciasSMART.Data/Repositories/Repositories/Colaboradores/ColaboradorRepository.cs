using Dapper;
using RegistroAsistenciasSMART.Data.Connection;
using RegistroAsistenciasSMART.Data.Repositories.Interfaces.Colaboradores;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RegistroAsistenciasSMART.Model.Models.Colaboradores;

namespace RegistroAsistenciasSMART.Data.Repositories.Repositories.Colaboradores
{
    public class ColaboradorRepository : IColaboradorRepository
    {
        private readonly string _connection_string = "";

        public ColaboradorRepository(string connection_string)
        {
            _connection_string = connection_string;
        }

        private MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connection_string);
        }

        public async Task<Colaborador> consultarColaboradorByCedula(string cedula)
        {
            var db = dbConnection();

            var sql = @"
                        SELECT cedula,
                            nombres,
                            cargo,
                            area,
                            jefe_inmediato,
                            sede,
                            correo,
                            turno,
                            estado,
                            fecha_adicion,
                            usuario_adiciono
                        FROM asistencia.colaboradores
                        where cedula = @cedula;
                        ";

            var p = new DynamicParameters();
            p.Add("@cedula", cedula);

            return await db.QueryFirstOrDefaultAsync<Colaborador>(sql,p);
        }

        public async Task<IEnumerable<Colaborador>> consultarColaboradores()
        {
            var db = dbConnection();

            var sql = @"
                        SELECT cedula,
                            nombres,
                            cargo,
                            area,
                            jefe_inmediato,
                            sede,
                            correo,
                            turno,
                            estado,
                            fecha_adicion,
                            usuario_adiciono
                        FROM asistencia.colaboradores;
                        ";

            return await db.QueryAsync<Colaborador>(sql);
        }

        public async Task<bool> insertarColaborador(Colaborador colaborador)
        {
            var db = dbConnection();

            var sql = @"INSERT INTO `asistencia`.`colaboradores`
                        (
                            `cedula`,
                            `nombres`,
                            `cargo`,
                            `area`,
                            `jefe_inmediato`,
                            `sede`,
                            `correo`,
                            `turno`,
                            `estado`,
                            `fecha_adicion`,
                            `usuario_adiciono`
                        )
                        VALUES
                        (
                            @cedula,
                            @nombres,
                            @cargo,
                            @area,
                            @jefe_inmediato,
                            @sede,
                            @correo,
                            @turno,
                            @estado,
                            CURRENT_TIMESTAMP,
                            @usuario_adiciono
                        );";

            DynamicParameters p = new DynamicParameters();
            p.Add("@cedula", colaborador.cedula);
            p.Add("@nombres", colaborador.nombres);
            p.Add("@cargo", colaborador.cargo);
            p.Add("@area", colaborador.area);
            p.Add("@jefe_inmediato", colaborador.jefe_inmediato);
            p.Add("@sede", colaborador.sede);
            p.Add("@correo", colaborador.correo);
            p.Add("@turno", colaborador.turno);
            p.Add("@estado", colaborador.estado);
            p.Add("@usuario_adiciono", colaborador.usuario_adiciono);

            var result = await db.ExecuteAsync(sql,p);

            return result > 0;
        }

        public async Task<bool> actualizarColaborador(Colaborador colaborador)
        {
            var db = dbConnection();

            var sql = @"UPDATE asistencia.colaboradores
                        SET
                            nombres = @nombres,
                            cargo = @cargo,
                            area = @area,
                            jefe_inmediato = @jefe_inmediato,
                            sede = @sede,
                            correo = @correo,
                            turno = @turno,
                            estado = @estado
                        WHERE cedula = @cedula;";

            DynamicParameters p = new DynamicParameters();
            p.Add("@cedula", colaborador.cedula);
            p.Add("@nombres", colaborador.nombres);
            p.Add("@cargo", colaborador.cargo);
            p.Add("@area", colaborador.area);
            p.Add("@jefe_inmediato", colaborador.jefe_inmediato);
            p.Add("@sede", colaborador.sede);
            p.Add("@correo", colaborador.correo);
            p.Add("@turno", colaborador.turno);
            p.Add("@estado", colaborador.estado);

            var result = await db.ExecuteAsync(sql, p);

            return result > 0;
        }

        public async Task<bool> eliminarColaborador(string cedula)
        {
            var db = dbConnection();

            var sql = @"delete from asistencia.colaboradores where cedula = @cedula";

            DynamicParameters p = new DynamicParameters();
            p.Add("@cedula", cedula);

            var result = await db.ExecuteAsync(sql, p);

            return result > 0;
        }

        public async Task<bool> insertarRegistroAsistencia(RegistroAsistencia registro)
        {
            var db = dbConnection();

            var sql = @"INSERT INTO registro 
                        (
                            fecha,
                            hora,
                            Cedula,
                            Sede,
                            Reporta,
                            Correo,
                            latitud, 
                            longitud,
                            ip_address
                        )
                        VALUES 
                        (
                            @fecha,
                            @hora,
                            @cedula,
                            @sede,
                            @reporta,
                            @email,
                            @latitud,
                            @longitud,
                            @ip_address
                        )";

            DynamicParameters p = new DynamicParameters();
            p.Add("@fecha", registro.fecha);
            p.Add("@hora", registro.hora);
            p.Add("@cedula", registro.cedula);
            p.Add("@sede", registro.sede);
            p.Add("@reporta", registro.reporta);
            p.Add("@email", registro.email);
            p.Add("@latitud", registro.latitud);
            p.Add("@longitud", registro.longitud);
            p.Add("@ip_address", registro.ip_address);

            var result = await db.ExecuteAsync(sql, p);

            return result > 0;
        }

        public async Task<IEnumerable<RegistroAsistencia>> consultarRegistrosAsistencia()
        {
            var db = dbConnection();

            var sql = @"
                        SELECT 
                            fecha,
                            hora,
                            Cedula,
                            Sede,
                            Reporta,
                            Correo as email,
                            latitud,
                            longitud,
                            fecha_SQL as fecha_adicion,
                            ip_address
                        FROM asistencia.registro
                        ";

            return await db.QueryAsync<RegistroAsistencia>(sql);
        }
    }
}
