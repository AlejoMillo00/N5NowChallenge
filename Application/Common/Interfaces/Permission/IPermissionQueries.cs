using Application.Common.Models;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IPermissionQueries
{
    Task<ServiceResponse<Permission>> GetAsync(int id);
    Task<ServiceResponse<List<Permission>>> ListAsync();
}
