using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Parametriz.AutoNFP.Core.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.Configs
{
    public static class BuilderConfig
    {
        public static IHostBuilder AddAppConfiguration(this IHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, builder) =>
            {
                builder
                    .SetBasePath(context.HostingEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true)
                    .AddEnvironmentVariables();
            });

            builder.ConfigureServices((context, services) =>
            {
                services.Configure<AppConfig>(context.Configuration.GetSection("AppConfig"));
            });

            return builder;
        }
    }
}
