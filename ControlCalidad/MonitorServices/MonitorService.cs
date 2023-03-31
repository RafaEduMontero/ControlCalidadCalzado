using CapaDatos.ViewModel.RegistroDefectos;
using ControlCalidad.Data.ViewModel.Monitor;
using MudBlazor;
using static CapaDatos.ViewModel.RegistroDefectos.DefectosTurnoActualViewModel;

namespace ControlCalidad.MonitorServices
{
    public class MonitorService
    {
        public MonitorViewModel MonitorViewModel { get; set; } = new();
        public List<RegistroDefectoViewModel> RegistroDefectos { get; set; } = new();

        public bool ExistenDefectos { get; set; } = false;

        public Color ColorVerde { get; set; }
        public Color ColorAmarillo { get; set; }
        public Color ColorRojo { get; set; }
        public Variant VariantVerde { get; set; }
        public Variant VariantAmarillo { get; set; }
        public Variant VariantRojo { get; set; }

        public event Action UpdateEvent;

        public void ActualizarMonitor(MonitorViewModel monitorViewModel)
        {
            MonitorViewModel = monitorViewModel;
            ActualizarListaDefectos();
            ExecuteAction();
        }

        public void ActualizarListaDefectos()
        {
            if (RegistroDefectos.Count > 0)
                RegistroDefectos.Clear();

            RegistroDefectos.AddRange(MonitorViewModel.DefectosTurnoActualViewModel.RegistroDefectoViewModelsDerecho);
            RegistroDefectos.AddRange(MonitorViewModel.DefectosTurnoActualViewModel.RegistroDefectoViewModelsIzquierdo);

            RegistroDefectos = RegistroDefectos.GroupBy(x => x.IdDefecto).Select(g => new RegistroDefectoViewModel
            {
                IdDefecto = g.Key,
                Cantidad = g.Sum(x => x.Cantidad)
            }).OrderByDescending(g => g.Cantidad).ToList();

            if (RegistroDefectos.Count == 0)
                ExistenDefectos = false;
            else
                ExistenDefectos = true;

            if (RegistroDefectos.Sum(x => x.Cantidad) <= MonitorViewModel.DefectosTurnoActualViewModel.DatosOpViewModel.LimiteInferior)
            {
                ColorVerde = Color.Success;
                VariantVerde = Variant.Filled;
                ColorAmarillo = Color.Default;
                VariantAmarillo = Variant.Outlined;
                ColorRojo = Color.Default;
                VariantRojo = Variant.Outlined;
            }
            else
            {
                if (MonitorViewModel.DefectosTurnoActualViewModel.DatosOpViewModel.LimiteInferior < RegistroDefectos.Sum(x => x.Cantidad) && RegistroDefectos.Sum(x => x.Cantidad) <= MonitorViewModel.DefectosTurnoActualViewModel.DatosOpViewModel.LimiteSiperior)
                {
                    ColorVerde = Color.Default;
                    VariantVerde = Variant.Outlined;
                    ColorAmarillo = Color.Warning;
                    VariantAmarillo = Variant.Filled;
                    ColorRojo = Color.Default;
                    VariantRojo = Variant.Outlined;
                }
                else
                {
                    ColorVerde = Color.Default;
                    VariantVerde = Variant.Outlined;
                    ColorAmarillo = Color.Default;
                    VariantAmarillo = Variant.Outlined;
                    ColorRojo = Color.Error;
                    VariantRojo = Variant.Filled;
                }
            }
        }
        void ExecuteAction() => UpdateEvent?.Invoke();
    }
}
