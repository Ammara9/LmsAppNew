using AutoMapper;
using Domain.Models.Entities;
using LMS.Shared.DTOs;
namespace LMS.Infrastructure.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserForRegistrationDto, ApplicationUser>();
    }
}
