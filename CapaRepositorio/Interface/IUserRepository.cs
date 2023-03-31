using CapaDatos.Negocio;
using CapaDatos.ViewModel.Login;

namespace CapaRepositorio.Interface
{
    public interface IUserRepository
    {
        Task<UserSession> Login(LoginViewModel datos);
        Task<UserSession> ObtenerEmpleadoSession(LoginViewModel datos);
        Task Logout(UserSession user);
    }
}