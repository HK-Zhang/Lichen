using Lichen.Trace.Abstractions;
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
            if(azureTableOptions == null) throw new ArgumentNullException(nameof(azureTableOptions));
            services.AddSingleton<AzureTableOptions>(azureTableOptions);
            return services.AddScoped<ITrace, TraceService>();
        }
    }
}
