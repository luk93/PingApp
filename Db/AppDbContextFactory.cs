using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextFileExport.Db;

namespace PingApp.Db
{
    public class AppDbContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;
        public AppDbContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
        {
            _configureDbContext = configureDbContext;
        }

        public AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>();
            _configureDbContext(options);
            return new AppDbContext(options.Options);
        }
    }
}
