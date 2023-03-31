using CapaControlador.Interface;
using CapaDatos.Helper;
using CapaDatos.ViewModel.RegistroDefectos;
using Microsoft.AspNetCore.Components;
using static CapaDatos.ViewModel.RegistroDefectos.DefectosTurnoActualViewModel;

namespace ControlCalidad.Shared.ControlOP
{
    public partial class ParesPrimera
    {
        #region Inject
        [Inject] IDefectosPrimeraController DefectosPrimeraController { get; set; } = default!;
        #endregion

        #region Parameters
        [CascadingParameter] public DatosNecesariosControlViewModel DatosNecesariosControlViewModel { get; set; } = default!;
        #endregion

        #region Properties
        #endregion

        #region Events
        protected override async Task OnInitializedAsync()
        {
            
        }
        #endregion

        #region Methods
        public async Task IncrementarDecrementarParesPrimera(bool incrementar = true)
        {
            await DefectosPrimeraController.IncrementarDecrementarParesPrimera(DatosNecesariosControlViewModel.IdJornadaLAboral,
                                                                                  DatosNecesariosControlViewModel.DefectosTurnoActualViewModel.HoraSeleccionada,
                                                                                  incrementar);

            DatosNecesariosControlViewModel.DefectosTurnoActualViewModel.PrimeraViewModels.Add(new PrimeraViewModel { Cantidad = incrementar ? 1 : -1, Hora = HelperStatics.ObtnerHoraRegistrar(DatosNecesariosControlViewModel.DefectosTurnoActualViewModel.HoraSeleccionada) });

            await InvokeAsync(StateHasChanged);
        }
            
        #endregion
    }
}
