using CapaDatos.ViewModel.OP;

namespace CapaRepositorio.Interface
{
    public interface IOrdenProduccionRepository
    {
        Task<DatosNecesariosOPViewModel> InicializarDatosOrdenProduccion();
        Task<OrdenProduccionViewModel> ObtenerOrdenProduccionPoirIdSupervisorLinea(int idSupervisorLinea);
        Task<OrdenProduccionViewModel> CrearOrdenProduccion(CreateOrdenProduccionViewModel datosSeleccionados);
        Task ActualizarEstadoOP(int idOP, int idTipoEstado);
        Task<List<OPDisponiblesViewModel>> ObtenerOPDisponiblesParaControl();
        Task<(bool, int)> VincularseOP(int idOP, int idSupervisorCalidad);
    }
}