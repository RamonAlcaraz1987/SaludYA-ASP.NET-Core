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
    public class CentroSaludApiController : ControllerBase
    {
        private readonly IRepositorioCentroSalud _repositorio;
        private readonly IRepositorioAuditoria _auditoria;

        public CentroSaludApiController(IRepositorioCentroSalud repositorio, IRepositorioAuditoria auditoria)
        {
            _repositorio = repositorio;
            _auditoria = auditoria;
        }

        // ── GET api/centrosalud  (público) ───────────────────────────────────────
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetTodos() => Ok(_repositorio.ObtenerTodos());

        // ── GET api/centrosalud/{id}  (público) ──────────────────────────────────
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetPorId(int id)
        {
            var centro = _repositorio.ObtenerPorId(id);
            if (centro == null) return NotFound(new { message = "Centro no encontrado" });
            return Ok(centro);
        }

        // ── POST api/centrosalud  (superadmin) ───────────────────────────────────
        [HttpPost]
        [Authorize(Policy = "SoloSuperadmin")]
        public IActionResult Crear([FromBody] CentroSalud centro)
        {
            try
            {
                if (string.IsNullOrEmpty(centro.Nombre) || string.IsNullOrEmpty(centro.Direccion))
                    return BadRequest(new { message = "Nombre y dirección son requeridos" });

                int id = _repositorio.Alta(centro);
                _auditoria.Registrar(new Auditoria
                {
                    UsuarioId = ObtenerIdActual(),
                    TablaAfectada = "centro_salud",
                    Accion = "INSERT",
                    ValNuevo = $"id={id}, nombre={centro.Nombre}",
                    IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
                });
                return Ok(new { message = "Centro creado correctamente", id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear centro: " + ex.Message });
            }
        }

        // ── PUT api/centrosalud/{id}  (superadmin) ───────────────────────────────
        [HttpPut("{id}")]
        [Authorize(Policy = "SoloSuperadmin")]
        public IActionResult Actualizar(int id, [FromBody] CentroSalud centro)
        {
            centro.Id = id;
            var filas = _repositorio.Actualizar(centro);
            if (filas == 0) return NotFound(new { message = "Centro no encontrado" });

            _auditoria.Registrar(new Auditoria
            {
                UsuarioId = ObtenerIdActual(),
                TablaAfectada = "centro_salud",
                Accion = "UPDATE",
                ValNuevo = $"id={id}, nombre={centro.Nombre}",
                IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
            });
            return Ok(new { message = "Centro actualizado correctamente" });
        }

        // ── DELETE api/centrosalud/{id}  (superadmin) ────────────────────────────
        [HttpDelete("{id}")]
        [Authorize(Policy = "SoloSuperadmin")]
        public IActionResult Desactivar(int id)
        {
            _repositorio.Desactivar(id);
            _auditoria.Registrar(new Auditoria
            {
                UsuarioId = ObtenerIdActual(),
                TablaAfectada = "centro_salud",
                Accion = "DELETE",
                ValNuevo = $"id={id}, activo=0",
                IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
            });
            return Ok(new { message = "Centro desactivado" });
        }

        private int ObtenerIdActual()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idClaim, out int id) ? id : 0;
        }
    }
}