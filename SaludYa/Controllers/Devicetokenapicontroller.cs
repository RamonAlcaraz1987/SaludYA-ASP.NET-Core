using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using SaludYa.Models;

namespace SaludYa.Controllers
{
    // ════════════════════════════════════════════════════════════════════════════
    // DEVICE TOKEN — suscripción a centros sin login
    // ════════════════════════════════════════════════════════════════════════════
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceTokenApiController : ControllerBase
    {
        private readonly IRepositorioDeviceToken _repositorio;

        public DeviceTokenApiController(IRepositorioDeviceToken repositorio)
        {
            _repositorio = repositorio;
        }

        // ── POST api/devicetoken/suscribir ───────────────────────────────────
        // El celular llama esto cuando el usuario marca un centro como favorito
        [HttpPost("suscribir")]
        public IActionResult Suscribir([FromBody] DeviceTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.FirebaseToken))
                return BadRequest(new { message = "Token requerido" });

            _repositorio.Registrar(request.FirebaseToken, request.CentroId);
            return Ok(new { message = "Suscripto correctamente" });
        }

        // ── POST api/devicetoken/desuscribir ─────────────────────────────────
        // El celular llama esto cuando el usuario quita un centro de favoritos
        [HttpPost("desuscribir")]
        public IActionResult Desuscribir([FromBody] DeviceTokenRequest request)
        {
            _repositorio.Eliminar(request.FirebaseToken, request.CentroId);
            return Ok(new { message = "Desuscripto correctamente" });
        }
    }

    public class DeviceTokenRequest
    {
        public string FirebaseToken { get; set; } = string.Empty;
        public int CentroId { get; set; }
    }

    // ════════════════════════════════════════════════════════════════════════════
    // SERVICIO FIREBASE — envío de notificaciones
    // ════════════════════════════════════════════════════════════════════════════
    public class FirebaseService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public FirebaseService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        // Envía notificación a todos los tokens de un centro
        public async Task EnviarNotificacionCentro(
            IList<string> tokens,
            string titulo,
            string cuerpo,
            int centroId)
        {
            if (tokens == null || tokens.Count == 0) return;

            string serverKey = _configuration["Firebase:ServerKey"] ?? "";
            if (string.IsNullOrEmpty(serverKey)) return;

            foreach (var token in tokens)
            {
                var payload = new
                {
                    to = token,
                    notification = new { title = titulo, body = cuerpo },
                    data = new { centro_id = centroId.ToString() }
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "key=" + serverKey);

                await _httpClient.PostAsync("https://fcm.googleapis.com/fcm/send", content);
            }
        }
    }
}