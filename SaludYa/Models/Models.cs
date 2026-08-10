namespace SaludYa.Models
{
    // ─── CENTRO DE SALUD ────────────────────────────────────────────────────────
    public class CentroSalud
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime CreadoEn { get; set; }
    }

    // ─── USUARIO ────────────────────────────────────────────────────────────────
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PasswordHash { get; set; }   // null en respuestas al cliente
        public string Rol { get; set; } = "ciudadano"; // superadmin | responsable | ciudadano
        public int? CentroId { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime CreadoEn { get; set; }
    }

    // ─── ESPECIALISTA ────────────────────────────────────────────────────────────
    public class Especialista
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
        public int CentroId { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime CreadoEn { get; set; }
    }

    // ─── CRONOGRAMA ──────────────────────────────────────────────────────────────
    public class Cronograma
    {
        public int Id { get; set; }
        public int EspecialistaId { get; set; }
        public int CentroId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string TipoPeriodo { get; set; } = "mensual"; // mensual | bimestral
        public int TurnosDisponibles { get; set; }
        public string TipoTurno { get; set; } = "orden_llegada"; // orden_llegada | turno_previo
        public int UsuarioCargaId { get; set; }
        public DateTime FechaCarga { get; set; }

        // Datos expandidos (JOIN) — opcionales
        public string? NombreEspecialista { get; set; }
        public string? Especialidad { get; set; }
        public List<HorarioCronograma> Horarios { get; set; } = new();
        public HorarioTurnos? HorarioTurnos { get; set; }  // cuándo se sacan los turnos
    }

    // ─── HORARIO TURNOS ──────────────────────────────────────────────────────────
    public class HorarioTurnos
    {
        public int Id { get; set; }
        public int CronogramaId { get; set; }
        public bool MismoDia { get; set; }          // true = el mismo día de atención
        public string? DiaSemana { get; set; }      // null si MismoDia = true
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string? Observaciones { get; set; }
    }

    // ─── HORARIO CRONOGRAMA ──────────────────────────────────────────────────────
    public class HorarioCronograma
    {
        public int Id { get; set; }
        public int CronogramaId { get; set; }
        public string DiaSemana { get; set; } = string.Empty;
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
    }

    // ─── NOVEDAD DIARIA ──────────────────────────────────────────────────────────
    public class NovedadDiaria
    {
        public int Id { get; set; }
        public int EspecialistaId { get; set; }
        public int CentroId { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoNovedad { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public TimeSpan? HoraNuevaInicio { get; set; }  // si hay cambio de horario
        public TimeSpan? HoraNuevaFin { get; set; }
        public string? LugarNuevo { get; set; }         // si se traslada
        public int UsuarioCargaId { get; set; }
        public DateTime FechaRegistro { get; set; }

        // JOIN
        public string? NombreEspecialista { get; set; }
        public string? Especialidad { get; set; }
    }

    // ─── VACUNATORIO ─────────────────────────────────────────────────────────────
    public class Vacunatorio
    {
        public int Id { get; set; }
        public int CentroId { get; set; }
        public TimeSpan HoraApertura { get; set; }
        public TimeSpan HoraCierre { get; set; }
        public string DiasAtencion { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
    }

    // ─── CATÁLOGO DE VACUNAS ─────────────────────────────────────────────────────
    public class CatalogoVacuna
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty; // calendario_fijo | campana_estacional
        public string? FranjaEtaria { get; set; }
        public string? CondicionAplicacion { get; set; }
        public bool Activo { get; set; } = true;
        public int CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    // ─── VACUNA DISPONIBLE ───────────────────────────────────────────────────────
    public class VacunaDisponible
    {
        public int Id { get; set; }
        public int VacunatorioId { get; set; }
        public int CatalogoVacunaId { get; set; }
        public bool Disponible { get; set; } = true;
        public string? Observaciones { get; set; }
        public int UsuarioCargaId { get; set; }
        public DateTime UltimaActualizacion { get; set; }

        // JOIN
        public string? NombreVacuna { get; set; }
        public string? TipoVacuna { get; set; }
        public string? FranjaEtaria { get; set; }
    }

    // ─── DEVICE TOKEN (favoritos sin login) ──────────────────────────────────────
    public class DeviceToken
    {
        public int Id { get; set; }
        public string FirebaseToken { get; set; } = string.Empty;
        public int CentroId { get; set; }
        public bool Activo { get; set; } = true;
    }

    // ─── AUDITORÍA ───────────────────────────────────────────────────────────────
    public class Auditoria
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string TablaAfectada { get; set; } = string.Empty;
        public string Accion { get; set; } = string.Empty; // INSERT | UPDATE | DELETE
        public string? ValAnterior { get; set; }
        public string? ValNuevo { get; set; }
        public DateTime TimestampOp { get; set; }
        public string? IpOrigen { get; set; }

        // JOIN
        public string? NombreUsuario { get; set; }
    }
}