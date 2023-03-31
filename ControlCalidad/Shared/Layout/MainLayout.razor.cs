using CapaControlador.Interface;
using CapaDatos.Negocio;
using CapaRepositorio.Negocio;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace ControlCalidad.Shared.Layout
{
    public partial class MainLayout
    {
        #region Inject
        [Inject] IAuthenticationController AuthenticationController { get; set; } = default!;
        [Inject] NavigationManager Na { get; set; } = default!;
        #endregion

        #region Parameters
        [CascadingParameter] private Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        #endregion

        #region Properties
        public UserSession EmpleadoSession { get; set; } = null;
        #endregion

        #region Events
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationState;
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var datos = user.UserProfile();
                EmpleadoSession = await AuthenticationController.ObtenerEmpleadoSession(datos);
            }
        }
        #endregion

        #region Methods
        private async Task Logout()
            => await AuthenticationController.Logout(EmpleadoSession);
        #endregion
    }
}
