using Application.Common.Interfaces;
using Application.Common.Models;
using Confluent.Kafka;
using Shared.Configuration;
using System.Text.Json;

namespace Infrastructure.Services;

internal sealed class TopicService : ITopicService
{
    public async Task<ServiceResponse> ProduceMessageAsync<T>(T message)
    {
        ServiceResponse sr = new();

        try
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = KafkaConfigurationOptions.Endpoint,
            };
            using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
            var serializedMessage = JsonSerializer.Serialize(message);
            var producerSr = await producer.ProduceAsync(KafkaConfigurationOptions.Topic, 
                new Message<Null, string> { Value = serializedMessage });

            if (producerSr.Status == PersistenceStatus.NotPersisted)
                sr.AddError("Error persisting the message");
        }
        catch (Exception ex)
        {
            sr.AddError(ex);
        }

        return sr;
    }
}
