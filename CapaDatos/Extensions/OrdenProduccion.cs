using CapaDatos.Context;
using CapaDatos.Enums;
using CapaDatos.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Models
{
    public partial class OrdenProduccion
    {
        [NotMapped]
        public bool LineaOcupada { get; set; } = false;
        [NotMapped]
        public bool NumeroOpExistente { get; set; } = false;

        [NotMapped]
        public bool UsuarioConOpNoFinalizada { get; set; } = false;

        public async Task<int> CrearJornadaLaboral(CONTROLDECALIDADContext context, int idOrdenProduccion, int idSupervisorCalidad)
        {
            var horaActual = DateTime.Now.TimeOfDay;
            var idTurno = await context.Turno
                                .Where(x =>
                                    (x.HoraInicio < x.HoraFin && horaActual >= x.HoraInicio && horaActual < x.HoraFin) ||
                                    (x.HoraInicio > x.HoraFin && (horaActual >= x.HoraInicio || horaActual < x.HoraFin)))
                                .Select(x => x.IdTurno)
                                .FirstOrDefaultAsync();

            var jornadaLaboral = new JornadaLaboral
            {
                IdTurno = idTurno,
                FechaInicio = DateTime.Now,
                IdOrdenProduccion = idOrdenProduccion,
                IdSupervisorCalidad = idSupervisorCalidad
            };

            await context.AddAsync(jornadaLaboral);
            await context.SaveChangesAsync();
            return jornadaLaboral.IdJornadaLaboral;
        }

        public async Task<bool> FinalizarJornadaLaboral(CONTROLDECALIDADContext context, int idJornadaLaboral)
        {
            bool returnOK = false;
            var jornadaLaboral = await context.JornadaLaboral.FindAsync(idJornadaLaboral);
            var totalPrimera = context.Primera.Where(x => x.IdJornadaLaboral == idJornadaLaboral).Select(x => x.Cantidad).Sum();
            var idTipoEstadoOP = context.OrdenProduccion.Where(x => x.IdOrdenProduccion == jornadaLaboral.IdOrdenProduccion).Select(x => x.IdTipoEstado);

            if(jornadaLaboral != null && idJornadaLaboral != (int)EstadosOP.Finalizada)
            {
                jornadaLaboral.FechaFin = DateTime.Now;
                jornadaLaboral.TotalPrimera = totalPrimera;
                context.Update(jornadaLaboral);
                await context.SaveChangesAsync();
                returnOK = true;
            }

            return returnOK;
        }

        public async Task<OrdenProduccion> CrearOrdenProduccion(CONTROLDECALIDADContext context, OrdenProduccion op)
        {
            var existUsuarioConOrdenNofinalizada = await context.OrdenProduccion.AnyAsync(x => x.IdSupervisorLinea == op.IdSupervisorLinea && x.IdTipoEstado != (int)EstadosOP.Finalizada);
            if(existUsuarioConOrdenNofinalizada)
                op.UsuarioConOpNoFinalizada = true;

            var existOrden = await context.OrdenProduccion.AnyAsync(x => x.IdTipoEstado != (int)EstadosOP.Finalizada && x.IdLinea == op.IdLinea);
            if (existOrden)
                op.LineaOcupada = true;

            var existNumero = await context.OrdenProduccion.AnyAsync(x => x.Numero.ToLower() == op.Numero.ToLower());
            if (existNumero)
                op.NumeroOpExistente = true;

            if(!existUsuarioConOrdenNofinalizada && !existOrden && !existNumero)
            {
                op.FechaInicio = DateTime.Now;
                op.IdTipoEstado = (int)EstadosOP.Iniciada;

                await context.AddAsync(op);
                await context.SaveChangesAsync();
            }

            return op;
        }
    }
}
