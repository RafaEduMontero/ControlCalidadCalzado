using CapaDatos.Context;
using CapaDatos.Models;
using CapaRepositorio.GenericRepository;
using CapaRepositorio.Interface;

namespace CapaRepositorio.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CONTROLDECALIDADContext _context;

        public UnitOfWork(CONTROLDECALIDADContext context)
        {
            _context = context;
        }

        private readonly IGenericRepository<Administrador> _administradorRepository;
        private readonly IGenericRepository<Color> _colorRepository;
        private readonly IGenericRepository<Defecto> _defectoRepository;
        private readonly IGenericRepository<Empleado> _empleadoRepository;
        private readonly IGenericRepository<JornadaLaboral> _jornadaLaboralRepository;
        private readonly IGenericRepository<Linea> _linea;
        private readonly IGenericRepository<Modelo> _modelo;
        private readonly IGenericRepository<OrdenProduccion> _ordenProduccion;
        private readonly IGenericRepository<Primera> _primera;
        private readonly IGenericRepository<RegistroDefecto> _registroDefecto;
        private readonly IGenericRepository<SupervisorLinea> _supervisorLinea;
        private readonly IGenericRepository<SupervisorCalidad> _supervisorCalidad;
        private readonly IGenericRepository<TipoDefecto> _tipoDefecto;
        private readonly IGenericRepository<Turno> _turno;

        public IGenericRepository<Administrador> AdministradorRepository => _administradorRepository ?? new GenericRepository<Administrador>(_context);
        public IGenericRepository<Color> ColorRepository => _colorRepository ?? new GenericRepository<Color>(_context);
        public IGenericRepository<Defecto> DefectoRepository => _defectoRepository ?? new GenericRepository<Defecto>(_context);
        public IGenericRepository<Empleado> EmpleadoRepository => _empleadoRepository ?? new GenericRepository<Empleado>(_context);
        public IGenericRepository<JornadaLaboral> JornadaLaboralRepository => _jornadaLaboralRepository ?? new GenericRepository<JornadaLaboral>(_context);
        public IGenericRepository<Linea> LineaRepository => _linea ?? new GenericRepository<Linea>(_context);
        public IGenericRepository<Modelo> ModeloRepository => _modelo ?? new GenericRepository<Modelo>(_context);
        public IGenericRepository<OrdenProduccion> OrdenProduccionRepository => _ordenProduccion ?? new GenericRepository<OrdenProduccion>(_context);
        public IGenericRepository<Primera> PrimeraRepository => _primera ?? new GenericRepository<Primera>(_context);
        public IGenericRepository<RegistroDefecto> RegistroDefectoRepository => _registroDefecto ?? new GenericRepository<RegistroDefecto>(_context);
        public IGenericRepository<SupervisorLinea> SupervisorLineaRepository => _supervisorLinea ?? new GenericRepository<SupervisorLinea>(_context);
        public IGenericRepository<SupervisorCalidad> SupervisorCalidadRepository => _supervisorCalidad ?? new GenericRepository<SupervisorCalidad>(_context);
        public IGenericRepository<TipoDefecto> TipoDefectoRepository => _tipoDefecto ?? new GenericRepository<TipoDefecto>(_context);
        public IGenericRepository<Turno> TurnoRepository => _turno ?? new GenericRepository<Turno>(_context);

        public void Dispose()
        {
            if (_context is not null)
                _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
