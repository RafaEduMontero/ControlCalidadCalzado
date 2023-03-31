using AutoMapper;
using CapaDatos.Models;
using CapaDatos.ViewModel.OP;
using static CapaDatos.ViewModel.RegistroDefectos.DefectosTurnoActualViewModel;

namespace ControlCalidad.Data.Mapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<OrdenProduccion, OrdenProduccionViewModel>().ReverseMap();
            CreateMap<OrdenProduccion, CreateOrdenProduccionViewModel>().ReverseMap();
            CreateMap<OrdenProduccionViewModel, CreateOrdenProduccionViewModel>().ReverseMap();
            CreateMap<DefectosViewModel, Defecto>().ReverseMap();
            CreateMap<TurnoViewModel, Turno>().ReverseMap();
            CreateMap<PrimeraViewModel, Primera>().ReverseMap();
            CreateMap<RegistroDefecto, RegistroDefectoViewModel>().ReverseMap();
        }
    }
}
