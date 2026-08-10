using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaludYa.Models;
using System.Security.Claims;

namespace SaludYa.Pages.Especialistas
{
    [Authorize(Policy = "SoloSuperadmin")]
    public class IndexModel : PageModel
    {
        private readonly IRepositorioEspecialista _repoEspecialista;
        private readonly IRepositorioCentroSalud _repoCentro;
        private readonly IRepositorioAuditoria _auditoria;

        public IndexModel(
            IRepositorioEspecialista repoEspecialista,
            IRepositorioCentroSalud repoCentro,
            IRepositorioAuditoria auditoria)
        {
            _repoEspecialista = repoEspecialista;
            _repoCentro = repoCentro;
            _auditoria = auditoria;
        }

        public IList<CentroSalud> Centros { get; set; } = new List<CentroSalud>();
        public IList<Especialista> TodosLosEspecialistas { get; set; } = new List<Especialista>();

        [BindProperty] public string Nombre { get; set; } = string.Empty;
        [BindProperty] public string Especialidad { get; set; } = string.Empty;
        [BindProperty] public int CentroIdNuevo { get; set; }

        

        [BindProperty] public int EditarId { get; set; }
        [BindProperty] public string EditarNombre { get; set; } = string.Empty;
        [BindProperty] public string EditarEspecialidad { get; set; } = string.Empty;

        public void OnGet() => CargarDatos();

        public IActionResult OnPostCrear()
        {
            CargarDatos();

            if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Especialidad) || CentroIdNuevo == 0)
            {
                TempData["Error"] = "Completá todos los campos";
                return RedirectToPage();
            }

            try
            {
                var nuevo = new Especialista { Nombre = Nombre, Especialidad = Especialidad, CentroId = CentroIdNuevo };
                int id = _repoEspecialista.Alta(nuevo);

                _auditoria.Registrar(new SaludYa.Models.Auditoria
                {
                    UsuarioId = ObtenerIdActual(),
                    TablaAfectada = "especialista",
                    Accion = "INSERT",
                    ValNuevo = $"id={id}, nombre={Nombre}, especialidad={Especialidad}, centro_id={CentroIdNuevo}",
                    IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
                });

                TempData["Mensaje"] = $"{Nombre} agregado correctamente";
            }
            catch (Exception)
            {
                TempData["Error"] = "No se pudo agregar el especialista por un problema de conexión con la base de datos. Probá de nuevo.";
            }

            return RedirectToPage();
        }

        public IActionResult OnPostEditar()
        {
            CargarDatos();

            if (string.IsNullOrWhiteSpace(EditarNombre) || string.IsNullOrWhiteSpace(EditarEspecialidad))
            {
                TempData["Error"] = "Completá todos los campos";
                return RedirectToPage();
            }

            var existente = _repoEspecialista.ObtenerPorId(EditarId);
            if (existente == null)
            {
                TempData["Error"] = "Especialista no encontrado";
                return RedirectToPage();
            }

            try
            {
                existente.Nombre = EditarNombre;
                existente.Especialidad = EditarEspecialidad;
                _repoEspecialista.Actualizar(existente);

                _auditoria.Registrar(new SaludYa.Models.Auditoria
                {
                    UsuarioId = ObtenerIdActual(),
                    TablaAfectada = "especialista",
                    Accion = "UPDATE",
                    ValNuevo = $"id={EditarId}, nombre={EditarNombre}, especialidad={EditarEspecialidad}",
                    IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
                });

                TempData["Mensaje"] = $"{EditarNombre} actualizado correctamente";
            }
            catch (Exception)
            {
                TempData["Error"] = "No se pudo actualizar el especialista por un problema de conexión con la base de datos. Probá de nuevo.";
            }

            return RedirectToPage();
        }

      

        public IActionResult OnPostCambiarEstado(int id, bool activo)
        {
            var especialista = _repoEspecialista.ObtenerPorId(id);
            if (especialista == null)
            {
                TempData["Error"] = "Especialista no encontrado";
                return RedirectToPage();
            }

            try
            {
                _repoEspecialista.CambiarEstado(id, activo);

                _auditoria.Registrar(new SaludYa.Models.Auditoria
                {
                    UsuarioId = ObtenerIdActual(),
                    TablaAfectada = "especialista",
                    Accion = activo ? "UPDATE" : "DELETE",
                    ValNuevo = $"id={id}, nombre={especialista.Nombre}, activo={(activo ? 1 : 0)}",
                    IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
                });

                TempData["Mensaje"] = activo ? $"{especialista.Nombre} activado" : $"{especialista.Nombre} desactivado";
            }
            catch (Exception)
            {
                TempData["Error"] = $"No se pudo cambiar el estado de {especialista.Nombre} por un problema de conexión con la base de datos. Probá de nuevo en unos segundos.";
            }

            return RedirectToPage();
        }

        private void CargarDatos()
        {
            Centros = _repoCentro.ObtenerTodos();
            TodosLosEspecialistas = Centros
                .SelectMany(c => _repoEspecialista.ObtenerTodosPorCentro(c.Id))
                .OrderBy(e => e.CentroId)
                .ThenBy(e => e.Especialidad)
                .ToList();
        }

        private int ObtenerIdActual()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idClaim, out int id) ? id : 0;
        }
    }
}