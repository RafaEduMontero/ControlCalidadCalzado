using AutoMapper;
using CapaControlador.Interface;
using CapaDatos.Enums;
using CapaDatos.Negocio;
using CapaDatos.ViewModel.OP;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ControlCalidad.Pages.OrdenProduccion
{
    public partial class AdministracionOrdenProduccion
    {
        #region Inject
        [Inject] IDialogService ServicesDialog { get; set; } = default!;
        [Inject] IMapper Mapper { get; set; } = default!;
        [Inject] IOrdenProduccionController OrdenProduccionController { get; set; } = default!;
        [Inject] NavigationManager NavigationManager { get; set; } = default!;
        #endregion

        #region Parameters
        [CascadingParameter] public UserSession EmpleadoSession { get; set; }
        #endregion

        #region Properties
        private DatosNecesariosOPViewModel DatosNecesariosOPViewModel { get; set; } = new();
        private OrdenProduccionViewModel OP { get; set; } = new();
        private CreateOrdenProduccionViewModel CrearOPViewModel { get; set; } = new();
        private MudForm RefForm { get; set; } = new();
        private bool IsVisible { get; set; } = false;
        private bool Spinner { get; set; } = false;
        private bool IsVisibleBtnIniciar { get; set; } = false;
        private bool IsVisibleBtnPausar { get; set; } = false;
        private bool IsVisibleBtnFinalizar { get; set; } = false;
        #endregion

        #region Events
        protected override async Task OnInitializedAsync()
        {
            Spinner = true;
            if (EmpleadoSession != null && EmpleadoSession.SupervisorLinea.IdEmpleado != 0)
            {
                var op = await OrdenProduccionController.ObtenerOrdenProduccionPorIdSupervisorLinea(EmpleadoSession.SupervisorLinea.IdSupervisorLinea);

                if (op.IdOrdenProduccion > 0 && op.IdTipoEstado != (int)EstadosOP.Finalizada)
                {
                    OP = op;
                    IsVisible = true;
                    EstadoBotones();
                }
                else
                    DatosNecesariosOPViewModel = await OrdenProduccionController.InicializarDatosOrdenProduccion();
            }
            Spinner = false;
        }
        #endregion

        #region Methods
        private async Task CrearOrdenProduccion()
        {
            Spinner = true;
            await RefForm.Validate();
            if (RefForm.IsValid)
            {
                CrearOPViewModel.IdSupervisorLinea = EmpleadoSession.SupervisorLinea.IdSupervisorLinea;
                var returnOp = await OrdenProduccionController.CrearOrdenProduccion(CrearOPViewModel);
                if (returnOp.IdOrdenProduccion > 0)
                {
                    OP = await OrdenProduccionController.ObtenerOrdenProduccionPorIdSupervisorLinea(returnOp.IdSupervisorLinea);
                    IsVisible = true;
                    EstadoBotones();
                }
                else
                {
                    Mapper.Map(returnOp, CrearOPViewModel);
                    if (returnOp.LineaOcupada)
                    {
                        var datosnecesarios = await OrdenProduccionController.InicializarDatosOrdenProduccion();
                        DatosNecesariosOPViewModel.LineasDisponibles = datosnecesarios.LineasDisponibles;
                        CrearOPViewModel.IdLinea = 0;
                    }
                }
            }

            Spinner = false;
        }

        private async Task IniciarOPausarOP(EstadosOP estadoOP)
        {
            Spinner = true;
            var idTipoEstadoOP = estadoOP switch
            {
                EstadosOP.Iniciada => (int)EstadosOP.Iniciada,
                EstadosOP.Pausada => (int)EstadosOP.Pausada,
                _ => (int)EstadosOP.Finalizada,
            };

            var returnOK = await OrdenProduccionController.ActualizarEstadoOP(OP.IdOrdenProduccion, idTipoEstadoOP);

            if (returnOK && (idTipoEstadoOP != (int)EstadosOP.Finalizada))
            {
                OP.IdTipoEstado = idTipoEstadoOP;
                EstadoBotones();
            }
            else
                NavigationManager.NavigateTo(NavigationManager.Uri,true);

            Spinner = false;
        }

        private static IEnumerable<string> ValidarLinea(int idLinea)
        {
            if (idLinea == 0)
                yield return "Debe seleccionar una línea";
        }

        private static IEnumerable<string> ValidarColor(int idColor)
        {
            if (idColor == 0)
                yield return "Debe seleccionar un color";
        }

        private static IEnumerable<string> ValidarModelo(int idModelo)
        {
            if (idModelo == 0)
                yield return "Debe seleccionar un modelo";
        }

        private static IEnumerable<string> ValidadNumeroOP(string numeroOP)
        {
            if (string.IsNullOrEmpty(numeroOP))
                yield return "Debe ingresar un número de OP";
        }

        private void EstadoBotones()
        {
            if (OP.IdTipoEstado == (int)EstadosOP.Iniciada)
            {
                IsVisibleBtnIniciar = true;
                IsVisibleBtnPausar = false;
                IsVisibleBtnFinalizar = false;
            }

            if (OP.IdTipoEstado == (int)EstadosOP.Pausada)
            {
                IsVisibleBtnIniciar = false;
                IsVisibleBtnPausar = true;
                IsVisibleBtnFinalizar = false;
            }
        }
        #endregion
    }
}
