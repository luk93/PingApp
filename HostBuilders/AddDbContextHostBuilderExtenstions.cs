using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PingApp.Db;
using Serilog;
using System.IO;


namespace PingApp.HostBuilders
{
    public static class AddDbContextHostBuilderExtensions
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

                services.AddSingleton<ILoggerFactory>(sp =>
                {
                    var loggerFactory = LoggerFactory.Create(builder =>
                    {
                        builder.AddSerilog(Log.Logger, dispose: true);
                    });
                    return loggerFactory;
                });

                services.AddDbContext<AppDbContext>((serviceProvider, options) =>
                {
                    configureDbContext(options);
                    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                    options.UseLoggerFactory(loggerFactory)
                           .EnableSensitiveDataLogging()
                           .LogTo(message => Log.Information(message), LogLevel.Information);
                });

                services.AddSingleton(sp =>
                {
                    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                    return new AppDbContextFactory(configureDbContext, loggerFactory);
                });
            });
        }
    }
}
