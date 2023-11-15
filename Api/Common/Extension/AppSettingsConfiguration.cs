using Shared.Configuration;
using Shared.Constants;

namespace Api.Common.Extensions;

internal static class AppSettingsConfiguration
{
    public static void AddAppSettingsConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        configuration.GetSection(AppSettings.DbConfiguration)
            .Get<DbConfigurationOptions>();
    }
}
