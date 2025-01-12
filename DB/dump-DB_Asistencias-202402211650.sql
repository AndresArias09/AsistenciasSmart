PGDMP      2                |            DB_Asistencias    16.1    16.1 D    �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            �           1262    17830    DB_Asistencias    DATABASE     �   CREATE DATABASE "DB_Asistencias" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Spanish_Colombia.1252';
     DROP DATABASE "DB_Asistencias";
                postgres    false                        2615    20155 
   asistencia    SCHEMA        CREATE SCHEMA asistencia;
    DROP SCHEMA asistencia;
                postgres    false                        2615    17853    aud    SCHEMA        CREATE SCHEMA aud;
    DROP SCHEMA aud;
                postgres    false                        2615    2200    public    SCHEMA        CREATE SCHEMA public;
    DROP SCHEMA public;
                pg_database_owner    false            �           0    0    SCHEMA public    COMMENT     6   COMMENT ON SCHEMA public IS 'standard public schema';
                   pg_database_owner    false    4                        2615    17831    seg    SCHEMA        CREATE SCHEMA seg;
    DROP SCHEMA seg;
                postgres    false            �            1255    17869 N   insertar_auditoria_descarga_archivos(text, text, text, text, text, text, text) 	   PROCEDURE     x  CREATE PROCEDURE aud.insertar_auditoria_descarga_archivos(IN p_ruta_original text, IN p_ruta_descargada text, IN p_nombre_archivo text, IN p_extension_archivo text, IN p_peso_archivo text, IN p_usuario text, IN p_ip_accion text)
    LANGUAGE plpgsql
    AS $$
begin
	INSERT INTO aud.auditoria_descarga_archivos
	(
		fecha_adicion,
		ruta_original,
		ruta_descargada,
		nombre_archivo,
		extension_archivo,
		peso_archivo,
		usuario,
		ip_accion
	)
	VALUES 
	(
		CURRENT_TIMESTAMP,
		p_ruta_original,
		p_ruta_descargada,
		p_nombre_archivo,
		p_extension_archivo,
		p_peso_archivo,
		p_usuario,
		p_ip_accion
	);
	
	commit;
end $$;
 �   DROP PROCEDURE aud.insertar_auditoria_descarga_archivos(IN p_ruta_original text, IN p_ruta_descargada text, IN p_nombre_archivo text, IN p_extension_archivo text, IN p_peso_archivo text, IN p_usuario text, IN p_ip_accion text);
       aud          postgres    false    7            �            1255    17887 2   insertar_auditoria_login_usuario(text, text, text)    FUNCTION     �  CREATE FUNCTION aud.insertar_auditoria_login_usuario(p_usuario text, p_descripcion text, p_ip_accion text) RETURNS integer
    LANGUAGE plpgsql
    AS $$
DECLARE
    nuevo_id INTEGER;
BEGIN
   INSERT INTO aud.auditoria_login_usuario
	(
		usuario,
		fecha,
		descripcion,
		ip_accion
	)
	VALUES 
	(
		p_usuario,
		CURRENT_TIMESTAMP,
		p_descripcion,
		p_ip_accion
	)
	RETURNING id_auditoria INTO nuevo_id;

	RETURN nuevo_id;

	commit;

    
END;
$$;
 j   DROP FUNCTION aud.insertar_auditoria_login_usuario(p_usuario text, p_descripcion text, p_ip_accion text);
       aud          postgres    false    7            �            1255    17861 q   insertar_auditoria_navegacion(text, text, text, text, text, text, text, text, text, text, text, text, text, text) 	   PROCEDURE     �  CREATE PROCEDURE aud.insertar_auditoria_navegacion(IN p_useragent text, IN p_navegador text, IN p_versionnavegador text, IN p_plataformanavegador text, IN p_urlactual text, IN p_idioma text, IN p_cookieshabilitadas text, IN p_anchopantalla text, IN p_altopantalla text, IN p_profundidadcolor text, IN p_nombreso text, IN p_versionso text, IN p_ip_address text, IN p_usuario_accion text)
    LANGUAGE plpgsql
    AS $$
begin
	INSERT INTO aud.auditoria_navegacion
	(
		fecha_adicion,
		useragent,
		navegador,
		versionnavegador,
		plataformanavegador,
		urlactual,
		idioma,
		cookieshabilitadas,
		anchopantalla,
		altopantalla,
		profundidadcolor,
		nombreso,
		versionso,
		ip_address,
		usuario_accion
	)
	VALUES 
	(
		CURRENT_TIMESTAMP,
		p_useragent,
		p_navegador,
		p_versionnavegador,
		p_plataformanavegador,
		p_urlactual,
		p_idioma,
		p_cookieshabilitadas,
		p_anchopantalla,
		p_altopantalla,
		p_profundidadcolor,
		p_nombreso,
		p_versionso,
		p_ip_address,
		p_usuario_accion);
		
		commit;
end $$;
 �  DROP PROCEDURE aud.insertar_auditoria_navegacion(IN p_useragent text, IN p_navegador text, IN p_versionnavegador text, IN p_plataformanavegador text, IN p_urlactual text, IN p_idioma text, IN p_cookieshabilitadas text, IN p_anchopantalla text, IN p_altopantalla text, IN p_profundidadcolor text, IN p_nombreso text, IN p_versionso text, IN p_ip_address text, IN p_usuario_accion text);
       aud          postgres    false    7            �            1255    17877 _   insertar_envio_email(text, text, text, text, text, text, boolean, text, text, text, text, text)    FUNCTION     �  CREATE FUNCTION aud.insertar_envio_email(p_email_destinatario text, p_email_emisor text, p_email_cc text, p_email_bcc text, p_asunto text, p_mensaje text, p_enviado boolean, p_descripcion_error text, p_usuario text, p_numero_identificacion_tercero text, p_pantalla text, p_descripcion text) RETURNS integer
    LANGUAGE plpgsql
    AS $$
DECLARE
    nuevo_id INTEGER;
begin
	INSERT INTO aud.auditoria_envio_email
	(
		email_destinatario,
		email_cc,
		email_bcc,
		asunto,
		email_emisor,
		mensaje,
		fecha_envio,
		enviado,
		descripcion_error,
		usuario,
		numero_identificacion_tercero,
		pantalla,
		descripcion
	)
	VALUES 
	(
		p_email_destinatario,
		p_email_cc,
		p_email_bcc,
		p_asunto,
		p_email_emisor,
		p_mensaje,
		CURRENT_TIMESTAMP,
		p_enviado,
		p_descripcion_error,
		p_usuario,
		p_numero_identificacion_tercero,
		p_pantalla,
		p_descripcion
	)RETURNING id_auditoria INTO nuevo_id;

	RETURN nuevo_id;

	commit;
end $$;
 "  DROP FUNCTION aud.insertar_envio_email(p_email_destinatario text, p_email_emisor text, p_email_cc text, p_email_bcc text, p_asunto text, p_mensaje text, p_enviado boolean, p_descripcion_error text, p_usuario text, p_numero_identificacion_tercero text, p_pantalla text, p_descripcion text);
       aud          postgres    false    7            �            1255    17852 #   actualizar_estado_otp(bigint, text) 	   PROCEDURE       CREATE PROCEDURE seg.actualizar_estado_otp(IN p_id_otp bigint, IN p_estado text)
    LANGUAGE plpgsql
    AS $$
begin
	update seg.registro_otp
	set fecha_validacion = current_timestamp,
	estado = p_estado
	where id_registro_otp = p_id_otp;

	commit;

end;$$;
 P   DROP PROCEDURE seg.actualizar_estado_otp(IN p_id_otp bigint, IN p_estado text);
       seg          postgres    false    6            �            1255    17841 -   insertar_info_usuario(text, text, text, text) 	   PROCEDURE       CREATE PROCEDURE seg.insertar_info_usuario(IN p_nombres text, IN p_apellidos text, IN p_email text, IN p_usuario text)
    LANGUAGE plpgsql
    AS $$
