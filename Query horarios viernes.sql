--Colaborador

alter table asistencia.colaborador
rename hora_entrada_lv to hora_entrada_lj;

alter table asistencia.colaborador
rename hora_salida_lv to hora_salida_lj;

alter table asistencia.colaborador 
add column hora_entrada_v time;

alter table asistencia.colaborador 
add column hora_salida_v time;

alter table asistencia.colaborador 
add column observaciones text;

--Historico colaborador

alter table asistencia.h_colaborador
rename hora_entrada_lv to hora_entrada_lj;

alter table asistencia.h_colaborador
rename hora_salida_lv to hora_salida_lj;

alter table asistencia.h_colaborador 
add column hora_entrada_v time;

alter table asistencia.h_colaborador 
add column hora_salida_v time;

alter table asistencia.h_colaborador 
add column observaciones text;



CREATE OR REPLACE FUNCTION asistencia.log_colaboradores_cambios()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
BEGIN
	INSERT INTO asistencia.h_colaborador
	(
		cedula, 
		nombres, 
		apellidos, 
		cargo, 
		area, 
		jefe_inmediato, 
		sede, 
		correo, 
		estado, 
		fecha_adicion, 
		usuario_adiciono, 
		hora_entrada_lj, 
		hora_salida_lj, 
		hora_entrada_s, 
		hora_salida_s,
		hora_entrada_v, 
		hora_salida_v,
		observaciones
	)
	values
	(
		new.cedula, 
		new.nombres, 
		new.apellidos, 
		new.cargo, 
		new.area, 
		new.jefe_inmediato, 
		new.sede, 
		new.correo, 
		new.estado, 
		CURRENT_TIMESTAMP, 
		new.usuario_adiciono, 
		new.hora_entrada_lj, 
		new.hora_salida_lj, 
		new.hora_entrada_s, 
		new.hora_salida_s,
		new.hora_entrada_v, 
		new.hora_salida_v,
		new.observaciones
	);

	RETURN NEW;
END;
$function$
;

