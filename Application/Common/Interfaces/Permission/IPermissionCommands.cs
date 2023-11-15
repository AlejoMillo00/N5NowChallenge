using Application.Common.Models;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IPermissionCommands
{
    Task<ServiceResponse> CreateAsync(Permission permission);
    Task<ServiceResponse> UpdateAsync(Permission permission);
}