begin
	if exists (SELECT * FROM seg.usuario where usuario = p_usuario) then
		UPDATE seg.usuario
		SET nombres=p_nombres, apellidos=p_apellidos, email=p_email
		WHERE usuario=p_usuario;
	
	else
		INSERT INTO seg.usuario(nombres, apellidos, email, usuario, fecha_adicion,estado)
			VALUES ( p_nombres, p_apellidos, p_email, p_usuario, CURRENT_TIMESTAMP,1);
	end if
	;

	commit;

end;$$;
 v   DROP PROCEDURE seg.insertar_info_usuario(IN p_nombres text, IN p_apellidos text, IN p_email text, IN p_usuario text);
       seg          postgres    false    6            �            1255    17851 9   insertar_registro_otp(text, text, text, text, text, text) 	   PROCEDURE     2  CREATE PROCEDURE seg.insertar_registro_otp(IN p_codigo_otp text, IN p_estado text, IN p_numero_identificacion_proceso text, IN p_tipo_proceso text, IN p_descripcion text, IN p_metodos_envio text)
    LANGUAGE plpgsql
    AS $$
begin
	INSERT INTO seg.registro_otp
	(
		codigo_otp,
		fecha_adicion,
		estado,
		numero_identificacion_proceso,
		tipo_proceso,
		descripcion,
		metodos_envio
	)
	VALUES 
	(
		p_codigo_otp,
		CURRENT_TIMESTAMP,
		p_estado,
		p_numero_identificacion_proceso,
		p_tipo_proceso,
		p_descripcion,
		p_metodos_envio
	);

	commit;

end;$$;
 �   DROP PROCEDURE seg.insertar_registro_otp(IN p_codigo_otp text, IN p_estado text, IN p_numero_identificacion_proceso text, IN p_tipo_proceso text, IN p_descripcion text, IN p_metodos_envio text);
       seg          postgres    false    6            �            1255    17888 7   registrar_info_cierre_session(bigint, text, text, text) 	   PROCEDURE     �  CREATE PROCEDURE seg.registrar_info_cierre_session(IN p_id_auditoria bigint, IN p_usuario_cierre_session text, IN p_ip_cierre_session text, IN p_motivo_cierre_session text)
    LANGUAGE plpgsql
    AS $$
begin
	update aud.auditoria_login_usuario
	set
	fecha_cierre_sesion = CURRENT_TIMESTAMP,
	usuario_cierre_sesion = p_usuario_cierre_session,
	ip_cierre_sesion = p_ip_cierre_session,
	motivo_cierre_sesion = p_motivo_cierre_session
	
	where id_auditoria = p_id_auditoria;

	commit;
