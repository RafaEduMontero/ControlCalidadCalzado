using ControlCalidad.Data.ViewModel.Monitor;
using ControlCalidad.MonitorServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using static CapaDatos.ViewModel.RegistroDefectos.DefectosTurnoActualViewModel;

namespace ControlCalidad.Pages.Monitor
{
    public partial class Monitor : IDisposable
    {
        [Inject] MonitorService MonitorService { get; set; } = default!;

        [Parameter] public int IdOP { get; set; }
        [Parameter] public int IdJornadaLaboral { get; set; }
        
        public List<RegistroDefectoViewModel> RegistrosDefectosViewModels { get; set; } = new();
        private const string InfoFormat = "{first_item}-{last_item} de {all_items}";

        private int TotalDefectos { get; set; }

        protected override void OnInitialized()
        {
            MonitorService.UpdateEvent += async () => await InvokeAsync(() => this.StateHasChanged());
        }

        public void Dispose()
        {
            MonitorService.UpdateEvent -= StateHasChanged;
            GC.SuppressFinalize(this);
        }
    }
}
