using CapaDatos.ViewModel.RegistroDefectos;
using static CapaDatos.ViewModel.RegistroDefectos.DefectosTurnoActualViewModel;

namespace CapaControlador.Interface
{
    public interface IDefectosPrimeraController
    {
        Task<DefectosTurnoActualViewModel> ObtenerDatosNecesariosControl(int idOP);
        Task IncrementarDecrementarParesPrimera(int idJornadaLaboral, int horaSeleccionada, bool incrementar = true);
        Task<RegistroDefectoViewModel> IcrementarDecrementarDefectos(int idJornadaLAboral, int horaSeleccionada, int idDefecto, bool derecho = true, bool incrementar = true);
        Task DesvincularseOP(int idJornadaLaboral);
    }
}