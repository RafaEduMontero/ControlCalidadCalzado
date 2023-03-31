using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.ViewModel.OP
{
    public class OPDisponiblesViewModel
    {
        public int IdOrdenProduccion { get; set; }
        public string NumeroOP { get; set; } = string.Empty;
        public string ColorCalzado { get; set; } = string.Empty;
        public string ModeloCalzado { get; set; } = string.Empty;
        public string NumeroLinea { get; set; } = string.Empty;
        public int EstadoOP { get; set; }
    }
}
