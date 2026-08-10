using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaludYa.Helpers;
using SaludYa.Models;
using System.Security.Claims;

namespace SaludYa.Pages.Usuarios
{
    [Authorize(Policy = "SoloSuperadmin")]
    public class IndexModel : PageModel
    {
        private readonly IRepositorioUsuario _repositorio;
        private readonly IRepositorioCentroSalud _repoCentros;
        private readonly IRepositorioAuditoria _auditoria;
        private readonly IConfiguration _configuration;

        public IndexModel(
            IRepositorioUsuario repositorio,
            IRepositorioCentroSalud repoCentros,
            IRepositorioAuditoria auditoria,
            IConfiguration configuration)
        {
            _repositorio = repositorio;
            _repoCentros = repoCentros;
            _auditoria = auditoria;
            _configuration = configuration;
        }

        public IList<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public IList<CentroSalud> Centros { get; set; } = new List<CentroSalud>();

        [BindProperty] public string Nombre { get; set; } = string.Empty;
        [BindProperty] public string Email { get; set; } = string.Empty;
        [BindProperty] public string Clave { get; set; } = string.Empty;
        [BindProperty] public string Rol { get; set; } = "responsable";
        [BindProperty] public int? CentroId { get; set; }
[BindProperty] public int EditarId { get; set; }
        [BindProperty] public string EditarNombre { get; set; } = string.Empty;
        [BindProperty] public string EditarEmail { get; set; } = string.Empty;
        [BindProperty] public string EditarRol { get; set; } = "responsable";
        [BindProperty] public int? EditarCentroId { get; set; }
        [BindProperty] public bool EditarCambiarPassword { get; set; }
        [BindProperty] public string? EditarClave { get; set; }

        public void OnGet() => CargarDatos();

        public IActionResult OnPost()
        {
            CargarDatos();

            if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Clave))
            {
                TempData["Error"] = "Completá todos los campos obligatorios";
                return RedirectToPage();
            }

            if (Rol == "responsable" && CentroId == null)
            {
                TempData["Error"] = "Un responsable debe tener un centro asignado";
                return RedirectToPage();
            }

            try
            {
                var existente = _repositorio.ObtenerPorEmail(Email);
                if (existente != null)
                {
                    TempData["Error"] = "Ya existe un usuario con ese email";
                    return RedirectToPage();
                }

                var nuevo = new Usuario
                {
                    Nombre = Nombre,
                    Email = Email,
                    PasswordHash = PasswordHasher.Hash(Clave, _configuration),
                    Rol = Rol,
                    CentroId = Rol == "responsable" ? CentroId : null
                };

                int id = _repositorio.Alta(nuevo);

                _auditoria.Registrar(new SaludYa.Models.Auditoria
                {
                    UsuarioId = ObtenerIdActual(),
                    TablaAfectada = "usuario",
                    Accion = "INSERT",
                    ValNuevo = $"id={id}, email={Email}, rol={Rol}",
                    IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
                });

                TempData["Mensaje"] = $"Usuario '{Nombre}' creado correctamente";
            }
            catch (Exception)
            {
                TempData["Error"] = "No se pudo crear el usuario por un problema de conexión con la base de datos. Probá de nuevo.";
            }

            return RedirectToPage();
        }

        public IActionResult OnPostEditar()
        {
            CargarDatos();

            if (string.IsNullOrWhiteSpace(EditarNombre) || string.IsNullOrWhiteSpace(EditarEmail))
            {
                TempData["Error"] = "Completá todos los campos obligatorios";
                return RedirectToPage();
            }

            if (EditarRol == "responsable" && EditarCentroId == null)
            {
                TempData["Error"] = "Un responsable debe tener un centro asignado";
                return RedirectToPage();
            }

            if (EditarCambiarPassword && (string.IsNullOrWhiteSpace(EditarClave) || EditarClave.Length < 6))
            {
                TempData["Error"] = "La nueva contraseña debe tener al menos 6 caracteres";
                return RedirectToPage();
            }

            var existente = _repositorio.ObtenerPorId(EditarId);
            if (existente == null)
            {
                TempData["Error"] = "Usuario no encontrado";
                return RedirectToPage();
            }

            try
            {
                var otro = _repositorio.ObtenerPorEmail(EditarEmail);
                if (otro != null && otro.Id != EditarId)
                {
                    TempData["Error"] = "Ese email ya está en uso por otro usuario";
                    return RedirectToPage();
                }

                existente.Nombre = EditarNombre;
                existente.Email = EditarEmail;
                existente.Rol = EditarRol;
                existente.CentroId = EditarRol == "responsable" ? EditarCentroId : null;

                _repositorio.Actualizar(existente);

                _auditoria.Registrar(new SaludYa.Models.Auditoria
                {
                    UsuarioId = ObtenerIdActual(),
                    TablaAfectada = "usuario",
                    Accion = "UPDATE",
                    ValNuevo = $"id={EditarId}, email={EditarEmail}, rol={EditarRol}",
                    IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
                });

                if (EditarCambiarPassword && !string.IsNullOrWhiteSpace(EditarClave))
                {
                    _repositorio.CambiarContrasena(EditarId, PasswordHasher.Hash(EditarClave, _configuration));

                    _auditoria.Registrar(new SaludYa.Models.Auditoria
                    {
                        UsuarioId = ObtenerIdActual(),
                        TablaAfectada = "usuario",
                        Accion = "UPDATE",
                        ValNuevo = $"id={EditarId}, contraseña cambiada por administrador",
                        IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
                    });
                }

                TempData["Mensaje"] = $"Usuario '{EditarNombre}' actualizado correctamente"
                    + (EditarCambiarPassword ? " (contraseña actualizada)" : "");
            }
            catch (Exception)
            {
                TempData["Error"] = "No se pudo actualizar el usuario por un problema de conexión con la base de datos. Probá de nuevo.";
            }

            return RedirectToPage();
        }
        
        public IActionResult OnPostCambiarEstado(int id, bool activo)
        {
            CargarDatos();

            if (!activo && id == ObtenerIdActual())
            {
                TempData["Error"] = "No podés desactivar tu propio usuario";
                return RedirectToPage();
            }

            var usuario = _repositorio.ObtenerPorId(id);
            if (usuario == null)
            {
                TempData["Error"] = "Usuario no encontrado";
                return RedirectToPage();
            }

            try
            {
                _repositorio.CambiarEstadoUsuario(id, activo);

                _auditoria.Registrar(new SaludYa.Models.Auditoria
                {
                    UsuarioId = ObtenerIdActual(),
                    TablaAfectada = "usuario",
                    Accion = activo ? "UPDATE" : "DELETE",
                    ValNuevo = $"id={id}, activo={(activo ? 1 : 0)}",
                    IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
                });

                TempData["Mensaje"] = activo ? $"{usuario.Nombre} activado" : $"{usuario.Nombre} desactivado";
            }
            catch (Exception)
            {
                TempData["Error"] = $"No se pudo cambiar el estado de {usuario.Nombre} por un problema de conexión con la base de datos. Probá de nuevo en unos segundos.";
            }

            return RedirectToPage();
        }

        private void CargarDatos()
        {
            Usuarios = _repositorio.ObtenerTodos();
            Centros = _repoCentros.ObtenerTodos();
        }

        private int ObtenerIdActual()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idClaim, out int id) ? id : 0;
        }
    }
}