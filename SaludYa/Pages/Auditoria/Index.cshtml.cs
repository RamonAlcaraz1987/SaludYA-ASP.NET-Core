using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaludYa.Models;
using System.Text.RegularExpressions;

namespace SaludYa.Pages.Auditoria
{
    [Authorize(Policy = "SoloSuperadmin")]
    public class IndexModel : PageModel
    {
        private readonly IRepositorioAuditoria _repositorio;
        private readonly IRepositorioEspecialista _repoEspecialista;
        private readonly IRepositorioCentroSalud _repoCentro;
        private readonly IRepositorioUsuario _repoUsuario;

        public IndexModel(
            IRepositorioAuditoria repositorio,
            IRepositorioEspecialista repoEspecialista,
            IRepositorioCentroSalud repoCentro,
            IRepositorioUsuario repoUsuario)
        {
            _repositorio = repositorio;
            _repoEspecialista = repoEspecialista;
            _repoCentro = repoCentro;
            _repoUsuario = repoUsuario;
        }

        public IList<SaludYa.Models.Auditoria> Registros { get; set; } = new List<SaludYa.Models.Auditoria>();

        [BindProperty(SupportsGet = true)]
        public DateTime? Fecha { get; set; }

        private Dictionary<int, string> _especialistas = new();
        private Dictionary<int, string> _centros = new();
        private Dictionary<int, string> _usuarios = new();

        public void OnGet()
        {
            Registros = Fecha.HasValue
                ? _repositorio.ObtenerPorFecha(Fecha.Value)
                : _repositorio.ObtenerRecientes(200);

            foreach (var c in _repoCentro.ObtenerTodos())
                _centros[c.Id] = c.Nombre;

            foreach (var c in _repoCentro.ObtenerTodos())
                foreach (var e in _repoEspecialista.ObtenerTodosPorCentro(c.Id))
                    _especialistas[e.Id] = e.Nombre;

            foreach (var u in _repoUsuario.ObtenerTodos())
                _usuarios[u.Id] = u.Nombre;
        }

        public string FormatearDetalle(string? texto, string tabla)
        {
            if (string.IsNullOrEmpty(texto)) return "—";

            texto = Regex.Replace(texto, @"especialista_id=(\d+)", m =>
            {
                int id = int.Parse(m.Groups[1].Value);
                return _especialistas.ContainsKey(id) ? $"especialista={_especialistas[id]}" : m.Value;
            });

            texto = Regex.Replace(texto, @"centro_id=(\d+)", m =>
            {
                int id = int.Parse(m.Groups[1].Value);
                return _centros.ContainsKey(id) ? $"centro={_centros[id]}" : m.Value;
            });

            texto = Regex.Replace(texto, @"usuario_id=(\d+)", m =>
            {
                int id = int.Parse(m.Groups[1].Value);
                return _usuarios.ContainsKey(id) ? $"usuario={_usuarios[id]}" : m.Value;
            });

            texto = Regex.Replace(texto, @"^id=\d+,?\s*", "");
            return texto;
        }
    }
}