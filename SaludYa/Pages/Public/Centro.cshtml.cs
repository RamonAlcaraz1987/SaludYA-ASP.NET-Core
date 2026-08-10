using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaludYa.Models;

namespace SaludYa.Pages.Public
{
    [AllowAnonymous]
    public class CentroModel : PageModel
    {
        private readonly IRepositorioCentroSalud _repoCentro;
        private readonly IRepositorioCronograma _repoCronograma;
        private readonly IRepositorioNovedadDiaria _repoNovedades;
        private readonly IRepositorioVacunatorio _repoVacunatorio;
        private readonly IRepositorioVacunaDisponible _repoVacunas;

        public CentroModel(
            IRepositorioCentroSalud repoCentro,
            IRepositorioCronograma repoCronograma,
            IRepositorioNovedadDiaria repoNovedades,
            IRepositorioVacunatorio repoVacunatorio,
            IRepositorioVacunaDisponible repoVacunas)
        {
            _repoCentro = repoCentro;
            _repoCronograma = repoCronograma;
            _repoNovedades = repoNovedades;
            _repoVacunatorio = repoVacunatorio;
            _repoVacunas = repoVacunas;
        }

        public CentroSalud? Centro { get; set; }
        public IList<SaludYa.Models.Cronograma> Cronogramas { get; set; } = new List<SaludYa.Models.Cronograma>();
        public IList<NovedadDiaria> Novedades { get; set; } = new List<NovedadDiaria>();
        public Vacunatorio? Vacunatorio { get; set; }
        public IList<VacunaDisponible> Vacunas { get; set; } = new List<VacunaDisponible>();

        public IActionResult OnGet(int id)
        {
            Centro = _repoCentro.ObtenerPorId(id);
            if (Centro == null) return NotFound();

            Cronogramas = _repoCronograma.ObtenerPorCentro(id);
            Novedades = _repoNovedades.ObtenerHoy(id);
            Vacunatorio = _repoVacunatorio.ObtenerPorCentro(id);

            if (Vacunatorio != null)
                Vacunas = _repoVacunas.ObtenerPorVacunatorio(Vacunatorio.Id);

            return Page();
        }
    }
}