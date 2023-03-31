using CapaControlador.Interface;
using CapaDatos.ViewModel.Login;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ControlCalidad.Pages.Login
{
    public partial class Login
    {
        #region Inject
        [Inject] public IAuthenticationController AuthenticationController { get; set; } = default!;
        #endregion

        #region Properties
        public LoginViewModel LoginViewModel { get; set; } = new();
        public MudForm RefForm { get; set; } = new();
        public bool isVisible { get; set; } = false;
        #endregion

        #region Parameters
        #endregion

        #region Events
        #endregion

        #region Methods
        private async Task LoginAccount()
        {
            isVisible = true;
            await Task.Delay(5);
            await RefForm.Validate();
            if (RefForm.IsValid)
                LoginViewModel = await AuthenticationController.LoginAccount(LoginViewModel);
            isVisible = false;
        }
        #endregion        
    }
}
