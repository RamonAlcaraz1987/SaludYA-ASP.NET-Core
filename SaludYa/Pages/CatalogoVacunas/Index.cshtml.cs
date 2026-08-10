using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaludYa.Models;
using System.Security.Claims;

namespace SaludYa.Pages.CatalogoVacunas
{
    [Authorize(Policy = "SoloSuperadmin")]
    public class IndexModel : PageModel
    {
        private readonly IRepositorioCatalogoVacunas _repositorio;
        private readonly IRepositorioAuditoria _auditoria;

        public IndexModel(IRepositorioCatalogoVacunas repositorio, IRepositorioAuditoria auditoria)
        {
            _repositorio = repositorio;
            _auditoria = auditoria;
        }

        public IList<CatalogoVacuna> Vacunas { get; set; } = new List<CatalogoVacuna>();

        [BindProperty] public string Nombre { get; set; } = string.Empty;
        [BindProperty] public string Tipo { get; set; } = "calendario_fijo";
        [BindProperty] public string? FranjaEtaria { get; set; }
        [BindProperty] public string? CondicionAplicacion { get; set; }

        [BindProperty] public int EditarId { get; set; }
        [BindProperty] public string EditarNombre { get; set; } = string.Empty;
        [BindProperty] public string EditarTipo { get; set; } = "calendario_fijo";
        [BindProperty] public string? EditarFranjaEtaria { get; set; }
        [BindProperty] public string? EditarCondicionAplicacion { get; set; }

        public void OnGet()
        {
            Vacunas = _repositorio.ObtenerTodos(soloActivos: false);
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                TempData["Error"] = "El nombre es obligatorio";
                return RedirectToPage();
            }

            try
            {
                var nueva = new CatalogoVacuna
                {
                    Nombre = Nombre,
                    Tipo = Tipo,
                    FranjaEtaria = FranjaEtaria,
                    CondicionAplicacion = CondicionAplicacion,
                    CreadoPor = ObtenerIdActual()
                };

                int id = _repositorio.Alta(nueva);

                _auditoria.Registrar(new SaludYa.Models.Auditoria
                {
                    UsuarioId = ObtenerIdActual(),
                    TablaAfectada = "catalogo_vacunas",
                    Accion = "INSERT",
                    ValNuevo = $"id={id}, nombre={Nombre}, tipo={Tipo}",
                    IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
                });

                TempData["Mensaje"] = $"Vacuna '{Nombre}' agregada al catálogo";
            }
            catch (Exception)
            {
                TempData["Error"] = "No se pudo agregar la vacuna por un problema de conexión con la base de datos. Probá de nuevo.";
            }

            return RedirectToPage();
        }

        public IActionResult OnPostEditar()
        {
            if (string.IsNullOrWhiteSpace(EditarNombre))
            {
                TempData["Error"] = "El nombre es obligatorio";
                return RedirectToPage();
            }

            var vacuna = new CatalogoVacuna
            {
                Id = EditarId,
                Nombre = EditarNombre,
                Tipo = EditarTipo,
                FranjaEtaria = EditarFranjaEtaria,
                CondicionAplicacion = EditarCondicionAplicacion
            };

            try
            {
                var filas = _repositorio.Actualizar(vacuna);
                if (filas == 0)
                {
                    TempData["Error"] = "Vacuna no encontrada";
                    return RedirectToPage();
                }

                _auditoria.Registrar(new SaludYa.Models.Auditoria
                {
                    UsuarioId = ObtenerIdActual(),
                    TablaAfectada = "catalogo_vacunas",
                    Accion = "UPDATE",
                    ValNuevo = $"id={EditarId}, nombre={EditarNombre}",
                    IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
                });

                TempData["Mensaje"] = $"Vacuna '{EditarNombre}' actualizada correctamente";
            }
            catch (Exception)
            {
                TempData["Error"] = "No se pudo actualizar la vacuna por un problema de conexión con la base de datos. Probá de nuevo.";
            }

            return RedirectToPage();
        }

        public IActionResult OnPostCambiarEstado(int id, bool activo)
        {
            var vacuna = _repositorio.ObtenerPorId(id);
            if (vacuna == null)
            {
                TempData["Error"] = "Vacuna no encontrada";
                return RedirectToPage();
            }

            try
            {
                _repositorio.CambiarEstado(id, activo);

                _auditoria.Registrar(new SaludYa.Models.Auditoria
                {
                    UsuarioId = ObtenerIdActual(),
                    TablaAfectada = "catalogo_vacunas",
                    Accion = activo ? "UPDATE" : "DELETE",
                    ValNuevo = $"id={id}, nombre={vacuna.Nombre}, activo={(activo ? 1 : 0)}",
                    IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
                });

                TempData["Mensaje"] = activo ? $"{vacuna.Nombre} activada" : $"{vacuna.Nombre} desactivada";
            }
            catch (Exception)
            {
                TempData["Error"] = $"No se pudo cambiar el estado de {vacuna.Nombre} por un problema de conexión con la base de datos. Probá de nuevo en unos segundos.";
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