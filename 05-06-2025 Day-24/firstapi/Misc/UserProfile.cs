namespace FirstApi.Misc;

using AutoMapper;
using FirstApi.Models;
using FirstApi.Models.DTOs.DoctorSpecialities;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<DoctorAddRequestDto, User>()
        .ForMember(dest => dest.Username, act => act.MapFrom(src => src.Email))
        .ForMember(dest => dest.Password, opt=> opt.Ignore());

        CreateMap<User, DoctorAddRequestDto>()
        .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Username));

    }
}