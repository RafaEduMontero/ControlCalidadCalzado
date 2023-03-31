using CapaDatos.Models;
using CapaDatos.ViewModel.RegistroDefectos;
using static CapaDatos.ViewModel.RegistroDefectos.DefectosTurnoActualViewModel;

namespace CapaRepositorio.Interface
{
    public interface IDefectosPrimeraRepository
    {
        Task<DefectosTurnoActualViewModel> ObtenerDatosNecesariosControl(int idOP);
        Task<(bool, bool)> IncrementarDecrementarParesPrimera(int idJornadaLaboral, int horaSeleccionada, bool incrementar = true);
        Task<RegistroDefectoViewModel> IncrementarDecrementarDefectos(int idJornadaLAboral, int horaSeleccionada, int idDefecto, bool derecho = true, bool incrementar = true);
        Task<int> ObtenerIdTipoDefecto(int idDefecto);
        Task<bool> DesvincularseOP(int idJornadaLaboral);
    }
}