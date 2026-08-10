using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaludYa.Models;

namespace SaludYa.Pages.Public
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly IRepositorioCentroSalud _repoCentros;
        private readonly IRepositorioNovedadDiaria _repoNovedades;

        public IndexModel(IRepositorioCentroSalud repoCentros, IRepositorioNovedadDiaria repoNovedades)
        {
            _repoCentros = repoCentros;
            _repoNovedades = repoNovedades;
        }

        public IList<CentroSalud> Centros { get; set; } = new List<CentroSalud>();
        public Dictionary<int, int> NovedadesHoy { get; set; } = new Dictionary<int, int>();

        public void OnGet()
        {
            Centros = _repoCentros.ObtenerTodos();
            foreach (var centro in Centros)
            {
                var novedades = _repoNovedades.ObtenerHoy(centro.Id);
                NovedadesHoy[centro.Id] = novedades.Count;
            }
        }
    }
}