using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SaludYa.Models;

namespace SaludYa.Controllers
{
    // ════════════════════════════════════════════════════════════════════════════
    // VACUNATORIO
    // ════════════════════════════════════════════════════════════════════════════
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VacunatorioApiController : ControllerBase
    {
        private readonly IRepositorioVacunatorio _repoVacunatorio;
        private readonly IRepositorioVacunaDisponible _repoDisponible;
        private readonly IRepositorioAuditoria _auditoria;

        public VacunatorioApiController(
            IRepositorioVacunatorio repoVacunatorio,
            IRepositorioVacunaDisponible repoDisponible,
            IRepositorioAuditoria auditoria)
        {
            _repoVacunatorio = repoVacunatorio;
            _repoDisponible = repoDisponible;
            _auditoria = auditoria;
        }

        // ── GET api/vacunatorio/porcentro/{centroId}  (público) ──────────────────
        [HttpGet("porcentro/{centroId}")]
        [AllowAnonymous]
        public IActionResult GetPorCentro(int centroId)
        {
            var vac = _repoVacunatorio.ObtenerPorCentro(centroId);
            if (vac == null) return NotFound(new { message = "Vacunatorio no encontrado para este centro" });
            return Ok(vac);
        }

        // ── GET api/vacunatorio/{vacunatorioId}/vacunas  (público) ───────────────
        [HttpGet("{vacunatorioId}/vacunas")]
        [AllowAnonymous]
        public IActionResult GetVacunas(int vacunatorioId) =>
            Ok(_repoDisponible.ObtenerPorVacunatorio(vacunatorioId));

        // ── POST api/vacunatorio  (superadmin) ───────────────────────────────────
        [HttpPost]
        [Authorize(Policy = "SoloSuperadmin")]
        public IActionResult Crear([FromBody] Vacunatorio vacunatorio)
        {
            try
            {
                int id = _repoVacunatorio.Alta(vacunatorio);
                _auditoria.Registrar(new Auditoria
                {
                    UsuarioId = ObtenerIdActual(),
                    TablaAfectada = "vacunatorio",
                    Accion = "INSERT",
                    ValNuevo = $"id={id}, centro_id={vacunatorio.CentroId}",
                    IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
                });
                return Ok(new { message = "Vacunatorio creado correctamente", id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear vacunatorio: " + ex.Message });
            }
        }

        // ── PUT api/vacunatorio/{id}  (responsable o superadmin) ─────────────────
        [HttpPut("{id}")]
        [Authorize(Policy = "ResponsableOSuperadmin")]
        public IActionResult Actualizar(int id, [FromBody] Vacunatorio vacunatorio)
        {
            vacunatorio.Id = id;
            _repoVacunatorio.Actualizar(vacunatorio);
            _auditoria.Registrar(new Auditoria
            {
                UsuarioId = ObtenerIdActual(),
                TablaAfectada = "vacunatorio",
                Accion = "UPDATE",
                ValNuevo = $"id={id}, apertura={vacunatorio.HoraApertura}, cierre={vacunatorio.HoraCierre}",
                IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
            });
            return Ok(new { message = "Vacunatorio actualizado correctamente" });
        }

        // ── PUT api/vacunatorio/disponibilidad  (responsable o superadmin) ────────
        // Actualiza una o varias vacunas del vacunatorio (disponible / no disponible)
        [HttpPut("disponibilidad")]
        [Authorize(Policy = "ResponsableOSuperadmin")]
        public IActionResult ActualizarDisponibilidad([FromBody] List<VacunaDisponible> vacunas)
        {
            try
            {
                int userId = ObtenerIdActual();
                foreach (var v in vacunas)
                {
                    v.UsuarioCargaId = userId;
                    _repoDisponible.AltaOActualizar(v);
                }
                _auditoria.Registrar(new Auditoria
                {
                    UsuarioId = userId,
                    TablaAfectada = "vacuna_disponible",
                    Accion = "UPDATE",
                    ValNuevo = $"vacunas actualizadas: {vacunas.Count}",
                    IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
                });
                return Ok(new { message = "Disponibilidad actualizada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar disponibilidad: " + ex.Message });
            }
        }

        private int ObtenerIdActual()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idClaim, out int id) ? id : 0;
        }
    }

