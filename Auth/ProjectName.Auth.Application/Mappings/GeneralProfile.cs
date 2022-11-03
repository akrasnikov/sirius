using AutoMapper;
using ProjectName.Auth.Application.Features.Registration.Commands;
using ProjectName.Auth.Domain.Entities.Identity;

namespace ProjectName.Auth.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {

            CreateMap<RegistrationCommand, User>();
        }
    }
}