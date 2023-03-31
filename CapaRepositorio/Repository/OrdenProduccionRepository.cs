using AutoMapper;
using CapaDatos.Context;
using CapaDatos.Enums;
using CapaDatos.Models;
using CapaDatos.ViewModel.OP;
using CapaRepositorio.Interface;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaRepositorio.Repository
{
    public class OrdenProduccionRepository : IOrdenProduccionRepository
    {
        private readonly CONTROLDECALIDADContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDialogService _dialogService;
        private readonly NavigationManager _navigationManager;

        #region Properties
        private const string LINEAOCUPADA = "La Línea ya se encuentra ocupada";
        private const string NUMEROOPEXISTENTE = "Ya existe una OP con el mismo número";
        private const string USUARIOCONOPNOFINALIZADA = "Usted cuenta con una OP activa";
        #endregion

        public OrdenProduccionRepository(CONTROLDECALIDADContext context, IUnitOfWork unitOfWork, IMapper mapper, IDialogService dialogService, NavigationManager navigationManager)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _dialogService = dialogService;
            _navigationManager = navigationManager;
        }

        public async Task<DatosNecesariosOPViewModel> InicializarDatosOrdenProduccion()
        {
            var colores = await _unitOfWork.ColorRepository.GetAllAsync();
            var modelos = await _unitOfWork.ModeloRepository.GetAllAsync();
            var lineasDisponibles = await _unitOfWork.LineaRepository.GetAllAsync();
            var opsEnCurso = await _unitOfWork.OrdenProduccionRepository.GetAllAsync();

            var opsEnCursoNoFinalizadas = opsEnCurso.Where(x => x.IdTipoEstado != (int)EstadosOP.Finalizada).Select(x => x.IdLinea);

            foreach (var op in opsEnCursoNoFinalizadas)
            {
                lineasDisponibles = lineasDisponibles.Where(x => x.IdLinea != op);
            }

            DatosNecesariosOPViewModel datosOP = new DatosNecesariosOPViewModel
            {
                ColoresDisponibles = colores.ToList(),
                LineasDisponibles = lineasDisponibles.ToList(),
                ModelosDisponibles = modelos.ToList(),
            };

            return datosOP;
        }

        public async Task<OrdenProduccionViewModel> ObtenerOrdenProduccionPoirIdSupervisorLinea(int idSupervisorLinea)
        {
            OrdenProduccionViewModel opView = new();
            try
            {
                var op = await _context.OrdenProduccion
                               .Include(x => x.IdColorNavigation)
                               .Include(x => x.IdLineaNavigation)
                               .Include(x => x.IdModeloNavigation)
                               .Include(x => x.IdSupervisorLineaNavigation).ThenInclude(x => x.IdEmpleadoNavigation)
                               .FirstAsync(x => (x.IdSupervisorLinea == idSupervisorLinea) && (x.IdTipoEstado != (int)EstadosOP.Finalizada));

                if (op != null)
                    opView = _mapper.Map<OrdenProduccionViewModel>(op);
            }
            catch (Exception ex)
            {

            }

            return opView;
        }

        public async Task<OrdenProduccionViewModel> CrearOrdenProduccion(CreateOrdenProduccionViewModel datosSeleccionados)
        {
            OrdenProduccionViewModel opView = new();
            OrdenProduccion op = new();
            _mapper.Map(datosSeleccionados, op);
            op = await op.CrearOrdenProduccion(_context, op);

            if (op.UsuarioConOpNoFinalizada)
            {
                await _dialogService.ShowMessageBox("Atención", USUARIOCONOPNOFINALIZADA);
                _navigationManager.NavigateTo(_navigationManager.Uri, true);
            }

            if (op.LineaOcupada || op.NumeroOpExistente)
            {
                var message = op.LineaOcupada ? LINEAOCUPADA : NUMEROOPEXISTENTE;
                await _dialogService.ShowMessageBox("Atención", message);
                _mapper.Map(datosSeleccionados, opView);
            }

            return _mapper.Map(op, opView);
        }

        public async Task ActualizarEstadoOP(int idOP, int idTipoEstado)
        {
            try
            {
                OrdenProduccion op = _unitOfWork.OrdenProduccionRepository.GetById(idOP);
                op.IdTipoEstado = idTipoEstado;
                op.FechaFin = idTipoEstado == (int)EstadosOP.Finalizada ? DateTime.Now : null;

                if(idTipoEstado == (int)EstadosOP.Finalizada)
                {
                    op.FechaFin = DateTime.Now;
                    var idJornadaLaboral = _context.JornadaLaboral
                                          .Where(x => (x.IdOrdenProduccion == op.IdOrdenProduccion) && (x.FechaFin == null))
                                          .Select(x => x.IdJornadaLaboral).OrderByDescending(x => x).FirstOrDefault();
                    await op.FinalizarJornadaLaboral(_context, idJornadaLaboral);
                }

                await _unitOfWork.OrdenProduccionRepository.UpdateAsync(op);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<OPDisponiblesViewModel>> ObtenerOPDisponiblesParaControl()
                =>  await _context.OrdenProduccion
                               .Include(m => m.IdModeloNavigation)
                               .Include(l => l.IdLineaNavigation)
                               .Include(c => c.IdColorNavigation)
                               .Include(d => d.JornadaLaboral)  
                               .Where(x => x.IdTipoEstado == (int)EstadosOP.Iniciada && x.JornadaLaboral.OrderByDescending(j => j).Select(jo => jo.FechaFin).FirstOrDefault() != null)
                               .Select(op => new OPDisponiblesViewModel
                               {
                                   IdOrdenProduccion = op.IdOrdenProduccion,
                                   NumeroOP = op.Numero,
                                   ColorCalzado = op.IdColorNavigation.Codigo,
                                   ModeloCalzado = op.IdModeloNavigation.Sku,
                                   NumeroLinea = op.IdLineaNavigation.Numero,
                                   EstadoOP = op.IdTipoEstado
                               }).ToListAsync() ?? new List<OPDisponiblesViewModel>() ;

        public async Task<(bool,int)> VincularseOP(int idOP,int idSupervisorCalidad)
        {
            bool returnCreateJLOK = false;
            int idJornadaLaboral = 0;
            try
            {
                returnCreateJLOK = await _context.OrdenProduccion
                                    .Where(x => x.IdOrdenProduccion == idOP)
                                    .Select(x => x.IdTipoEstado != (int)EstadosOP.Iniciada)
                                    .FirstAsync();

                idJornadaLaboral = await _context.JornadaLaboral
                                    .Where(x => x.IdOrdenProduccion == idOP && x.FechaFin == null)                                    
                                    .Select(x => x.IdJornadaLaboral)
                                    .OrderByDescending(x => x)
                                    .FirstOrDefaultAsync();

                if (!returnCreateJLOK && idJornadaLaboral == 0)
                {
                    var op = new OrdenProduccion();
                    idJornadaLaboral = await op.CrearJornadaLaboral(_context, idOP, idSupervisorCalidad);
                    returnCreateJLOK = true;
                }
            }catch(Exception ex)
            {
                returnCreateJLOK = false;
            }

            return (returnCreateJLOK, idJornadaLaboral);
        }

    }
}
