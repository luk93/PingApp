using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Db
{
    public class AppDbContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext = configureDbContext;

        public AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>();
            _configureDbContext(options);
            return new AppDbContext(options.Options);
        }
    }
}
