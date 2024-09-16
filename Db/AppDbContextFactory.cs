using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Db
{
    public class AppDbContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;
        private readonly ILoggerFactory _loggerFactory;

        public AppDbContextFactory(Action<DbContextOptionsBuilder> configureDbContext, ILoggerFactory loggerFactory)
        {
            _configureDbContext = configureDbContext;
            _loggerFactory = loggerFactory;
        }

        public AppDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            _configureDbContext(optionsBuilder);
            return new AppDbContext(optionsBuilder.Options, _loggerFactory);
        }
    }
}
