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

namespace PingApp.HostBuilders
{
    public static class AddDbContextHostBuilderExtenstions
    {
        public static IHostBuilder AddDbContext(this IHostBuilder host)
        {
            return host.ConfigureServices((context, services) =>
            {
                string connectionString = "Data Source=internalDb.db";
                void configureDbContext(DbContextOptionsBuilder o) => o.UseSqlite(connectionString);

                services.AddDbContext<AppDbContext>(configureDbContext);
                services.AddSingleton(new AppDbContextFactory(configureDbContext));
            });
        }
    }
}
