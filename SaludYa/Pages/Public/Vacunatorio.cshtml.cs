using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaludYa.Models;

namespace SaludYa.Pages.Public
{
    [AllowAnonymous]
    public class VacunatorioPublicoModel : PageModel
    {
        private readonly IRepositorioCentroSalud _repoCentros;
        private readonly IRepositorioVacunatorio _repoVacunatorio;
        private readonly IRepositorioVacunaDisponible _repoVacunas;

        public VacunatorioPublicoModel(
            IRepositorioCentroSalud repoCentros,
            IRepositorioVacunatorio repoVacunatorio,
            IRepositorioVacunaDisponible repoVacunas)
        {
            _repoCentros = repoCentros;
            _repoVacunatorio = repoVacunatorio;
            _repoVacunas = repoVacunas;
        }

        public IList<CentroSalud> Centros { get; set; } = new List<CentroSalud>();
        public CentroSalud? CentroSeleccionado { get; set; }
        public int CentroSeleccionadoId { get; set; }
        public Vacunatorio? Vacunatorio { get; set; }
        public IList<VacunaDisponible> Vacunas { get; set; } = new List<VacunaDisponible>();

        public IActionResult OnGet(int? id)
        {
            Centros = _repoCentros.ObtenerTodos();

            if (id.HasValue)
            {
                CentroSeleccionadoId = id.Value;
                CentroSeleccionado = _repoCentros.ObtenerPorId(id.Value);
                if (CentroSeleccionado != null)
                {
                    Vacunatorio = _repoVacunatorio.ObtenerPorCentro(id.Value);
                    if (Vacunatorio != null)
                        Vacunas = _repoVacunas.ObtenerPorVacunatorio(Vacunatorio.Id);
                }
            }

            return Page();
        }
    }
}