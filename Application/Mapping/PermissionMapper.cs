using Application.Models;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping;

internal sealed class PermissionMapper : Profile
{
    public PermissionMapper()
    {
        CreateMap<Permission, PermissionDto>();
    }
}
