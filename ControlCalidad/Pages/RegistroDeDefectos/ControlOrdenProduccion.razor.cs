using CapaControlador.Interface;
using CapaDatos.Enums;
using CapaDatos.Helper;
using CapaDatos.ViewModel.RegistroDefectos;
using ControlCalidad.Data.ViewModel.Monitor;
using ControlCalidad.MonitorServices;
using Microsoft.AspNetCore.Components;
using static CapaDatos.ViewModel.RegistroDefectos.DefectosTurnoActualViewModel;

namespace ControlCalidad.Pages.RegistroDeDefectos
{
    public partial class ControlOrdenProduccion : IDisposable
    {
        #region Inject
        [Inject] private IDefectosPrimeraController DefectosPrimeraController { get; set; } = default!;
        [Inject] NavigationManager NavigationManager { get; set; } = default!;
        [Inject] MonitorService MonitorService { get; set; } = default!;
        #endregion

        #region Parameters
        [Parameter] public int IdOP { get; set; }
        [Parameter] public int IdJornadaLaboral { get; set; }
        #endregion

        #region Properties
        private DefectosTurnoActualViewModel DefectosTurnoActualViewModel { get; set; } = new();
        private MonitorViewModel MonitorViewModel { get; set; } = new();
        public bool DisabledButtons { get; set; } = false;
        public bool DisabledAllButtons { get; set; } = false;
        public bool IsVisibleActualizar { get; set; } = false;
        #endregion

        #region Events
        protected override async Task OnInitializedAsync()
        {
            MonitorService.UpdateEvent += async () => await InvokeAsync(() => this.StateHasChanged());
            DefectosTurnoActualViewModel = await DefectosPrimeraController.ObtenerDatosNecesariosControl(IdOP);
            IsVisibleActualizar = DefectosTurnoActualViewModel.DatosOpViewModel.IdEstadoOP == (int)EstadosOP.Pausada ? true : false;
            if (IsVisibleActualizar)
                DisabledAllButtons = true;

            MonitorViewModel.DefectosTurnoActualViewModel = DefectosTurnoActualViewModel;
            MonitorViewModel.IdOP = DefectosTurnoActualViewModel.DatosOpViewModel.IdEstadoOP;
            MonitorService.ActualizarMonitor(MonitorViewModel);
        }
        #endregion

        #region Methods
        private async Task IncrementarDecrementarParesPrimera(bool incrementar = true)
        {
            await DefectosPrimeraController.IncrementarDecrementarParesPrimera(IdJornadaLaboral,
                                                                               DefectosTurnoActualViewModel.HoraSeleccionada,
                                                                               incrementar);

            DefectosTurnoActualViewModel.PrimeraViewModels.Add(new PrimeraViewModel { Cantidad = incrementar ? 1 : -1, Hora = HelperStatics.ObtnerHoraRegistrar(DefectosTurnoActualViewModel.HoraSeleccionada) });
        }

        private async Task IncrementarDecrementarDefectos(DefectosViewModel defectoView, bool observado = true, bool derecho = true, bool incrementar = true)
        {
            DisabledButtons = true;
            var defectoReturn = await DefectosPrimeraController.IcrementarDecrementarDefectos(IdJornadaLaboral,
                                                                                              DefectosTurnoActualViewModel.HoraSeleccionada,
                                                                                              defectoView.IdDefecto, derecho, incrementar);
            if (defectoReturn != null && defectoReturn.IdDefecto > 0)
            {

                if (!defectoReturn.OpFinalizada)
                {
                    if (defectoReturn.IdPie == (int)Pies.Izquierdo)
                        DefectosTurnoActualViewModel.RegistroDefectoViewModelsIzquierdo.Add(defectoReturn);
                    else
                        DefectosTurnoActualViewModel.RegistroDefectoViewModelsDerecho.Add(defectoReturn);

                    try
                    {
                        MonitorService.MonitorViewModel.DefectosTurnoActualViewModel = DefectosTurnoActualViewModel;
                        MonitorService.ActualizarMonitor(MonitorService.MonitorViewModel);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    DisabledButtons = true;
                    MonitorService.ActualizarMonitor(MonitorService.MonitorViewModel);
                    return;
                }
            }
            DisabledButtons = false;
        }

        private int TotalDefectoRegistrado(DefectosViewModel defectoView, bool izquierdo = true)
        {
            return izquierdo ?
                    DefectosTurnoActualViewModel.RegistroDefectoViewModelsIzquierdo.Where(x => x.IdDefecto == defectoView.IdDefecto && x.HoraGet == DefectosTurnoActualViewModel.HoraSeleccionada).Sum(x => x.Cantidad) :
                    DefectosTurnoActualViewModel.RegistroDefectoViewModelsDerecho.Where(x => x.IdDefecto == defectoView.IdDefecto && x.HoraGet == DefectosTurnoActualViewModel.HoraSeleccionada).Sum(x => x.Cantidad);
        }

        private int TotalParesPrimera()
            => DefectosTurnoActualViewModel.PrimeraViewModels.Where(x => x.HoraGet == DefectosTurnoActualViewModel.HoraSeleccionada).Sum(x => x.Cantidad);

        private void DesvincularseOP()
            => DefectosPrimeraController.DesvincularseOP(IdJornadaLaboral);

        public void Dispose()
        {
            MonitorService.UpdateEvent -= StateHasChanged;
            GC.SuppressFinalize(this);
        }

        public void OpenMonito()
        {
            var optionsNavigation = new  NavigationOptions {  };
        }
        #endregion
    }
}
