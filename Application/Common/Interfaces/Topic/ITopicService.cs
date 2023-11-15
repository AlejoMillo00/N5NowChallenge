using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface ITopicService
{
    Task<ServiceResponse> ProduceMessageAsync<T>(T message);
}
