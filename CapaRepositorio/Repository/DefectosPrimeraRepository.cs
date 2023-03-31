using AutoMapper;
using CapaDatos.Context;
using CapaDatos.Enums;
using CapaDatos.Helper;
using CapaDatos.Models;
using CapaDatos.ViewModel.RegistroDefectos;
using CapaRepositorio.Interface;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using static CapaDatos.ViewModel.RegistroDefectos.DefectosTurnoActualViewModel;

namespace CapaRepositorio.Repository
{
    public class DefectosPrimeraRepository : IDefectosPrimeraRepository
    {
        private readonly CONTROLDECALIDADContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDialogService _dialogService;
        private readonly NavigationManager _navigationManager;

        public DefectosPrimeraRepository(CONTROLDECALIDADContext context, IUnitOfWork unitOfWork, IMapper mapper, IDialogService dialogService, NavigationManager navigationManager)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _dialogService = dialogService;
            _navigationManager = navigationManager;
        }

        public async Task<DefectosTurnoActualViewModel> ObtenerDatosNecesariosControl(int idOP)
        {
            var turnos = await _unitOfWork.TurnoRepository.GetAllAsync();
            var defectos = await _unitOfWork.DefectoRepository.GetAllAsync();

            var turnoActual = ObtenerTurnoActual(turnos);

            DefectosTurnoActualViewModel datosNecesariosControlViewModel = new();

            ActualizarHorasTurno(datosNecesariosControlViewModel, turnoActual);

            _mapper.Map(defectos.ToList(), datosNecesariosControlViewModel.Defectos);
            _mapper.Map(turnoActual, datosNecesariosControlViewModel.TurnoActual);

            ObtenerRegistroDefectosParesPrimera(idOP, datosNecesariosControlViewModel);

            datosNecesariosControlViewModel.DatosOpViewModel = _context.OrdenProduccion
                                                                       .Include(x => x.IdModeloNavigation)
                                                                       .Include(x => x.IdLineaNavigation)
                                                                       .Include(x => x.IdColorNavigation)
                                                                       .Where(x => x.IdOrdenProduccion == idOP)
                                                                       .Select(x => new DatosOPViewModel
                                                                       {
                                                                           IdEstadoOP = x.IdTipoEstado,
                                                                           Color = x.IdColorNavigation.Codigo,
                                                                           Sku = x.IdModeloNavigation.Sku,
                                                                           NumeroLinea = x.IdLineaNavigation.Numero,
                                                                           NumeroOP = x.Numero,
                                                                           LimiteInferior = int.Parse(x.IdModeloNavigation.LimiteInferiorReproceso),
                                                                           LimiteSiperior = int.Parse(x.IdModeloNavigation.LimiteSuperiorReproceso)
                                                                       }).FirstOrDefault() ?? new();
            return datosNecesariosControlViewModel;
        }

        private Turno ObtenerTurnoActual(IEnumerable<Turno> turnos)
        {
            foreach (var turno in turnos)
            {
                if (turno.IdTurno == (int)Turnos.Noche)
                {
                    TimeSpan antesMediaNoche = new TimeSpan(23, 59, 59);
                    TimeSpan despuesMediaNoche = new TimeSpan(0, 0, 0);
                    if ((turno.HoraInicio <= DateTime.Now.TimeOfDay && antesMediaNoche >= DateTime.Now.TimeOfDay) ||
                        (despuesMediaNoche <= DateTime.Now.TimeOfDay && turno.HoraFin >= DateTime.Now.TimeOfDay))
                        return turno;
                }
                else
                {
                    if (turno.HoraInicio <= DateTime.Now.TimeOfDay && turno.HoraFin >= DateTime.Now.TimeOfDay)
                        return turno;
                }
            }
            return new Turno();
        }

        private void ActualizarHorasTurno(DefectosTurnoActualViewModel datosNecesariosControlViewModel, Turno turnoActual)
        {
            var horaInicioTurno = turnoActual.HoraInicio.Hours;
            var horaFinTurno = turnoActual.HoraFin.Hours;
            if (turnoActual.IdTurno == (int)Turnos.Noche)
            {
                while (horaInicioTurno <= 23)
                {
                    datosNecesariosControlViewModel.HorasTurno.Add(horaInicioTurno);
                    horaInicioTurno++;
                }

                int medianoche = 0;

                while (medianoche <= horaFinTurno)
                {
                    datosNecesariosControlViewModel.HorasTurno.Add(medianoche);
                    medianoche++;
                }
            }
            else
            {
                while (horaInicioTurno <= horaFinTurno)
                {
                    datosNecesariosControlViewModel.HorasTurno.Add(horaInicioTurno);
                    horaInicioTurno++;
                }
            }
            datosNecesariosControlViewModel.HoraSeleccionada = DateTime.Now.Hour;
        }

