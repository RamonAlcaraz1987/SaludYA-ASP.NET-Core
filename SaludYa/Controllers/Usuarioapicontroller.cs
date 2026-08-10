using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SaludYa.Models;

namespace SaludYa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuarioApiController : ControllerBase
    {
        private readonly IRepositorioUsuario _repositorio;
        private readonly IRepositorioAuditoria _auditoria;
        private readonly IConfiguration _configuration;

        public UsuarioApiController(IRepositorioUsuario repositorio, IRepositorioAuditoria auditoria, IConfiguration configuration)
        {
            _repositorio = repositorio;
            _auditoria = auditoria;
            _configuration = configuration;
        }

        // ── POST api/usuarioapi/login ────────────────────────────────────────────
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            try
            {
                if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Clave))
                    return BadRequest(new { message = "Email y contraseña son requeridos" });

                string hashed = HashearContrasena(login.Clave);
                var usuario = _repositorio.ObtenerPorEmail(login.Email);

                if (usuario == null || usuario.PasswordHash != hashed)
                    return BadRequest(new { message = "Credenciales incorrectas" });

                if (!usuario.Activo)
                    return BadRequest(new { message = "Usuario inactivo" });

                return Ok(new { token = GenerarTokenJWT(usuario), rol = usuario.Rol, nombre = usuario.Nombre });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al iniciar sesión: " + ex.Message });
            }
        }

        // ── GET api/usuarioapi/me ────────────────────────────────────────────────
        [HttpGet("me")]
        public IActionResult GetMiPerfil()
        {
            var usuario = ObtenerUsuarioActual();
            if (usuario == null) return Unauthorized();
            usuario.PasswordHash = null;
            return Ok(usuario);
        }

        // ── GET api/usuarioapi  (solo superadmin) ────────────────────────────────
        [HttpGet]
        [Authorize(Policy = "SoloSuperadmin")]
        public IActionResult GetTodos()
        {
            var lista = _repositorio.ObtenerTodos();
            foreach (var u in lista) u.PasswordHash = null;
            return Ok(lista);
        }

        // ── POST api/usuarioapi  (crear usuario institucional — superadmin) ──────
        [HttpPost]
        [Authorize(Policy = "SoloSuperadmin")]
        public IActionResult Crear([FromBody] CrearUsuarioRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Clave))
                    return BadRequest(new { message = "Email y contraseña requeridos" });

                var existe = _repositorio.ObtenerPorEmail(request.Email);
                if (existe != null)
                    return BadRequest(new { message = "El email ya está registrado" });

                var nuevo = new Usuario
                {
                    Nombre = request.Nombre,
                    Email = request.Email,
                    PasswordHash = HashearContrasena(request.Clave),
                    Rol = request.Rol,
                    CentroId = request.CentroId
                };

                int id = _repositorio.Alta(nuevo);
                _auditoria.Registrar(new Auditoria
                {
                    UsuarioId = ObtenerIdActual(),
                    TablaAfectada = "usuario",
                    Accion = "INSERT",
                    ValNuevo = $"id={id}, email={request.Email}, rol={request.Rol}",
                    IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
                });

                return Ok(new { message = "Usuario creado correctamente", id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear usuario: " + ex.Message });
            }
        }

        // ── PUT api/usuarioapi/{id}/desactivar (superadmin) ──────────────────────
        [HttpPut("{id}/desactivar")]
        [Authorize(Policy = "SoloSuperadmin")]
        public IActionResult Desactivar(int id)
        {
            _repositorio.DesactivarUsuario(id);
            _auditoria.Registrar(new Auditoria
            {
                UsuarioId = ObtenerIdActual(),
                TablaAfectada = "usuario",
                Accion = "UPDATE",
                ValNuevo = $"id={id}, activo=0",
                IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
            });
            return Ok(new { message = "Usuario desactivado" });
        }

        // ── PUT api/usuarioapi/cambiarContrasena ─────────────────────────────────
        [HttpPut("cambiarContrasena")]
        public IActionResult CambiarContrasena([FromBody] CambiarContrasenaRequest request)
        {
            var userId = ObtenerIdActual();
            if (userId == 0) return Unauthorized();

            _repositorio.CambiarContrasena(userId, HashearContrasena(request.NuevaClave));
            return Ok(new { message = "Contraseña actualizada correctamente" });
        }

        // ─── Helpers ─────────────────────────────────────────────────────────────
        private string HashearContrasena(string clave) =>
            SaludYa.Helpers.PasswordHasher.Hash(clave, _configuration);

        private string GenerarTokenJWT(Usuario usuario)
        {
            string secretKey = _configuration["TokenAuthentication:SecretKey"] ?? "SaludYaClaveSecretaMuyLarga2026!";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("centro_id", usuario.CentroId?.ToString() ?? "")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["TokenAuthentication:Issuer"] ?? "SaludYa",
                audience: _configuration["TokenAuthentication:Audience"] ?? "SaludYaAPI",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Usuario? ObtenerUsuarioActual()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(idClaim) || !int.TryParse(idClaim, out int id)) return null;
            return _repositorio.ObtenerPorId(id);
        }

        private int ObtenerIdActual()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idClaim, out int id) ? id : 0;
        }
    }

    // ─── Request DTOs ──────────────────────────────────────────────────────────
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
    }

    public class CrearUsuarioRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
        public string Rol { get; set; } = "responsable";
        public int? CentroId { get; set; }
    }

    public class CambiarContrasenaRequest
    {
        public string NuevaClave { get; set; } = string.Empty;
    }
}