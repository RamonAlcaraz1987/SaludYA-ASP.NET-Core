using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SaludYa.Models;

namespace SaludYa.Controllers
{
    // ════════════════════════════════════════════════════════════════════════════
    // CRONOGRAMA
    // ════════════════════════════════════════════════════════════════════════════
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CronogramaApiController : ControllerBase
    {
        private readonly IRepositorioCronograma _repositorio;
        private readonly IRepositorioAuditoria _auditoria;

        public CronogramaApiController(IRepositorioCronograma repositorio, IRepositorioAuditoria auditoria)
        {
            _repositorio = repositorio;
            _auditoria = auditoria;
        }

        // ── GET api/cronograma/porcentro/{centroId}  (público) ───────────────────
        [HttpGet("porcentro/{centroId}")]
        [AllowAnonymous]
        public IActionResult GetPorCentro(int centroId) =>
            Ok(_repositorio.ObtenerPorCentro(centroId));

        // ── GET api/cronograma/porespecialista/{espId}  (público) ───────────────
        [HttpGet("porespecialista/{espId}")]
        [AllowAnonymous]
        public IActionResult GetPorEspecialista(int espId) =>
            Ok(_repositorio.ObtenerPorEspecialista(espId));

        // ── GET api/cronograma/{id}  (público) ───────────────────────────────────
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetPorId(int id)
        {
            var c = _repositorio.ObtenerPorId(id);
            if (c == null) return NotFound(new { message = "Cronograma no encontrado" });
            return Ok(c);
        }

        // ── POST api/cronograma  (responsable o superadmin) ──────────────────────
        // Body: CronogramaRequest con el cronograma + lista de horarios
        [HttpPost]
        [Authorize(Policy = "ResponsableOSuperadmin")]
        public IActionResult Crear([FromBody] CronogramaRequest request)
        {
            try
            {
                var cronograma = request.Cronograma;
                cronograma.UsuarioCargaId = ObtenerIdActual();

                if (!EsSuperadmin() && cronograma.CentroId != ObtenerCentroActual())
                    return Forbid();

                int id = _repositorio.Alta(cronograma);

                foreach (var horario in request.Horarios)
                {
                    horario.CronogramaId = id;
                    _repositorio.AgregarHorario(horario);
                }

                _auditoria.Registrar(new Auditoria
                {
                    UsuarioId = ObtenerIdActual(),
                    TablaAfectada = "cronograma",
                    Accion = "INSERT",
                    ValNuevo = $"id={id}, especialista_id={cronograma.EspecialistaId}, periodo={cronograma.TipoPeriodo}",
                    IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
                });

                return Ok(new { message = "Cronograma cargado correctamente", id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al cargar cronograma: " + ex.Message });
            }
        }

        private int ObtenerIdActual()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idClaim, out int id) ? id : 0;
        }

        private int ObtenerCentroActual()
        {
            var centroClaim = User.FindFirst("centro_id")?.Value;
            return int.TryParse(centroClaim, out int id) ? id : 0;
        }

        private bool EsSuperadmin() =>
            User.FindFirst(ClaimTypes.Role)?.Value == "superadmin";
    }

    public class CronogramaRequest
    {
        public Cronograma Cronograma { get; set; } = new();
        public List<HorarioCronograma> Horarios { get; set; } = new();
    }

    // ════════════════════════════════════════════════════════════════════════════
    // NOVEDAD DIARIA
    // ════════════════════════════════════════════════════════════════════════════
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NovedadDiariaApiController : ControllerBase
    {
        private readonly IRepositorioNovedadDiaria _repositorio;
        private readonly IRepositorioDeviceToken _repoTokens;
        private readonly IRepositorioAuditoria _auditoria;
        private readonly FirebaseService _firebase;

        public NovedadDiariaApiController(
            IRepositorioNovedadDiaria repositorio,
            IRepositorioDeviceToken repoTokens,
            IRepositorioAuditoria auditoria,
            FirebaseService firebase)
        {
            _repositorio = repositorio;
            _repoTokens = repoTokens;
            _auditoria = auditoria;
            _firebase = firebase;
        }

        [HttpGet("hoy/{centroId}")]
        [AllowAnonymous]
        public IActionResult GetHoy(int centroId) =>
            Ok(_repositorio.ObtenerHoy(centroId));

        [HttpGet("{centroId}")]
        [AllowAnonymous]
        public IActionResult GetPorFecha(int centroId, [FromQuery] DateTime fecha) =>
            Ok(_repositorio.ObtenerPorCentroYFecha(centroId, fecha));

        [HttpPost]
        [Authorize(Policy = "ResponsableOSuperadmin")]
        public async Task<IActionResult> Crear([FromBody] NovedadDiaria novedad)
        {
            try
            {
                novedad.UsuarioCargaId = ObtenerIdActual();
                novedad.Fecha = novedad.Fecha == default ? DateTime.Today : novedad.Fecha;

                if (!EsSuperadmin() && novedad.CentroId != ObtenerCentroActual())
                    return Forbid();

                int id = _repositorio.Alta(novedad);

                _auditoria.Registrar(new Auditoria
                {
                    UsuarioId = ObtenerIdActual(),
                    TablaAfectada = "novedad_diaria",
                    Accion = "INSERT",
                    ValNuevo = $"id={id}, tipo={novedad.TipoNovedad}, especialista_id={novedad.EspecialistaId}",
                    IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
                });

                // Notificar a todos los suscritos al centro
                var tokens = _repoTokens.ObtenerTokensPorCentro(novedad.CentroId);
                if (tokens.Count > 0)
                {
                    string titulo = novedad.TipoNovedad switch
                    {
                        "ausencia"         => "⚠ Médico ausente hoy",
                        "cambio_horario"   => "🕐 Cambio de horario",
                        "reduccion_turnos" => "📉 Turnos reducidos",
                        _                  => "ℹ Novedad en tu centro"
                    };

                    var sb = new System.Text.StringBuilder();
                    sb.Append(novedad.NombreEspecialista ?? "Especialista");
                    if (novedad.HoraNuevaInicio.HasValue && novedad.HoraNuevaFin.HasValue)
                        sb.Append(" — Nuevo horario: " + novedad.HoraNuevaInicio.Value.ToString(@"hh\:mm") + " a " + novedad.HoraNuevaFin.Value.ToString(@"hh\:mm"));
                    else if (!string.IsNullOrEmpty(novedad.Descripcion))
                        sb.Append($" — {novedad.Descripcion}");
                    if (!string.IsNullOrEmpty(novedad.LugarNuevo))
                        sb.Append($" — Lugar: {novedad.LugarNuevo}");

                    await _firebase.EnviarNotificacionCentro(tokens, titulo, sb.ToString(), novedad.CentroId);
                }

                return Ok(new { message = "Novedad registrada correctamente", id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al registrar novedad: " + ex.Message });
            }
        }

        private int ObtenerIdActual()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idClaim, out int id) ? id : 0;
        }

        private int ObtenerCentroActual()
        {
            var centroClaim = User.FindFirst("centro_id")?.Value;
            return int.TryParse(centroClaim, out int id) ? id : 0;
        }

        private bool EsSuperadmin() =>
            User.FindFirst(ClaimTypes.Role)?.Value == "superadmin";
    }
}