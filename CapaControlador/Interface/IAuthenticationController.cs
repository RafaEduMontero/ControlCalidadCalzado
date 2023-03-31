using CapaDatos.Negocio;
using CapaDatos.ViewModel.Login;

namespace CapaControlador.Interface
{
    public interface IAuthenticationController
    {
        Task<LoginViewModel> LoginAccount(LoginViewModel loginView);
        Task<UserSession> ObtenerEmpleadoSession(LoginViewModel loginView);
        Task Logout(UserSession userSession);
    }
}