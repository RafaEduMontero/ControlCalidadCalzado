using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Enums
{
    public enum RolesEmpleados
    {
        Administrador = 1,
        SupervisorLinea = 2,
        SupervisorCalidad = 3
    }

    public enum EstadosOP
    {
        Iniciada = 1,
        Pausada = 2,
        Finalizada = 3
    }

    public enum Pies
    {
        Izquierdo = 1,
        Derecho = 2
    }

    public enum Turnos
    {
        Mañana = 3,
        Tarde = 4,
        Noche = 5
    }

    public enum TipoDefecto
    {
        Reproceso = 1,
        Observado = 2
    }
}
