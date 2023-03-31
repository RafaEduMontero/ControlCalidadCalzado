using CapaDatos.Models;

namespace CapaRepositorio.Interface
{
    public interface IUnitOfWork
    {
        IGenericRepository<Administrador> AdministradorRepository { get; }
        IGenericRepository<Color> ColorRepository { get; }
        IGenericRepository<Defecto> DefectoRepository { get; }
        IGenericRepository<Empleado> EmpleadoRepository { get; }
        IGenericRepository<JornadaLaboral> JornadaLaboralRepository { get; }
        IGenericRepository<Linea> LineaRepository { get; }
        IGenericRepository<Modelo> ModeloRepository { get; }
        IGenericRepository<OrdenProduccion> OrdenProduccionRepository { get; }
        IGenericRepository<Primera> PrimeraRepository { get; }
        IGenericRepository<RegistroDefecto> RegistroDefectoRepository { get; }
        IGenericRepository<SupervisorCalidad> SupervisorCalidadRepository { get; }
        IGenericRepository<SupervisorLinea> SupervisorLineaRepository { get; }
        IGenericRepository<TipoDefecto> TipoDefectoRepository { get; }
        IGenericRepository<Turno> TurnoRepository { get; }

        void Dispose();
        void SaveChanges();
        Task SaveChangesAsync();
    }
}