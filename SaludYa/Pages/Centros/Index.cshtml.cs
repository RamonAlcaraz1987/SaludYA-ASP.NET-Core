using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaludYa.Models;
using System.Security.Claims;

namespace SaludYa.Pages.Centros
{
    [Authorize(Policy = "SoloSuperadmin")]
    public class IndexModel : PageModel
    {
        private readonly IRepositorioCentroSalud _repositorio;
        private readonly IRepositorioAuditoria _auditoria;

        public IndexModel(IRepositorioCentroSalud repositorio, IRepositorioAuditoria auditoria)
        {
            _repositorio = repositorio;
            _auditoria = auditoria;
        }

        public IList<CentroSalud> Centros { get; set; } = new List<CentroSalud>();

        [BindProperty]
        public CentroSalud NuevoCentro { get; set; } = new();

        [BindProperty] public int EditarId { get; set; }
        [BindProperty] public string EditarNombre { get; set; } = string.Empty;
        [BindProperty] public string EditarDireccion { get; set; } = string.Empty;
        [BindProperty] public string? EditarTelefono { get; set; }
        [BindProperty] public string? EditarEmail { get; set; }
        [BindProperty] public double? EditarLatitud { get; set; }
        [BindProperty] public double? EditarLongitud { get; set; }
        public void OnGet()
        {
            Centros = _repositorio.ObtenerTodos(soloActivos: false);
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(NuevoCentro.Nombre) || string.IsNullOrWhiteSpace(NuevoCentro.Direccion))
            {
                TempData["Error"] = "Nombre y dirección son obligatorios";
                return RedirectToPage();
            }

            try
            {
                int id = _repositorio.Alta(NuevoCentro);

                _auditoria.Registrar(new Models.Auditoria
                {
                    UsuarioId = ObtenerIdActual(),
                    TablaAfectada = "centro_salud",
                    Accion = "INSERT",
                    ValNuevo = $"id={id}, nombre={NuevoCentro.Nombre}",
                    IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
                });

                TempData["Mensaje"] = $"Centro '{NuevoCentro.Nombre}' creado correctamente";
            }
            catch (Exception)
            {
                TempData["Error"] = "No se pudo crear el centro por un problema de conexión con la base de datos. Probá de nuevo en unos segundos.";
            }

            return RedirectToPage();
        }

        public IActionResult OnPostEditar()
        {
            if (string.IsNullOrWhiteSpace(EditarNombre) || string.IsNullOrWhiteSpace(EditarDireccion))
            {
                TempData["Error"] = "Nombre y dirección son obligatorios";
                return RedirectToPage();
            }

            // Traemos el centro actual para no perder latitud/longitud si el
            // formulario llega sin esos valores (por ejemplo, si el modal no
            // los precargó correctamente).
            var existente = _repositorio.ObtenerPorId(EditarId);
            if (existente == null)
            {
                TempData["Error"] = "Centro no encontrado";
                return RedirectToPage();
            }

            var centro = new CentroSalud
            {
                Id = EditarId,
                Nombre = EditarNombre,
                Direccion = EditarDireccion,
                Telefono = EditarTelefono,
                Email = EditarEmail,
                Latitud = EditarLatitud ?? existente.Latitud,
                Longitud = EditarLongitud ?? existente.Longitud
            };

            try
            {
                var filas = _repositorio.Actualizar(centro);
                if (filas == 0)
                {
                    TempData["Error"] = "Centro no encontrado";
                    return RedirectToPage();
                }

                _auditoria.Registrar(new Models.Auditoria
                {
                    UsuarioId = ObtenerIdActual(),
                    TablaAfectada = "centro_salud",
                    Accion = "UPDATE",
                    ValNuevo = $"id={EditarId}, nombre={EditarNombre}",
                    IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
                });

                TempData["Mensaje"] = $"Centro '{EditarNombre}' actualizado correctamente";
            }
            catch (Exception)
            {
                TempData["Error"] = "No se pudo actualizar el centro por un problema de conexión con la base de datos. Probá de nuevo.";
            }

            return RedirectToPage();
        }
        
        public IActionResult OnPostCambiarEstado(int id, bool activo)
        {
            var centro = _repositorio.ObtenerPorId(id);
            if (centro == null)
            {
                TempData["Error"] = "Centro no encontrado";
                return RedirectToPage();
            }

            try
            {
                _repositorio.CambiarEstado(id, activo);

                _auditoria.Registrar(new Models.Auditoria
                {
                    UsuarioId = ObtenerIdActual(),
                    TablaAfectada = "centro_salud",
                    Accion = activo ? "UPDATE" : "DELETE",
                    ValNuevo = $"id={id}, activo={(activo ? 1 : 0)}",
                    IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
                });

                TempData["Mensaje"] = activo ? $"{centro.Nombre} activado" : $"{centro.Nombre} desactivado";
            }
            catch (Exception)
            {
                TempData["Error"] = $"No se pudo cambiar el estado de {centro.Nombre} por un problema de conexión con la base de datos. Probá de nuevo en unos segundos.";
            }

            return RedirectToPage();
        }

        private int ObtenerIdActual()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idClaim, out int id) ? id : 0;
        }
    }
}