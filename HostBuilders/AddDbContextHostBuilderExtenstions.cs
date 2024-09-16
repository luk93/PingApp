using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PingApp.Db;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.HostBuilders
{
    public static class AddDbContextHostBuilderExtenstions
    {
        public static IHostBuilder AddDbContext(this IHostBuilder host)
        {
            return host.ConfigureServices((context, services) =>
            {
                string connectionString = context.Configuration.GetConnectionString("DefaultConnection")
                                          ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

                var basePath = AppContext.BaseDirectory;
                var dbPath = Path.Combine(basePath, connectionString);
                connectionString = $"Data Source={dbPath}";

                void configureDbContext(DbContextOptionsBuilder o) => o.UseSqlite(connectionString);

                var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

                services.AddDbContext<AppDbContext>(options =>
                {
                    configureDbContext(options);
                    options.UseLoggerFactory(loggerFactory);
                });

                services.AddSingleton(new AppDbContextFactory(configureDbContext, loggerFactory));
            });
        }
    }
}
