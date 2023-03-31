namespace CapaDatos.ViewModel.RegistroDefectos
{
    public class DefectosTurnoActualViewModel
    {
        public List<int> HorasTurno { get; set; } = new();
        public int HoraSeleccionada { get; set; }
        public List<DefectosViewModel> Defectos { get; set; } = new();
        public DatosOPViewModel DatosOpViewModel { get; set; } = new();
        public List<PrimeraViewModel> PrimeraViewModels { get; set; } = new();
        public List<RegistroDefectoViewModel> RegistroDefectoViewModelsDerecho { get; set; } = new();
        public List<RegistroDefectoViewModel> RegistroDefectoViewModelsIzquierdo { get; set; } = new();
        public TurnoViewModel TurnoActual { get; set; } = new();
        public class DefectosViewModel
        {
            public int IdDefecto { get; set; }
            public int IdTipoDefecto { get; set; }
            public int IdPie { get; set; }
            public string Descripcion { get; set; } = String.Empty;
            public int CantidadReprocesoIzquierdo { get; set; }
            public int CantidadReprocesoDerecho { get; set; }
            public int CantidadObservadoIzquierdo { get; set; }
            public int CantidadObservadoDerecho { get; set; }
            public int HoraDefecto { get; set; }
        }

        public class PrimeraViewModel
        {
            public DateTime Hora { get; set; }
            public int Cantidad { get; set; }
            public int HoraGet { get => Hora.Hour; }
            public int IdPie { get; set; }
        }

        public class RegistroDefectoViewModel
        {
            public int IdDefecto { get; set; }
            public DateTime Hora { get; set; }
            public int HoraGet { get => Hora.Hour; }
            public int Cantidad { get; set; }
            public int IdPie { get; set; }
            public bool GuardarOK { get; set; } = false;
            public bool OpFinalizada { get; set; } = false;
            public string NombreDefecto { get; set; } = String.Empty;
        }

        public class TurnoViewModel
        {
            public int IdTurno { get; set; }
            public string Descripcion { get; set; } = string.Empty;
        }

        public class DatosOPViewModel
        {
            public int IdEstadoOP { get; set; }
            public string Color { get; set; } = string.Empty;
            public string NumeroOP { get; set; } = string.Empty;
            public string NumeroLinea { get; set; } = string.Empty;
            public string Sku { get; set; } = string.Empty;
            public int LimiteInferior { get; set; }
            public int LimiteSiperior { get; set; }
        }
    }
}
