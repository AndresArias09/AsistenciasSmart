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
                        FROM colaboradores
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
                        FROM colaboradores;
                        ";

            return await db.QueryAsync<Colaborador>(sql);
        }

        public async Task<bool> insertarColaborador(Colaborador colaborador)
        {
            var db = dbConnection();

            var sql = @"INSERT INTO `colaboradores`
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

            var sql = @"UPDATE colaboradores
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

            var sql = @"delete from colaboradores where cedula = @cedula";

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

        public async Task<IEnumerable<RegistroAsistencia>> consultarRegistrosAsistencia(FiltroAsistencia filtros)
        {
            var db = dbConnection();

            var sql = @"
                        SELECT 
                        fecha,
                        cast(hora as char) as hora,
                        c.Cedula,
                        c.nombres,
                        r.sede,
                        c.area,
                        Reporta,
                        c.correo,
                        latitud,
                        longitud,
                        fecha_SQL as fecha_adicion,
                        ip_address as ip_address
                    FROM registro r
                    inner join colaboradores c on r.cedula = c.cedula
                    where c.cedula <> ''
            ";

            if(filtros.fecha_desde is not null)
            {
                sql += " and date(r.fecha_SQL) >= date(@fecha_desde)";
            }

            if (filtros.fecha_hasta is not null)
            {
                sql += " and date(r.fecha_SQL) <= date(@fecha_hasta)";
            }

            if (!string.IsNullOrEmpty(filtros.nombres))
            {
                sql += " and LOWER(c.nombres) like CONCAT('%', @nombres, '%')";
            }

            if (!string.IsNullOrEmpty(filtros.cedula))
            {
                sql += " and LOWER(c.cedula) like CONCAT('%', @cedula, '%')";
            }

            if (!string.IsNullOrEmpty(filtros.sede))
            {
                sql += " and LOWER(r.sede) like CONCAT('%', @sede, '%')";
            }

            if (!string.IsNullOrEmpty(filtros.area))
            {
                sql += " and LOWER(c.area) like CONCAT('%', @area, '%')";
            }

            if (!string.IsNullOrEmpty(filtros.reporta))
            {
                sql += " and LOWER(r.reporta) like CONCAT('%', @reporta, '%')";
            }

            if (!string.IsNullOrEmpty(filtros.jefe_inmediato))
            {
                sql += " and LOWER(c.jefe_inmediato) like CONCAT('%', @jefe, '%')";
            }

            if (!string.IsNullOrEmpty(filtros.cargo))
            {
                sql += " and LOWER(c.cargo) like CONCAT('%', @cargo, '%')";
            }

            if (!string.IsNullOrEmpty(filtros.correo))
            {
                sql += " and LOWER(c.correo) like CONCAT('%', @correo, '%')";
            }

            sql += " order by fecha_SQL desc";

            var p = new DynamicParameters();
            p.Add("@fecha_desde",filtros.fecha_desde.GetValueOrDefault().ToString("yyyy-MM-dd"));
            p.Add("@fecha_hasta", filtros.fecha_hasta.GetValueOrDefault().ToString("yyyy-MM-dd"));
            p.Add("@nombres",filtros.nombres.ToLower());
            p.Add("@cedula",filtros.cedula.ToLower());
            p.Add("@sede",filtros.sede.ToLower());
            p.Add("@area",filtros.area.ToLower());
            p.Add("@reporta",filtros.reporta.ToLower());
            p.Add("@jefe",filtros.jefe_inmediato.ToLower());
            p.Add("@cargo",filtros.cargo.ToLower());
            p.Add("@correo",filtros.correo.ToLower());

            return await db.QueryAsync<RegistroAsistencia>(sql,p);
        }
    }
}
