using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaludYa.Models;
using System.Security.Claims;

namespace SaludYa.Pages.Cronograma
{
    [Authorize(Policy = "ResponsableOSuperadmin")]
    public class IndexModel : PageModel
    {
        private readonly IRepositorioCronograma _repoCronograma;
        private readonly IRepositorioEspecialista _repoEspecialista;
        private readonly IRepositorioAuditoria _auditoria;

        public IndexModel(
            IRepositorioCronograma repoCronograma,
            IRepositorioEspecialista repoEspecialista,
            IRepositorioAuditoria auditoria)
        {
            _repoCronograma = repoCronograma;
            _repoEspecialista = repoEspecialista;
            _auditoria = auditoria;
        }

        public IList<Especialista> Especialistas { get; set; } = new List<Especialista>();
        public IList<SaludYa.Models.Cronograma> Cronogramas { get; set; } = new List<SaludYa.Models.Cronograma>();

        // Fecha sugerida de inicio = día siguiente al último cronograma vigente
        // Si no hay ninguno, hoy.
        public DateTime FechaInicioSugerida { get; set; } = DateTime.Today;
        public DateTime FechaFinSugerida { get; set; } = DateTime.Today.AddMonths(1).AddDays(-1);
        public bool TieneCronogramaVigente { get; set; } = false;
        public DateTime? VencimientoActual { get; set; }

        [BindProperty] public int EspecialistaId { get; set; }
        [BindProperty] public DateTime FechaInicio { get; set; }
        [BindProperty] public string TipoPeriodo { get; set; } = "mensual";
        [BindProperty] public int TurnosDisponibles { get; set; }
        [BindProperty] public string TipoTurno { get; set; } = "orden_llegada";
        [BindProperty] public bool TurnoMismoDia { get; set; }
        [BindProperty] public string? DiaTurno { get; set; }
        [BindProperty] public TimeSpan? HoraInicioTurno { get; set; }
        [BindProperty] public TimeSpan? HoraFinTurno { get; set; }
        [BindProperty] public string? ObservacionesTurno { get; set; }

        public void OnGet()
        {
            CargarDatos();
            // Inicializar fecha por defecto en hoy (no en 0001)
            FechaInicio = FechaInicioSugerida;
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

            // ── Validar que no haya un cronograma vigente para ese especialista ──
            var cronogramasEsp = _repoCronograma.ObtenerPorEspecialista(EspecialistaId);
            var vigente = cronogramasEsp.FirstOrDefault(c => c.FechaFin >= DateTime.Today);
            if (vigente != null)
            {
                // Permitir carga anticipada solo si vence en 7 días o menos
                int diasRestantes = (vigente.FechaFin - DateTime.Today).Days;
                if (diasRestantes > 7)
                {
                    TempData["Error"] = $"Este especialista ya tiene un cronograma vigente hasta el " +
                        $"{vigente.FechaFin:dd/MM/yyyy} ({diasRestantes} días restantes). " +
                        $"Podés cargar el próximo cuando falten 7 días o menos para que venza.";
                    return RedirectToPage();
                }

                // La fecha de inicio debe ser el día siguiente al vencimiento
                FechaInicio = vigente.FechaFin.AddDays(1);
            }

            if (FechaInicio < DateTime.Today)
            {
                TempData["Error"] = "La fecha de inicio no puede ser en el pasado";
                return RedirectToPage();
            }

            // ── Calcular fecha fin automáticamente según tipo período ────────────
            DateTime fechaFin = TipoPeriodo == "bimestral"
                ? FechaInicio.AddMonths(2).AddDays(-1)
                : FechaInicio.AddMonths(1).AddDays(-1);

            var cronograma = new SaludYa.Models.Cronograma
            {
                EspecialistaId = EspecialistaId,
                CentroId = centroId,
                FechaInicio = FechaInicio,
                FechaFin = fechaFin,
                TipoPeriodo = TipoPeriodo,
                TurnosDisponibles = TurnosDisponibles,
                TipoTurno = TipoTurno,
                UsuarioCargaId = ObtenerIdActual()
            };

            int cronogramaId = _repoCronograma.Alta(cronograma);

            // ── Horarios de atención ─────────────────────────────────────────────
            var diasValidos = new[] { "lunes", "martes", "miercoles", "jueves", "viernes", "sabado" };
            int diasCargados = 0;

            foreach (var dia in diasValidos)
            {
                if (Request.Form["DiasSeleccionados"].Contains(dia))
                {
                    var hiStr = Request.Form[$"HoraInicio_{dia}"].ToString();
                    var hfStr = Request.Form[$"HoraFin_{dia}"].ToString();

                    if (TimeSpan.TryParse(hiStr, out var hi) && TimeSpan.TryParse(hfStr, out var hf))
                    {
                        _repoCronograma.AgregarHorario(new HorarioCronograma
                        {
                            CronogramaId = cronogramaId,
                            DiaSemana = dia,
                            HoraInicio = hi,
                            HoraFin = hf
                        });
                        diasCargados++;
                    }
                }
            }

            if (diasCargados == 0)
                TempData["Error"] = "El cronograma se creó pero no se cargó ningún día de atención.";

            // ── Horario de turnos ────────────────────────────────────────────────
            if (HoraInicioTurno.HasValue && HoraFinTurno.HasValue)
            {
                _repoCronograma.AgregarHorarioTurnos(new HorarioTurnos
                {
                    CronogramaId = cronogramaId,
                    MismoDia = TurnoMismoDia,
                    DiaSemana = TurnoMismoDia ? null : DiaTurno,
                    HoraInicio = HoraInicioTurno.Value,
                    HoraFin = HoraFinTurno.Value,
                    Observaciones = ObservacionesTurno
                });
            }

            _auditoria.Registrar(new SaludYa.Models.Auditoria
            {
                UsuarioId = ObtenerIdActual(),
                TablaAfectada = "cronograma",
                Accion = "INSERT",
                ValNuevo = $"id={cronogramaId}, esp={EspecialistaId}, inicio={FechaInicio:yyyy-MM-dd}, fin={fechaFin:yyyy-MM-dd}",
                IpOrigen = HttpContext.Connection.RemoteIpAddress?.ToString()
            });

            TempData["Mensaje"] = $"Cronograma cargado: {FechaInicio:dd/MM/yyyy} al {fechaFin:dd/MM/yyyy}";
            return RedirectToPage();
        }

        private void CargarDatos()
        {
            int centroId = ObtenerCentroActual();
            Especialistas = _repoEspecialista.ObtenerPorCentro(centroId);
            Cronogramas = _repoCronograma.ObtenerPorCentro(centroId);

            // Calcular fecha sugerida: día siguiente al último vencimiento de cualquier especialista
            var ultimoVencimiento = Cronogramas
                .Where(c => c.FechaFin >= DateTime.Today)
                .OrderByDescending(c => c.FechaFin)
                .FirstOrDefault();

            if (ultimoVencimiento != null)
            {
                TieneCronogramaVigente = true;
                VencimientoActual = ultimoVencimiento.FechaFin;
                FechaInicioSugerida = ultimoVencimiento.FechaFin.AddDays(1);
            }
            else
            {
                FechaInicioSugerida = DateTime.Today;
            }

            FechaFinSugerida = FechaInicioSugerida.AddMonths(1).AddDays(-1);
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
