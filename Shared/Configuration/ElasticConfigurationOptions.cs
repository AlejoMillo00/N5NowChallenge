using Elasticsearch.Net;

namespace Shared.Configuration;

public class ElasticConfigurationOptions
{
    public static string Url { get; set; }
    public static string DefaultIndex { get; set; }
}
