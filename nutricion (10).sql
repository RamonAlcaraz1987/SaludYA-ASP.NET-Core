-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 13-08-2026 a las 21:28:00
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
-- Base de datos: `nutricion`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `alimentos`
--

CREATE TABLE `alimentos` (
  `id_alimento` int(11) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `calorias` int(11) NOT NULL,
  `proteina` int(11) NOT NULL,
  `grasas` int(11) NOT NULL,
  `carbohidratos` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `alimentos`
--

INSERT INTO `alimentos` (`id_alimento`, `nombre`, `calorias`, `proteina`, `grasas`, `carbohidratos`) VALUES
(1, 'Pechuga de Pollo', 165, 31, 4, 0),
(2, 'Arroz Integral', 123, 3, 1, 26),
(3, 'Palta', 160, 2, 15, 9),
(4, 'Huevo Cocido', 155, 13, 11, 1),
(5, 'Banana', 89, 1, 0, 23);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `comidas`
--

CREATE TABLE `comidas` (
  `id_comida` int(11) NOT NULL,
  `id_usuario` int(11) NOT NULL,
  `id_alimento` int(11) NOT NULL,
  `id_tipo_comida` int(11) NOT NULL,
  `cantidad_gramos` int(11) NOT NULL,
  `fecha_registro` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `comidas`
--

INSERT INTO `comidas` (`id_comida`, `id_usuario`, `id_alimento`, `id_tipo_comida`, `cantidad_gramos`, `fecha_registro`) VALUES
(10, 3, 1, 1, 100, '2026-04-08 20:59:04'),
(11, 3, 3, 1, 100, '2026-04-08 21:00:27'),
(12, 3, 3, 1, 100, '2026-04-08 21:06:17');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `ejercicios`
--

CREATE TABLE `ejercicios` (
  `id_ejercicio` int(11) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `url_imagen` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `ejercicios`
--

INSERT INTO `ejercicios` (`id_ejercicio`, `nombre`, `url_imagen`) VALUES
(2, 'press de banca', 'bench_press'),
(3, 'sentadilla con peso', 'sentadilla'),
(4, 'bicep curl', 'bicep_curl'),
(5, 'pull up', 'pull_up');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `generos`
--

CREATE TABLE `generos` (
  `id_genero` int(11) NOT NULL,
  `nombre` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `generos`
--

INSERT INTO `generos` (`id_genero`, `nombre`) VALUES
(2, 'Femenino'),
(1, 'Masculino');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `historial_pesos`
--

CREATE TABLE `historial_pesos` (
  `id_historial` int(11) NOT NULL,
  `id_usuario` int(11) NOT NULL,
  `id_peso` int(11) NOT NULL,
  `fecha_registro` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `historial_pesos`
--

INSERT INTO `historial_pesos` (`id_historial`, `id_usuario`, `id_peso`, `fecha_registro`) VALUES
(1, 3, 70, '2026-04-13 10:16:51'),
(2, 3, 75, '2026-04-13 20:10:55'),
(3, 3, 65, '2026-04-13 20:11:23');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `niveles_actividad`
--

CREATE TABLE `niveles_actividad` (
  `id_nivel` int(11) NOT NULL,
  `nombre` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `niveles_actividad`
--

INSERT INTO `niveles_actividad` (`id_nivel`, `nombre`) VALUES
(5, 'Atleta'),
(4, 'Intenso'),
(2, 'Ligero'),
(3, 'Moderado'),
(1, 'Sedentario');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `objetivos`
--

CREATE TABLE `objetivos` (
  `id_objetivo` int(11) NOT NULL,
  `nombre` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `objetivos`
--

INSERT INTO `objetivos` (`id_objetivo`, `nombre`) VALUES
(2, 'Bajar'),
(1, 'Mantener'),
(3, 'Subir');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pesos`
--

CREATE TABLE `pesos` (
  `id_peso` int(50) NOT NULL,
  `peso` int(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pesos`
--

INSERT INTO `pesos` (`id_peso`, `peso`) VALUES
(1, 75),
(2, 75),
(65, 65),
(70, 70),
(75, 75);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `registro_ejercicios`
--

CREATE TABLE `registro_ejercicios` (
  `id_registro` int(11) NOT NULL,
  `id_usuario` int(11) NOT NULL,
  `id_ejercicio` int(11) NOT NULL,
  `repeticiones` int(11) NOT NULL,
  `series` int(11) NOT NULL,
  `peso` decimal(5,2) NOT NULL,
  `fecha_registro` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipos_comida`
--

CREATE TABLE `tipos_comida` (
  `id_tipo_comida` int(11) NOT NULL,
  `nombre` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tipos_comida`
--

INSERT INTO `tipos_comida` (`id_tipo_comida`, `nombre`) VALUES
(2, 'Almuerzo'),
(4, 'Cena'),
(1, 'Desayuno'),
(3, 'Merienda');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `id_usuario` int(11) NOT NULL,
  `nombre_usuario` varchar(50) NOT NULL,
  `contrasena` varchar(256) NOT NULL,
  `nombre` varchar(32) NOT NULL,
  `apellido` varchar(32) NOT NULL,
  `bandera` int(11) NOT NULL,
  `id_genero` int(11) NOT NULL,
  `edad` int(11) NOT NULL,
  `altura` int(11) NOT NULL,
  `id_nivel_actividad` int(11) NOT NULL,
  `id_objetivo` int(11) NOT NULL,
  `id_peso` int(11) NOT NULL,
  `calorias_diarias` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`id_usuario`, `nombre_usuario`, `contrasena`, `nombre`, `apellido`, `bandera`, `id_genero`, `edad`, `altura`, `id_nivel_actividad`, `id_objetivo`, `id_peso`, `calorias_diarias`) VALUES
(1, 'prueba123', '4Nnu7+OaOy45mZi00FHREz1mn7tYaOxT+MH2uFsXwoI=', 'ramon', 'alcaraz', 1, 1, 28, 175, 2, 1, 1, 0),
(2, 'testuser2025', 'rsK3jMGfkeWPakfm1ijWY8nNTRLTbv6F6iQRL/z4rKY=', 'soledad', 'perez', 1, 1, 30, 180, 2, 1, 2, 0),
(3, 'test', 'RzzYOC7izzsNxnlWb47BQ9JZM8UnEfjKU9x/StUV/Rg=', 'Juan', 'Perez', 1, 1, 27, 170, 1, 1, 65, 1899),
(20, 'usuario2', '+b8lBAGauq80OdALZF/iHcvhsQkBuqqLZ8jsFv6WGgo=', 'Pedro', 'Garcia', 0, 1, 25, 175, 1, 1, 1, 2000),
(22, 'google_1128988700', 'GOOGLE_LOGIN_NO_USADA', 'pedro', 'lopezee', 1, 1, 25, 177, 2, 1, 1, 1369);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `alimentos`
--
ALTER TABLE `alimentos`
  ADD PRIMARY KEY (`id_alimento`);

--
-- Indices de la tabla `comidas`
--
ALTER TABLE `comidas`
  ADD PRIMARY KEY (`id_comida`),
  ADD KEY `id_usuario` (`id_usuario`),
  ADD KEY `id_alimento` (`id_alimento`),
  ADD KEY `id_tipo_comida` (`id_tipo_comida`);

--
-- Indices de la tabla `ejercicios`
--
ALTER TABLE `ejercicios`
  ADD PRIMARY KEY (`id_ejercicio`);

--
-- Indices de la tabla `generos`
--
ALTER TABLE `generos`
  ADD PRIMARY KEY (`id_genero`),
  ADD UNIQUE KEY `nombre` (`nombre`);

--
-- Indices de la tabla `historial_pesos`
--
ALTER TABLE `historial_pesos`
  ADD PRIMARY KEY (`id_historial`),
  ADD KEY `id_usuario` (`id_usuario`);

--
-- Indices de la tabla `niveles_actividad`
--
ALTER TABLE `niveles_actividad`
  ADD PRIMARY KEY (`id_nivel`),
  ADD UNIQUE KEY `nombre` (`nombre`);

--
-- Indices de la tabla `objetivos`
--
ALTER TABLE `objetivos`
  ADD PRIMARY KEY (`id_objetivo`),
  ADD UNIQUE KEY `nombre` (`nombre`);

--
-- Indices de la tabla `pesos`
--
ALTER TABLE `pesos`
  ADD PRIMARY KEY (`id_peso`);

--
-- Indices de la tabla `registro_ejercicios`
--
ALTER TABLE `registro_ejercicios`
  ADD PRIMARY KEY (`id_registro`),
  ADD KEY `id_usuario` (`id_usuario`),
  ADD KEY `id_ejercicio` (`id_ejercicio`);

--
-- Indices de la tabla `tipos_comida`
--
ALTER TABLE `tipos_comida`
  ADD PRIMARY KEY (`id_tipo_comida`),
  ADD UNIQUE KEY `nombre` (`nombre`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`id_usuario`),
  ADD UNIQUE KEY `nombre_usuario` (`nombre_usuario`),
  ADD KEY `id_genero` (`id_genero`),
  ADD KEY `id_nivel_actividad` (`id_nivel_actividad`),
  ADD KEY `id_objetivo` (`id_objetivo`),
  ADD KEY `id_peso` (`id_peso`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `alimentos`
--
ALTER TABLE `alimentos`
  MODIFY `id_alimento` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `comidas`
--
ALTER TABLE `comidas`
  MODIFY `id_comida` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT de la tabla `ejercicios`
--
ALTER TABLE `ejercicios`
  MODIFY `id_ejercicio` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT de la tabla `generos`
--
ALTER TABLE `generos`
  MODIFY `id_genero` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `historial_pesos`
--
ALTER TABLE `historial_pesos`
  MODIFY `id_historial` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `niveles_actividad`
--
ALTER TABLE `niveles_actividad`
  MODIFY `id_nivel` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `objetivos`
--
ALTER TABLE `objetivos`
  MODIFY `id_objetivo` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `registro_ejercicios`
--
ALTER TABLE `registro_ejercicios`
  MODIFY `id_registro` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `tipos_comida`
--
ALTER TABLE `tipos_comida`
  MODIFY `id_tipo_comida` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `id_usuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `comidas`
--
ALTER TABLE `comidas`
  ADD CONSTRAINT `comidas_ibfk_1` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id_usuario`) ON DELETE CASCADE,
  ADD CONSTRAINT `comidas_ibfk_2` FOREIGN KEY (`id_alimento`) REFERENCES `alimentos` (`id_alimento`),
  ADD CONSTRAINT `comidas_ibfk_3` FOREIGN KEY (`id_tipo_comida`) REFERENCES `tipos_comida` (`id_tipo_comida`);

--
-- Filtros para la tabla `historial_pesos`
--
ALTER TABLE `historial_pesos`
  ADD CONSTRAINT `historial_pesos_ibfk_1` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id_usuario`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
