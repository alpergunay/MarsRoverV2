using AutoMapper;
using MarsRover.Api.Application.Modules.Infrastructure.Models.Plateau;
using MarsRover.Domain.DomainModels;

namespace MarsRover.Api.Application.Modules.Infrastructure.Mapper
{
    public class ResponseModelMapperConfiguration : Profile
    {
        public ResponseModelMapperConfiguration()
        {
            CreateMapForInitializePlateau();
        }

        private void CreateMapForInitializePlateau()
        {
            CreateMap<Plateau, InitializePlateauResponseModel>().ForMember(d => d.Id,
                    s => s.MapFrom(src => src.Id))
                .ForMember(d => d.X,
                    s => s.MapFrom(src => src.Coordinate.XCoordinate))
                .ForMember(d => d.Y,
                    s => s.MapFrom(src => src.Coordinate.YCoordinate));
        }
    }
}
