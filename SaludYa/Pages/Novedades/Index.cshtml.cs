using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaludYa.Models;
using System.Security.Claims;

namespace SaludYa.Pages.Novedades
{
    [Authorize(Policy = "ResponsableOSuperadmin")]
    public class IndexModel : PageModel
    {
        private readonly IRepositorioNovedadDiaria _repoNovedad;
        private readonly IRepositorioEspecialista _repoEspecialista;
        private readonly IRepositorioAuditoria _auditoria;

        public IndexModel(
            IRepositorioNovedadDiaria repoNovedad,
            IRepositorioEspecialista repoEspecialista,
            IRepositorioAuditoria auditoria)
        {
            _repoNovedad = repoNovedad;
            _repoEspecialista = repoEspecialista;
            _auditoria = auditoria;
        }

        public IList<Especialista> Especialistas { get; set; } = new List<Especialista>();
        public IList<NovedadDiaria> NovedadesHoy { get; set; } = new List<NovedadDiaria>();

        [BindProperty]
        public int EspecialistaId { get; set; }

        [BindProperty]
        public DateTime Fecha { get; set; } = DateTime.Today;

        [BindProperty]
        public string TipoNovedad { get; set; } = "ausencia";

        [BindProperty]
        public string? Descripcion { get; set; }

        [BindProperty]
        public TimeSpan? HoraNuevaInicio { get; set; }

        [BindProperty]
        public TimeSpan? HoraNuevaFin { get; set; }

        [BindProperty]
        public string? LugarNuevo { get; set; }

        public void OnGet()
        {
            CargarDatos();
            Fecha = DateTime.Today;
        }

        public IActionResult OnPost()
        {
            int centroId = ObtenerCentroActual();
            CargarDatos();

            if (EspecialistaId == 0)
            {
                TempData["Error"] = "Seleccioná un especialista";
                return RedirectToPage();
            }

            var especialista = _repoEspecialista.ObtenerPorId(EspecialistaId);
            if (especialista == null || especialista.CentroId != centroId)
            {
                TempData["Error"] = "Especialista no válido para tu centro";
                return RedirectToPage();
            }

            var novedad = new NovedadDiaria
            {
                EspecialistaId = EspecialistaId,
                CentroId = centroId,
                Fecha = Fecha == default ? DateTime.Today : Fecha,
                TipoNovedad = TipoNovedad,
                Descripcion = Descripcion,
                HoraNuevaInicio = TipoNovedad == "cambio_horario" ? HoraNuevaInicio : null,
                HoraNuevaFin = TipoNovedad == "cambio_horario" ? HoraNuevaFin : null,
                LugarNuevo = LugarNuevo,
                UsuarioCargaId = ObtenerIdActual()
            };

            int id = _repoNovedad.Alta(novedad);

            _auditoria.Registrar(new SaludYa.Models.Auditoria
            {
                UsuarioId = ObtenerIdActual(),
                TablaAfectada = "novedad_diaria",
                Accion = "INSERT",
                ValNuevo = $"id={id}, tipo={TipoNovedad}, especialista_id={EspecialistaId}",
                IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
            });

            TempData["Mensaje"] = "Novedad registrada correctamente. Los ciudadanos la verán al consultar el centro.";
            return RedirectToPage();
        }

        private void CargarDatos()
        {
            int centroId = ObtenerCentroActual();
            Especialistas = _repoEspecialista.ObtenerPorCentro(centroId);
            NovedadesHoy = _repoNovedad.ObtenerHoy(centroId);
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