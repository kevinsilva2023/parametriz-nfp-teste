using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Parametriz.AutoNFP.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.Configs
{
    public static class DatabaseConfig
    {
        public static IHostBuilder AddDatabaseConfiguration(this IHostBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<AutoNfpDbContext>(options =>
                    options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnection")));
            });

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            return builder;
        }
    }
}
