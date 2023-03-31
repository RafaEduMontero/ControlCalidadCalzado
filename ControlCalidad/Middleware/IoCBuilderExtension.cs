using CapaControlador.Middleware;
using CapaRepositorio.Middleware;
using ControlCalidad.MonitorServices;

namespace ControlCalidad.Middleware
{
    public static class IoCBuilderExtension
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            #region PDF
            //builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            #endregion

            #region Mapper
            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            builder.Services.AddSingleton<MonitorService>();
            #endregion

            #region Repositories
            builder.ConfigureServicesRepository();
            #endregion

            #region Controllers
            builder.ConfigureServicesController();
            #endregion
        }
    }
}
