using CapaControlador.Authentication;
using CapaControlador.Controller;
using CapaControlador.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CapaControlador.Middleware
{
    public static class BuilderExtensionController
    {
        public static void ConfigureServicesController(this WebApplicationBuilder builder)
        {
            #region Mapper
            //builder.Services.AddAutoMapper(typeof(Program).Assembly);
            #endregion

            #region Auth

            #endregion

            #region Controllers
            builder.Services.AddScoped<IAuthenticationController, AuthenticationController>();
            builder.Services.AddScoped<IOrdenProduccionController, OrdenProduccionController>();
            builder.Services.AddScoped<IDefectosPrimeraController, DefectosPrimeraController>();
            #endregion
        }
    }
}
