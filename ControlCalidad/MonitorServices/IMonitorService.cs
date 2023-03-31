using ControlCalidad.Data.ViewModel.Monitor;

namespace ControlCalidad.MonitorServices
{
    public interface IMonitorService
    {
        Task ActualizarMonitor(MonitorViewModel monitorViewModel);
        event Func<Task> UpdateEvent;
    }
}