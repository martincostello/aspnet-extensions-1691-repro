// Copyright (c) Martin Costello, 2019. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AspNetExtensions1691Repro
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ServerOptions>(Configuration);
            services.AddScoped((p) => p.GetRequiredService<IOptionsMonitor<ServerOptions>>().CurrentValue);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting()
               .UseEndpoints(routes =>
               {
                   routes.MapGet(
                       "/",
                       async (context) =>
                       {
                           var options = context.RequestServices.GetRequiredService<ServerOptions>();
                           await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(options.Echo ?? string.Empty));
                       });
               });
        }
    }
}
