using System;
using Lichen.AspNetCore.SpaProxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lichen.AspNetCore.SpaProxy
{
    internal class SpaProxyStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                builder.UseMiddleware<SpaProxyMiddleware>();
                next(builder);
            };
        }
    }
}