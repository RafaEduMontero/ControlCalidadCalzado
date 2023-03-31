using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.ViewModel.RegistroDefectos
{
    public class DatosNecesariosControlViewModel
    {
        public int IdOrdenProduccion { get; set; }
        public int IdJornadaLAboral { get; set; }

        public DefectosTurnoActualViewModel DefectosTurnoActualViewModel { get; set; } = new();
    }
}