        private void ObtenerRegistroDefectosParesPrimera(int idOP, DefectosTurnoActualViewModel defectosTurnoActualViewModel)
        {
            var jornadaLaboralPorIdOP = _context.JornadaLaboral.Where(x => (x.IdOrdenProduccion == idOP) && (x.FechaFin == null));

            if (jornadaLaboralPorIdOP != null && jornadaLaboralPorIdOP.Count() > 0)
            {
                List<RegistroDefecto> registrosDefectos = new();
                List<Primera> primera = new();

                foreach (var jornada in jornadaLaboralPorIdOP)
                {
                    var registrosPorJornada = _context.RegistroDefecto.Where(x => x.IdJornadaLaboral == jornada.IdJornadaLaboral);
                    var paresPrimeraPorJornada = _context.Primera.Where(x => x.IdJornadaLaboral == jornada.IdJornadaLaboral);

                    registrosDefectos.AddRange(registrosPorJornada);
                    primera.AddRange(paresPrimeraPorJornada);
                }

                defectosTurnoActualViewModel.RegistroDefectoViewModelsDerecho = _mapper.Map<List<RegistroDefectoViewModel>>(registrosDefectos.Where(x => x.IdPie == (int)Pies.Derecho).ToList());
                defectosTurnoActualViewModel.RegistroDefectoViewModelsIzquierdo = _mapper.Map<List<RegistroDefectoViewModel>>(registrosDefectos.Where(x => x.IdPie == (int)Pies.Izquierdo).ToList()); ;

                _mapper.Map(primera, defectosTurnoActualViewModel.PrimeraViewModels);
            }

        }

        public async Task<(bool,bool)> IncrementarDecrementarParesPrimera(int idJornadaLaboral,int horaSeleccionada,bool incrementar=true)
        {
            bool guardarOK = false;            
            int cantidad = incrementar ? 1 : -1;

            var horaRegistrar = HelperStatics.ObtnerHoraRegistrar(horaSeleccionada);

            var primera = new Primera
            {
                IdJornadaLaboral = idJornadaLaboral,
                Cantidad = cantidad,
                Hora = horaRegistrar
            };

            var estadoOP = _context.JornadaLaboral.Include(x => x.IdOrdenProduccionNavigation).Where(x => x.IdJornadaLaboral == idJornadaLaboral).Select(x => x.IdOrdenProduccionNavigation.IdTipoEstado).FirstOrDefault();

            if (estadoOP == (int)EstadosOP.Iniciada)
            {
                await _unitOfWork.PrimeraRepository.CreateAsync(primera);
                await _unitOfWork.SaveChangesAsync();
                guardarOK = true;
            }

            bool finalizada = estadoOP == (int)EstadosOP.Finalizada ? true : false;

            return (guardarOK, finalizada);
        }

        public async Task<RegistroDefectoViewModel> IncrementarDecrementarDefectos(int idJornadaLAboral,int horaSeleccionada, int idDefecto, bool derecho = true, bool incrementar = true)
        {
            RegistroDefectoViewModel registroDefectoViewModel = new();

            var idPie = derecho ? (int)Pies.Derecho : (int)Pies.Izquierdo;
            var cantidad = incrementar ? 1 : -1;
            var horaRegistrar = HelperStatics.ObtnerHoraRegistrar(horaSeleccionada);

            var registroDefecto = new RegistroDefecto
            {
                IdJornadaLaboral = idJornadaLAboral,
                IdDefecto = idDefecto,
                IdPie = idPie,
                Cantidad = cantidad,
                Hora = horaRegistrar
            };

            var estadoOP = _context.JornadaLaboral.Include(x => x.IdOrdenProduccionNavigation).Where(x => x.IdJornadaLaboral == idJornadaLAboral).Select(x => x.IdOrdenProduccionNavigation.IdTipoEstado).FirstOrDefault();

            if(estadoOP == (int)EstadosOP.Iniciada)
            {
                try
                {
                    await _unitOfWork.RegistroDefectoRepository.CreateEntityAsync(registroDefecto);
                    await _unitOfWork.SaveChangesAsync();
                    _mapper.Map(registroDefecto, registroDefectoViewModel);
                    registroDefectoViewModel.GuardarOK = true;
                }catch(Exception ex)
                {
                    registroDefectoViewModel.GuardarOK = false;
                }
            }
            else
                registroDefectoViewModel.OpFinalizada = (estadoOP == (int)EstadosOP.Finalizada) ? true : false;


            return registroDefectoViewModel;
        }

        public async Task<int> ObtenerIdTipoDefecto(int idDefecto)
        {
            var defecto = await _unitOfWork.DefectoRepository.GetByIdAsync(idDefecto);
            return defecto.IdTipoDefecto;
        }

        public async Task<bool> DesvincularseOP(int idJornadaLaboral)
        {
            OrdenProduccion orden = new();
            return await orden.FinalizarJornadaLaboral(_context, idJornadaLaboral);
        }

    }
}
