using Application.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

internal sealed class PermissionMapper : Profile
{
    public PermissionMapper()
    {
        CreateMap<Permission, PermissionDto>()
            .ForMember(dest => dest.PermissionDate, 
                opt => opt.MapFrom(src => src.PermissionDate.ToShortDateString()));
    }
}
