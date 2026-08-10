using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaludYa.Helpers;
using SaludYa.Models;
using System.Security.Claims;

namespace SaludYa.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IRepositorioUsuario _repositorio;
        private readonly IConfiguration _configuration;

        public LoginModel(IRepositorioUsuario repositorio, IConfiguration configuration)
        {
            _repositorio = repositorio;
            _configuration = configuration;
        }

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Clave { get; set; } = string.Empty;

        public string? ErrorMensaje { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Clave))
            {
                ErrorMensaje = "Ingresá email y contraseña";
                return Page();
            }

            var usuario = _repositorio.ObtenerPorEmail(Email);
            string hashed = PasswordHasher.Hash(Clave, _configuration);

            if (usuario == null || usuario.PasswordHash != hashed)
            {
                ErrorMensaje = "Credenciales incorrectas";
                return Page();
            }

            if (!usuario.Activo)
            {
                ErrorMensaje = "Usuario inactivo. Contactá al administrador";
                return Page();
            }

            // Solo superadmin y responsable acceden al panel web.
            if (usuario.Rol != "superadmin" && usuario.Rol != "responsable")
            {
                ErrorMensaje = "No tenés permisos para acceder al panel";
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("centro_id", usuario.CentroId?.ToString() ?? "")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToPage("/Index");
        }
    }
}
