using CapaDatos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.ViewModel.OP
{
    public class OrdenProduccionViewModel
    {
        public int IdOrdenProduccion { get; set; }
        public int IdLinea { get; set; }
        public int IdSupervisorLinea { get; set; }
        public int IdModelo { get; set; }
        public int IdColor { get; set; }
        public int IdTipoEstado { get; set; }
        public string Numero { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool LineaOcupada { get; set; } = false;
        public bool NumeroOpExistente { get; set; } = false;

        public Color IdColorNavigation { get; set; }
        public Linea IdLineaNavigation { get; set; }
        public Modelo IdModeloNavigation { get; set; }
        public SupervisorLinea IdSupervisorLineaNavigation { get; set; }
        public ICollection<JornadaLaboral> JornadaLaboral { get; set; }
    }
}
