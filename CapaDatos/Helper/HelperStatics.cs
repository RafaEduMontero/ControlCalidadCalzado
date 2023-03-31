using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Helper
{
    public static class HelperStatics
    {
        public static DateTime ObtnerHoraRegistrar(int horaSeleccionada)
        {
            TimeSpan horaPrimeraODefecto = TimeSpan.FromHours(horaSeleccionada);
            return new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, DateTime.Now.Day) + horaPrimeraODefecto;
        }
    }
}
