using Application.Common.Models;
using Domain.Entities;

namespace Application.Common.Interfaces;
public interface IPermissionTypeQueries
{
    Task<ServiceResponse<PermissionType>> GetAsync(int id);
}
