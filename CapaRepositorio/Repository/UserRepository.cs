using CapaDatos.Context;
using CapaDatos.Enums;
using CapaDatos.Negocio;
using CapaDatos.ViewModel.Login;
using CapaRepositorio.Authentication;
using CapaRepositorio.Interface;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CapaRepositorio.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly CONTROLDECALIDADContext _context;
        private readonly AuthenticationStateProvider _authStateProvider;

        public UserRepository(CONTROLDECALIDADContext context, AuthenticationStateProvider authStateProvider)
        {
            _context = context;
            _authStateProvider = authStateProvider;
        }

        public async Task<UserSession> Login(LoginViewModel datos)
        {
            var userSession = new UserSession();
            var customAuthStateProvider = (AuthenticationStatePtoviderRepository)_authStateProvider;
            var admin = await _context.Administrador.Include(x => x.IdEmpleadoNavigation).Where(x => x.IdEmpleadoNavigation.Email == datos.Email && x.IdEmpleadoNavigation.Password == datos.Password).FirstOrDefaultAsync();
            var supervisorLinea = await _context.SupervisorLinea.Include(x => x.IdEmpleadoNavigation).Where(x => x.IdEmpleadoNavigation.Email == datos.Email && x.IdEmpleadoNavigation.Password == datos.Password).FirstOrDefaultAsync();
            var supervisorCalidad = await _context.SupervisorCalidad.Include(x => x.IdEmpleadoNavigation).Where(x => x.IdEmpleadoNavigation.Email == datos.Email && x.IdEmpleadoNavigation.Password == datos.Password).FirstOrDefaultAsync();

            if (admin != null)
            {
                userSession.Email = admin.IdEmpleadoNavigation.Email;
                userSession.Password = admin.IdEmpleadoNavigation.Password;
                userSession.Rol = Enum.GetName(typeof(RolesEmpleados), admin.IdEmpleadoNavigation.Rol);
                await customAuthStateProvider.UpdateAuthenticationState(userSession);
                userSession.Administrador = admin;
                return userSession;
            }

            if (supervisorLinea != null)
            {
                userSession.Email = supervisorLinea.IdEmpleadoNavigation.Email;
                userSession.Password = supervisorLinea.IdEmpleadoNavigation.Password;
                userSession.Rol = Enum.GetName(typeof(RolesEmpleados), supervisorLinea.IdEmpleadoNavigation.Rol);
                await customAuthStateProvider.UpdateAuthenticationState(userSession);
                userSession.SupervisorLinea = supervisorLinea;
                return userSession;
            }

            if (supervisorCalidad != null)
            {
                userSession.Email = supervisorCalidad.IdEmpleadoNavigation.Email;
                userSession.Password = supervisorCalidad.IdEmpleadoNavigation.Password;
                userSession.Rol = Enum.GetName(typeof(RolesEmpleados), supervisorCalidad.IdEmpleadoNavigation.Rol);
                await customAuthStateProvider.UpdateAuthenticationState(userSession);
                userSession.SupervisorCalidad = supervisorCalidad;
                return userSession;
            }

            return userSession = null;
        }

        public async Task<UserSession> ObtenerEmpleadoSession(LoginViewModel datos)
        {
            var userSession = new UserSession();

            var admin = await _context.Administrador
                                      .Include(x => x.IdEmpleadoNavigation)
                                      .Where(x => x.IdEmpleadoNavigation.Email == datos.Email && x.IdEmpleadoNavigation.Password == datos.Password)
                                      .FirstOrDefaultAsync();

            var supervisorLinea = await _context.SupervisorLinea
                                      .Include(x => x.IdEmpleadoNavigation)
                                      .Where(x => x.IdEmpleadoNavigation.Email == datos.Email && x.IdEmpleadoNavigation.Password == datos.Password)
                                      .FirstOrDefaultAsync();

            var supervisorCalidad = await _context.SupervisorCalidad
                                         .Include(x => x.IdEmpleadoNavigation)
                                         .Where(x => x.IdEmpleadoNavigation.Email == datos.Email && x.IdEmpleadoNavigation.Password == datos.Password)
                                         .FirstOrDefaultAsync();

            if (admin is not null)
            {
                userSession.Administrador = admin;
                return userSession;
            }

            if (supervisorLinea is not null)
            {
                userSession.SupervisorLinea = supervisorLinea;             
                return userSession;
            }

            if (supervisorCalidad is not null)
            {
                userSession.SupervisorCalidad = supervisorCalidad;
                userSession.JornadaLaboralActiva = _context.JornadaLaboral
                                                   .Where(x => (x.IdSupervisorCalidad == userSession.SupervisorCalidad.IdSupervisorCalidad) && (x.FechaFin == null))
                                                   .OrderByDescending(x => x.IdJornadaLaboral).FirstOrDefault() ?? new CapaDatos.Models.JornadaLaboral();
                return userSession;
            }

            return userSession = null;
        }

        public async Task Logout(UserSession user)
        {
            var customAuthStateProvider = (AuthenticationStatePtoviderRepository)_authStateProvider;
            await customAuthStateProvider.UpdateAuthenticationState(user);
        }
    }
}
