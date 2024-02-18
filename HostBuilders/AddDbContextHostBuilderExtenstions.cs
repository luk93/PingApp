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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextFileExport.Db;

namespace PingApp.HostBuilders
{
    public static class AddDbContextHostBuilderExtenstions
    {
        public static IHostBuilder AddDbContext(this IHostBuilder host)
        {
            return host.ConfigureServices((context, services) =>
            {
                string connectionString = "Data Source=internalDb.db";
                Action<DbContextOptionsBuilder> configureDbContext = o => o.UseSqlite(connectionString);
                    //.LogTo(message => Debug.WriteLine(message))  // Optional: You can remove this line if using Serilog exclusively
                    //.UseLoggerFactory(loggerFactory)
                    //.EnableSensitiveDataLogging();

                services.AddDbContext<AppDbContext>(configureDbContext);
                services.AddSingleton(new AppDbContextFactory(configureDbContext));
            });
        }
    }
}
