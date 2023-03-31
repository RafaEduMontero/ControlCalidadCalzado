using AutoMapper;
using CapaControlador.Interface;
using CapaDatos.Enums;
using CapaDatos.ViewModel.RegistroDefectos;
using CapaRepositorio.Interface;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using static CapaDatos.ViewModel.RegistroDefectos.DefectosTurnoActualViewModel;

namespace CapaControlador.Controller
{
    public class DefectosPrimeraController : IDefectosPrimeraController
    {
        private readonly IDefectosPrimeraRepository _defectosPrimeraRepository;
        private readonly IMapper _mapper;
        private readonly IDialogService _dialogService;
        private readonly NavigationManager _navigationManager;

        public DefectosPrimeraController(IDefectosPrimeraRepository defectosPrimeraRepository, IMapper mapper, NavigationManager navigationManager, IDialogService dialogService)
        {
            _defectosPrimeraRepository = defectosPrimeraRepository;
            _mapper = mapper;
            _navigationManager = navigationManager;
            _dialogService = dialogService;
        }

        public async Task<DefectosTurnoActualViewModel> ObtenerDatosNecesariosControl(int idOP) => await _defectosPrimeraRepository.ObtenerDatosNecesariosControl(idOP);

        public async Task IncrementarDecrementarParesPrimera(int idJornadaLaboral, int horaSeleccionada, bool incrementar = true)
        {
            var (guardarOk, isFinalizada) = await _defectosPrimeraRepository.IncrementarDecrementarParesPrimera(idJornadaLaboral, horaSeleccionada, incrementar);

            if (!guardarOk)
            {
                await _dialogService.ShowMessageBox("Error", $"Error al intentar insertar un par a Primera. La OP se encuentra en estado {(isFinalizada ? Enum.GetName(typeof(EstadosOP), (int)EstadosOP.Finalizada) : Enum.GetName(typeof(EstadosOP), (int)EstadosOP.Pausada))}");
                _navigationManager.NavigateTo($"{(isFinalizada ? "/OrdenesProduccion" : _navigationManager.Uri)}", true);
            }
        }

        public async Task<RegistroDefectoViewModel> IcrementarDecrementarDefectos(int idJornadaLAboral, int horaSeleccionada, int idDefecto, bool derecho = true, bool incrementar = true)
        {
            var registroDefecto = await _defectosPrimeraRepository.IncrementarDecrementarDefectos(idJornadaLAboral, horaSeleccionada, idDefecto, derecho, incrementar);

            if (!registroDefecto.GuardarOK)
            {
                await _dialogService.ShowMessageBox("Error", $"Error al intentar insertar un defecto. La OP se encuentra en estado {(registroDefecto.OpFinalizada ? Enum.GetName(typeof(EstadosOP), (int)EstadosOP.Finalizada) : Enum.GetName(typeof(EstadosOP), (int)EstadosOP.Pausada))}");
                _navigationManager.NavigateTo($"{(registroDefecto.OpFinalizada ? "/OrdenesProduccion" : _navigationManager.Uri)}", true);
            }

            return registroDefecto;
        }

        public async Task DesvincularseOP(int idJornadaLaboral)
        {
            var pressOK = await _dialogService.ShowMessageBox("Atención", "Está por desvincularse de OP. ¿Está seguro de continuar con la acción?", "Si", null, "No");

            if (pressOK == true)
            {
                var returnOK = await _defectosPrimeraRepository.DesvincularseOP(idJornadaLaboral);
                _navigationManager.NavigateTo("/OrdenesProduccion", true);
            }
        }
    }
}
