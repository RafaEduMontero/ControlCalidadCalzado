using CapaControlador.Interface;
using CapaDatos.Enums;
using CapaDatos.ViewModel.OP;
using CapaRepositorio.Interface;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador.Controller
{
    public class OrdenProduccionController : IOrdenProduccionController
    {
        private readonly IOrdenProduccionRepository _ordenProduccionRepository;
        private readonly IDialogService _dialogService;
        private readonly ISnackbar _snackbar;
        private readonly NavigationManager _navigationManager;
        public OrdenProduccionController(IOrdenProduccionRepository ordenProduccionRepository, IDialogService dialogService, ISnackbar snackbar, NavigationManager navigationManager)
        {
            _ordenProduccionRepository = ordenProduccionRepository;
            _dialogService = dialogService;
            _snackbar = snackbar;
            _navigationManager = navigationManager;
        }

        #region Properties
        private const string REDIRECTO = "/ControlDefectos/Op/";
        private const string ERRORVINCULARSE = "Error al querer vincularse a OP";
        #endregion

        public async Task<DatosNecesariosOPViewModel> InicializarDatosOrdenProduccion() => await _ordenProduccionRepository.InicializarDatosOrdenProduccion();

        public async Task<OrdenProduccionViewModel> ObtenerOrdenProduccionPorIdSupervisorLinea(int idSupervisorLinea) => await _ordenProduccionRepository.ObtenerOrdenProduccionPoirIdSupervisorLinea(idSupervisorLinea);

        public async Task<OrdenProduccionViewModel> CrearOrdenProduccion(CreateOrdenProduccionViewModel datosSeleccionados) => await _ordenProduccionRepository.CrearOrdenProduccion(datosSeleccionados);

        public async Task<bool> ActualizarEstadoOP(int idOP,int idTipoEstadoOP)
        {
            bool returnOK = true;
            try
            {
                await _ordenProduccionRepository.ActualizarEstadoOP(idOP, idTipoEstadoOP);

                if (idTipoEstadoOP != (int)EstadosOP.Finalizada)
                {
                    Severity severity = idTipoEstadoOP == (int)EstadosOP.Iniciada ? Severity.Success : Severity.Warning;
                    _snackbar.Clear();
                    _snackbar.Add($"La OP paso a estado {Enum.GetName(typeof(EstadosOP), idTipoEstadoOP)}", severity);
                }
            }catch(Exception ex)
            {
                await _dialogService.ShowMessageBox("Error", "Error al actualizar estado OP. Comunicarse con el Administrador.");
                returnOK = false;
            }

            return returnOK;
        }

        public Task<List<OPDisponiblesViewModel>> ObtenerOPDisponiblesParaControl() => _ordenProduccionRepository.ObtenerOPDisponiblesParaControl();

        public async Task VincularseOP(OPDisponiblesViewModel op, int idSupervisorCalidad)
        {
            (bool retorno, int idJornadaLaboral) = await _ordenProduccionRepository.VincularseOP(op.IdOrdenProduccion, idSupervisorCalidad);
            if (retorno)
                _navigationManager.NavigateTo($"{REDIRECTO}{op.IdOrdenProduccion}/{idJornadaLaboral}");
            else
            {
                await _dialogService.ShowMessageBox("Error", $"{ERRORVINCULARSE} N°{op.NumeroOP}");
                _navigationManager.NavigateTo(_navigationManager.Uri, true);
            }
        }
    }
}
