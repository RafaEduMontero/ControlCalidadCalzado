using CapaDatos.ViewModel.RegistroDefectos;
using static CapaDatos.ViewModel.RegistroDefectos.DefectosTurnoActualViewModel;

namespace ControlCalidad.Data.ViewModel.Monitor
{
    public class MonitorViewModel
    {
        public DefectosTurnoActualViewModel DefectosTurnoActualViewModel { get; set; } = new();
        public int IdOP { get; set; }
        public int IdTipoEstadoOP { get; set; }
    }
}
