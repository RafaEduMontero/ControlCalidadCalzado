using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.ViewModel.OP
{
    public class CreateOrdenProduccionViewModel
    {
        public int IdLinea { get; set; }
        public int IdSupervisorLinea { get; set; }
        public int IdModelo { get; set; }
        public int IdColor { get; set; }
        public int IdTipoEstado { get; set; }
        public string Numero { get; set; }
        public DateTime FechaInicio { get; set; }
    }
}
