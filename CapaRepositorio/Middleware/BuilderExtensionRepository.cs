using CapaRepositorio.Authentication;
using CapaRepositorio.GenericRepository;
using CapaRepositorio.Interface;
using CapaRepositorio.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace CapaRepositorio.Middleware
{
    public static class BuilderExtensionRepository
    {
        public static void ConfigureServicesRepository(this WebApplicationBuilder builder)
        {
            #region Mapper
            
            #endregion

            #region Auth
            builder.Services.AddAuthenticationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationStatePtoviderRepository>();
            #endregion

            #region Repositories            
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork, CapaRepositorio.UnitOfWork.UnitOfWork>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IOrdenProduccionRepository, OrdenProduccionRepository>();
            builder.Services.AddScoped<IDefectosPrimeraRepository, DefectosPrimeraRepository>();
            #endregion
        }
    }
}
