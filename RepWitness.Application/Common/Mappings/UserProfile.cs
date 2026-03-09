using AutoMapper;
using RepWitness.Application.Features.User.Dtos;
using RepWitness.Domain.Entities;
using RepWitness.Infrastructure.Security;

namespace RepWitness.Application.Common.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserRequestDto, User>()
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
            .AfterMap((src, dest) =>
            {
                var (hash, salt) = PasswordHasher.CreatePasswordHash(src.Password);
                dest.PasswordHash = hash;
                dest.PasswordSalt = salt;
            });

        CreateMap<UpdateUserRequestDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
            .AfterMap((src, dest) =>
            {
                if (!string.IsNullOrWhiteSpace(src.Password))
                {
                    var (hash, salt) = PasswordHasher.CreatePasswordHash(src.Password);
                    dest.PasswordHash = hash;
                    dest.PasswordSalt = salt;
                }
            })
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<User, UserResponseDto>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
            .ForMember(dest => dest.RoleDescription, opt => opt.MapFrom(src => src.Role.Description));
    }
}
