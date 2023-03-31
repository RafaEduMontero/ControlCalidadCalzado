using CapaDatos.ViewModel.OP;

namespace CapaControlador.Interface
{
    public interface IOrdenProduccionController
    {
        Task<DatosNecesariosOPViewModel> InicializarDatosOrdenProduccion();
        Task<OrdenProduccionViewModel> ObtenerOrdenProduccionPorIdSupervisorLinea(int idSupervisorLinea);
        Task<OrdenProduccionViewModel> CrearOrdenProduccion(CreateOrdenProduccionViewModel datosSeleccionados);
        Task<bool> ActualizarEstadoOP(int idOP, int idTipoEstadoOP);
        Task<List<OPDisponiblesViewModel>> ObtenerOPDisponiblesParaControl();
        Task VincularseOP(OPDisponiblesViewModel op, int idSupervisorCalidad);
    }
}