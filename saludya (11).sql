-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 13-08-2026 a las 21:49:01
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `saludya`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `auditoria`
--

CREATE TABLE `auditoria` (
  `id` int(11) NOT NULL,
  `usuario_id` int(11) NOT NULL,
  `tabla_afectada` varchar(100) NOT NULL,
  `accion` enum('INSERT','UPDATE','DELETE') NOT NULL,
  `val_anterior` text DEFAULT NULL,
  `val_nuevo` text DEFAULT NULL,
  `timestamp_op` datetime NOT NULL DEFAULT current_timestamp(),
  `ip_origen` varchar(45) DEFAULT NULL COMMENT 'IPv4 o IPv6'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `auditoria`
--

INSERT INTO `auditoria` (`id`, `usuario_id`, `tabla_afectada`, `accion`, `val_anterior`, `val_nuevo`, `timestamp_op`, `ip_origen`) VALUES
(1, 2, 'novedad_diaria', 'INSERT', NULL, 'id=3, tipo=ausencia, especialista_id=5', '2026-06-15 19:54:04', '127.0.0.1'),
(2, 2, 'novedad_diaria', 'INSERT', NULL, 'id=4, tipo=ausencia, especialista_id=4', '2026-06-16 15:09:04', '127.0.0.1'),
(3, 1, 'especialista', 'DELETE', NULL, 'id=5, nombre=Dr. Carlos Ruiz, activo=0', '2026-07-28 10:38:29', '127.0.0.1'),
(4, 1, 'especialista', 'UPDATE', NULL, 'id=5, nombre=Dr. Carlos Ruiz, activo=1', '2026-07-28 10:58:53', '127.0.0.1'),
(5, 1, 'especialista', 'DELETE', NULL, 'id=5, nombre=Dr. Carlos Ruiz, activo=0', '2026-07-28 19:07:09', '127.0.0.1'),
(6, 1, 'especialista', 'UPDATE', NULL, 'id=5, nombre=Dr. Carlos Ruiz, activo=1', '2026-07-28 19:07:13', '127.0.0.1'),
(7, 1, 'especialista', 'DELETE', NULL, 'id=5, nombre=Dr. Carlos Ruiz, activo=0', '2026-07-28 19:17:17', '127.0.0.1'),
(8, 1, 'especialista', 'UPDATE', NULL, 'id=5, nombre=Dr. Carlos Ruiz, activo=1', '2026-07-28 19:17:21', '127.0.0.1'),
(9, 1, 'catalogo_vacunas', 'DELETE', NULL, 'id=2, nombre=Hepatitis B, activo=0', '2026-07-28 20:32:46', '127.0.0.1'),
(10, 1, 'catalogo_vacunas', 'UPDATE', NULL, 'id=2, nombre=Hepatitis B, activo=1', '2026-07-28 20:32:50', '127.0.0.1'),
(11, 1, 'especialista', 'DELETE', NULL, 'id=5, nombre=Dr. Carlos Ruiz, activo=0', '2026-07-28 20:59:13', '127.0.0.1'),
(12, 1, 'catalogo_vacunas', 'DELETE', NULL, 'id=1, nombre=BCG, activo=0', '2026-07-28 20:59:49', '127.0.0.1'),
(13, 1, 'catalogo_vacunas', 'UPDATE', NULL, 'id=1, nombre=BCG, activo=1', '2026-07-28 20:59:54', '127.0.0.1'),
(14, 1, 'especialista', 'UPDATE', NULL, 'id=5, nombre=Dr. Carlos Ruiz, activo=1', '2026-07-28 21:00:09', '127.0.0.1'),
(15, 1, 'especialista', 'DELETE', NULL, 'id=5, nombre=Dr. Carlos Ruiz, activo=0', '2026-07-28 21:07:04', '127.0.0.1'),
(16, 1, 'especialista', 'UPDATE', NULL, 'id=5, nombre=Dr. Carlos Ruiz, activo=1', '2026-07-28 21:07:21', '127.0.0.1'),
(17, 1, 'especialista', 'DELETE', NULL, 'id=5, nombre=Dr. Carlos Ruiz, activo=0', '2026-07-28 21:08:41', '127.0.0.1'),
(18, 1, 'especialista', 'UPDATE', NULL, 'id=5, nombre=Dr. Carlos Ruiz, activo=1', '2026-07-29 05:52:15', '127.0.0.1'),
(19, 1, 'especialista', 'DELETE', NULL, 'id=5, nombre=Dr. Carlos Ruiz, activo=0', '2026-07-30 09:47:29', '127.0.0.1'),
(20, 1, 'especialista', 'UPDATE', NULL, 'id=5, nombre=Dr. Carlos Ruiz, activo=1', '2026-07-30 09:47:55', '127.0.0.1'),
(21, 1, 'especialista', 'DELETE', NULL, 'id=5, nombre=Dr. Carlos Ruiz, activo=0', '2026-07-30 09:48:05', '127.0.0.1'),
(22, 1, 'especialista', 'UPDATE', NULL, 'id=5, nombre=Dr. Carlos Ruiz, activo=1', '2026-07-30 09:48:39', '127.0.0.1'),
(23, 1, 'especialista', 'DELETE', NULL, 'id=5, nombre=Dr. Carlos Ruiz, activo=0', '2026-07-30 09:53:07', '127.0.0.1'),
(24, 1, 'catalogo_vacunas', 'DELETE', NULL, 'id=1, nombre=BCG, activo=0', '2026-07-30 09:55:01', '127.0.0.1'),
(25, 1, 'catalogo_vacunas', 'UPDATE', NULL, 'id=1, nombre=BCG, activo=1', '2026-07-30 09:55:16', '127.0.0.1'),
(26, 1, 'catalogo_vacunas', 'DELETE', NULL, 'id=1, nombre=BCG, activo=0', '2026-07-30 09:55:51', '127.0.0.1'),
(27, 1, 'especialista', 'UPDATE', NULL, 'id=5, nombre=Dr. Carlos Ruiz, activo=1', '2026-07-30 10:04:37', '127.0.0.1'),
(28, 1, 'catalogo_vacunas', 'UPDATE', NULL, 'id=1, nombre=BCG, activo=1', '2026-07-30 10:04:43', '127.0.0.1'),
(29, 1, 'centro_salud', 'DELETE', NULL, 'id=1, activo=0', '2026-07-30 10:15:25', '127.0.0.1'),
(30, 1, 'centro_salud', 'UPDATE', NULL, 'id=1, activo=1', '2026-07-30 10:16:38', '127.0.0.1'),
(31, 1, 'centro_salud', 'UPDATE', NULL, 'id=1, nombre=Centro de Salud Cerro de la Cruz ', '2026-07-30 10:18:02', '127.0.0.1'),
(32, 1, 'centro_salud', 'UPDATE', NULL, 'id=1, nombre=Centro de Salud Cerro de la Cruz  4', '2026-07-30 10:18:10', '127.0.0.1'),
(33, 1, 'usuario', 'UPDATE', NULL, 'id=2, email=resp1@saludya.com, rol=responsable', '2026-07-30 16:56:41', '127.0.0.1'),
(34, 1, 'centro_salud', 'UPDATE', NULL, 'id=1, nombre=Centro de Salud Cerro de la Cruz', '2026-07-30 16:58:09', '127.0.0.1'),
(35, 1, 'usuario', 'UPDATE', NULL, 'id=3, email=resp2@saludya.com, rol=responsable', '2026-07-30 16:58:23', '127.0.0.1'),
(36, 1, 'usuario', 'UPDATE', NULL, 'id=3, email=resp2@saludya.com, rol=responsable', '2026-07-30 17:06:00', '127.0.0.1'),
(37, 1, 'usuario', 'UPDATE', NULL, 'id=3, contraseña cambiada por administrador', '2026-07-30 17:06:00', '127.0.0.1'),
(38, 1, 'centro_salud', 'UPDATE', NULL, 'id=2, nombre=Centro de Salud El Lince', '2026-07-30 17:15:51', '127.0.0.1'),
(39, 1, 'centro_salud', 'UPDATE', NULL, 'id=1, nombre=Centro de Salud Cerro de la Cruz', '2026-07-30 17:16:15', '127.0.0.1'),
(40, 1, 'centro_salud', 'UPDATE', NULL, 'id=1, nombre=Centro de Salud Cerro de la Cruz', '2026-07-30 17:16:25', '127.0.0.1'),
(41, 1, 'especialista', 'UPDATE', 'centro_id=1 (Centro de Salud Cerro de la Cruz)', 'id=5, centro_id=2 (Centro de Salud El Lince)', '2026-07-31 11:08:22', '127.0.0.1'),
(42, 1, 'especialista', 'INSERT', NULL, 'id=22, nombre=jorge alaniz, especialidad=Nutrición, centro_id=1', '2026-07-31 11:09:26', '127.0.0.1'),
(43, 1, 'catalogo_vacunas', 'INSERT', NULL, 'id=7, nombre=covid, tipo=calendario_fijo', '2026-07-31 11:09:50', '127.0.0.1'),
(44, 1, 'catalogo_vacunas', 'DELETE', NULL, 'id=7, nombre=covid, activo=0', '2026-07-31 11:09:57', '127.0.0.1'),
(45, 1, 'catalogo_vacunas', 'UPDATE', NULL, 'id=7, nombre=covid, activo=1', '2026-07-31 11:10:27', '127.0.0.1'),
(46, 1, 'centro_salud', 'INSERT', NULL, 'id=4, nombre=caca', '2026-07-31 12:42:00', '127.0.0.1'),
(47, 1, 'especialista', 'DELETE', NULL, 'id=22, nombre=jorge alaniz, activo=0', '2026-07-31 12:56:33', '127.0.0.1'),
(48, 1, 'especialista', 'UPDATE', NULL, 'id=22, nombre=jorge alaniz, activo=1', '2026-07-31 13:00:52', '127.0.0.1'),
(49, 1, 'especialista', 'DELETE', NULL, 'id=22, nombre=jorge alaniz, activo=0', '2026-07-31 13:02:43', '127.0.0.1'),
(50, 2, 'vacunatorio', 'UPDATE', NULL, 'id=1, apertura=08:30:00, cierre=13:00:00, dias=lunes,martes,miercoles,jueves,viernes', '2026-07-31 14:53:14', '127.0.0.1'),
(51, 2, 'vacuna_disponible', 'UPDATE', NULL, 'vacunatorio_id=1, vacunas_actualizadas=6', '2026-07-31 14:53:34', '127.0.0.1'),
(52, 2, 'vacuna_disponible', 'UPDATE', NULL, 'vacunatorio_id=1, vacunas_actualizadas=6', '2026-07-31 14:53:45', '127.0.0.1'),
(53, 1, 'centro_salud', 'INSERT', NULL, 'id=5, nombre=centro las 9 reinas', '2026-07-31 15:17:53', '127.0.0.1'),
(54, 1, 'centro_salud', 'DELETE', NULL, 'id=5, activo=0', '2026-07-31 15:18:08', '127.0.0.1'),
(55, 1, 'especialista', 'DELETE', NULL, 'id=5, nombre=Dr. Carlos Ruiz, activo=0', '2026-07-31 15:19:17', '127.0.0.1'),
(56, 1, 'especialista', 'UPDATE', NULL, 'id=5, nombre=Dr. Carlos Ruiz, activo=1', '2026-07-31 15:19:31', '127.0.0.1'),
(57, 2, 'vacunatorio', 'UPDATE', NULL, 'id=1, apertura=09:30:00, cierre=13:00:00, dias=lunes,martes,miercoles,jueves,viernes', '2026-07-31 15:23:30', '127.0.0.1'),
(58, 2, 'vacuna_disponible', 'UPDATE', NULL, 'vacunatorio_id=1, vacunas_actualizadas=6', '2026-07-31 15:23:31', '127.0.0.1'),
(59, 2, 'vacuna_disponible', 'UPDATE', NULL, 'vacunatorio_id=1, vacunas_actualizadas=6', '2026-07-31 15:23:43', '127.0.0.1'),
(60, 2, 'novedad_diaria', 'INSERT', NULL, 'id=5, tipo=ausencia, especialista_id=5', '2026-07-31 15:24:39', '127.0.0.1'),
(61, 2, 'vacuna_disponible', 'UPDATE', NULL, 'vacunatorio_id=1, vacunas_actualizadas=6', '2026-08-07 14:38:32', '127.0.0.1'),
(62, 1, 'especialista', 'DELETE', NULL, 'id=5, nombre=Dr. Carlos Ruiz, activo=0', '2026-08-07 15:11:18', '127.0.0.1'),
(63, 1, 'especialista', 'UPDATE', NULL, 'id=5, nombre=Dr. Carlos Ruiz, activo=1', '2026-08-07 15:11:27', '127.0.0.1'),
(64, 2, 'novedad_diaria', 'INSERT', NULL, 'id=6, tipo=cambio_horario, especialista_id=5', '2026-08-07 15:17:58', '127.0.0.1'),
(65, 2, 'vacunatorio', 'UPDATE', NULL, 'id=1, apertura=10:30:00, cierre=13:00:00, dias=lunes,martes,miercoles,viernes', '2026-08-07 15:18:30', '127.0.0.1'),
(66, 2, 'vacuna_disponible', 'UPDATE', NULL, 'vacunatorio_id=1, vacunas_actualizadas=6', '2026-08-07 15:19:08', '127.0.0.1'),
(67, 2, 'vacuna_disponible', 'UPDATE', NULL, 'vacunatorio_id=1, vacunas_actualizadas=6', '2026-08-07 15:19:16', '127.0.0.1');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `catalogo_vacunas`
--

CREATE TABLE `catalogo_vacunas` (
  `id` int(11) NOT NULL,
  `nombre` varchar(150) NOT NULL,
  `tipo` enum('calendario_fijo','campana_estacional') NOT NULL,
  `franja_etaria` varchar(100) DEFAULT NULL COMMENT 'Ej: 0-6 meses, adultos, embarazadas',
  `condicion_aplicacion` text DEFAULT NULL COMMENT 'Condiciones especiales si aplica',
  `activo` tinyint(1) NOT NULL DEFAULT 1,
  `creado_por` int(11) NOT NULL,
  `fecha_creacion` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `catalogo_vacunas`
--

INSERT INTO `catalogo_vacunas` (`id`, `nombre`, `tipo`, `franja_etaria`, `condicion_aplicacion`, `activo`, `creado_por`, `fecha_creacion`) VALUES
(1, 'BCG', 'calendario_fijo', 'Recién nacidos', 'Dosis única al nacer', 1, 1, '2026-06-13 09:07:32'),
(2, 'Hepatitis B', 'calendario_fijo', '0-6 meses', 'Serie de 3 dosis', 1, 1, '2026-06-13 09:07:32'),
(3, 'Triple Bacteriana', 'calendario_fijo', '2-6 meses', 'DPT - 3 dosis + refuerzo', 1, 1, '2026-06-13 09:07:32'),
(4, 'Triple Viral', 'calendario_fijo', '12 meses / 5 años', 'Sarampión, Rubéola, Paperas', 1, 1, '2026-06-13 09:07:32'),
(5, 'Antigripal', 'campana_estacional', 'Mayores 65 / riesgo', 'Campaña anual invierno', 1, 1, '2026-06-13 09:07:32'),
(6, 'Dengue', 'campana_estacional', 'Adultos', 'Campaña según brote', 1, 1, '2026-06-13 09:07:32'),
(7, 'covid', 'calendario_fijo', 'adultos', 'refuezo', 1, 1, '2026-07-31 11:09:50');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `centro_salud`
--

CREATE TABLE `centro_salud` (
  `id` int(11) NOT NULL,
  `nombre` varchar(150) NOT NULL,
  `direccion` varchar(255) NOT NULL,
  `telefono` varchar(30) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `latitud` decimal(10,7) DEFAULT NULL,
  `longitud` decimal(10,7) DEFAULT NULL,
  `activo` tinyint(1) NOT NULL DEFAULT 1,
  `creado_en` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `centro_salud`
--

INSERT INTO `centro_salud` (`id`, `nombre`, `direccion`, `telefono`, `email`, `latitud`, `longitud`, `activo`, `creado_en`) VALUES
(1, 'Centro de Salud Cerro de la Cruz', 'Av. Presidente Illia s/n, Cerro de la Cruz, San Luis', '2664-430100', 'cerrocruz@saludya.com', -33.2954000, -66.3356000, 1, '2026-06-12 11:06:26'),
(2, 'Centro de Salud El Lince', 'Calle Los Pumas 250, Barrio El Lince, San Luis', '2664-430200', 'ellince@saludya.com', -33.3021000, -66.3412000, 1, '2026-06-12 11:06:26'),
(3, 'Centro de Salud Tres Barrios', 'Calle Rivadavia 800, Tres Barrios, San Luis', '2664-430300', 'tresbarrios@saludya.com', -33.2889000, -66.3298000, 1, '2026-06-12 11:06:26'),
(5, 'centro las 9 reinas', 'lopez 1234', NULL, 'emanuel.08.1987@gmail.com', NULL, NULL, 0, '2026-07-31 15:17:53');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `cronograma`
--

CREATE TABLE `cronograma` (
  `id` int(11) NOT NULL,
  `especialista_id` int(11) NOT NULL,
  `centro_id` int(11) NOT NULL,
  `fecha_inicio` date NOT NULL,
  `fecha_fin` date NOT NULL,
  `tipo_periodo` enum('mensual','bimestral') NOT NULL DEFAULT 'mensual',
  `turnos_disponibles` int(11) NOT NULL DEFAULT 0,
  `tipo_turno` enum('orden_llegada','turno_previo') NOT NULL DEFAULT 'orden_llegada',
  `usuario_carga_id` int(11) NOT NULL,
  `fecha_carga` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `cronograma`
--

INSERT INTO `cronograma` (`id`, `especialista_id`, `centro_id`, `fecha_inicio`, `fecha_fin`, `tipo_periodo`, `turnos_disponibles`, `tipo_turno`, `usuario_carga_id`, `fecha_carga`) VALUES
(1, 4, 1, '2026-06-01', '2026-06-30', 'mensual', 20, 'orden_llegada', 2, '2026-06-13 09:07:32'),
(2, 5, 1, '2026-06-01', '2026-06-30', 'mensual', 15, 'orden_llegada', 2, '2026-06-13 09:07:32'),
(3, 6, 1, '2026-06-01', '2026-06-30', 'mensual', 10, 'orden_llegada', 2, '2026-06-13 09:07:32'),
(4, 7, 1, '2026-06-01', '2026-06-30', 'mensual', 12, 'orden_llegada', 2, '2026-06-13 09:07:32'),
(5, 8, 1, '2026-06-01', '2026-06-30', 'mensual', 18, 'orden_llegada', 2, '2026-06-13 09:07:32'),
(6, 9, 1, '2026-06-01', '2026-06-30', 'mensual', 10, 'orden_llegada', 2, '2026-06-13 09:07:32'),
(7, 10, 2, '2026-06-01', '2026-06-30', 'mensual', 20, 'orden_llegada', 3, '2026-06-13 09:07:32'),
(8, 11, 2, '2026-06-01', '2026-06-30', 'mensual', 15, 'orden_llegada', 3, '2026-06-13 09:07:32'),
(9, 12, 2, '2026-06-01', '2026-06-30', 'mensual', 10, 'orden_llegada', 3, '2026-06-13 09:07:32'),
(10, 13, 2, '2026-06-01', '2026-06-30', 'mensual', 12, 'orden_llegada', 3, '2026-06-13 09:07:32'),
(11, 14, 2, '2026-06-01', '2026-06-30', 'mensual', 18, 'orden_llegada', 3, '2026-06-13 09:07:32'),
(12, 15, 2, '2026-06-01', '2026-06-30', 'mensual', 10, 'orden_llegada', 3, '2026-06-13 09:07:32'),
(13, 16, 3, '2026-06-01', '2026-06-30', 'mensual', 20, 'orden_llegada', 4, '2026-06-13 09:07:32'),
(14, 17, 3, '2026-06-01', '2026-06-30', 'mensual', 15, 'orden_llegada', 4, '2026-06-13 09:07:32'),
(15, 18, 3, '2026-06-01', '2026-06-30', 'mensual', 10, 'orden_llegada', 4, '2026-06-13 09:07:32'),
(16, 19, 3, '2026-06-01', '2026-06-30', 'mensual', 12, 'orden_llegada', 4, '2026-06-13 09:07:32'),
(17, 20, 3, '2026-06-01', '2026-06-30', 'mensual', 18, 'orden_llegada', 4, '2026-06-13 09:07:32'),
(18, 21, 3, '2026-06-01', '2026-06-30', 'mensual', 10, 'turno_previo', 4, '2026-06-13 09:07:32'),
(19, 4, 1, '2026-07-01', '2026-07-31', 'mensual', 20, 'orden_llegada', 2, '2026-07-05 09:12:19'),
(20, 5, 1, '2026-07-01', '2026-07-31', 'mensual', 15, 'turno_previo', 2, '2026-07-05 09:12:19'),
(21, 6, 1, '2026-07-01', '2026-07-31', 'mensual', 10, 'turno_previo', 2, '2026-07-05 09:12:19'),
(22, 7, 1, '2026-07-01', '2026-07-31', 'mensual', 12, 'orden_llegada', 2, '2026-07-05 09:12:19'),
(23, 8, 1, '2026-07-01', '2026-07-31', 'mensual', 18, 'orden_llegada', 2, '2026-07-05 09:12:19'),
(24, 9, 1, '2026-07-01', '2026-07-31', 'mensual', 10, 'turno_previo', 2, '2026-07-05 09:12:19'),
(25, 10, 2, '2026-07-01', '2026-07-31', 'mensual', 20, 'orden_llegada', 3, '2026-07-05 09:12:19'),
(26, 11, 2, '2026-07-01', '2026-07-31', 'mensual', 15, 'turno_previo', 3, '2026-07-05 09:12:19'),
(27, 12, 2, '2026-07-01', '2026-07-31', 'mensual', 10, 'turno_previo', 3, '2026-07-05 09:12:19'),
(28, 13, 2, '2026-07-01', '2026-07-31', 'mensual', 12, 'orden_llegada', 3, '2026-07-05 09:12:19'),
(29, 14, 2, '2026-07-01', '2026-07-31', 'mensual', 18, 'orden_llegada', 3, '2026-07-05 09:12:19'),
(30, 15, 2, '2026-07-01', '2026-07-31', 'mensual', 10, 'turno_previo', 3, '2026-07-05 09:12:19'),
(31, 16, 3, '2026-07-01', '2026-07-31', 'mensual', 20, 'orden_llegada', 4, '2026-07-05 09:12:19'),
(32, 17, 3, '2026-07-01', '2026-07-31', 'mensual', 15, 'turno_previo', 4, '2026-07-05 09:12:19'),
(33, 18, 3, '2026-07-01', '2026-07-31', 'mensual', 10, 'turno_previo', 4, '2026-07-05 09:12:19'),
(34, 19, 3, '2026-07-01', '2026-07-31', 'mensual', 12, 'orden_llegada', 4, '2026-07-05 09:12:19'),
(35, 20, 3, '2026-07-01', '2026-07-31', 'mensual', 18, 'orden_llegada', 4, '2026-07-05 09:12:19'),
(36, 21, 3, '2026-07-01', '2026-07-31', 'mensual', 10, 'turno_previo', 4, '2026-07-05 09:12:19'),
(37, 4, 1, '2026-08-01', '2026-08-31', 'mensual', 20, 'orden_llegada', 2, '2026-08-07 14:41:19'),
(38, 5, 1, '2026-08-01', '2026-08-31', 'mensual', 15, 'turno_previo', 2, '2026-08-07 14:41:19'),
(39, 6, 1, '2026-08-01', '2026-08-31', 'mensual', 10, 'turno_previo', 2, '2026-08-07 14:41:19'),
(40, 7, 1, '2026-08-01', '2026-08-31', 'mensual', 12, 'orden_llegada', 2, '2026-08-07 14:41:19'),
(41, 8, 1, '2026-08-01', '2026-08-31', 'mensual', 18, 'orden_llegada', 2, '2026-08-07 14:41:19'),
(42, 9, 1, '2026-08-01', '2026-08-31', 'mensual', 10, 'turno_previo', 2, '2026-08-07 14:41:19'),
(43, 10, 2, '2026-08-01', '2026-08-31', 'mensual', 20, 'orden_llegada', 3, '2026-08-07 14:41:19'),
(44, 11, 2, '2026-08-01', '2026-08-31', 'mensual', 15, 'turno_previo', 3, '2026-08-07 14:41:19'),
(45, 12, 2, '2026-08-01', '2026-08-31', 'mensual', 10, 'turno_previo', 3, '2026-08-07 14:41:19'),
(46, 13, 2, '2026-08-01', '2026-08-31', 'mensual', 12, 'orden_llegada', 3, '2026-08-07 14:41:19'),
(47, 14, 2, '2026-08-01', '2026-08-31', 'mensual', 18, 'orden_llegada', 3, '2026-08-07 14:41:19'),
(48, 15, 2, '2026-08-01', '2026-08-31', 'mensual', 10, 'turno_previo', 3, '2026-08-07 14:41:19'),
(49, 16, 3, '2026-08-01', '2026-08-31', 'mensual', 20, 'orden_llegada', 4, '2026-08-07 14:41:19'),
(50, 17, 3, '2026-08-01', '2026-08-31', 'mensual', 15, 'turno_previo', 4, '2026-08-07 14:41:19'),
(51, 18, 3, '2026-08-01', '2026-08-31', 'mensual', 10, 'turno_previo', 4, '2026-08-07 14:41:19'),
(52, 19, 3, '2026-08-01', '2026-08-31', 'mensual', 12, 'orden_llegada', 4, '2026-08-07 14:41:19'),
(53, 20, 3, '2026-08-01', '2026-08-31', 'mensual', 18, 'orden_llegada', 4, '2026-08-07 14:41:19'),
(54, 21, 3, '2026-08-01', '2026-08-31', 'mensual', 10, 'turno_previo', 4, '2026-08-07 14:41:19');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `device_tokens`
--

CREATE TABLE `device_tokens` (
  `id` int(11) NOT NULL,
  `firebase_token` varchar(255) NOT NULL,
  `centro_id` int(11) NOT NULL,
  `creado_en` datetime NOT NULL DEFAULT current_timestamp(),
  `activo` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `especialista`
--

CREATE TABLE `especialista` (
  `id` int(11) NOT NULL,
  `nombre` varchar(150) NOT NULL,
  `especialidad` varchar(100) NOT NULL,
  `centro_id` int(11) NOT NULL,
  `activo` tinyint(1) NOT NULL DEFAULT 1,
  `creado_en` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `especialista`
--

INSERT INTO `especialista` (`id`, `nombre`, `especialidad`, `centro_id`, `activo`, `creado_en`) VALUES
(4, 'Dra. Laura Fernández', 'Pediatría', 1, 1, '2026-06-12 11:06:26'),
(5, 'Dr. Carlos Ruiz', 'Clínica Médica', 1, 1, '2026-06-12 11:06:26'),
(6, 'Dra. Mónica Gómez', 'Gastroenterología', 1, 1, '2026-06-12 11:06:26'),
(7, 'Lic. Valeria Torres', 'Nutrición', 1, 1, '2026-06-12 11:06:26'),
(8, 'Dr. Héctor Sosa', 'Odontología', 1, 1, '2026-06-12 11:06:26'),
(9, 'Lic. Gabriela Martín', 'Psicología', 1, 1, '2026-06-12 11:06:26'),
(10, 'Dr. Ignacio Paz', 'Pediatría', 2, 1, '2026-06-12 11:06:26'),
(11, 'Dra. Silvia Romero', 'Clínica Médica', 2, 1, '2026-06-12 11:06:26'),
(12, 'Dr. Matías Herrera', 'Gastroenterología', 2, 1, '2026-06-12 11:06:26'),
(13, 'Lic. Ana Lucía Vera', 'Nutrición', 2, 1, '2026-06-12 11:06:26'),
(14, 'Dra. Patricia Juárez', 'Odontología', 2, 1, '2026-06-12 11:06:26'),
(15, 'Lic. Roberto Medina', 'Psicología', 2, 1, '2026-06-12 11:06:26'),
(16, 'Dra. Florencia Álvarez', 'Pediatría', 3, 1, '2026-06-12 11:06:26'),
(17, 'Dr. Sebastián Molina', 'Clínica Médica', 3, 1, '2026-06-12 11:06:26'),
(18, 'Dra. Claudia Ramos', 'Gastroenterología', 3, 1, '2026-06-12 11:06:26'),
(19, 'Lic. Diego Ortiz', 'Nutrición', 3, 1, '2026-06-12 11:06:26'),
(20, 'Dr. Fernando Aguirre', 'Odontología', 3, 1, '2026-06-12 11:06:26'),
(21, 'Lic. Natalia Suárez', 'Psicología', 3, 1, '2026-06-12 11:06:26'),
(22, 'jorge alaniz', 'Nutrición', 1, 0, '2026-07-31 11:09:26');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `horario_cronograma`
--

CREATE TABLE `horario_cronograma` (
  `id` int(11) NOT NULL,
  `cronograma_id` int(11) NOT NULL,
  `dia_semana` enum('lunes','martes','miercoles','jueves','viernes','sabado') NOT NULL,
  `hora_inicio` time NOT NULL,
  `hora_fin` time NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `horario_cronograma`
--

INSERT INTO `horario_cronograma` (`id`, `cronograma_id`, `dia_semana`, `hora_inicio`, `hora_fin`) VALUES
(1, 1, 'lunes', '08:00:00', '12:00:00'),
(2, 1, 'miercoles', '08:00:00', '12:00:00'),
(3, 2, 'martes', '08:00:00', '12:00:00'),
(4, 2, 'jueves', '08:00:00', '12:00:00'),
(5, 3, 'viernes', '08:00:00', '13:00:00'),
(6, 4, 'lunes', '13:00:00', '17:00:00'),
(7, 4, 'jueves', '13:00:00', '17:00:00'),
(8, 5, 'martes', '08:00:00', '13:00:00'),
(9, 5, 'viernes', '08:00:00', '13:00:00'),
(10, 6, 'miercoles', '13:00:00', '17:00:00'),
(11, 7, 'lunes', '08:00:00', '12:00:00'),
(12, 7, 'jueves', '08:00:00', '12:00:00'),
(13, 8, 'martes', '08:00:00', '12:00:00'),
(14, 8, 'viernes', '08:00:00', '12:00:00'),
(15, 9, 'miercoles', '08:00:00', '13:00:00'),
(16, 10, 'lunes', '13:00:00', '17:00:00'),
(17, 10, 'miercoles', '13:00:00', '17:00:00'),
(18, 11, 'martes', '08:00:00', '13:00:00'),
(19, 11, 'jueves', '08:00:00', '13:00:00'),
(20, 12, 'viernes', '13:00:00', '17:00:00'),
(21, 13, 'lunes', '08:00:00', '12:00:00'),
(22, 13, 'miercoles', '08:00:00', '12:00:00'),
(23, 14, 'martes', '08:00:00', '12:00:00'),
(24, 14, 'jueves', '08:00:00', '12:00:00'),
(25, 15, 'viernes', '08:00:00', '13:00:00'),
(26, 16, 'lunes', '13:00:00', '17:00:00'),
(27, 16, 'jueves', '13:00:00', '17:00:00'),
(28, 17, 'martes', '08:00:00', '13:00:00'),
(29, 17, 'viernes', '08:00:00', '13:00:00'),
(30, 18, 'miercoles', '13:00:00', '17:00:00'),
(31, 19, 'lunes', '08:00:00', '12:00:00'),
(32, 19, 'miercoles', '08:00:00', '12:00:00'),
(33, 20, 'martes', '08:00:00', '12:00:00'),
(34, 20, 'jueves', '08:00:00', '12:00:00'),
(35, 21, 'viernes', '08:00:00', '13:00:00'),
(36, 22, 'lunes', '13:00:00', '17:00:00'),
(37, 22, 'jueves', '13:00:00', '17:00:00'),
(38, 23, 'martes', '08:00:00', '13:00:00'),
(39, 23, 'viernes', '08:00:00', '13:00:00'),
(40, 24, 'miercoles', '13:00:00', '17:00:00'),
(41, 25, 'lunes', '08:00:00', '12:00:00'),
(42, 25, 'jueves', '08:00:00', '12:00:00'),
(43, 26, 'martes', '08:00:00', '12:00:00'),
(44, 26, 'viernes', '08:00:00', '12:00:00'),
(45, 27, 'miercoles', '08:00:00', '13:00:00'),
(46, 28, 'lunes', '13:00:00', '17:00:00'),
(47, 28, 'miercoles', '13:00:00', '17:00:00'),
(48, 29, 'martes', '08:00:00', '13:00:00'),
(49, 29, 'jueves', '08:00:00', '13:00:00'),
(50, 30, 'viernes', '13:00:00', '17:00:00'),
(51, 31, 'lunes', '08:00:00', '12:00:00'),
(52, 31, 'miercoles', '08:00:00', '12:00:00'),
(53, 32, 'martes', '08:00:00', '12:00:00'),
(54, 32, 'jueves', '08:00:00', '12:00:00'),
(55, 33, 'viernes', '08:00:00', '13:00:00'),
(56, 34, 'lunes', '13:00:00', '17:00:00'),
(57, 34, 'jueves', '13:00:00', '17:00:00'),
(58, 35, 'martes', '08:00:00', '13:00:00'),
(59, 35, 'viernes', '08:00:00', '13:00:00'),
(60, 36, 'miercoles', '13:00:00', '17:00:00'),
(61, 37, 'lunes', '08:00:00', '12:00:00'),
(62, 37, 'miercoles', '08:00:00', '12:00:00'),
(63, 38, 'martes', '08:00:00', '12:00:00'),
(64, 38, 'jueves', '08:00:00', '12:00:00'),
(65, 39, 'viernes', '08:00:00', '13:00:00'),
(66, 40, 'lunes', '13:00:00', '17:00:00'),
(67, 40, 'jueves', '13:00:00', '17:00:00'),
(68, 41, 'martes', '08:00:00', '13:00:00'),
(69, 41, 'viernes', '08:00:00', '13:00:00'),
(70, 42, 'miercoles', '13:00:00', '17:00:00'),
(71, 43, 'lunes', '08:00:00', '12:00:00'),
(72, 43, 'jueves', '08:00:00', '12:00:00'),
(73, 44, 'martes', '08:00:00', '12:00:00'),
(74, 44, 'viernes', '08:00:00', '12:00:00'),
(75, 45, 'miercoles', '08:00:00', '13:00:00'),
(76, 46, 'lunes', '13:00:00', '17:00:00'),
(77, 46, 'miercoles', '13:00:00', '17:00:00'),
(78, 47, 'martes', '08:00:00', '13:00:00'),
(79, 47, 'jueves', '08:00:00', '13:00:00'),
(80, 48, 'viernes', '13:00:00', '17:00:00'),
(81, 49, 'lunes', '08:00:00', '12:00:00'),
(82, 49, 'miercoles', '08:00:00', '12:00:00'),
(83, 50, 'martes', '08:00:00', '12:00:00'),
(84, 50, 'jueves', '08:00:00', '12:00:00'),
(85, 51, 'viernes', '08:00:00', '13:00:00'),
(86, 52, 'lunes', '13:00:00', '17:00:00'),
(87, 52, 'jueves', '13:00:00', '17:00:00'),
(88, 53, 'martes', '08:00:00', '13:00:00'),
(89, 53, 'viernes', '08:00:00', '13:00:00'),
(90, 54, 'miercoles', '13:00:00', '17:00:00');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `horario_turnos`
--

CREATE TABLE `horario_turnos` (
  `id` int(11) NOT NULL,
  `cronograma_id` int(11) NOT NULL,
  `mismo_dia` tinyint(1) NOT NULL DEFAULT 0 COMMENT '1 = el mismo día de atención',
  `dia_semana` enum('lunes','martes','miercoles','jueves','viernes','sabado') DEFAULT NULL COMMENT 'Si mismo_dia=0, qué día se sacan',
  `hora_inicio` time NOT NULL COMMENT 'Desde qué hora se empiezan a dar turnos',
  `hora_fin` time NOT NULL COMMENT 'Hasta qué hora se dan turnos',
  `observaciones` varchar(255) DEFAULT NULL COMMENT 'Ej: Presentarse con DNI, cupo limitado a 15 personas'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `horario_turnos`
--

INSERT INTO `horario_turnos` (`id`, `cronograma_id`, `mismo_dia`, `dia_semana`, `hora_inicio`, `hora_fin`, `observaciones`) VALUES
(1, 1, 1, NULL, '07:00:00', '08:00:00', 'Cupo: 20 personas. Presentarse con libreta sanitaria'),
(2, 2, 0, 'lunes', '06:00:00', '08:00:00', 'Turnos para toda la semana. Llevar DNI'),
(3, 3, 1, NULL, '07:30:00', '08:30:00', 'Cupo limitado a 10 personas'),
(4, 4, 1, NULL, '12:30:00', '13:00:00', NULL),
(5, 5, 0, 'lunes', '07:00:00', '09:00:00', 'Turnos para el martes. También viernes de 07:00 a 09:00'),
(6, 6, 0, 'martes', '08:00:00', '09:00:00', 'Cupo: 10 personas'),
(7, 7, 1, NULL, '07:00:00', '08:00:00', 'Mismo día, cupo 20'),
(8, 8, 0, 'lunes', '06:30:00', '08:00:00', 'Turnos para toda la semana'),
(9, 9, 1, NULL, '07:30:00', '08:30:00', NULL),
(10, 10, 1, NULL, '12:30:00', '13:00:00', NULL),
(11, 11, 0, 'lunes', '07:00:00', '09:00:00', NULL),
(12, 12, 0, 'jueves', '08:00:00', '09:00:00', NULL),
(13, 13, 1, NULL, '07:00:00', '08:00:00', 'Cupo 20 personas'),
(14, 14, 0, 'lunes', '06:00:00', '08:00:00', 'Turnos para toda la semana'),
(15, 15, 1, NULL, '07:30:00', '08:30:00', NULL),
(16, 16, 1, NULL, '12:30:00', '13:00:00', NULL),
(17, 17, 0, 'lunes', '07:00:00', '09:00:00', NULL),
(18, 18, 0, 'miercoles', '08:00:00', '09:00:00', NULL),
(19, 19, 1, NULL, '07:00:00', '08:00:00', 'Cupo 20 personas. Traer libreta sanitaria'),
(20, 20, 0, 'lunes', '06:00:00', '08:00:00', 'Turnos para toda la semana. Traer DNI'),
(21, 21, 1, NULL, '07:30:00', '08:30:00', 'Cupo limitado a 10 personas'),
(22, 22, 1, NULL, '12:30:00', '13:00:00', NULL),
(23, 23, 0, 'lunes', '07:00:00', '09:00:00', NULL),
(24, 24, 0, 'martes', '08:00:00', '09:00:00', 'Cupo 10 personas'),
(25, 25, 1, NULL, '07:00:00', '08:00:00', 'Cupo 20 personas'),
(26, 26, 0, 'lunes', '06:30:00', '08:00:00', 'Turnos para toda la semana'),
(27, 27, 1, NULL, '07:30:00', '08:30:00', NULL),
(28, 28, 1, NULL, '12:30:00', '13:00:00', NULL),
(29, 29, 0, 'lunes', '07:00:00', '09:00:00', NULL),
(30, 30, 0, 'jueves', '08:00:00', '09:00:00', NULL),
(31, 31, 1, NULL, '07:00:00', '08:00:00', 'Cupo 20 personas'),
(32, 32, 0, 'lunes', '06:00:00', '08:00:00', 'Turnos para toda la semana'),
(33, 33, 1, NULL, '07:30:00', '08:30:00', NULL),
(34, 34, 1, NULL, '12:30:00', '13:00:00', NULL),
(35, 35, 0, 'lunes', '07:00:00', '09:00:00', NULL),
(36, 36, 0, 'miercoles', '08:00:00', '09:00:00', NULL),
(37, 37, 1, NULL, '07:00:00', '08:00:00', 'Cupo 20 personas. Traer libreta sanitaria'),
(38, 38, 0, 'lunes', '06:00:00', '08:00:00', 'Turnos para toda la semana. Traer DNI'),
(39, 39, 1, NULL, '07:30:00', '08:30:00', 'Cupo limitado a 10 personas'),
(40, 40, 1, NULL, '12:30:00', '13:00:00', NULL),
(41, 41, 0, 'lunes', '07:00:00', '09:00:00', NULL),
(42, 42, 0, 'martes', '08:00:00', '09:00:00', 'Cupo 10 personas'),
(43, 43, 1, NULL, '07:00:00', '08:00:00', 'Cupo 20 personas'),
(44, 44, 0, 'lunes', '06:30:00', '08:00:00', 'Turnos para toda la semana'),
(45, 45, 1, NULL, '07:30:00', '08:30:00', NULL),
(46, 46, 1, NULL, '12:30:00', '13:00:00', NULL),
(47, 47, 0, 'lunes', '07:00:00', '09:00:00', NULL),
(48, 48, 0, 'jueves', '08:00:00', '09:00:00', NULL),
(49, 49, 1, NULL, '07:00:00', '08:00:00', 'Cupo 20 personas'),
(50, 50, 0, 'lunes', '06:00:00', '08:00:00', 'Turnos para toda la semana'),
(51, 51, 1, NULL, '07:30:00', '08:30:00', NULL),
(52, 52, 1, NULL, '12:30:00', '13:00:00', NULL),
(53, 53, 0, 'lunes', '07:00:00', '09:00:00', NULL),
(54, 54, 0, 'miercoles', '08:00:00', '09:00:00', NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `novedad_diaria`
--

CREATE TABLE `novedad_diaria` (
  `id` int(11) NOT NULL,
  `especialista_id` int(11) NOT NULL,
  `centro_id` int(11) NOT NULL,
  `fecha` date NOT NULL,
  `tipo_novedad` enum('ausencia','cambio_horario','reduccion_turnos','otro') NOT NULL,
  `descripcion` text DEFAULT NULL,
  `hora_nueva_inicio` time DEFAULT NULL COMMENT 'Si hay cambio de horario, nuevo inicio',
  `hora_nueva_fin` time DEFAULT NULL COMMENT 'Si hay cambio de horario, nuevo fin',
  `lugar_nuevo` varchar(255) DEFAULT NULL COMMENT 'Si se traslada a otro lugar',
  `usuario_carga_id` int(11) NOT NULL,
  `fecha_registro` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `novedad_diaria`
--

INSERT INTO `novedad_diaria` (`id`, `especialista_id`, `centro_id`, `fecha`, `tipo_novedad`, `descripcion`, `hora_nueva_inicio`, `hora_nueva_fin`, `lugar_nuevo`, `usuario_carga_id`, `fecha_registro`) VALUES
(1, 4, 1, '2026-06-14', 'cambio_horario', 'La Dra. Fernández atiende más tarde hoy', '14:00:00', '16:00:00', NULL, 2, '2026-06-13 18:08:25'),
(2, 5, 1, '2026-06-13', 'ausencia', 'El Dr. Ruiz no atiende hoy por enfermedad', NULL, NULL, NULL, 2, '2026-06-13 18:08:25'),
(3, 5, 1, '2026-06-15', 'ausencia', 'no asiste', NULL, NULL, NULL, 2, '2026-06-15 19:54:04'),
(4, 4, 1, '2026-06-16', 'ausencia', 'gripe a ausencia.', NULL, NULL, NULL, 2, '2026-06-16 15:09:04'),
(5, 5, 1, '2026-07-31', 'ausencia', 'falto por temas personales', NULL, NULL, NULL, 2, '2026-07-31 15:24:39'),
(6, 5, 1, '2026-08-10', 'cambio_horario', 'reduccion de horario ', NULL, NULL, NULL, 2, '2026-08-07 15:17:58');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `id` int(11) NOT NULL,
  `nombre` varchar(150) NOT NULL,
  `email` varchar(150) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `rol` enum('superadmin','responsable','ciudadano') NOT NULL DEFAULT 'ciudadano',
  `centro_id` int(11) DEFAULT NULL,
  `ultimo_login` datetime DEFAULT NULL,
  `activo` tinyint(1) NOT NULL DEFAULT 1,
  `creado_en` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`id`, `nombre`, `email`, `password_hash`, `rol`, `centro_id`, `ultimo_login`, `activo`, `creado_en`) VALUES
(1, 'Administrador', 'admin@saludya.com', 'reciZMBiftw3+tJz69/RfRJrzX9j87yroT01KruqUYo=', 'superadmin', NULL, NULL, 1, '2026-06-13 09:07:32'),
(2, 'Resp. Cerro de la Cruz', 'resp1@saludya.com', 'jcBwnJKkp0JUS3XhBlJRB69MUqzO0CA9qF/J6bz2Zlw=', 'responsable', 1, NULL, 1, '2026-06-13 09:07:32'),
(3, 'Resp. El Lince', 'resp2@saludya.com', 'VlTh54lKIqdcColHtKQ/H2NWoKdVBAPH9Ua3+tirLbw=', 'responsable', 2, NULL, 1, '2026-06-13 09:07:32'),
(4, 'Resp. Tres Barrios', 'resp3@saludya.com', 'BWyZWGzTKOC3Lcqk9KnPubFa3tx0N6iGCEmqnC2S/Nw=', 'responsable', 3, NULL, 1, '2026-06-13 09:07:32');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `vacunatorio`
--

CREATE TABLE `vacunatorio` (
  `id` int(11) NOT NULL,
  `centro_id` int(11) NOT NULL,
  `hora_apertura` time NOT NULL,
  `hora_cierre` time NOT NULL,
  `dias_atencion` varchar(100) NOT NULL COMMENT 'Ej: lunes,miercoles,viernes',
  `activo` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `vacunatorio`
--

INSERT INTO `vacunatorio` (`id`, `centro_id`, `hora_apertura`, `hora_cierre`, `dias_atencion`, `activo`) VALUES
(1, 1, '10:30:00', '13:00:00', 'lunes,martes,miercoles,viernes', 1),
(2, 2, '08:00:00', '13:00:00', 'lunes,miercoles,viernes', 1),
(3, 3, '08:00:00', '14:00:00', 'lunes,martes,miercoles,jueves,viernes', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `vacuna_disponible`
--

CREATE TABLE `vacuna_disponible` (
  `id` int(11) NOT NULL,
  `vacunatorio_id` int(11) NOT NULL,
  `catalogo_vacuna_id` int(11) NOT NULL,
  `disponible` tinyint(1) NOT NULL DEFAULT 1,
  `observaciones` text DEFAULT NULL COMMENT 'Ej: stock agotado hasta el viernes',
  `usuario_carga_id` int(11) NOT NULL,
  `ultima_actualizacion` datetime NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `vacuna_disponible`
--

INSERT INTO `vacuna_disponible` (`id`, `vacunatorio_id`, `catalogo_vacuna_id`, `disponible`, `observaciones`, `usuario_carga_id`, `ultima_actualizacion`) VALUES
(13, 1, 1, 1, NULL, 2, '2026-08-07 15:19:16'),
(14, 1, 2, 1, NULL, 2, '2026-08-07 15:19:16'),
(15, 1, 3, 1, NULL, 2, '2026-08-07 15:19:16'),
(16, 1, 4, 1, NULL, 2, '2026-08-07 15:19:16'),
(17, 1, 5, 1, NULL, 2, '2026-08-07 15:19:16'),
(18, 1, 6, 0, 'esta agotada hasta nuevo aviso', 2, '2026-08-07 15:19:16'),
(19, 2, 1, 1, NULL, 3, '2026-06-13 09:07:32'),
(20, 2, 2, 1, NULL, 3, '2026-06-13 09:07:32'),
(21, 2, 3, 0, 'Sin stock hasta la próxima semana', 3, '2026-06-13 09:07:32'),
(22, 2, 4, 1, NULL, 3, '2026-06-13 09:07:32'),
(23, 2, 5, 1, NULL, 3, '2026-06-13 09:07:32'),
(24, 2, 6, 0, 'Pendiente de entrega', 3, '2026-06-13 09:07:32'),
(25, 3, 1, 1, NULL, 4, '2026-06-13 09:07:32'),
(26, 3, 2, 1, NULL, 4, '2026-06-13 09:07:32'),
(27, 3, 3, 1, NULL, 4, '2026-06-13 09:07:32'),
(28, 3, 4, 0, 'Consultar disponibilidad por teléfono', 4, '2026-06-13 09:07:32'),
(29, 3, 5, 1, NULL, 4, '2026-06-13 09:07:32'),
(30, 3, 6, 1, NULL, 4, '2026-06-13 09:07:32');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `auditoria`
--
ALTER TABLE `auditoria`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_auditoria_usuario` (`usuario_id`);

--
-- Indices de la tabla `catalogo_vacunas`
--
ALTER TABLE `catalogo_vacunas`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_catalogo_usuario` (`creado_por`);

--
-- Indices de la tabla `centro_salud`
--
ALTER TABLE `centro_salud`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `cronograma`
--
ALTER TABLE `cronograma`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_cronograma_especialista` (`especialista_id`),
  ADD KEY `fk_cronograma_centro` (`centro_id`),
  ADD KEY `fk_cronograma_usuario` (`usuario_carga_id`);

--
-- Indices de la tabla `device_tokens`
--
ALTER TABLE `device_tokens`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `uq_token_centro` (`firebase_token`,`centro_id`),
  ADD KEY `fk_dt_centro` (`centro_id`);

--
-- Indices de la tabla `especialista`
--
ALTER TABLE `especialista`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_especialista_centro` (`centro_id`);

--
-- Indices de la tabla `horario_cronograma`
--
ALTER TABLE `horario_cronograma`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_horario_cronograma` (`cronograma_id`);

--
-- Indices de la tabla `horario_turnos`
--
ALTER TABLE `horario_turnos`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_ht_cronograma` (`cronograma_id`);

--
-- Indices de la tabla `novedad_diaria`
--
ALTER TABLE `novedad_diaria`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_novedad_especialista` (`especialista_id`),
  ADD KEY `fk_novedad_centro` (`centro_id`),
  ADD KEY `fk_novedad_usuario` (`usuario_carga_id`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `uq_usuario_email` (`email`),
  ADD KEY `fk_usuario_centro` (`centro_id`);

--
-- Indices de la tabla `vacunatorio`
--
ALTER TABLE `vacunatorio`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `uq_vacunatorio_centro` (`centro_id`);

--
-- Indices de la tabla `vacuna_disponible`
--
ALTER TABLE `vacuna_disponible`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `uq_vacuna_x_vacunatorio` (`vacunatorio_id`,`catalogo_vacuna_id`),
  ADD KEY `fk_vd_catalogo` (`catalogo_vacuna_id`),
  ADD KEY `fk_vd_usuario` (`usuario_carga_id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `auditoria`
--
ALTER TABLE `auditoria`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=68;

--
-- AUTO_INCREMENT de la tabla `catalogo_vacunas`
--
ALTER TABLE `catalogo_vacunas`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `centro_salud`
--
ALTER TABLE `centro_salud`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `cronograma`
--
ALTER TABLE `cronograma`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=55;

--
-- AUTO_INCREMENT de la tabla `device_tokens`
--
ALTER TABLE `device_tokens`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `especialista`
--
ALTER TABLE `especialista`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;

--
-- AUTO_INCREMENT de la tabla `horario_cronograma`
--
ALTER TABLE `horario_cronograma`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=91;

--
-- AUTO_INCREMENT de la tabla `horario_turnos`
--
ALTER TABLE `horario_turnos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=55;

--
-- AUTO_INCREMENT de la tabla `novedad_diaria`
--
ALTER TABLE `novedad_diaria`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `vacunatorio`
--
ALTER TABLE `vacunatorio`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `vacuna_disponible`
--
ALTER TABLE `vacuna_disponible`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=73;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `auditoria`
--
ALTER TABLE `auditoria`
  ADD CONSTRAINT `fk_auditoria_usuario` FOREIGN KEY (`usuario_id`) REFERENCES `usuario` (`id`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `catalogo_vacunas`
--
ALTER TABLE `catalogo_vacunas`
  ADD CONSTRAINT `fk_catalogo_usuario` FOREIGN KEY (`creado_por`) REFERENCES `usuario` (`id`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `cronograma`
--
ALTER TABLE `cronograma`
  ADD CONSTRAINT `fk_cronograma_centro` FOREIGN KEY (`centro_id`) REFERENCES `centro_salud` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_cronograma_especialista` FOREIGN KEY (`especialista_id`) REFERENCES `especialista` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_cronograma_usuario` FOREIGN KEY (`usuario_carga_id`) REFERENCES `usuario` (`id`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `device_tokens`
--
ALTER TABLE `device_tokens`
  ADD CONSTRAINT `fk_dt_centro` FOREIGN KEY (`centro_id`) REFERENCES `centro_salud` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `especialista`
--
ALTER TABLE `especialista`
  ADD CONSTRAINT `fk_especialista_centro` FOREIGN KEY (`centro_id`) REFERENCES `centro_salud` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `horario_cronograma`
--
ALTER TABLE `horario_cronograma`
  ADD CONSTRAINT `fk_horario_cronograma` FOREIGN KEY (`cronograma_id`) REFERENCES `cronograma` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `horario_turnos`
--
ALTER TABLE `horario_turnos`
  ADD CONSTRAINT `fk_ht_cronograma` FOREIGN KEY (`cronograma_id`) REFERENCES `cronograma` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `novedad_diaria`
--
ALTER TABLE `novedad_diaria`
  ADD CONSTRAINT `fk_novedad_centro` FOREIGN KEY (`centro_id`) REFERENCES `centro_salud` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_novedad_especialista` FOREIGN KEY (`especialista_id`) REFERENCES `especialista` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_novedad_usuario` FOREIGN KEY (`usuario_carga_id`) REFERENCES `usuario` (`id`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD CONSTRAINT `fk_usuario_centro` FOREIGN KEY (`centro_id`) REFERENCES `centro_salud` (`id`) ON DELETE SET NULL ON UPDATE CASCADE;

--
-- Filtros para la tabla `vacunatorio`
--
ALTER TABLE `vacunatorio`
  ADD CONSTRAINT `fk_vacunatorio_centro` FOREIGN KEY (`centro_id`) REFERENCES `centro_salud` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `vacuna_disponible`
--
ALTER TABLE `vacuna_disponible`
  ADD CONSTRAINT `fk_vd_catalogo` FOREIGN KEY (`catalogo_vacuna_id`) REFERENCES `catalogo_vacunas` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_vd_usuario` FOREIGN KEY (`usuario_carga_id`) REFERENCES `usuario` (`id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_vd_vacunatorio` FOREIGN KEY (`vacunatorio_id`) REFERENCES `vacunatorio` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
