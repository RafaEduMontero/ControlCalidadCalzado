using CapaControlador.Interface;
using CapaDatos.Negocio;
using CapaDatos.ViewModel.OP;
using Microsoft.AspNetCore.Components;

namespace ControlCalidad.Pages.RegistroDeDefectos
{
    public partial class OrdenesDisponiblesParaControl
    {
        #region Inject
        [Inject] IOrdenProduccionController OrdenProduccionController { get; set; } = default!;
        [Inject] NavigationManager NavigationManager { get; set; } = default!;
        #endregion

        #region Parameters
        [CascadingParameter] public UserSession EmpleadoSession { get; set; }
        #endregion

        #region Properties
        private List<OPDisponiblesViewModel> OrdenesProduccionDisponiblesParaControl { get; set; } = new();
        private const string OPDISPONIBLES = "Órdenes de Producción disponibles para Controlar";
        private const string OPNODISPONIBLES = "No existen Órdenes de Producción disponibles para Controlar";
        #endregion

        #region Events
        protected override async Task OnInitializedAsync()
        {
            if(EmpleadoSession.JornadaLaboralActiva.IdJornadaLaboral > 0)
            {
                NavigationManager.NavigateTo($"/ControlDefectos/OP/{EmpleadoSession.JornadaLaboralActiva.IdOrdenProduccion}/{EmpleadoSession.JornadaLaboralActiva.IdJornadaLaboral}",true);
                return;
            }

            OrdenesProduccionDisponiblesParaControl = await OrdenProduccionController.ObtenerOPDisponiblesParaControl();
        }
        #endregion

        #region Methods
        private async Task VincularseOP(OPDisponiblesViewModel opDiponible) => await OrdenProduccionController.VincularseOP(opDiponible, EmpleadoSession.SupervisorCalidad.IdSupervisorCalidad);
        #endregion
    }
}
