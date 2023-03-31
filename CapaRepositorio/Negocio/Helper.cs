using CapaDatos.ViewModel.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CapaRepositorio.Negocio
{
    public static class Helper
    {
        public static LoginViewModel UserProfile(this ClaimsPrincipal claimsPrincipal)
        {
            LoginViewModel datos = new();
            datos.Email = claimsPrincipal.Identity.Name;
            datos.Password = claimsPrincipal.FindFirst(x => x.Type == "Password").Value;
            return datos;
        }
    }
}
