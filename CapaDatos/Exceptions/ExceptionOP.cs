using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Exceptions
{
    public class ExceptionOP : Exception
    {
        public string ErrorLinea { get; set; } = "La línea ya se encuentra ocupada";
        public string ErrorNumero { get; set; } = "Ya existe una OP con el mismo número";
    }
}
