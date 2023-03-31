using CapaDatos.Models;

namespace CapaDatos.ViewModel.OP
{
    public class DatosNecesariosOPViewModel
    {
        public List<Linea> LineasDisponibles { get; set; } = new();
        public List<Modelo> ModelosDisponibles { get; set; } = new();
        public List<Color> ColoresDisponibles { get; set; } = new();
    }
}
