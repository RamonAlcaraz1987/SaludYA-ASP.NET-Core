using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace SaludYa.Models
{
    // ════════════════════════════════════════════════════════════════════════════
    // REPOSITORIO USUARIO
    // ════════════════════════════════════════════════════════════════════════════
    public class RepositorioUsuario : RepositorioBase, IRepositorioUsuario
    {
        public RepositorioUsuario(IConfiguration configuration) : base(configuration) { }

        public int Alta(Usuario e)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = @"INSERT INTO usuario (nombre, email, password_hash, rol, centro_id)
                          VALUES (@Nombre, @Email, @PasswordHash, @Rol, @CentroId)";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Nombre", e.Nombre);
            cmd.Parameters.AddWithValue("@Email", e.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", e.PasswordHash);
            cmd.Parameters.AddWithValue("@Rol", e.Rol);
            cmd.Parameters.AddWithValue("@CentroId", (object?)e.CentroId ?? DBNull.Value);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT LAST_INSERT_ID()";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public Usuario? ObtenerPorEmail(string email)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = "SELECT * FROM usuario WHERE email = @Email AND activo = 1";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Email", email);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapUsuario(reader) : null;
        }

        public Usuario? ObtenerPorId(int id)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = "SELECT * FROM usuario WHERE id = @Id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapUsuario(reader) : null;
        }

        public IList<Usuario> ObtenerTodos()
        {
            var lista = new List<Usuario>();
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = "SELECT * FROM usuario ORDER BY nombre";
            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) lista.Add(MapUsuario(reader));
            return lista;
        }

        public int Actualizar(Usuario e)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = @"UPDATE usuario SET nombre=@Nombre, email=@Email, rol=@Rol, centro_id=@CentroId
                          WHERE id=@Id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", e.Id);
            cmd.Parameters.AddWithValue("@Nombre", e.Nombre);
            cmd.Parameters.AddWithValue("@Email", e.Email);
            cmd.Parameters.AddWithValue("@Rol", e.Rol);
            cmd.Parameters.AddWithValue("@CentroId", (object?)e.CentroId ?? DBNull.Value);
            return cmd.ExecuteNonQuery();
        }

        public void CambiarContrasena(int idUsuario, string hashNuevo)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = "UPDATE usuario SET password_hash=@Hash WHERE id=@Id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Hash", hashNuevo);
            cmd.Parameters.AddWithValue("@Id", idUsuario);
            cmd.ExecuteNonQuery();
        }

        public void DesactivarUsuario(int id) => CambiarEstadoUsuario(id, false);

        // NUEVO: método genérico para activar / desactivar usuarios
        public void CambiarEstadoUsuario(int id, bool activo)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = "UPDATE usuario SET activo=@Activo WHERE id=@Id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Activo", activo);
            cmd.ExecuteNonQuery();
        }

        private static Usuario MapUsuario(MySqlDataReader r) => new()
        {
            Id = r.GetInt32("id"),
            Nombre = r.GetString("nombre"),
            Email = r.GetString("email"),
            PasswordHash = r.GetString("password_hash"),
            Rol = r.GetString("rol"),
            CentroId = r.IsDBNull(r.GetOrdinal("centro_id")) ? null : r.GetInt32("centro_id"),
            UltimoLogin = r.IsDBNull(r.GetOrdinal("ultimo_login")) ? null : r.GetDateTime("ultimo_login"),
            Activo = r.GetBoolean("activo"),
            CreadoEn = r.GetDateTime("creado_en")
        };
    }
      // ════════════════════════════════════════════════════════════════════════════
    // REPOSITORIO CENTRO DE SALUD
    // ════════════════════════════════════════════════════════════════════════════
    public class RepositorioCentroSalud : RepositorioBase, IRepositorioCentroSalud
    {
        public RepositorioCentroSalud(IConfiguration configuration) : base(configuration) { }

        // NUEVO: soloActivos=false permite ver también los centros desactivados (gestión superadmin)
        public IList<CentroSalud> ObtenerTodos(bool soloActivos = true)
        {
            var lista = new List<CentroSalud>();
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = soloActivos
                ? "SELECT * FROM centro_salud WHERE activo=1 ORDER BY nombre"
                : "SELECT * FROM centro_salud ORDER BY activo DESC, nombre";
            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) lista.Add(MapCentro(reader));
            return lista;
        }

        public CentroSalud? ObtenerPorId(int id)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = "SELECT * FROM centro_salud WHERE id=@Id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapCentro(reader) : null;
        }

        public int Alta(CentroSalud e)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = @"INSERT INTO centro_salud (nombre, direccion, telefono, email, latitud, longitud)
                          VALUES (@Nombre, @Direccion, @Telefono, @Email, @Latitud, @Longitud)";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Nombre", e.Nombre);
            cmd.Parameters.AddWithValue("@Direccion", e.Direccion);
            cmd.Parameters.AddWithValue("@Telefono", (object?)e.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object?)e.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Latitud", (object?)e.Latitud ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Longitud", (object?)e.Longitud ?? DBNull.Value);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT LAST_INSERT_ID()";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public int Actualizar(CentroSalud e)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = @"UPDATE centro_salud SET nombre=@Nombre, direccion=@Direccion,
                          telefono=@Telefono, email=@Email, latitud=@Latitud, longitud=@Longitud
                          WHERE id=@Id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", e.Id);
            cmd.Parameters.AddWithValue("@Nombre", e.Nombre);
            cmd.Parameters.AddWithValue("@Direccion", e.Direccion);
            cmd.Parameters.AddWithValue("@Telefono", (object?)e.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object?)e.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Latitud", (object?)e.Latitud ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Longitud", (object?)e.Longitud ?? DBNull.Value);
            return cmd.ExecuteNonQuery();
        }
        public void Desactivar(int id) => CambiarEstado(id, false);

        // NUEVO: método genérico para activar / desactivar centros (reemplaza el borrado físico)
        public void CambiarEstado(int id, bool activo)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = "UPDATE centro_salud SET activo=@Activo WHERE id=@Id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Activo", activo);
            cmd.ExecuteNonQuery();
        }

        private static CentroSalud MapCentro(MySqlDataReader r) => new()
        {
            Id = r.GetInt32("id"),
            Nombre = r.GetString("nombre"),
            Direccion = r.GetString("direccion"),
            Telefono = r.IsDBNull(r.GetOrdinal("telefono")) ? null : r.GetString("telefono"),
            Email = r.IsDBNull(r.GetOrdinal("email")) ? null : r.GetString("email"),
            Latitud = r.IsDBNull(r.GetOrdinal("latitud")) ? null : r.GetDouble("latitud"),
            Longitud = r.IsDBNull(r.GetOrdinal("longitud")) ? null : r.GetDouble("longitud"),
            Activo = r.GetBoolean("activo"),
            CreadoEn = r.GetDateTime("creado_en")
        };
    }

    // ════════════════════════════════════════════════════════════════════════════
    // REPOSITORIO ESPECIALISTA  (sin cambios respecto a tu versión)
    // ════════════════════════════════════════════════════════════════════════════
    public class RepositorioEspecialista : RepositorioBase, IRepositorioEspecialista
    {
        public RepositorioEspecialista(IConfiguration configuration) : base(configuration) { }

        public IList<Especialista> ObtenerPorCentro(int centroId)
        {
            var lista = new List<Especialista>();
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = "SELECT * FROM especialista WHERE centro_id=@CentroId AND activo=1 ORDER BY especialidad, nombre";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@CentroId", centroId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) lista.Add(MapEspecialista(reader));
            return lista;
        }

        public IList<Especialista> ObtenerTodosPorCentro(int centroId)
        {
            var lista = new List<Especialista>();
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = "SELECT * FROM especialista WHERE centro_id=@CentroId ORDER BY activo DESC, especialidad, nombre";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@CentroId", centroId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) lista.Add(MapEspecialista(reader));
            return lista;
        }

        public IList<Especialista> ObtenerPorEspecialidad(string especialidad)
        {
            var lista = new List<Especialista>();
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = "SELECT * FROM especialista WHERE especialidad LIKE @Especialidad AND activo=1";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Especialidad", $"%{especialidad}%");
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) lista.Add(MapEspecialista(reader));
            return lista;
        }

        public Especialista? ObtenerPorId(int id)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = "SELECT * FROM especialista WHERE id=@Id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapEspecialista(reader) : null;
        }

        public int Alta(Especialista e)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = "INSERT INTO especialista (nombre, especialidad, centro_id) VALUES (@Nombre, @Especialidad, @CentroId)";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Nombre", e.Nombre);
            cmd.Parameters.AddWithValue("@Especialidad", e.Especialidad);
            cmd.Parameters.AddWithValue("@CentroId", e.CentroId);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT LAST_INSERT_ID()";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public int Actualizar(Especialista e)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = "UPDATE especialista SET nombre=@Nombre, especialidad=@Especialidad WHERE id=@Id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", e.Id);
            cmd.Parameters.AddWithValue("@Nombre", e.Nombre);
            cmd.Parameters.AddWithValue("@Especialidad", e.Especialidad);
            return cmd.ExecuteNonQuery();
        }

        public void Desactivar(int id) => CambiarEstado(id, false);

        // NUEVO: método genérico para activar / desactivar especialistas
        public void CambiarEstado(int id, bool activo)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = "UPDATE especialista SET activo=@Activo WHERE id=@Id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Activo", activo);
            cmd.ExecuteNonQuery();
        }

        private static Especialista MapEspecialista(MySqlDataReader r) => new()
        {
            Id = r.GetInt32("id"),
            Nombre = r.GetString("nombre"),
            Especialidad = r.GetString("especialidad"),
            CentroId = r.GetInt32("centro_id"),
            Activo = r.GetBoolean("activo"),
            CreadoEn = r.GetDateTime("creado_en")
        };
    }


    // ════════════════════════════════════════════════════════════════════════════
    // REPOSITORIO CRONOGRAMA
    // ════════════════════════════════════════════════════════════════════════════
    public class RepositorioCronograma : RepositorioBase, IRepositorioCronograma
    {
        public RepositorioCronograma(IConfiguration configuration) : base(configuration) { }

        public IList<Cronograma> ObtenerPorCentro(int centroId)
        {
            var lista = new List<Cronograma>();
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = @"SELECT c.*, e.nombre AS nombre_especialista, e.especialidad
                          FROM cronograma c
                          JOIN especialista e ON e.id = c.especialista_id
                          WHERE c.centro_id=@CentroId
                            AND e.activo = 1
                            AND c.fecha_fin >= CURDATE()
                          ORDER BY e.especialidad, c.fecha_inicio";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@CentroId", centroId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) lista.Add(MapCronograma(reader));
            reader.Close();

            // Cargar horarios de atención y horario de turnos de cada cronograma
            foreach (var cron in lista)
            {
                var qHorario = "SELECT * FROM horario_cronograma WHERE cronograma_id=@Id ORDER BY FIELD(dia_semana,'lunes','martes','miercoles','jueves','viernes','sabado')";
                using var cmdH = new MySqlCommand(qHorario, connection);
                cmdH.Parameters.AddWithValue("@Id", cron.Id);
                using var rH = cmdH.ExecuteReader();
                while (rH.Read()) cron.Horarios.Add(MapHorario(rH));
                rH.Close();

                var qTurno = "SELECT * FROM horario_turnos WHERE cronograma_id=@Id LIMIT 1";
                using var cmdT = new MySqlCommand(qTurno, connection);
                cmdT.Parameters.AddWithValue("@Id", cron.Id);
                using var rT = cmdT.ExecuteReader();
                if (rT.Read()) cron.HorarioTurnos = MapHorarioTurnos(rT);
            }
            return lista;
        }

        public IList<Cronograma> ObtenerPorEspecialista(int especialistaId)
        {
            var lista = new List<Cronograma>();
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = @"SELECT c.*, e.nombre AS nombre_especialista, e.especialidad
                          FROM cronograma c
                          JOIN especialista e ON e.id = c.especialista_id
                          WHERE c.especialista_id=@EspecialistaId AND c.fecha_fin >= CURDATE()";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@EspecialistaId", especialistaId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) lista.Add(MapCronograma(reader));
            reader.Close();

            foreach (var cron in lista)
            {
                var qHorario = "SELECT * FROM horario_cronograma WHERE cronograma_id=@Id ORDER BY FIELD(dia_semana,'lunes','martes','miercoles','jueves','viernes','sabado')";
                using var cmdH = new MySqlCommand(qHorario, connection);
                cmdH.Parameters.AddWithValue("@Id", cron.Id);
                using var rH = cmdH.ExecuteReader();
                while (rH.Read()) cron.Horarios.Add(MapHorario(rH));
                rH.Close();

                var qTurno = "SELECT * FROM horario_turnos WHERE cronograma_id=@Id LIMIT 1";
                using var cmdT = new MySqlCommand(qTurno, connection);
                cmdT.Parameters.AddWithValue("@Id", cron.Id);
                using var rT = cmdT.ExecuteReader();
                if (rT.Read()) cron.HorarioTurnos = MapHorarioTurnos(rT);
            }
            return lista;
        }

        public Cronograma? ObtenerPorId(int id)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = @"SELECT c.*, e.nombre AS nombre_especialista, e.especialidad
                          FROM cronograma c
                          JOIN especialista e ON e.id = c.especialista_id
                          WHERE c.id=@Id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapCronograma(reader) : null;
        }

        public int Alta(Cronograma e)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = @"INSERT INTO cronograma
                          (especialista_id, centro_id, fecha_inicio, fecha_fin, tipo_periodo, turnos_disponibles, usuario_carga_id)
                          VALUES (@EspId, @CentroId, @FechaInicio, @FechaFin, @TipoPeriodo, @Turnos, @UsuarioCargaId)";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@EspId", e.EspecialistaId);
            cmd.Parameters.AddWithValue("@CentroId", e.CentroId);
            cmd.Parameters.AddWithValue("@FechaInicio", e.FechaInicio.Date);
            cmd.Parameters.AddWithValue("@FechaFin", e.FechaFin.Date);
            cmd.Parameters.AddWithValue("@TipoPeriodo", e.TipoPeriodo);
            cmd.Parameters.AddWithValue("@Turnos", e.TurnosDisponibles);
            cmd.Parameters.AddWithValue("@UsuarioCargaId", e.UsuarioCargaId);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT LAST_INSERT_ID()";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void AgregarHorario(HorarioCronograma h)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = @"INSERT INTO horario_cronograma (cronograma_id, dia_semana, hora_inicio, hora_fin)
                          VALUES (@CronogramaId, @Dia, @HoraInicio, @HoraFin)";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@CronogramaId", h.CronogramaId);
            cmd.Parameters.AddWithValue("@Dia", h.DiaSemana);
            cmd.Parameters.AddWithValue("@HoraInicio", h.HoraInicio.ToString(@"hh\:mm\:ss"));
            cmd.Parameters.AddWithValue("@HoraFin", h.HoraFin.ToString(@"hh\:mm\:ss"));
            cmd.ExecuteNonQuery();
        }

        public void AgregarHorarioTurnos(HorarioTurnos h)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = @"INSERT INTO horario_turnos
                          (cronograma_id, mismo_dia, dia_semana, hora_inicio, hora_fin, observaciones)
                          VALUES (@CronogramaId, @MismoDia, @Dia, @HoraInicio, @HoraFin, @Observaciones)";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@CronogramaId", h.CronogramaId);
            cmd.Parameters.AddWithValue("@MismoDia", h.MismoDia);
            cmd.Parameters.AddWithValue("@Dia", (object?)h.DiaSemana ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@HoraInicio", h.HoraInicio.ToString(@"hh\:mm\:ss"));
            cmd.Parameters.AddWithValue("@HoraFin", h.HoraFin.ToString(@"hh\:mm\:ss"));
            cmd.Parameters.AddWithValue("@Observaciones", (object?)h.Observaciones ?? DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        public void EliminarHorarios(int cronogramaId)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = "DELETE FROM horario_cronograma WHERE cronograma_id=@Id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", cronogramaId);
            cmd.ExecuteNonQuery();
        }

        private static Cronograma MapCronograma(MySqlDataReader r) => new()
        {
            Id = r.GetInt32("id"),
            EspecialistaId = r.GetInt32("especialista_id"),
            CentroId = r.GetInt32("centro_id"),
            FechaInicio = r.GetDateTime("fecha_inicio"),
            FechaFin = r.GetDateTime("fecha_fin"),
            TipoPeriodo = r.GetString("tipo_periodo"),
            TurnosDisponibles = r.GetInt32("turnos_disponibles"),
            TipoTurno = r.IsDBNull(r.GetOrdinal("tipo_turno")) ? "orden_llegada" : r.GetString("tipo_turno"),
            UsuarioCargaId = r.GetInt32("usuario_carga_id"),
            FechaCarga = r.GetDateTime("fecha_carga"),
            NombreEspecialista = r.IsDBNull(r.GetOrdinal("nombre_especialista")) ? null : r.GetString("nombre_especialista"),
            Especialidad = r.IsDBNull(r.GetOrdinal("especialidad")) ? null : r.GetString("especialidad")
        };

        private static HorarioTurnos MapHorarioTurnos(MySqlDataReader r) => new()
        {
            Id = r.GetInt32("id"),
            CronogramaId = r.GetInt32("cronograma_id"),
            MismoDia = r.GetBoolean("mismo_dia"),
            DiaSemana = r.IsDBNull(r.GetOrdinal("dia_semana")) ? null : r.GetString("dia_semana"),
            HoraInicio = r.GetTimeSpan("hora_inicio"),
            HoraFin = r.GetTimeSpan("hora_fin"),
            Observaciones = r.IsDBNull(r.GetOrdinal("observaciones")) ? null : r.GetString("observaciones")
        };

        private static HorarioCronograma MapHorario(MySqlDataReader r) => new()
        {
            Id = r.GetInt32("id"),
            CronogramaId = r.GetInt32("cronograma_id"),
            DiaSemana = r.GetString("dia_semana"),
            HoraInicio = r.GetTimeSpan("hora_inicio"),
            HoraFin = r.GetTimeSpan("hora_fin")
        };
    }

    // ════════════════════════════════════════════════════════════════════════════
    // REPOSITORIO NOVEDAD DIARIA
    // ════════════════════════════════════════════════════════════════════════════
    public class RepositorioNovedadDiaria : RepositorioBase, IRepositorioNovedadDiaria
    {
        public RepositorioNovedadDiaria(IConfiguration configuration) : base(configuration) { }

        public IList<NovedadDiaria> ObtenerPorCentroYFecha(int centroId, DateTime fecha)
        {
            var lista = new List<NovedadDiaria>();
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = @"SELECT n.*, e.nombre AS nombre_especialista, e.especialidad
                          FROM novedad_diaria n
                          JOIN especialista e ON e.id = n.especialista_id
                          WHERE n.centro_id=@CentroId AND n.fecha=@Fecha
                          ORDER BY n.fecha_registro DESC";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@CentroId", centroId);
            cmd.Parameters.AddWithValue("@Fecha", fecha.Date);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) lista.Add(MapNovedad(reader));
            return lista;
        }

        public IList<NovedadDiaria> ObtenerHoy(int centroId)
            => ObtenerPorCentroYFecha(centroId, DateTime.Today);

        public int Alta(NovedadDiaria e)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = @"INSERT INTO novedad_diaria
                          (especialista_id, centro_id, fecha, tipo_novedad, descripcion, usuario_carga_id)
                          VALUES (@EspId, @CentroId, @Fecha, @TipoNovedad, @Descripcion, @UsuarioCargaId)";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@EspId", e.EspecialistaId);
            cmd.Parameters.AddWithValue("@CentroId", e.CentroId);
            cmd.Parameters.AddWithValue("@Fecha", e.Fecha.Date);
            cmd.Parameters.AddWithValue("@TipoNovedad", e.TipoNovedad);
            cmd.Parameters.AddWithValue("@Descripcion", (object?)e.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UsuarioCargaId", e.UsuarioCargaId);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT LAST_INSERT_ID()";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private static NovedadDiaria MapNovedad(MySqlDataReader r) => new()
        {
            Id = r.GetInt32("id"),
            EspecialistaId = r.GetInt32("especialista_id"),
            CentroId = r.GetInt32("centro_id"),
            Fecha = r.GetDateTime("fecha"),
            TipoNovedad = r.GetString("tipo_novedad"),
            Descripcion = r.IsDBNull(r.GetOrdinal("descripcion")) ? null : r.GetString("descripcion"),
            HoraNuevaInicio = r.IsDBNull(r.GetOrdinal("hora_nueva_inicio")) ? null : r.GetTimeSpan("hora_nueva_inicio"),
            HoraNuevaFin = r.IsDBNull(r.GetOrdinal("hora_nueva_fin")) ? null : r.GetTimeSpan("hora_nueva_fin"),
            LugarNuevo = r.IsDBNull(r.GetOrdinal("lugar_nuevo")) ? null : r.GetString("lugar_nuevo"),
            UsuarioCargaId = r.GetInt32("usuario_carga_id"),
            FechaRegistro = r.GetDateTime("fecha_registro"),
            NombreEspecialista = r.IsDBNull(r.GetOrdinal("nombre_especialista")) ? null : r.GetString("nombre_especialista"),
            Especialidad = r.IsDBNull(r.GetOrdinal("especialidad")) ? null : r.GetString("especialidad")
        };
    }

   // ════════════════════════════════════════════════════════════════════════════
    // REPOSITORIO VACUNATORIO (sin cambios respecto a tu versión — Actualizar ya
    // guarda hora_apertura, hora_cierre y dias_atencion, que es lo que necesitamos
    // para poder editar la línea de horario de vacunación)
    // ════════════════════════════════════════════════════════════════════════════
    public class RepositorioVacunatorio : RepositorioBase, IRepositorioVacunatorio
    {
        public RepositorioVacunatorio(IConfiguration configuration) : base(configuration) { }

        public Vacunatorio? ObtenerPorCentro(int centroId)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = "SELECT * FROM vacunatorio WHERE centro_id=@CentroId";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@CentroId", centroId);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapVacunatorio(reader) : null;
        }

        public int Alta(Vacunatorio e)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = @"INSERT INTO vacunatorio (centro_id, hora_apertura, hora_cierre, dias_atencion)
                          VALUES (@CentroId, @Apertura, @Cierre, @Dias)";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@CentroId", e.CentroId);
            cmd.Parameters.AddWithValue("@Apertura", e.HoraApertura.ToString(@"hh\:mm\:ss"));
            cmd.Parameters.AddWithValue("@Cierre", e.HoraCierre.ToString(@"hh\:mm\:ss"));
            cmd.Parameters.AddWithValue("@Dias", e.DiasAtencion);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT LAST_INSERT_ID()";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public int Actualizar(Vacunatorio e)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = @"UPDATE vacunatorio SET hora_apertura=@Apertura, hora_cierre=@Cierre, dias_atencion=@Dias
                          WHERE id=@Id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", e.Id);
            cmd.Parameters.AddWithValue("@Apertura", e.HoraApertura.ToString(@"hh\:mm\:ss"));
            cmd.Parameters.AddWithValue("@Cierre", e.HoraCierre.ToString(@"hh\:mm\:ss"));
            cmd.Parameters.AddWithValue("@Dias", e.DiasAtencion);
            return cmd.ExecuteNonQuery();
        }

        private static Vacunatorio MapVacunatorio(MySqlDataReader r) => new()
        {
            Id = r.GetInt32("id"),
            CentroId = r.GetInt32("centro_id"),
            HoraApertura = r.GetTimeSpan("hora_apertura"),
            HoraCierre = r.GetTimeSpan("hora_cierre"),
            DiasAtencion = r.GetString("dias_atencion"),
            Activo = r.GetBoolean("activo")
        };
    }
   // ════════════════════════════════════════════════════════════════════════════
    // REPOSITORIO CATÁLOGO VACUNAS (sin cambios respecto a tu versión)
    // ════════════════════════════════════════════════════════════════════════════
    public class RepositorioCatalogoVacunas : RepositorioBase, IRepositorioCatalogoVacunas
    {
        public RepositorioCatalogoVacunas(IConfiguration configuration) : base(configuration) { }

        public IList<CatalogoVacuna> ObtenerTodos(bool soloActivos = true)
        {
            var lista = new List<CatalogoVacuna>();
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = soloActivos
                ? "SELECT * FROM catalogo_vacunas WHERE activo=1 ORDER BY tipo, nombre"
                : "SELECT * FROM catalogo_vacunas ORDER BY tipo, nombre";
            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) lista.Add(MapCatalogo(reader));
            return lista;
        }

        public CatalogoVacuna? ObtenerPorId(int id)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = "SELECT * FROM catalogo_vacunas WHERE id=@Id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapCatalogo(reader) : null;
        }

        public int Alta(CatalogoVacuna e)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = @"INSERT INTO catalogo_vacunas (nombre, tipo, franja_etaria, condicion_aplicacion, creado_por)
                          VALUES (@Nombre, @Tipo, @FranjaEtaria, @Condicion, @CreadoPor)";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Nombre", e.Nombre);
            cmd.Parameters.AddWithValue("@Tipo", e.Tipo);
            cmd.Parameters.AddWithValue("@FranjaEtaria", (object?)e.FranjaEtaria ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Condicion", (object?)e.CondicionAplicacion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreadoPor", e.CreadoPor);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT LAST_INSERT_ID()";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public int Actualizar(CatalogoVacuna e)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = @"UPDATE catalogo_vacunas SET nombre=@Nombre, tipo=@Tipo,
                          franja_etaria=@FranjaEtaria, condicion_aplicacion=@Condicion WHERE id=@Id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", e.Id);
            cmd.Parameters.AddWithValue("@Nombre", e.Nombre);
            cmd.Parameters.AddWithValue("@Tipo", e.Tipo);
            cmd.Parameters.AddWithValue("@FranjaEtaria", (object?)e.FranjaEtaria ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Condicion", (object?)e.CondicionAplicacion ?? DBNull.Value);
            return cmd.ExecuteNonQuery();
        }

        public void Desactivar(int id) => CambiarEstado(id, false);

        // NUEVO: método genérico para activar / desactivar vacunas del catálogo
        public void CambiarEstado(int id, bool activo)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = "UPDATE catalogo_vacunas SET activo=@Activo WHERE id=@Id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Activo", activo);
            cmd.ExecuteNonQuery();
        }

        private static CatalogoVacuna MapCatalogo(MySqlDataReader r) => new()
        {
            Id = r.GetInt32("id"),
            Nombre = r.GetString("nombre"),
            Tipo = r.GetString("tipo"),
            FranjaEtaria = r.IsDBNull(r.GetOrdinal("franja_etaria")) ? null : r.GetString("franja_etaria"),
            CondicionAplicacion = r.IsDBNull(r.GetOrdinal("condicion_aplicacion")) ? null : r.GetString("condicion_aplicacion"),
            Activo = r.GetBoolean("activo"),
            CreadoPor = r.GetInt32("creado_por"),
            FechaCreacion = r.GetDateTime("fecha_creacion")
        };
    }

    // ════════════════════════════════════════════════════════════════════════════
    // REPOSITORIO VACUNA DISPONIBLE
    // ════════════════════════════════════════════════════════════════════════════
    public class RepositorioVacunaDisponible : RepositorioBase, IRepositorioVacunaDisponible
    {
        public RepositorioVacunaDisponible(IConfiguration configuration) : base(configuration) { }

        public IList<VacunaDisponible> ObtenerPorVacunatorio(int vacunatorioId)
                {
                    var lista = new List<VacunaDisponible>();
                    using var connection = (MySqlConnection)GetConnection();
                    connection.Open();
                    var query = @"SELECT vd.*, cv.nombre AS nombre_vacuna, cv.tipo AS tipo_vacuna, cv.franja_etaria
                                FROM vacuna_disponible vd
                                JOIN catalogo_vacunas cv ON cv.id = vd.catalogo_vacuna_id
                                WHERE vd.vacunatorio_id=@VacunatorioId
                                    AND cv.activo = 1
                                ORDER BY cv.tipo, cv.nombre";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@VacunatorioId", vacunatorioId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) lista.Add(MapVacunaDisponible(reader));
            return lista;
        }

        public int AltaOActualizar(VacunaDisponible e)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            // INSERT ... ON DUPLICATE KEY UPDATE (la tabla tiene unique key vacunatorio+catalogo)
            var query = @"INSERT INTO vacuna_disponible (vacunatorio_id, catalogo_vacuna_id, disponible, observaciones, usuario_carga_id)
                          VALUES (@VacunatorioId, @CatalogoId, @Disponible, @Observaciones, @UsuarioCargaId)
                          ON DUPLICATE KEY UPDATE
                            disponible=VALUES(disponible),
                            observaciones=VALUES(observaciones),
                            usuario_carga_id=VALUES(usuario_carga_id),
                            ultima_actualizacion=CURRENT_TIMESTAMP";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@VacunatorioId", e.VacunatorioId);
            cmd.Parameters.AddWithValue("@CatalogoId", e.CatalogoVacunaId);
            cmd.Parameters.AddWithValue("@Disponible", e.Disponible);
            cmd.Parameters.AddWithValue("@Observaciones", (object?)e.Observaciones ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UsuarioCargaId", e.UsuarioCargaId);
            return cmd.ExecuteNonQuery();
        }

        private static VacunaDisponible MapVacunaDisponible(MySqlDataReader r) => new()
        {
            Id = r.GetInt32("id"),
            VacunatorioId = r.GetInt32("vacunatorio_id"),
            CatalogoVacunaId = r.GetInt32("catalogo_vacuna_id"),
            Disponible = r.GetBoolean("disponible"),
            Observaciones = r.IsDBNull(r.GetOrdinal("observaciones")) ? null : r.GetString("observaciones"),
            UsuarioCargaId = r.GetInt32("usuario_carga_id"),
            UltimaActualizacion = r.GetDateTime("ultima_actualizacion"),
            NombreVacuna = r.IsDBNull(r.GetOrdinal("nombre_vacuna")) ? null : r.GetString("nombre_vacuna"),
            TipoVacuna = r.IsDBNull(r.GetOrdinal("tipo_vacuna")) ? null : r.GetString("tipo_vacuna"),
            FranjaEtaria = r.IsDBNull(r.GetOrdinal("franja_etaria")) ? null : r.GetString("franja_etaria")
        };
    }

    // ════════════════════════════════════════════════════════════════════════════
    // REPOSITORIO DEVICE TOKEN
    // ════════════════════════════════════════════════════════════════════════════
    public interface IRepositorioDeviceToken
    {
        void Registrar(string firebaseToken, int centroId);
        void Eliminar(string firebaseToken, int centroId);
        IList<string> ObtenerTokensPorCentro(int centroId);
    }

    public class RepositorioDeviceToken : RepositorioBase, IRepositorioDeviceToken
    {
        public RepositorioDeviceToken(IConfiguration configuration) : base(configuration) { }

        public void Registrar(string firebaseToken, int centroId)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            // INSERT IGNORE para no duplicar si ya existe
            var query = @"INSERT IGNORE INTO device_tokens (firebase_token, centro_id)
                          VALUES (@Token, @CentroId)";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Token", firebaseToken);
            cmd.Parameters.AddWithValue("@CentroId", centroId);
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(string firebaseToken, int centroId)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = "DELETE FROM device_tokens WHERE firebase_token=@Token AND centro_id=@CentroId";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Token", firebaseToken);
            cmd.Parameters.AddWithValue("@CentroId", centroId);
            cmd.ExecuteNonQuery();
        }

        public IList<string> ObtenerTokensPorCentro(int centroId)
        {
            var lista = new List<string>();
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = "SELECT firebase_token FROM device_tokens WHERE centro_id=@CentroId AND activo=1";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@CentroId", centroId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) lista.Add(reader.GetString("firebase_token"));
            return lista;
        }
    }

    // ════════════════════════════════════════════════════════════════════════════
    // REPOSITORIO AUDITORÍA
    // ════════════════════════════════════════════════════════════════════════════
    public class RepositorioAuditoria : RepositorioBase, IRepositorioAuditoria
    {
        public RepositorioAuditoria(IConfiguration configuration) : base(configuration) { }

        public void Registrar(Auditoria a)
        {
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = @"INSERT INTO auditoria (usuario_id, tabla_afectada, accion, val_anterior, val_nuevo, ip_origen)
                          VALUES (@UsuarioId, @Tabla, @Accion, @ValAnterior, @ValNuevo, @Ip)";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@UsuarioId", a.UsuarioId);
            cmd.Parameters.AddWithValue("@Tabla", a.TablaAfectada);
            cmd.Parameters.AddWithValue("@Accion", a.Accion);
            cmd.Parameters.AddWithValue("@ValAnterior", (object?)a.ValAnterior ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ValNuevo", (object?)a.ValNuevo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Ip", (object?)a.IpOrigen ?? DBNull.Value);
            cmd.ExecuteNonQuery();
        }
        public IList<Auditoria> ObtenerPorFecha(DateTime fecha, int cantidad = 500)
        {
            var lista = new List<Auditoria>();
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = @"SELECT a.*, u.nombre AS nombre_usuario
                          FROM auditoria a
                          JOIN usuario u ON u.id = a.usuario_id
                          WHERE DATE(a.timestamp_op) = @Fecha
                          ORDER BY a.timestamp_op DESC
                          LIMIT @Cantidad";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Fecha", fecha.Date);
            cmd.Parameters.AddWithValue("@Cantidad", cantidad);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Auditoria
                {
                    Id = reader.GetInt32("id"),
                    UsuarioId = reader.GetInt32("usuario_id"),
                    TablaAfectada = reader.GetString("tabla_afectada"),
                    Accion = reader.GetString("accion"),
                    ValAnterior = reader.IsDBNull(reader.GetOrdinal("val_anterior")) ? null : reader.GetString("val_anterior"),
                    ValNuevo = reader.IsDBNull(reader.GetOrdinal("val_nuevo")) ? null : reader.GetString("val_nuevo"),
                    TimestampOp = reader.GetDateTime("timestamp_op"),
                    IpOrigen = reader.IsDBNull(reader.GetOrdinal("ip_origen")) ? null : reader.GetString("ip_origen"),
                    NombreUsuario = reader.GetString("nombre_usuario")
                });
            }
            return lista;
        }

        public IList<Auditoria> ObtenerRecientes(int cantidad = 100)
        {
            var lista = new List<Auditoria>();
            using var connection = (MySqlConnection)GetConnection();
            connection.Open();
            var query = @"SELECT a.*, u.nombre AS nombre_usuario
                          FROM auditoria a
                          JOIN usuario u ON u.id = a.usuario_id
                          ORDER BY a.timestamp_op DESC
                          LIMIT @Cantidad";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Cantidad", cantidad);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Auditoria
                {
                    Id = reader.GetInt32("id"),
                    UsuarioId = reader.GetInt32("usuario_id"),
                    TablaAfectada = reader.GetString("tabla_afectada"),
                    Accion = reader.GetString("accion"),
                    ValAnterior = reader.IsDBNull(reader.GetOrdinal("val_anterior")) ? null : reader.GetString("val_anterior"),
                    ValNuevo = reader.IsDBNull(reader.GetOrdinal("val_nuevo")) ? null : reader.GetString("val_nuevo"),
                    TimestampOp = reader.GetDateTime("timestamp_op"),
                    IpOrigen = reader.IsDBNull(reader.GetOrdinal("ip_origen")) ? null : reader.GetString("ip_origen"),
                    NombreUsuario = reader.GetString("nombre_usuario")
                });
            }
            return lista;
        }
    }
}