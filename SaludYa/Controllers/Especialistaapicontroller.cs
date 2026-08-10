using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SaludYa.Models;

namespace SaludYa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EspecialistaApiController : ControllerBase
    {
        private readonly IRepositorioEspecialista _repositorio;
        private readonly IRepositorioAuditoria _auditoria;

        public EspecialistaApiController(IRepositorioEspecialista repositorio, IRepositorioAuditoria auditoria)
        {
            _repositorio = repositorio;
            _auditoria = auditoria;
        }

        // ── GET api/especialista/porcentro/{centroId}  (público) ─────────────────
        [HttpGet("porcentro/{centroId}")]
        [AllowAnonymous]
        public IActionResult GetPorCentro(int centroId) =>
            Ok(_repositorio.ObtenerPorCentro(centroId));

        // ── GET api/especialista/buscarespecialidad?q=pediatria  (público) ───────
        [HttpGet("buscarespecialidad")]
        [AllowAnonymous]
        public IActionResult BuscarEspecialidad([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest(new { message = "Ingresá la especialidad a buscar" });
            return Ok(_repositorio.ObtenerPorEspecialidad(q));
        }

        // ── GET api/especialista/{id}  (público) ─────────────────────────────────
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetPorId(int id)
        {
            var esp = _repositorio.ObtenerPorId(id);
            if (esp == null) return NotFound(new { message = "Especialista no encontrado" });
            return Ok(esp);
        }

        // ── POST api/especialista  (responsable o superadmin) ────────────────────
        [HttpPost]
        [Authorize(Policy = "ResponsableOSuperadmin")]
        public IActionResult Crear([FromBody] Especialista especialista)
        {
            try
            {
                // El responsable solo puede cargar para su propio centro
                if (!EsSuperadmin())
                {
                    int centroActual = ObtenerCentroActual();
                    if (especialista.CentroId != centroActual)
                        return Forbid();
                }

                int id = _repositorio.Alta(especialista);
                _auditoria.Registrar(new Auditoria
                {
                    UsuarioId = ObtenerIdActual(),
                    TablaAfectada = "especialista",
                    Accion = "INSERT",
                    ValNuevo = $"id={id}, nombre={especialista.Nombre}, especialidad={especialista.Especialidad}",
                    IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
                });
                return Ok(new { message = "Especialista creado correctamente", id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear especialista: " + ex.Message });
            }
        }

        // ── PUT api/especialista/{id}  (responsable o superadmin) ────────────────
        [HttpPut("{id}")]
        [Authorize(Policy = "ResponsableOSuperadmin")]
        public IActionResult Actualizar(int id, [FromBody] Especialista especialista)
        {
            var existente = _repositorio.ObtenerPorId(id);
            if (existente == null) return NotFound(new { message = "Especialista no encontrado" });

            if (!EsSuperadmin() && existente.CentroId != ObtenerCentroActual())
                return Forbid();

            especialista.Id = id;
            _repositorio.Actualizar(especialista);
            _auditoria.Registrar(new Auditoria
            {
                UsuarioId = ObtenerIdActual(),
                TablaAfectada = "especialista",
                Accion = "UPDATE",
                ValNuevo = $"id={id}, nombre={especialista.Nombre}",
                IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
            });
            return Ok(new { message = "Especialista actualizado correctamente" });
        }

        // ── DELETE api/especialista/{id}  (responsable o superadmin) ─────────────
        [HttpDelete("{id}")]
        [Authorize(Policy = "ResponsableOSuperadmin")]
        public IActionResult Desactivar(int id)
        {
            var existente = _repositorio.ObtenerPorId(id);
            if (existente == null) return NotFound(new { message = "Especialista no encontrado" });

            if (!EsSuperadmin() && existente.CentroId != ObtenerCentroActual())
                return Forbid();

            _repositorio.Desactivar(id);
            return Ok(new { message = "Especialista desactivado" });
        }

        // ─── Helpers ─────────────────────────────────────────────────────────────
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