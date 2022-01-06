using Lichen.Trace.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lichen.Trace.Azure.Table
{
    public static class TraceServiceSetup
    {
        public static IServiceCollection AddTraceServiceAzureTableProvider(this IServiceCollection services, AzureTableOptions azureTableOptions)
        {
            if(azureTableOptions != null)
            {
                services.AddSingleton(azureTableOptions);
            }
            else
            {
                throw new ArgumentNullException(nameof(azureTableOptions));
            }
   
            return services.AddScoped<ITrace, TraceService>();
        }

        public static IServiceCollection AddTraceServiceAzureTableProvider(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            if (configurationSection != null)
            {
                services.AddSingleton(configurationSection.Get<AzureTableOptions>());
            }
            else
            {
                throw new ArgumentNullException(nameof(configurationSection));
            }

            return services.AddScoped<ITrace, TraceService>();
        }

        public static IServiceCollection AddTraceServiceAzureTableProvider(this IServiceCollection services, IConfiguration configuration = null)
        {
            if (configuration != null)
            {
                services.AddSingleton(configuration.GetSection(AzureTableOptions.AzureTableTraceOptions).Get<AzureTableOptions>());
            }
            else
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return services.AddScoped<ITrace, TraceService>();
        }

        public static IServiceCollection AddTraceServiceAzureTableProvider(this IServiceCollection services, Func<AzureTableOptions> setupOption)
        {
            services.AddSingleton(setupOption());
            return services.AddScoped<ITrace, TraceService>();
        }
    }
}
