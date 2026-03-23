using AutoMapper;
using RepWitness.Application.Features.Role.Dtos;
using RepWitness.Domain.Entities;

namespace RepWitness.Application.Common.Mappings;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<CreateRoleRequestDto, Role>()
           .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));

        CreateMap<UpdateRoleRequestDto, User>()
           .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
           .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Role, RoleResponseDto>();
    }
}
