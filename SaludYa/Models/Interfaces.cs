namespace SaludYa.Models
{
    // ─── USUARIO ─────────────────────────────────────────────────────────────────
    public interface IRepositorioUsuario
    {
        Usuario? ObtenerPorEmail(string email);
        Usuario? ObtenerPorId(int id);
        IList<Usuario> ObtenerTodos();
        int Alta(Usuario usuario);
        int Actualizar(Usuario usuario);
        void CambiarContrasena(int idUsuario, string hashNuevo);
        void DesactivarUsuario(int id);
        // NUEVO: permite reactivar o desactivar en un solo método
        void CambiarEstadoUsuario(int id, bool activo);
    }

    // ─── CENTRO DE SALUD ─────────────────────────────────────────────────────────
    public interface IRepositorioCentroSalud
    {
        // NUEVO: parámetro opcional para poder listar también los inactivos (gestión superadmin)
        IList<CentroSalud> ObtenerTodos(bool soloActivos = true);
        CentroSalud? ObtenerPorId(int id);
        int Alta(CentroSalud centro);
        int Actualizar(CentroSalud centro);
        void Desactivar(int id);
        // NUEVO: permite reactivar o desactivar en un solo método
        void CambiarEstado(int id, bool activo);
    }

    // ─── ESPECIALISTA ─────────────────────────────────────────────────────────────
    public interface IRepositorioEspecialista
    {
        IList<Especialista> ObtenerPorCentro(int centroId);           // solo activos (para la app y cronograma)
        IList<Especialista> ObtenerTodosPorCentro(int centroId);      // activos e inactivos (para gestión)
        IList<Especialista> ObtenerPorEspecialidad(string especialidad);
        Especialista? ObtenerPorId(int id);
        int Alta(Especialista especialista);
        int Actualizar(Especialista especialista);
        void Desactivar(int id);
        // NUEVO: permite reactivar o desactivar en un solo método
        void CambiarEstado(int id, bool activo);
    }

    // ─── CRONOGRAMA ──────────────────────────────────────────────────────────────
    public interface IRepositorioCronograma
    {
        IList<Cronograma> ObtenerPorCentro(int centroId);
        IList<Cronograma> ObtenerPorEspecialista(int especialistaId);
        Cronograma? ObtenerPorId(int id);
        int Alta(Cronograma cronograma);
        void AgregarHorario(HorarioCronograma horario);
        void AgregarHorarioTurnos(HorarioTurnos horarioTurnos);
        void EliminarHorarios(int cronogramaId);
    }

    // ─── NOVEDAD DIARIA ───────────────────────────────────────────────────────────
    public interface IRepositorioNovedadDiaria
    {
        IList<NovedadDiaria> ObtenerPorCentroYFecha(int centroId, DateTime fecha);
        IList<NovedadDiaria> ObtenerHoy(int centroId);
        int Alta(NovedadDiaria novedad);
    }

    // ─── VACUNATORIO ──────────────────────────────────────────────────────────────
    public interface IRepositorioVacunatorio
    {
        Vacunatorio? ObtenerPorCentro(int centroId);
        int Alta(Vacunatorio vacunatorio);
        int Actualizar(Vacunatorio vacunatorio);
    }

    // ─── CATÁLOGO VACUNAS ─────────────────────────────────────────────────────────
    public interface IRepositorioCatalogoVacunas
    {
        IList<CatalogoVacuna> ObtenerTodos(bool soloActivos = true);
        CatalogoVacuna? ObtenerPorId(int id);
        int Alta(CatalogoVacuna vacuna);
        int Actualizar(CatalogoVacuna vacuna);
        void Desactivar(int id);
        // NUEVO: permite reactivar o desactivar en un solo método
        void CambiarEstado(int id, bool activo);
    }

    // ─── VACUNA DISPONIBLE ────────────────────────────────────────────────────────
    public interface IRepositorioVacunaDisponible
    {
        IList<VacunaDisponible> ObtenerPorVacunatorio(int vacunatorioId);
        int AltaOActualizar(VacunaDisponible vd);
    }

    // ─── AUDITORÍA ────────────────────────────────────────────────────────────────
    public interface IRepositorioAuditoria
    {
        void Registrar(Auditoria auditoria);
        IList<Auditoria> ObtenerRecientes(int cantidad = 100);
        IList<Auditoria> ObtenerPorFecha(DateTime fecha, int cantidad = 500);
    }
}
