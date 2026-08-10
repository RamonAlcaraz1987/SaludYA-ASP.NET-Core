using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaludYa.Models;
using System.Security.Claims;

namespace SaludYa.Pages.Vacunas
{
    [Authorize(Policy = "ResponsableOSuperadmin")]
    public class IndexModel : PageModel
    {
        private readonly IRepositorioVacunatorio _repoVacunatorio;
        private readonly IRepositorioVacunaDisponible _repoVacunaDisponible;
        private readonly IRepositorioAuditoria _auditoria;

        public IndexModel(
            IRepositorioVacunatorio repoVacunatorio,
            IRepositorioVacunaDisponible repoVacunaDisponible,
            IRepositorioAuditoria auditoria)
        {
            _repoVacunatorio = repoVacunatorio;
            _repoVacunaDisponible = repoVacunaDisponible;
            _auditoria = auditoria;
        }

        public Vacunatorio? Vacunatorio { get; set; }
        public IList<VacunaDisponible> Vacunas { get; set; } = new List<VacunaDisponible>();

        [BindProperty]
        public int CantidadVacunas { get; set; }

        // ── Editar horario del vacunatorio ──────────────────────────────────────
        [BindProperty] public TimeSpan HoraAperturaEditar { get; set; }
        [BindProperty] public TimeSpan HoraCierreEditar { get; set; }

        public void OnGet()
        {
            CargarDatos();
            CantidadVacunas = Vacunas.Count;
        }

        public IActionResult OnPost()
        {
            int centroId = ObtenerCentroActual();
            Vacunatorio = _repoVacunatorio.ObtenerPorCentro(centroId);

            if (Vacunatorio == null)
            {
                TempData["Error"] = "No hay vacunatorio configurado para tu centro";
                return RedirectToPage();
            }

            int userId = ObtenerIdActual();
            int actualizadas = 0;

            for (int i = 0; i < CantidadVacunas; i++)
            {
                var catalogoIdStr = Request.Form[$"CatalogoVacunaId_{i}"].ToString();
                if (!int.TryParse(catalogoIdStr, out int catalogoId)) continue;

                bool disponible = Request.Form[$"Disponible_{i}"].ToString() == "true";
                string? observaciones = Request.Form[$"Observaciones_{i}"].ToString();

                var vd = new VacunaDisponible
                {
                    VacunatorioId = Vacunatorio.Id,
                    CatalogoVacunaId = catalogoId,
                    Disponible = disponible,
                    Observaciones = string.IsNullOrWhiteSpace(observaciones) ? null : observaciones,
                    UsuarioCargaId = userId
                };

                _repoVacunaDisponible.AltaOActualizar(vd);
                actualizadas++;
            }

            _auditoria.Registrar(new SaludYa.Models.Auditoria
            {
                UsuarioId = userId,
                TablaAfectada = "vacuna_disponible",
                Accion = "UPDATE",
                ValNuevo = $"vacunatorio_id={Vacunatorio.Id}, vacunas_actualizadas={actualizadas}",
                IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
            });

            TempData["Mensaje"] = "Disponibilidad de vacunas actualizada correctamente";
            return RedirectToPage();
        }

        // ── POST: Editar la línea de horario del vacunatorio (apertura/cierre/días) ─
        public IActionResult OnPostEditarHorario()
        {
            int centroId = ObtenerCentroActual();
            var vacunatorio = _repoVacunatorio.ObtenerPorCentro(centroId);

            if (vacunatorio == null)
            {
                TempData["Error"] = "No hay vacunatorio configurado para tu centro";
                return RedirectToPage();
            }

            var diasValidos = new[] { "lunes", "martes", "miercoles", "jueves", "viernes", "sabado" };
            var diasSeleccionados = diasValidos.Where(d => Request.Form["DiasEditar"].Contains(d)).ToList();

            if (!diasSeleccionados.Any())
            {
                TempData["Error"] = "Seleccioná al menos un día de atención";
                return RedirectToPage();
            }

            vacunatorio.HoraApertura = HoraAperturaEditar;
            vacunatorio.HoraCierre = HoraCierreEditar;
            vacunatorio.DiasAtencion = string.Join(",", diasSeleccionados);

            _repoVacunatorio.Actualizar(vacunatorio);

            _auditoria.Registrar(new SaludYa.Models.Auditoria
            {
                UsuarioId = ObtenerIdActual(),
                TablaAfectada = "vacunatorio",
                Accion = "UPDATE",
                ValNuevo = $"id={vacunatorio.Id}, apertura={vacunatorio.HoraApertura}, cierre={vacunatorio.HoraCierre}, dias={vacunatorio.DiasAtencion}",
                IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
            });

            TempData["Mensaje"] = "Horario del vacunatorio actualizado correctamente";
            return RedirectToPage();
        }

        private void CargarDatos()
        {
            int centroId = ObtenerCentroActual();
            Vacunatorio = _repoVacunatorio.ObtenerPorCentro(centroId);

            if (Vacunatorio != null)
            {
                Vacunas = _repoVacunaDisponible.ObtenerPorVacunatorio(Vacunatorio.Id);
            }
        }

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
    }
}
