using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface IElasticService
{
    Task<ServiceResponse> IndexAsync<T>(string index, T payload) 
        where T : class;
}