    // ════════════════════════════════════════════════════════════════════════════
    // CATÁLOGO DE VACUNAS
    // ════════════════════════════════════════════════════════════════════════════
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CatalogoVacunasApiController : ControllerBase
    {
        private readonly IRepositorioCatalogoVacunas _repositorio;
        private readonly IRepositorioAuditoria _auditoria;

        public CatalogoVacunasApiController(IRepositorioCatalogoVacunas repositorio, IRepositorioAuditoria auditoria)
        {
            _repositorio = repositorio;
            _auditoria = auditoria;
        }

        // ── GET api/catalogovacunas  (público) ───────────────────────────────────
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetTodos([FromQuery] bool soloActivos = true) =>
            Ok(_repositorio.ObtenerTodos(soloActivos));

        // ── GET api/catalogovacunas/{id}  (público) ──────────────────────────────
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetPorId(int id)
        {
            var vac = _repositorio.ObtenerPorId(id);
            if (vac == null) return NotFound(new { message = "Vacuna no encontrada" });
            return Ok(vac);
        }

        // ── POST api/catalogovacunas  (superadmin) ───────────────────────────────
        [HttpPost]
        [Authorize(Policy = "SoloSuperadmin")]
        public IActionResult Crear([FromBody] CatalogoVacuna vacuna)
        {
            try
            {
                vacuna.CreadoPor = ObtenerIdActual();
                int id = _repositorio.Alta(vacuna);
                _auditoria.Registrar(new Auditoria
                {
                    UsuarioId = ObtenerIdActual(),
                    TablaAfectada = "catalogo_vacunas",
                    Accion = "INSERT",
                    ValNuevo = $"id={id}, nombre={vacuna.Nombre}, tipo={vacuna.Tipo}",
                    IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
                });
                return Ok(new { message = "Vacuna creada correctamente", id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear vacuna: " + ex.Message });
            }
        }

        // ── PUT api/catalogovacunas/{id}  (superadmin) ───────────────────────────
        [HttpPut("{id}")]
        [Authorize(Policy = "SoloSuperadmin")]
        public IActionResult Actualizar(int id, [FromBody] CatalogoVacuna vacuna)
        {
            vacuna.Id = id;
            var filas = _repositorio.Actualizar(vacuna);
            if (filas == 0) return NotFound(new { message = "Vacuna no encontrada" });

            _auditoria.Registrar(new Auditoria
            {
                UsuarioId = ObtenerIdActual(),
                TablaAfectada = "catalogo_vacunas",
                Accion = "UPDATE",
                ValNuevo = $"id={id}, nombre={vacuna.Nombre}",
                IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
            });
            return Ok(new { message = "Vacuna actualizada correctamente" });
        }

        // ── DELETE api/catalogovacunas/{id}  (superadmin) ────────────────────────
        [HttpDelete("{id}")]
        [Authorize(Policy = "SoloSuperadmin")]
        public IActionResult Desactivar(int id)
        {
            _repositorio.Desactivar(id);
            _auditoria.Registrar(new Auditoria
            {
                UsuarioId = ObtenerIdActual(),
                TablaAfectada = "catalogo_vacunas",
                Accion = "DELETE",
                ValNuevo = $"id={id}, activo=0",
                IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
            });
            return Ok(new { message = "Vacuna desactivada" });
        }

        private int ObtenerIdActual()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idClaim, out int id) ? id : 0;
        }
    }

    // ════════════════════════════════════════════════════════════════════════════
    // AUDITORÍA
    // ════════════════════════════════════════════════════════════════════════════
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "SoloSuperadmin")]
    public class AuditoriaApiController : ControllerBase
    {
        private readonly IRepositorioAuditoria _repositorio;

        public AuditoriaApiController(IRepositorioAuditoria repositorio)
        {
            _repositorio = repositorio;
        }

        // ── GET api/auditoria?cantidad=100  (superadmin) ─────────────────────────
        [HttpGet]
        public IActionResult GetRecientes([FromQuery] int cantidad = 100) =>
            Ok(_repositorio.ObtenerRecientes(cantidad));
    }
}