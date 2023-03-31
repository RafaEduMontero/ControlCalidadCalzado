using CapaControlador.Interface;
using CapaDatos.Negocio;
using CapaDatos.ViewModel.Login;
using CapaRepositorio.Interface;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador.Authentication
{
    public class AuthenticationController : IAuthenticationController
    {
        private readonly NavigationManager _navigationManager;
        private readonly ISnackbar _snackbar;
        private readonly IUserRepository _userRepository;

        public AuthenticationController(NavigationManager navigationManager, ISnackbar snackbar, IUserRepository userRepository)
        {
            _navigationManager = navigationManager;
            _snackbar = snackbar;
            _userRepository = userRepository;
        }

        public async Task<LoginViewModel> LoginAccount(LoginViewModel loginView)
        {
            var userSession = await _userRepository.Login(loginView);

            if (userSession is not null)
            {
                if (userSession.Administrador is not null && userSession.Administrador.IdAdministrador != 0)
                    _navigationManager.NavigateTo("/Administracion", true);

                if (userSession.SupervisorLinea is not null && userSession.SupervisorLinea.IdSupervisorLinea != 0)
                    _navigationManager.NavigateTo("/OrdenProduccion", true);

                if (userSession.SupervisorCalidad is not null && userSession.SupervisorCalidad.IdSupervisorCalidad != 0)
                {
                    if(userSession.JornadaLaboralActiva.IdJornadaLaboral > 0)
                        _navigationManager.NavigateTo($"/ControlDefectos/OP/{userSession.JornadaLaboralActiva.IdOrdenProduccion}/{userSession.JornadaLaboralActiva.IdJornadaLaboral}", true);
                    else
                        _navigationManager.NavigateTo("/OrdenesProduccion", true);
                }
            }
            else
            {
                _snackbar.Clear();
                _snackbar.Add("Email o contraseña erroneos", Severity.Error);
            }

            loginView = new();
            return loginView;
        }
        public async Task<UserSession> ObtenerEmpleadoSession(LoginViewModel loginView) => await _userRepository.ObtenerEmpleadoSession(loginView);

        public async Task Logout(UserSession userSession)
        {
            userSession = null;
            await _userRepository.Logout(userSession);
            _navigationManager.NavigateTo("/", true);
        }
    }
}