end $$;
 �   DROP PROCEDURE seg.registrar_info_cierre_session(IN p_id_auditoria bigint, IN p_usuario_cierre_session text, IN p_ip_cierre_session text, IN p_motivo_cierre_session text);
       seg          postgres    false    6            �            1259    20156    colaborador    TABLE     1  CREATE TABLE asistencia.colaborador (
    cedula bigint NOT NULL,
    nombres text,
    apellidos text,
    cargo text,
    area text,
    jefe_inmediato bigint,
    sede text,
    correo text,
    turno text,
    estado bigint,
    fecha_adicion timestamp without time zone,
    usuario_adiciono text
);
 #   DROP TABLE asistencia.colaborador;
    
   asistencia         heap    postgres    false    8            �            1259    20188    registro_asistencia    TABLE       CREATE TABLE asistencia.registro_asistencia (
    id_registro_asistencia integer NOT NULL,
    fecha_adicion timestamp without time zone,
    cedula_colaborador bigint,
    sede text,
    tipo_reporte text,
    latitud text,
    lontitud text,
    ip_address text
);
 +   DROP TABLE asistencia.registro_asistencia;
    
   asistencia         heap    postgres    false    8            �            1259    20187 .   registro_asistencia_id_registro_asistencia_seq    SEQUENCE     �   CREATE SEQUENCE asistencia.registro_asistencia_id_registro_asistencia_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 I   DROP SEQUENCE asistencia.registro_asistencia_id_registro_asistencia_seq;
    
   asistencia          postgres    false    232    8            �           0    0 .   registro_asistencia_id_registro_asistencia_seq    SEQUENCE OWNED BY     �   ALTER SEQUENCE asistencia.registro_asistencia_id_registro_asistencia_seq OWNED BY asistencia.registro_asistencia.id_registro_asistencia;
       
   asistencia          postgres    false    231            �            1259    17863    auditoria_descarga_archivos    TABLE     '  CREATE TABLE aud.auditoria_descarga_archivos (
    id_auditoria integer NOT NULL,
    fecha_adicion timestamp without time zone,
    ruta_original text,
    ruta_descargada text,
    nombre_archivo text,
    extension_archivo text,
    peso_archivo text,
    usuario text,
    ip_accion text
);
 ,   DROP TABLE aud.auditoria_descarga_archivos;
       aud         heap    postgres    false    7            �            1259    17862 ,   auditoria_descarga_archivos_id_auditoria_seq    SEQUENCE     �   CREATE SEQUENCE aud.auditoria_descarga_archivos_id_auditoria_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 @   DROP SEQUENCE aud.auditoria_descarga_archivos_id_auditoria_seq;
       aud          postgres    false    7    225                        0    0 ,   auditoria_descarga_archivos_id_auditoria_seq    SEQUENCE OWNED BY     w   ALTER SEQUENCE aud.auditoria_descarga_archivos_id_auditoria_seq OWNED BY aud.auditoria_descarga_archivos.id_auditoria;
          aud          postgres    false    224            �            1259    17871    auditoria_envio_email    TABLE     �  CREATE TABLE aud.auditoria_envio_email (
    id_auditoria integer NOT NULL,
    email_destinatario text,
    email_cc text,
    email_bcc text,
    asunto text,
    email_emisor text,
    mensaje text,
    fecha_envio timestamp without time zone,
    enviado boolean,
    descripcion_error text,
    usuario text,
    numero_identificacion_tercero text,
    pantalla text,
    descripcion text
);
 &   DROP TABLE aud.auditoria_envio_email;
       aud         heap    postgres    false    7            �            1259    17870 &   auditoria_envio_email_id_auditoria_seq    SEQUENCE     �   CREATE SEQUENCE aud.auditoria_envio_email_id_auditoria_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 :   DROP SEQUENCE aud.auditoria_envio_email_id_auditoria_seq;
       aud          postgres    false    7    227                       0    0 &   auditoria_envio_email_id_auditoria_seq    SEQUENCE OWNED BY     k   ALTER SEQUENCE aud.auditoria_envio_email_id_auditoria_seq OWNED BY aud.auditoria_envio_email.id_auditoria;
          aud          postgres    false    226            �            1259    17879    auditoria_login_usuario    TABLE     B  CREATE TABLE aud.auditoria_login_usuario (
    id_auditoria integer NOT NULL,
    usuario text,
    fecha timestamp without time zone,
    descripcion text,
    ip_accion text,
    fecha_cierre_sesion timestamp without time zone,
    usuario_cierre_sesion text,
    ip_cierre_sesion text,
    motivo_cierre_sesion text
);
 (   DROP TABLE aud.auditoria_login_usuario;
       aud         heap    postgres    false    7            �            1259    17878 (   auditoria_login_usuario_id_auditoria_seq    SEQUENCE     �   CREATE SEQUENCE aud.auditoria_login_usuario_id_auditoria_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 <   DROP SEQUENCE aud.auditoria_login_usuario_id_auditoria_seq;
       aud          postgres    false    7    229                       0    0 (   auditoria_login_usuario_id_auditoria_seq    SEQUENCE OWNED BY     o   ALTER SEQUENCE aud.auditoria_login_usuario_id_auditoria_seq OWNED BY aud.auditoria_login_usuario.id_auditoria;
          aud          postgres    false    228            �            1259    17855    auditoria_navegacion    TABLE     �  CREATE TABLE aud.auditoria_navegacion (
    id_auditoria_navegacion integer NOT NULL,
    fecha_adicion timestamp without time zone,
    useragent text,
    navegador text,
    versionnavegador text,
    plataformanavegador text,
    urlactual text,
    idioma text,
    cookieshabilitadas text,
    anchopantalla text,
    altopantalla text,
    profundidadcolor text,
    nombreso text,
    versionso text,
    ip_address text,
    usuario_accion text,
    ubicacion text
);
 %   DROP TABLE aud.auditoria_navegacion;
       aud         heap    postgres    false    7            �            1259    17854 0   auditoria_navegacion_id_auditoria_navegacion_seq    SEQUENCE     �   CREATE SEQUENCE aud.auditoria_navegacion_id_auditoria_navegacion_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 D   DROP SEQUENCE aud.auditoria_navegacion_id_auditoria_navegacion_seq;
       aud          postgres    false    7    223                       0    0 0   auditoria_navegacion_id_auditoria_navegacion_seq    SEQUENCE OWNED BY        ALTER SEQUENCE aud.auditoria_navegacion_id_auditoria_navegacion_seq OWNED BY aud.auditoria_navegacion.id_auditoria_navegacion;
          aud          postgres    false    222            �            1259    17843    registro_otp    TABLE     <  CREATE TABLE seg.registro_otp (
    id_registro_otp integer NOT NULL,
    codigo_otp text,
    fecha_adicion timestamp without time zone,
    fecha_validacion timestamp without time zone,
    estado text,
    numero_identificacion_proceso text,
    tipo_proceso text,
    descripcion text,
    metodos_envio text
);
    DROP TABLE seg.registro_otp;
       seg         heap    postgres    false    6            �            1259    17842     registro_otp_id_registro_otp_seq    SEQUENCE     �   CREATE SEQUENCE seg.registro_otp_id_registro_otp_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 4   DROP SEQUENCE seg.registro_otp_id_registro_otp_seq;
       seg          postgres    false    6    221                       0    0     registro_otp_id_registro_otp_seq    SEQUENCE OWNED BY     _   ALTER SEQUENCE seg.registro_otp_id_registro_otp_seq OWNED BY seg.registro_otp.id_registro_otp;
          seg          postgres    false    220            �            1259    17833    usuario    TABLE     �   CREATE TABLE seg.usuario (
    id_usuario integer NOT NULL,
    nombres text,
    apellidos text,
    email text,
    usuario text,
    fecha_adicion timestamp without time zone,
    estado integer
);
    DROP TABLE seg.usuario;
       seg         heap    postgres    false    6            �            1259    17832    usuario_id_usuario_seq    SEQUENCE     �   CREATE SEQUENCE seg.usuario_id_usuario_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 *   DROP SEQUENCE seg.usuario_id_usuario_seq;
       seg          postgres    false    6    219                       0    0    usuario_id_usuario_seq    SEQUENCE OWNED BY     K   ALTER SEQUENCE seg.usuario_id_usuario_seq OWNED BY seg.usuario.id_usuario;
          seg          postgres    false    218            M           2604    20191 *   registro_asistencia id_registro_asistencia    DEFAULT     �   ALTER TABLE ONLY asistencia.registro_asistencia ALTER COLUMN id_registro_asistencia SET DEFAULT nextval('asistencia.registro_asistencia_id_registro_asistencia_seq'::regclass);
 ]   ALTER TABLE asistencia.registro_asistencia ALTER COLUMN id_registro_asistencia DROP DEFAULT;
    
   asistencia          postgres    false    231    232    232            J           2604    17866 (   auditoria_descarga_archivos id_auditoria    DEFAULT     �   ALTER TABLE ONLY aud.auditoria_descarga_archivos ALTER COLUMN id_auditoria SET DEFAULT nextval('aud.auditoria_descarga_archivos_id_auditoria_seq'::regclass);
 T   ALTER TABLE aud.auditoria_descarga_archivos ALTER COLUMN id_auditoria DROP DEFAULT;
       aud          postgres    false    225    224    225            K           2604    17874 "   auditoria_envio_email id_auditoria    DEFAULT     �   ALTER TABLE ONLY aud.auditoria_envio_email ALTER COLUMN id_auditoria SET DEFAULT nextval('aud.auditoria_envio_email_id_auditoria_seq'::regclass);
 N   ALTER TABLE aud.auditoria_envio_email ALTER COLUMN id_auditoria DROP DEFAULT;
       aud          postgres    false    226    227    227            L           2604    17882 $   auditoria_login_usuario id_auditoria    DEFAULT     �   ALTER TABLE ONLY aud.auditoria_login_usuario ALTER COLUMN id_auditoria SET DEFAULT nextval('aud.auditoria_login_usuario_id_auditoria_seq'::regclass);
 P   ALTER TABLE aud.auditoria_login_usuario ALTER COLUMN id_auditoria DROP DEFAULT;
       aud          postgres    false    228    229    229            I           2604    17858 ,   auditoria_navegacion id_auditoria_navegacion    DEFAULT     �   ALTER TABLE ONLY aud.auditoria_navegacion ALTER COLUMN id_auditoria_navegacion SET DEFAULT nextval('aud.auditoria_navegacion_id_auditoria_navegacion_seq'::regclass);
 X   ALTER TABLE aud.auditoria_navegacion ALTER COLUMN id_auditoria_navegacion DROP DEFAULT;
       aud          postgres    false    223    222    223            H           2604    17846    registro_otp id_registro_otp    DEFAULT     �   ALTER TABLE ONLY seg.registro_otp ALTER COLUMN id_registro_otp SET DEFAULT nextval('seg.registro_otp_id_registro_otp_seq'::regclass);
 H   ALTER TABLE seg.registro_otp ALTER COLUMN id_registro_otp DROP DEFAULT;
       seg          postgres    false    220    221    221            G           2604    17836    usuario id_usuario    DEFAULT     r   ALTER TABLE ONLY seg.usuario ALTER COLUMN id_usuario SET DEFAULT nextval('seg.usuario_id_usuario_seq'::regclass);
 >   ALTER TABLE seg.usuario ALTER COLUMN id_usuario DROP DEFAULT;
       seg          postgres    false    219    218    219            �          0    20156    colaborador 
   TABLE DATA           �   COPY asistencia.colaborador (cedula, nombres, apellidos, cargo, area, jefe_inmediato, sede, correo, turno, estado, fecha_adicion, usuario_adiciono) FROM stdin;
 
   asistencia          postgres    false    230   �n       �          0    20188    registro_asistencia 
   TABLE DATA           �   COPY asistencia.registro_asistencia (id_registro_asistencia, fecha_adicion, cedula_colaborador, sede, tipo_reporte, latitud, lontitud, ip_address) FROM stdin;
 
   asistencia          postgres    false    232   �n       �          0    17863    auditoria_descarga_archivos 
   TABLE DATA           �   COPY aud.auditoria_descarga_archivos (id_auditoria, fecha_adicion, ruta_original, ruta_descargada, nombre_archivo, extension_archivo, peso_archivo, usuario, ip_accion) FROM stdin;
    aud          postgres    false    225   �n       �          0    17871    auditoria_envio_email 
   TABLE DATA           �   COPY aud.auditoria_envio_email (id_auditoria, email_destinatario, email_cc, email_bcc, asunto, email_emisor, mensaje, fecha_envio, enviado, descripcion_error, usuario, numero_identificacion_tercero, pantalla, descripcion) FROM stdin;
    aud          postgres    false    227   7p       �          0    17879    auditoria_login_usuario 
   TABLE DATA           �   COPY aud.auditoria_login_usuario (id_auditoria, usuario, fecha, descripcion, ip_accion, fecha_cierre_sesion, usuario_cierre_sesion, ip_cierre_sesion, motivo_cierre_sesion) FROM stdin;
    aud          postgres    false    229   ��       �          0    17855    auditoria_navegacion 
   TABLE DATA           "  COPY aud.auditoria_navegacion (id_auditoria_navegacion, fecha_adicion, useragent, navegador, versionnavegador, plataformanavegador, urlactual, idioma, cookieshabilitadas, anchopantalla, altopantalla, profundidadcolor, nombreso, versionso, ip_address, usuario_accion, ubicacion) FROM stdin;
    aud          postgres    false    223   9�       �          0    17843    registro_otp 
   TABLE DATA           �   COPY seg.registro_otp (id_registro_otp, codigo_otp, fecha_adicion, fecha_validacion, estado, numero_identificacion_proceso, tipo_proceso, descripcion, metodos_envio) FROM stdin;
    seg          postgres    false    221   ��       �          0    17833    usuario 
   TABLE DATA           e   COPY seg.usuario (id_usuario, nombres, apellidos, email, usuario, fecha_adicion, estado) FROM stdin;
    seg          postgres    false    219   ��                  0    0 .   registro_asistencia_id_registro_asistencia_seq    SEQUENCE SET     a   SELECT pg_catalog.setval('asistencia.registro_asistencia_id_registro_asistencia_seq', 1, false);
       
   asistencia          postgres    false    231                       0    0 ,   auditoria_descarga_archivos_id_auditoria_seq    SEQUENCE SET     W   SELECT pg_catalog.setval('aud.auditoria_descarga_archivos_id_auditoria_seq', 5, true);
          aud          postgres    false    224                       0    0 &   auditoria_envio_email_id_auditoria_seq    SEQUENCE SET     R   SELECT pg_catalog.setval('aud.auditoria_envio_email_id_auditoria_seq', 25, true);
          aud          postgres    false    226            	           0    0 (   auditoria_login_usuario_id_auditoria_seq    SEQUENCE SET     T   SELECT pg_catalog.setval('aud.auditoria_login_usuario_id_auditoria_seq', 43, true);
          aud          postgres    false    228            
           0    0 0   auditoria_navegacion_id_auditoria_navegacion_seq    SEQUENCE SET     ]   SELECT pg_catalog.setval('aud.auditoria_navegacion_id_auditoria_navegacion_seq', 122, true);
          aud          postgres    false    222                       0    0     registro_otp_id_registro_otp_seq    SEQUENCE SET     L   SELECT pg_catalog.setval('seg.registro_otp_id_registro_otp_seq', 19, true);
          seg          postgres    false    220                       0    0    usuario_id_usuario_seq    SEQUENCE SET     A   SELECT pg_catalog.setval('seg.usuario_id_usuario_seq', 1, true);
          seg          postgres    false    218            W           2606    20162    colaborador colaborador_pkey 
   CONSTRAINT     b   ALTER TABLE ONLY asistencia.colaborador
    ADD CONSTRAINT colaborador_pkey PRIMARY KEY (cedula);
 J   ALTER TABLE ONLY asistencia.colaborador DROP CONSTRAINT colaborador_pkey;
    
   asistencia            postgres    false    230            U           2606    17886 4   auditoria_login_usuario auditoria_login_usuario_pkey 
   CONSTRAINT     y   ALTER TABLE ONLY aud.auditoria_login_usuario
    ADD CONSTRAINT auditoria_login_usuario_pkey PRIMARY KEY (id_auditoria);
 [   ALTER TABLE ONLY aud.auditoria_login_usuario DROP CONSTRAINT auditoria_login_usuario_pkey;
       aud            postgres    false    229            S           2606    17850    registro_otp registro_otp_pkey 
   CONSTRAINT     f   ALTER TABLE ONLY seg.registro_otp
    ADD CONSTRAINT registro_otp_pkey PRIMARY KEY (id_registro_otp);
 E   ALTER TABLE ONLY seg.registro_otp DROP CONSTRAINT registro_otp_pkey;
       seg            postgres    false    221            O           2606    20164    usuario unique_username_usuario 
   CONSTRAINT     Z   ALTER TABLE ONLY seg.usuario
    ADD CONSTRAINT unique_username_usuario UNIQUE (usuario);
 F   ALTER TABLE ONLY seg.usuario DROP CONSTRAINT unique_username_usuario;
       seg            postgres    false    219            Q           2606    17840    usuario usuario_pkey 
   CONSTRAINT     W   ALTER TABLE ONLY seg.usuario
    ADD CONSTRAINT usuario_pkey PRIMARY KEY (id_usuario);
 ;   ALTER TABLE ONLY seg.usuario DROP CONSTRAINT usuario_pkey;
       seg            postgres    false    219            X           2606    20170 (   colaborador fk_colaborador_jefeinmediato    FK CONSTRAINT     �   ALTER TABLE ONLY asistencia.colaborador
    ADD CONSTRAINT fk_colaborador_jefeinmediato FOREIGN KEY (jefe_inmediato) REFERENCES asistencia.colaborador(cedula);
 V   ALTER TABLE ONLY asistencia.colaborador DROP CONSTRAINT fk_colaborador_jefeinmediato;
    
   asistencia          postgres    false    230    230    4695            Y           2606    20165 "   colaborador fk_colaborador_usuario    FK CONSTRAINT     �   ALTER TABLE ONLY asistencia.colaborador
    ADD CONSTRAINT fk_colaborador_usuario FOREIGN KEY (usuario_adiciono) REFERENCES seg.usuario(usuario);
 P   ALTER TABLE ONLY asistencia.colaborador DROP CONSTRAINT fk_colaborador_usuario;
    
   asistencia          postgres    false    230    4687    219            �      x������ � �      �      x������ � �      �   9  x���KN�0�ur�^ �g�qVT vlJK(Dj��nP��Y8.E�,�f��ǯ�e}2F�(M%�L�r!�2���&7��[�i}[��1�)���a������r�6fe_+p�҇bweU�;��~�����t�o\��������q���E�5�q�Q�ʷ��k[��bk��)?-��yZ�hۺjc�}]�ؗ�&B̀P�� yS�O�$��3�	�逽�A@�� �.�:}@��c6C@��t:`/e� �]@��R1��u��逽�@)8�@ĜdZ
��"&�&�S Uhh ��/�      �      x���r�ƕ��?E[���-�x��l��ؙ��R�x7�$��!!�;�%��$���g�#��� ��]V�������n����_�>���ǯ��,n�����ٓ'O~��Ƌx>S_,�?��p<P?~���TM�G4_���������Q�^��i<FO��yt;�p9���c>������/��^}��?��<UZ�������suR;=������/_}�������W��t1�ǳ�`�NO_|{�N����z��ݻ��f}6�:}������f�����k}�N�IA<M����tq�9z���V=Y��{qr7����(iw�f<���˸6����&���]`Vt��e�����U�r��`$�+��q���񥺊#u����_�NH"+:Ҭ��2�/gû������xz�(�Yz}1���}��ק����&�������Y���t����NK������O?�S4�/�R���&�*�cj��܍����l�|{�ګ�щ��uqG����:W���|�?���v&ߓ��O7����q��v6�3�|7����m�������4����b8�D������k���IT���ɟ7�b1��j��r�|��]Yx��}�qr���Y��R����O_Ԟ�nn�89}��/_\D���[�����x={z�����켈?L"'�k}���EQK���M4JF��t�A-��(���t�>�OWס�:����g�oE�Q�~W��ޙ�Rjs� 9��x|���i|^����gH��&�8�F�8yLj���j�W����1�� ���ø���z3x_�B�қz0�Fss3�H��_�[���`4J���It�U�.�9��W�e���e�Q�Fɖܣ�A�v2��W�'�ᛟ�lo�r�۝ߪ���?���|�W�d�	��.�ٜ�r`�g�����"Y�L;c����{̋7�LM*?����G�<�����"��������t��<�q���=\Oҙ}S�˝j��1�KF��$��/�5�q�W��VC�xz���U���i��Am��0�����蓿�"J��|���&`�ن�� }o��lٺ�e�ʬ[��`i���Ww��xeN�4}x7M���=b�V����2I�f҆��ԫ@�u��f�^�ɛ�e�}[ԯf��I4�/��;q������f<�p�M�=�'�뷂��n�z�ľX�ܞ�y4�8Y�������>�׽e�/�ۗoq���N����0iδ��du����/Nv�^�R{�~3�k���5�F�}��W���׃᛫�,���}s��w��wm^�^�H�`��^7!;|��dV�c�c�n���rc���Vc�xtqb��m7�O-�`��{q;�{<�t�g�i�&�q��y�S�3u?�=�&��ȗl�^��v��d}�\�'���O�]\y��n,�%�i��LM�O��m��%�[{=�o�;���~r.^���_�ܻ[���}����|�Yr'�n�U�d�æn\g?�M:�Yڨe �w;���M����zx��G�m32_��7��w�Dr�������I�6�9g�}���lu�|�������,�ė��i������o��j���5}�x�=-���[���;ϼa�or7a����zӗ���O��(�H�O�>����Y��߹|��>���������$��Q`4�����[>ߍ��'�V՗�l2�Z��cW�w�q�}^��xv#�t�ޮ��i��{ݗ@��ö�g9���N�����3�߲Zz?i��ߣu��4�%�'eΚ���T��_����+Ψ���?3��O��役�]ޟ�\K�C�}K�S��_8�jX[w`�&S[�����`��u4y�7��$��"�e5���g��t-����m�r�8��H��3cz��Z��z�y�e�X�m{�3
}��ts;���l{��ZӯB��vχz��V����4"�}??��0���_� �7W���l�1�*������pv������?��o�G��������`'����N����Ϳ�w�ͽ���U�\[];��,j���������l������Ӳ�<} �׻��x�,:�@�����7ωe�|�zC6���N�[^���|�5�%���w��ӸH\at��I\ӯt��+�])�ĩ��z���&YQ	Q���:	.���#����o����6�`u��'�Oc�>�=w���3�R'�NB���:��Ҝ��l4��� 7��h6�$���n�M�<ʍw�Me�XW����;�Dԉ*։��n�Ǒ�Q�'��\g��Tz��:���������)l��Ӳ��~Ԭ��s�ρU��Ov:�O��f�W���F#=��w�v���x���6}���Wwj����W�tN���$�h��t������]l����=OFj�ѩ����y%������Kr����.�ͽ�e�ٿo4{G��7
M�������l�<ƶ�������>�^��=i��4yY�橢p���~���C�ە�n0^'o7��h��������!b>L��w-�ީt��h��qq��֭?]=3.N�C����d��m�b�<g��W�J��Ϟ��w��5f#5��>xpE]'��(	��^O�7'��ͭw��|���w��
Q����#��iv������B��c�gI~�{�^�Nɕ;%�2�L��T�>�`�i����v��ݦ���^�r�o���B�EN:�r��`�-��-���í6j��ڤ9��Q:�z����m�cf2���{a�m��~s����I-�*ٴ6l$w�ɳ�q�u�W�7�������w)�Kf�>��%W�/�M�I�Y����)�,{ˮG�ֽ_��t�n":���O�A�G�w�$��$��DI:��tV�p�|I_D���y��5��$�,��)q�����۟)��m��B����%�$��\�ә�w�lQ��ˢ'_ 8#@@f���6X�B� &@L@���?Q����	V������ ��C�Ua�"���=�������nǛM}:�t!��P���}��T��p�5 ��]�����gq��/�u�������6�Kg������/q��ݹ���׿
;��=BX�>�`�5�jy'��Lɓ�Avt*m�d��@�@�@�@�P�N�`�D�@�P!mX.��v,q�t	�ڱ_\�`��8y��s��዗߾��k��w�_�x�GUS�f��BM���o@4�N��~��o���T��Rqo����RS��:�/��Kc��d��-[�JW�n��d���Z�==�F,Y�=i�V-k�Y�@��~Ю�5�f�I�D�M�}����x�$�L����+]�r�G L�� �Ia�o��Io@��0a:4 L� �	�)& L�TpF Lv��d�V�	�T���	& L���>���>��z4���098���p(�<X�S� �0a2&+'��G��"k Q'�N�o����2�r L�xX7�:�Ώ�-& L���Բ�S#���3& L@��0a�
��k�&�7�'�g	?J��	���/�  3@f`,b�c &�R �	�4�
ǆ� MAp��P��b�H�0�6�6T1҆\w�6l�I$O��ѩ�&���^H	Ľ� aҶ�C���~;�7z�^�sdS��* aR@��[�0a�& L@��u a�d�	Sƀ0��]��0���o�$�n�0a��	Ӿ1���}���¤�t= LN�c>
5�l�T%  L@����	���F��H�I���['��w��̣���M�N��c�eK��	��'#����H����	& L@�|�0u��N����M�I�Y��a�ĸ��D�� ��� g�����@���	� a�� M�±�)@S��){���B?� L��U��!ם�[o�ɓ�Avt*m ���>:��Rqo�@��-{�����Qo��n�}dS��* aR@��[�0a�& L@��u a�d�	Sƀ0��]��0���o�$�n�0a��	Ӿ1���}���¤�t= LN�c>
5�l�T%  L@����	���F��H�I���['��w��̣���M�N    ��c�eK��	��'#����H����	& L@�|�0��f�� �d�&�$�,�G��Ҁ0ab\�z"@@�`�E�� �3d�l�EL tb�T
�0AS��P����)������Al�i&�҆*Fڐ�N҆�7i��I� ;:�6 a�dR�)��7R Lږ=tS�釭z���ǆ0��0i�&�)�&�a���Ѐ0Yw & L���0eS��0ٵ
�)Z�& LR�& L@��0�s������> L��L����:�áP�`�F LU ���`@���.yo����D��:��u L~�[�<ʁ0y[�a�$�D;?VX�4 L@��0Yz2RˎN�Ԯ��@��0a���'S�{�n4�0�I<I<K�Q"�4 L@���� |  @@��� �2�`�� 1� L��)T864h
�34�b/~`[�G� ��������6亓�a�M� y�6ȎN�@��0�G��@�BJ  ��e���7�z����#C�:@�4W��
�	�ހ0a�th@��; &SL@�2����@��Z���~&�v�	& L@����}���}n&�h��arpZ��P�y�d#�* a�d0 LVN��7�E�@�NB���:	&��-e�@��-�nu��+,[& L@�,=�eG�FjW�g L@��0a�����lw:@�L�$�$�%�(X& L��ZO�  @pF��� ��m��	��AL���J&h
�*�4��B�?0�-�#m �D�@�P�Hr�Iڰ�&m�<IdG�� L@��CJ {!%�FJ �I۲�aj4U��7�~�Wﵺg���!L] L�� �Ia�o��Io@��0a:4 L� �	�)& L�TpF Lv��d�V�	�T���	& L���>���>��z4���098���p(�<X�S� �0a2&+'��G��"k Q'�N�o����2�r L�xX7�:�Ώ�-& L���Բ�S#���3& L@��0a�	��[A����M�I�Y��a�ĸ��D�� ��� g�����@���	� a�� M�±�)@S��){���B?� L��U��!ם�[o�ɓ�Avt*m ���>:��Rqo�@��-{��V?��;�h�zr���xr7O`��٩n��ՏqtS�6��?Άo�x�����at�&9���}�Ti|�Uﳾ��l'��b8�Ά�Ѭ��ig���tQ�:�ӝ_L�����_�F����g����yC2���n���og�tqa��
lw�\������}�^N�N+y��)����&J�~s��)������F��UN�?p����g�i4���^F�l�¶������'���;�t�Q�C}�^�o����+��{��r�m��X���h�V��܋��~����5��c�� �iF�a
`X~+�0�az0`ء�� �a �L1����3�k�0S��M ä:#�0�a � ��7�2��y� �ԣ�Z
0��)���DM��� ê 0`�� �Y9!�=�ވlY��:	u�$ ��.��y����_ԉv~�ni � ���d����]��0`���A �H<�����0�a��:O�  @pF��� ��m��	��AL���J�0��?*����G�?0�-�#m F�@�P�Hr�Iڰ�&m�<IdG���a ��CJ {!%�FJ 0L�2�a�N�tBxa��|����ixa
^X~+�0xaz�/^ء��� xa��L1�����3��k�0S��M�¤2#�0xa������7��2��i���ԣ�Y
/���L�DL����ª /^����Y9��=��hlY��:	u�$���.��y�����_ԉv~,ni�������d����]����/^���A��H<�����0xa��:O�  @pF��� ��m��	��AL���J�0��?*����G�?0�-�#m�F�@�P�Hr�Iڰ�&m�<IdG��xa���CJ {!%�FJ /L�2xaͰ��4{��0�a��� d�f� �@�巂�7�a �@��0� 2�Ȱ��+8#�0�V�3E��2L�4�2dȰ}c&*3Q}��
2L=�ɥ ����M��,0
2�J  �@�d��2�#�̖���P'�N2��rK�G9�0o<��E�h��z��2d�0KOFj�ѩ����d�0�a �@�y�녽Vd���ēĳ�%K2�qY� ���� 3�1��1�	P) ��G�c����!8��(����~� �HH�iC�;I�ޤ�'i���T� 2d�}tH	d/���H	@�i[���a�~�{�Vb�0�a!�0͐1LA�o�1Lo� �A;4�a� 1b�)&�a�VpF�av��f�V�	b�Th�1b�0�a��DT&��>b�z4sK!�98�ɛ�)=X_bX�  �A3�0+'T�G��-vQ'�N�o�b���2�r�a�xX�:�Ώ��-b�0�a���Բ�S#���3�0�a� �A�V6�a�����xzX"�1�qY� ���� 3�1��1�	P)���G�c�� �!8��(����~��HH�iC�;I�ޤ�'i���T� 1b�}tH	d/���H	 �i[� 1,�Ճ0�6z �@�y�k�ӌ �Ȱ�V�a ��2dȰCf� �@��b�1�agf�*�a�h��@�I�F�a �@���o�De&��3QA��G3�d���@����҃FA�U	 d�0���rBf{佑ٲbu�$��I@��]n)�(�m��U����X��@��f��H-;:5R�:>�2d�0�ae� &��x����%�a ��u� |  @@��� �2�`�� 1��a�?`T86���3�b/~`[�G� 2�������6亓�a�M� y�6ȎN� �@��G��@�BJ  d��e ��zp�j5A�����3@�)�a�� �@��d�0�a�2̺@��3�2,c �
�2̮U �L��7��*� �@��2lߘ��LT�g��S�fr)�0'�2{=���� �0�af����{#�e�.�$�I������R�Q2���|Q'������2�ғ�Zvtj�vu|2d�0�a>!ú�V��f�&�$�,�G���@��c\�z"@@�`�E�� �3d�l�EL tb�T
@�����Q�ذ?`ΰ?����Al�i�0�҆*Fڐ�N҆�7i��I� ;:�6�fR�)��7R�aږ=xdX��f�v �A�ւ�2 �)�a��� ��b�0�a�1̺ �A3�1,c�
�1̮U�L��7A�
�� �A�1lߘ��DT�'�BS�fn)�0�2y9��B� �0�a�f����{��e�.�$�I���@��R�Q1���|Q'�����A�1�ғ�Zvtj�vu|�1b�0�a��1Lp#�7OK� �1.�<  @0�"@@�� �2d��"&:1b*�@���pl��?g��^�� �Џ�biiC#m�u'i�֛�A�$m��J �A��)�셔@�)�0m�\ ��뭠��A�����3@�)�a�� �@��d�0�a�2̺@��3�2,c �
�2̮U �L��7��*� �@��2lߘ��LT�g��S�fr)�0'�2{=���� �0�af����{#�e�.�$�I������R�Q2���|Q'������2�ғ�Zvtj�vu|2d�0�a ��2Lp#�7OK �@�1.�<  @0�"@@�� �2d��"&:1b* �`���pl��?g��^�� �Џ� �  diiC#m�u'i�֛�A�$m��J@����)�셔@�)�0m�@�5�z��
�B�a üE�u@�i��a
dX~+�0�az2dء��� �a �L1��Ȱ�3��k�0S��M äJ#�0�a �@���7f�2���� �ԣ�\
2��I���DO��� ê 2d��@�Y9!�=���lY��:	u�$ ��.��y�����*_ԉv~�ni �@����d����]�A��2d�OȰV���aoO�~�,d�0�e�'_ 8#@@f���6X�B� &@L@� d���������؋���6�#m m�b���$m�z�6H����Si�0�a��!%����{#% �m�CG�5�~Ю7{�V��$~rT
S
��2@aRP��[�0Aa�&(LP�
�u@a��d�	
SƠ0�
�]��0���o��$o�0Aa���	
Ӿ1���}�O�¤�|=(LΫcB5�l��T% (LP����	���F��"H�I���['���w��̣
��N�N��c�eK���	
��'#����H����	
&(LP�|�0����ۃ�d�&�$�,�G��Ҡ0Aab\�z"@@�`�E�� �3d�l�EL tb�T
�0�S �P����)������Al�i&�҆*Fڐ�N҆�7i��I� ;:�6@a��dR�)��7R(Lږ=`
S[�~��{�F����t�Is�0) L��@��0�& L��ɺ�0a2��)c@�
��ɮU@�L��7a�j7@��0a��iߘ���>���aR�f�&��1��K6a� & L�d�p��{#\d$�$�I��� a��R�Q����&Q'���²�a���ғ�Zvtj�vu|��	& L>A�Z��^����M�I�Y��a�ĸ��D�� ��� g�����@���	� a�� M�±�)@S��){���B?� L��U��!ם�[o�ɓ�Avt*m ���>:��Rqo�@��-{��^?��v��:�6�Na�Aa�\(L

S~+&(Lz���	
ӡAa�� (LP�L1Aa���3Ba�k&S��MP���&(LP��0Aa�7&�1����}P�ԣ�����yuL�C�����P�� �	
���0Y9�\<��(Y�:	u�$P��.��y�Ca����Iԉv~,�liP��0Aa��d����]��0Aa���	
�O�vtgP��$�$�%�(X&(L��ZO�  @pF��� ��m��	��AL���J&p
�*�8��B�?0�-�#m��D�@�P�Hr�Iڰ�&m�<IdG��(LP��CJ {!%�FJ �I۲�Na
���Q�4ςv���Fpl
Sm�al�@1�b:�����k���@1�(�"WPL+_PL��b:4PL�����bŤk(�#�
�b�(&��ӡ�b*4PL��c�3�~�%�A19:i�����N̓�A1U	 (&��b�rB�x佑/�u�$��I@1�]n)�(��m��Փ���Xg��@1�b�d��H-;:5R�:>�b��	(&�PL�N#l����$�$�%�(X(&PL��ZO�  @pF��� ��m��	��AL���J(&�
0*�L��B�?0�-�#m �D�@�P�Hr�Iڰ�&m�<IdG��PL���CJ {!%�FJ �I۲��bj��Yo��zG'1����K�IAb�o���Io�� 1Ab:4HL� �	�)&HL��TpFHLv���d�V�	�T����	$&HL��?&��>��z4s� 198��Iq��<X�S�  1Ab2$&+'ԋG��"!Q'�N�o����2�rHL�xX<�:�Ώe�-$&HL���Բ�S#���3$&HL�� 1Ab���jax���M�I�Y��Ab��ĸ��D�� ��� g�����@���	�@b� R�±A*�T�A*{���B?�HL��U��!ם�[o�ɓ�Avt*m����>:��Rqo����-{�$�F-Up�o��F���v�slS�t-@1)PL����@1�(&PL��ɺ@1�b2��)c��
��ɮU��L��7�b�*8��@1�b��iߘ��?�g��bR�f�(&'�1+��7�b� (&PL�d�|��{#_d%$�$�I����b��R�Q����'Q'���β��b���ғ�Zvtj�vu|��	(&PL>���A��@1�I<I<K�Q"�4PL����� |  @@��� �2�`�� 1�PL0`*T86L�
�3L�b/~`[�G� ��������6亓�a�M� y�6ȎN���@1�G��@�BJ  ��e����o���V���F15A1I��Ŕ�
�	��@1�b�th���; (&SL��2�������Z���~(&���	(&PL����~���}�(&�h&�brpr��Щy�p#(�*�b�d0PLVN���7�EVB�NB���:	(&��-e堘�-�zu���,[(&PL��,=�eG�FjW�gPL��@1�b������6MPLoO�~�,(&�e�'_ 8#@@f���6X�B� &@L@� L�
�S���S�؋���6�b"m m�b���$m�z�6H����Si(&PL��!%����{#% Ťm��F15��N?���s�h���$]PL
S~+(&PLz��	ӡ�b�� PL��L1�b�(��3�b�k(&S��M���
(&PL��@1�b�7f�1���~��ԣ������ůC������ �	��@1Y9!_<���Y	�:	u�$���.��y��b����Iԉv~��li��@1�b��d����]�A1�b��	�W(�f'�vA1�I<I<K�Q"�4PL����� |  @@��� �2�`�� 1�PL0`*T86L�
�3L�b/~`[�G� ��������6亓�a�M� y�6ȎN���@1�G��@�BJ  ��e�6Up�����z'lw��&1�!1I���Ĕ�
�	�� 1Ab��th���; $&SL��2�������Z���~$&���	$&HL����	~L��}�$&�h��Abrpn�␩y�n#$�*@b��d0HLVN���7�EB�NB���:	$&��-e吘�-�xu���,[$&HL��,=�eG�FjW�gHL�� 1Ab��������HL&oO�~�,$&�e�'_ 8#@@f���6X�B� &@L@� H�
�R���R�؋���6@b"m m�b���$m�z�6H����Si$&HL��!%����{#%�Ĥm�&1uRS3��F�6�GF1����G�?�n�      �   /  x���K��@���)| ����G�
	��k6fb�,e�d��\���Ĵ����Կ����f�t�oϏ����	�r$w�0u貋�!��������r<7�#h������R�&Jv�O������|�Oݧ�qx�2�>�p����������p��������0JF�ƈf� �蹙�0��D�Ї���D 
cM�C��`�S����k���(�!53�*�����R �fF�ƈf��Gf��t�[����<Uڅ��I�@��(�s�N�ىe\P�k�u��N�DcM�n�u�aƐA\�N_+�?NX>=��\����q��8�N���<�ǟ?�cx��[\g��65��7�Y�YR��P�a�Ð2�,2��H�F�iH�!g¹K��rhd��FnE�d��JO}�Z[�d���.�4��DːZD�v,o����"��Y �d�6WI�l�9��s�5e֐�4l.@Q��Z���XDm�[�xT��ʀX�x��Z7k;#e�\�,u$�v�Le�LNԮ
35)j]�W ����ޱ�v��B�ى��ڱ'�Z�y�*�(��@�r|uވ��B�۹4-݄FV}u��=�T�m���W���|��1����m���-�p5�y��)�\�* %�,�l{�Ug�� ���"j����.�%���-��7�e�jq�#Ϣ�ӽ\�2��KY�X˓��!����H�=/F�5)Y��)�������y�F�m�RF��Jl �iU�vX��0'֎�
X��n.H6W#b�E��arJ��%9�6�)9̫ܿz7�D'��w������f�>      �      x��]s�̚���~Eg�f���$��d���e�D��@]s"�ٖ�
���ɞy�Z8U&���� ����������W �
G_!�����Y_�M�R��� |�'��֛��+a_!��j�����������}��K�̟^�?l<`��?=MY4����K��,�j��_���Y�������$�?���?_�,�]���/�[oh��ї�����ԛ"��ͮ�"��cj^�e����|a����F���/���}ᗻ;��~ݾ���5ٶ,���|�w�tւw ��E�`,������3�,����^ �D�zO��;A�o� ]>�����I�HF�٣���}ֺ�F��8�	:�	�� �����	�Zh��:w �����7�B������r�['�k���	��Aפ���l۾u���=B�ȶF�'�3��a����o�]��#�&Cgd߼�!~��cc�֣p޵����������Ƶ�Sp��~e}ʕp�<l�@��'�E�u.���k�\�k��C�h+奰@e�s�-]�{�7^�M��x9;����w�\Q?%g�!�^7[u?�n4��*C5��I	j�'d>}��I(���ˉJ��{�b�9:XR��)}țrE�d��F���b�l옩�@���8oĔ�.Y
h�|
�Җ?Z�h��*�~1%���#��Z/�[��K�bU���($^�*l#�xL��ٴt�B�$�#��&�����9�_�"$�z�F��P6�:|��ۈ�F��(�1����/�v���m�(���*�D?&\ω,���C�l�|Q�.9Y,c�_.��</������.Dh��k̩3ğ��?KKZ��:���u�ep�ΐ�$���t\*��H�T�����e���a���P�*�~m�Ue�_�R�PV��^�v<_��N+,V���S��S�&+�sN'E��&Mާ��C�� Z��{8Y����<Ѝ�tV ��"_e�:֞��ns��m)w��b9����8�O|Zx�!�VQ#��D'3<^\esF�9��X�/��M)��`�v>˹�rY��Bz�$?H��'ިOd�A7�^9O���=�FS�-'��P�RP{O��U3P�4x>�H���Ϩ'd�~|S Eńb�^�J�|��t�Z�eT����ߕ',��TVt>��h)�5E�~�a��I�������霷��s�
-���5��9�c����6�>��e���w��\�uG�/B8�	Bw�"��O����bQ��#^�&�M��mS����?��k���+���0񵌐���>�I���~/�N%,����A�(T�˪}z��U�Uˀ_�+�!�'y�DҲ�ķM�ߦ����k�����L�2��Ӹ/]�����xJ'uGĸ)$|�Ȟ偞����X����e��a�u�a�Q���Ȯ㥝��#�߈j�M<�!Cj�K�=D���a�c<�Ě��ZQNr�+�����⃹���1ԁ{�|���9"<�Wx����y[,H�����>��o
tj����En�yu&�7:�đ��7V"-���ƛ��P��y���z7��Z.�XEjg��:f>� ��T���ɋ�pȁ�ѭ�Ņ��p�N3u�`��lp�!|7rO�;��'��'�a�L��]F�|6���[8��>��)��(���$�;����ɧ�Fׂ׽�7/KF&j�6$P�K�x$}x��g��&Iw\6�/Bq�a |)����B�"(�N&�)'Na�gڇUZ�-��|b�iN��}������5��	�s.7ǥU��~}��B�d�͎݁��*�`�LG��>�C:��eo<e�e37j%��j.�q)ۧ%�J4�c�����F�2��q^oE�{Q����UȍE8�"@I�{2!ߢ*��2%1��R���p�$��D7����LU�-��^A:�}t�z�F�n���S&݈�*���W<t����ߌL8��u�
��eWa-�j+W��������6C��H����v�C�!�;�^�/\g���t��`��W�k0�F�6����5�
'�������u�`<*`tJj����Oͯ�x�AO��F֭[�+(�sj(��`h�F���x1�s+�-�5�c���[�B{0�px�^��H|�G�F׶���/��ο�d, ���r�g`�`��b�C�o]y_��9G�Or�F���t~Į����'���nY��r������1��3Qvt_#��Z�X��,Bd�_�m�J[�|�
?����z��&�I�6��C!Q$�M���=���aB�b�T*q���y�m���d������7����#�`�v3��+pB����;+jI�hs�X9��|���C>���"����I�W?fʒ�0�l����~I��[Ƀ+�$T˦L��B'"�r�z�fф(��0m��p�״��>J&�mP*u@j����k�t�x)Hd��*�������v�܅^����$q��o��P�y@`��'
B����h%0��>��!	�eR�$(򅀲߳����#��qd��( }E�K��A��~�k���}��[��O�p�J��F�D��
}!B�S�#�A���N�&�F���w0@�3/C���`B�ֵ�5�h�cށ�`�0��qȥ�ͧAx��C �[O�k"�܀ڣ;�v>��\��]���;4�XΧ����=B#h�n~�k�Z�9Y�{�s����A���&�OP���}������d��)�����/E�~���,������������n_��7��P�=I�á��[�y�%���n�}�3�]
�~�2��xI�	���y�>M�vn���8�ϝ��k�o]1]�����fZ.���y}D��!?�9���/�2���6G�q?�(M����⁃,�zp^)5�s;d�w6` n�)�K	��=����g0�R���A�i^�M���u!�w�gx�s1��[_������� �G7_9��A�܏{g�eh���?t-�tA����N�m�[Gy)B�=B�!0W�q���̫.eǎ�e}z&���գ�p�{�Р=0R"GV���<LU���+�V")�c���^=����<"��v٪fI�.�T��T�´I��z�.�� �$�+�(��k,Ø�Z)P����k{5	ߊ�{�=谦濣��
�t-v�R��IH?��Z+��E�9�E��ޯsώ�E�+~����!��]���OƛL��˺T"�>aTōX��U	�񵥦���t�����H�>�U[d�{�ue��I9��:��׫\�{�Ԉ7�[�h��_ǲ\q ��z��&�J�w��r��K1~ߦ��9SC��bJ�L�l��9�����Z�o���I��Q �İ�b����:=��@˾u%q��!<������}��K�� �����x�">�� ��� �i�.Ch��;�z�KF�T
��C���e�Hj3�dӰN�H	b�l������t�y
��	�Y���gB�<hC*�o�����T�~��'��P	EC<���%?<}��ؙa�I�B��Ť|#�eM0/�Ec�%u�d�~�`��\��G�@���&k�@T�DL��Y�AV�f@�u,�r~������C����ϫP�~��bs	WL�~Vٛ���n��bT �i�\(�)*D;%��\�:�B+�[�?�HB���0Z�;�C�<����{Ui�;��EV��.Ldj8�S�nn����z�z���|�9f�h�F�%��Q
�y�W]K}���x��8o����b��nǦ�gB����@�r�{���\S��3�����;Z�[,�Z��3�1������н�m�,�Y%ʢ�[&M�jǔ��t�h=N��o��~��֟��[Ξ�򠿜��cJl$�F7u���q�s��*e�.�jt4e~2�;�����U=�g~x|�%J��bC98H���Ƅu�\d�+��w��}���.{Z�Y����9T�'D�ss���G.�ÜG�|qo�f�x|���U����r�\5\}W}|��C$$�u�%�� L
  (��t�S4���h�7��cc�`���Najb񖣒�ᠧ5^r{#P~)s�ǜ�Q��A��*�CW��x嚋Z
�ueR����z���݁=���'ߘTƬ�e6��zQ�IO2ͻpΉ���/y���R�o,&芭B����}�/J��!�?����JB���H;����o�?"����!��u?t������0�)�|!�l%��w���c��=��C�ke��*�׻�o��s��e	����Rь�Rh#�rL^u-fԿ?fLHMA$�/�i#Q釈Ѯ�C�6&/Pg�z���5���7�V�FK�2��N�y�:��x��:Z�g�l蘂n��恦��b}��<uL���S���ڊ�a+���+�\y���Wʓ��Kނ,P0F�T��eY��٤9�����JWQ�F���\�9�*�c�*�{��iW�j���*���{e��VO�1Y��ڽ�)R�R#�%�F���X��H�y�D�T�?t������V	'��O��`������)6s#�M�	�$��L�x��;���#��Wȿ<�h�FH���Ρ��"D��;8<�@��g��!~�����n_�; ���Aܥ�B:�}I����q�l��	����NU��"0����˦|�_�o�w�|U���?d������
QHT�%k4����(��y҅) �{���3"�<�)�#خ��NH� �fȆl=~��g�@��Or)�ɂ>�4a|d����o�:�2���q!·�i�Oyg�����e�w��]�ub>O���$p���7.یaG(oL�vg>[=��ޟ�U�΅�RP_��k�H���5g<��{�t�9��D�I��U��!�eXA8/&�=G�æ�V�=��jX��F{���0,�z���c�&1:�$�O��Yޖ5m�u"V�E��
���#���1�!#�}^�p`~��Ps�;��5M������~��n^C_��y��D"�O��}"���'G�cJч�'����xE��JmL�Ni[Z��qA�����+�^M���7������ݲ�x.vQ�Oo�t�����^�#�I\'��u��p#�:̍�Ik刉��Yd�Ʋ�u����'��T�P�Ƶ�Â�b�*��{X����YO��否�����y=��걗1b��NQN�YuXk��B/b�&������"����L�o�)��I����>=/}�t8#OJ�8�EĽ�_<�zM��#��c$�8v�p�M�1Na	)��i|�d���G2��ҡj�z��j��8LV�W�1ecE0��[��`���GV�}ƽ���xp��`�\A���r y����됃������¹C`�F���#W�yqM�s�1�3 �p���.<���hh"�#7^�^���f
�f
pt��p�l�u��"D�Z��v����������4���Z�u�l�IE����_��4[Nt�J�ظ�W4K�0ʫ�U�J�n���l-JWuG�/کjiz��q�"b�}�9�M���g�H�J��\����y�/��舍�|�X����ݩ@�]w*�s��Ř���)�q�*Yp��A6?��ܝe�RQO�n�U����~n�7�v����:=��}���"�TG�s.(/ˆx�t�HV��^�?��[������<Ǩ�S-��T��|�Ac�Kc�_gH�\�G����1�a�	c�9�.*�f@a�Ԉ�!ϛ�^ND��.XN���޿E�Ǎ�Λr�ԏ�p;�����+�\�y8��p�U�^{bn~�YV{���!n�k�s�y^������?��:Èѝ��~������s���A��+�~��D�s�qt�3�rFxt���k0�0Z�iCǲ�=���7����c�9thhC�y~��"t�#4n: ~���#p��=m���!�y/z1C�S�#쌬�}E,�3��:u��k�.f���8��й"�κ8?#������݀�w�x1B�=B�O�t�>t��߁���R���Sfyu��X�XSo)�,�7��S2���Otn�~eQn8�l��q���Ʉ�	��cƷ)�c�)������-�ֶ:"�iB⹭B�Ӫy�F.����\�L�0ӕ�P��C�k`�u��zƽxJ5��^��HԘ�Ñ�˩oi�#��E��x�P$�o���Z<e2$r}�%b���k/��WKVB�˩�CȚ0.�V�F'��g��hѮ)lc�֘T��?�~�E��_��$��x��h��l���D=�@�$P�y��tZ{�W���t2_��. _�`��B�otѾ2q#}�kG��{��X�	��6��J4��<LT����;�����[��C+Z]���<1����F�y�U��s�qإ5�?W}_�p����e�ݔ�w@xU$����q-����(�"��'O����݂�w`xM(Bx�����Ɗ?���߁�m�t:��Zۄ��Px�}�ʭ} ��,�O��m�z|������{�&L���[G{1C�'����CO��^Ս�s��i1����9�j��ю� �#����'�����h� �[��j���F�;�#g����K�w��d�Mx:����b��|�i� �غ��"~1��;_t�@v02�ӽu�2���_��� f�H�      �     x���=rA���S�ܥ�;"!���r�d�j��vՂ�G�����k����>I��4X�A�����w��l��$���V.����t���/����y��������o��}yw�u�+���t����m?|��}����M�l&{W���aK)���� q>�����`��Ϫ�y-e�L�ϧKAgsZ �:��!᱖(%6q9���y�C.!�٪���CR����9uV��x��	;KmF^�ޗ�w?�?�jZ����tk( �>N'���s!�i���$�tI�U1k#�P$����0̙  ��R"��z�+J��D�0[2�l�_K"U�h@T�E3{Eip�|snK�ҵ5�YG�E1JN��I �%��f�8� )�eI��Ũ��ri8M� �퓒����5󝞃��$YA:�}@�V8w�%%o��q��f������x����~�pW���k)�p�l���Om"�s�����t-�ۨ�ˈr�����L�H�y��RJ{P��o�\��f��͋�      �   C   x�3����)-�LJu(�M,*�KM)�K��*fd`d�k`�kh�`h`e`ne`�gfbblj�i����� �z
     