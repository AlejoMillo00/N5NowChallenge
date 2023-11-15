using Application.Common.Interfaces;
using Application.Common.Models;
using Nest;
using Shared.Configuration;

namespace Infrastructure.Services;

internal sealed class ElasticService : IElasticService
{
    private readonly ElasticClient _client;
    public ElasticService()
    {
        var config = new ConnectionSettings(new Uri(ElasticConfigurationOptions.Url))
            .DefaultIndex(ElasticConfigurationOptions.DefaultIndex);
        _client = new ElasticClient(config);

        var defaultIndex = _client.Indices.Exists(ElasticConfigurationOptions.DefaultIndex);
        if (!defaultIndex.Exists)
        {
            _client.Indices.Create(ElasticConfigurationOptions.DefaultIndex);
        }
    }

    public async Task<ServiceResponse> IndexAsync<T>(string index, T payload) 
        where T : class
    {
        ServiceResponse sr = new();
        try
        {
            var response = await _client.IndexAsync(new IndexRequest<T>(payload, index));

            if (response.Result == Result.Error)
                sr.AddError(response.DebugInformation);
        }
        catch (Exception ex)
        {
            sr.AddError(ex);
        }

        return sr;
    }
}
