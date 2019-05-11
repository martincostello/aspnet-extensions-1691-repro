// Copyright (c) Martin Costello, 2019. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AspNetExtensions1691Repro
{
    public class HttpServerFixture : IAsyncLifetime, IConfigurationSource
    {
        private readonly Dictionary<string, string> _config = new Dictionary<string, string>();
        private readonly IHost _host;

        public HttpServerFixture()
        {
            _host = Host.CreateDefaultBuilder()
                        .ConfigureLogging(builder => builder.SetMinimumLevel(LogLevel.None)

#if false // Set to true to "fix" the issue by removing the console logger
                                                            .ClearProviders()
#endif

                        )
                        .ConfigureWebHostDefaults(builder =>
                        {
                            builder.ConfigureAppConfiguration(services => services.Add(this))
                                   .UseStartup<Startup>();
                        })
                        .Build();
        }

        public void ClearConfigurationOverrides()
        {
            _config.Clear();
            ReloadConfiguration();
        }

        public void OverrideConfiguration(string key, string value)
        {
            _config[key] = value;
            ReloadConfiguration();
        }

        public async Task DisposeAsync() => await _host.StopAsync();

        public async Task InitializeAsync() => await _host.StartAsync();

        private void ReloadConfiguration()
        {
            var config = _host.Services.GetRequiredService<IConfiguration>() as IConfigurationRoot;
            config.Reload();
        }

        IConfigurationProvider IConfigurationSource.Build(IConfigurationBuilder builder)
            => new FixtureConfigurationProvider(this);

        private sealed class FixtureConfigurationProvider : ConfigurationProvider
        {
            public FixtureConfigurationProvider(HttpServerFixture repro)
            {
                Data = repro._config;
            }
        }
    }
}
