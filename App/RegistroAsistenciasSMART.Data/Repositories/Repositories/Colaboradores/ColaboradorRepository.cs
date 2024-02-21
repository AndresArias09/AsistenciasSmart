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
    public class ColaboradorRepository : PostgreSQLRepository, IColaboradorRepository
    {
        public ColaboradorRepository(string connection_string)
        {
            ConnectionString = connection_string;
        }

        public async Task<Colaborador> consultarColaboradorByCedula(long cedula)
        {
            var db = dbConnection();

            var sql = @"SELECT 
                            cedula,
                            nombres,
                            apellidos,
                            cargo,
                            area,
                            jefe_inmediato,
                            sede,
                            correo,
                            turno,
                            estado,
                            fecha_adicion,
                            usuario_adiciono
                            FROM asistencia.colaborador
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
                        SELECT 
                            cedula,
                            nombres,
                            apellidos,
                            cargo,
                            area,
                            jefe_inmediato,
                            sede,
                            correo,
                            turno,
                            estado,
                            fecha_adicion,
                            usuario_adiciono
                            FROM asistencia.colaborador
                        ";

            return await db.QueryAsync<Colaborador>(sql);
        }

        public async Task<bool> insertarColaborador(Colaborador colaborador)
        {
            var db = dbConnection();

            var sql = @"
                        INSERT INTO 
                        asistencia.colaborador
                        (
                            cedula,
                            nombres,
                            apellidos,
                            cargo,
                            area,
                            jefe_inmediato,
                            sede,
                            correo,
                            turno,
                            estado,
                            fecha_adicion,
                            usuario_adiciono
                        )
                        VALUES
                        (
                            @p_cedula,
                            @p_nombres,
                            @p_apellidos,
                            @p_cargo,
                            @p_area,
                            @p_jefe_inmediato,
                            @p_sede,
                            @p_correo,
                            @p_turno,
                            @p_estado,
                            CURRENT_TIMESTAMP,
                            @p_usuario_adiciono
                        );";

            DynamicParameters p = new DynamicParameters();
            p.Add("@p_cedula", colaborador.cedula);
            p.Add("@p_nombres", colaborador.nombres);
            p.Add("@p_apellidos", colaborador.apellidos);
            p.Add("@p_cargo", colaborador.cargo);
            p.Add("@p_area", colaborador.area);
            p.Add("@p_jefe_inmediato", colaborador.jefe_inmediato);
            p.Add("@p_sede", colaborador.sede);
            p.Add("@p_correo", colaborador.correo);
            p.Add("@p_turno", colaborador.turno);
            p.Add("@p_estado", colaborador.estado);
            p.Add("@p_usuario_adiciono", colaborador.usuario_adiciono);

            var result = await db.ExecuteAsync(sql,p);

            return result > 0;
        }

        public async Task<bool> actualizarColaborador(Colaborador colaborador)
        {
            var db = dbConnection();

            var sql = @"UPDATE 
                        asistencia.colaborador
                        SET 
                        nombres=@p_nombres, 
                        apellidos=@p_apellidos,
                        cargo=@p_cargo, 
                        area=@p_area,
                        jefe_inmediato=@p_jefe_inmediato,
                        sede=@p_sede,
                        correo=@p_correo,
                        turno=@p_turno, 
                        estado=@p_estado,
                        WHERE cedula = @cedula";

            DynamicParameters p = new DynamicParameters();
            p.Add("@p_cedula", colaborador.cedula);
            p.Add("@p_nombres", colaborador.nombres);
            p.Add("@p_apellidos", colaborador.apellidos);
            p.Add("@p_cargo", colaborador.cargo);
            p.Add("@p_area", colaborador.area);
            p.Add("@p_jefe_inmediato", colaborador.jefe_inmediato);
            p.Add("@p_sede", colaborador.sede);
            p.Add("@p_correo", colaborador.correo);
            p.Add("@p_turno", colaborador.turno);
            p.Add("@p_estado", colaborador.estado);

            var result = await db.ExecuteAsync(sql, p);

            return result > 0;
        }

        public async Task<bool> eliminarColaborador(long cedula)
        {
            var db = dbConnection();

            var sql = @"delete from asistencia.colaborador where cedula = @cedula";

            DynamicParameters p = new DynamicParameters();
            p.Add("@cedula", cedula);

            var result = await db.ExecuteAsync(sql, p);

            return result > 0;
        }

        public async Task<bool> insertarRegistroAsistencia(RegistroAsistencia registro)
        {
            var db = dbConnection();

            var sql = @"INSERT INTO 
                        asistencia.registro_asistencia
                        (
                            fecha_adicion,
                            cedula_colaborador,
                            sede,
                            tipo_reporte,
                            latitud,
                            lontitud,
                            ip_address
                        )
                        VALUES
                        (
                            CURRENT_TIMESTAMP,
                            @p_cedula_colaborador,
                            @p_sede,
                            @p_tipo_reporte,
                            @p_latitud,
                            @p_lontitud,
                            @p_ip_address
                        );";

            DynamicParameters p = new DynamicParameters();

            p.Add("@p_cedula_colaborador", registro.cedula_colaborador);
            p.Add("@p_sede", registro.sede);
            p.Add("@p_tipo_reporte", registro.tipo_reporte);
            p.Add("@p_latitud", registro.latitud);
            p.Add("@p_lontitud", registro.longitud);
            p.Add("@p_ip_address", registro.ip_address);

            var result = await db.ExecuteAsync(sql, p);

            return result > 0;
        }

        public async Task<IEnumerable<RegistroAsistenciaDTO>> consultarRegistrosAsistencia(FiltroAsistencia filtros)
        {
            var db = dbConnection();

            var sql = @"
                        select
                            ra.fecha_adicion,
                            c.cedula,
                            c.nombres,
                            c.apellidos,
                            ra.sede,
                            c.area,
                            ra.tipo_reporte,
                            c.correo,
                            ra.latitud,
                            ra.lontitud,
                            ra.ip_address,
                            c.cargo,
                            c.jefe_inmediato
                        from 
                        asistencia.colaborador c 
                        inner join asistencia.registro_asistencia ra on c.cedula = ra.cedula_colaborador 
                        where cedula > 0
            ";

            if(filtros.fecha_desde is not null)
            {
                sql += " and date(ra.fecha_SQL) >= date(@fecha_desde)";
            }

            if (filtros.fecha_hasta is not null)
            {
                sql += " and date(ra.fecha_SQL) <= date(@fecha_hasta)";
            }

            if (!string.IsNullOrEmpty(filtros.nombres))
            {
                sql += " and LOWER(c.nombres) like CONCAT('%', @nombres, '%')";
            }

            if (!string.IsNullOrEmpty(filtros.apellidos))
            {
                sql += " and LOWER(c.apellidos) like CONCAT('%', @apellidos, '%')";
            }

            if (filtros.cedula is not null)
            {
                sql += " and c.cedula::text like CONCAT('%', @cedula, '%')";
            }

            if (!string.IsNullOrEmpty(filtros.sede))
            {
                sql += " and ra.sede = @sede";
            }

            if (!string.IsNullOrEmpty(filtros.area))
            {
                sql += " and LOWER(c.area) like CONCAT('%', @area, '%')";
            }

            if (!string.IsNullOrEmpty(filtros.tipo_reporte))
            {
                sql += " and ra.tipo_reporte = @reporta";
            }

            if (filtros.jefe_inmediato is not null)
            {
                sql += " and c.jefe_inmediato = @sede";
            }

            if (!string.IsNullOrEmpty(filtros.cargo))
            {
                sql += " and LOWER(c.cargo) like CONCAT('%', @cargo, '%')";
            }

            if (!string.IsNullOrEmpty(filtros.correo))
            {
                sql += " and LOWER(c.correo) like CONCAT('%', @correo, '%')";
            }

            sql += " order by ra.fecha_adicion desc";

            var p = new DynamicParameters();
            p.Add("@fecha_desde",filtros.fecha_desde.GetValueOrDefault().ToString("yyyy-MM-dd"));
            p.Add("@fecha_hasta", filtros.fecha_hasta.GetValueOrDefault().ToString("yyyy-MM-dd"));
            p.Add("@nombres",filtros.nombres.ToLower());
            p.Add("@apellidos",filtros.apellidos.ToLower());
            p.Add("@cedula",filtros.cedula);
            p.Add("@sede",filtros.sede);
            p.Add("@area",filtros.area.ToLower());
            p.Add("@reporta",filtros.tipo_reporte);
            p.Add("@jefe",filtros.jefe_inmediato);
            p.Add("@cargo",filtros.cargo.ToLower());
            p.Add("@correo",filtros.correo.ToLower());

            return await db.QueryAsync<RegistroAsistenciaDTO>(sql,p);
        }

        public async Task<IEnumerable<Colaborador>> consultarJefesInmediatos()
        {
            var db = dbConnection();

            var sql = @"
                        SELECT 
                            cedula,
                            nombres,
                            apellidos,
                            cargo,
                            area,
                            jefe_inmediato,
                            sede,
                            correo,
                            turno,
                            estado,
                            fecha_adicion,
                            usuario_adiciono
                        FROM asistencia.colaborador
                        where cedula in 
                        (
                            select distinct c.jefe_inmediato from asistencia.colaborador c where c.jefe_inmediato is not null
                        )
                ";

            return await db.QueryAsync<Colaborador>(sql);
        }
    }
}
