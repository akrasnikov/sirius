using AutoMapper;
using ProjectName.ServiceName.Application.Features.Employees.Queries.GetEmployees;
using ProjectName.ServiceName.Application.Features.Positions.Commands.CreatePosition;
using ProjectName.ServiceName.Application.Features.Positions.Queries.GetPositions;
using ProjectName.ServiceName.Domain.Entities;

namespace ProjectName.ServiceName.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Position, GetPositionsViewModel>().ReverseMap();
            CreateMap<Employee, GetEmployeesViewModel>().ReverseMap();
            CreateMap<CreatePositionCommand, Position>();
        }
    }
}