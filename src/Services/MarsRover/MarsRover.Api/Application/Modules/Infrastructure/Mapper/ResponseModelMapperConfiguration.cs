using AutoMapper;
using MarsRover.Api.Application.Modules.Infrastructure.Models.Plateau;
using MarsRover.Api.Application.Modules.Infrastructure.Models.Rover;
using MarsRover.Domain.DomainModels;

namespace MarsRover.Api.Application.Modules.Infrastructure.Mapper
{
    public class ResponseModelMapperConfiguration : Profile
    {
        public ResponseModelMapperConfiguration()
        {
            CreateMapForInitializePlateau();
            CreateMapForInitializeRover();
            CreateMapForSendInstruction();
        }

        private void CreateMapForSendInstruction()
        {
            CreateMap<Rover, RoverLastPositionResponseModel>().ForMember(d => d.XCoordinate,
                    s => s.MapFrom(src => src.CurrentCoordinate.XCoordinate))
                .ForMember(d => d.YCoordinate,
                    s => s.MapFrom(src => src.CurrentCoordinate.YCoordinate))
                .ForMember(d => d.Direction,
                    s => s.MapFrom(src => src.Direction.Code));
        }

        private void CreateMapForInitializeRover()
        {
            CreateMap<Rover, InitializeRoverResponseModel>().ForMember(d => d.Id,
                    s => s.MapFrom(src => src.Id))
                .ForMember(d => d.X,
                    s => s.MapFrom(src=>src.CurrentCoordinate.XCoordinate))
                .ForMember(d => d.Y,
                    s => s.MapFrom(src => src.CurrentCoordinate.YCoordinate))
                .ForMember(d=>d.Direction,
                    s=>s.MapFrom(src=>src.Direction.Code));
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
