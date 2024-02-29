using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Windows.Navigation;
using PingApp.Models;

namespace PingApp.Db
{
    public partial class AppDbContext : DbContext
    {
        private const string _connString = "Data Source=internalDb.db";

        private readonly ILoggerFactory _loggerFactory;
        public virtual DbSet<Device> Devices { get; set; } = null!;
        public virtual DbSet<PingResult> PingResults { get; set; } = null!;

        public AppDbContext(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        //Used for scaffold purposes
        public AppDbContext() : this(new DbContextOptionsBuilder<AppDbContext>().UseSqlite("Data Source=internalDb.db").Options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. 
                //You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder
                    .LogTo(message => Debug.WriteLine(message))
                    .UseLoggerFactory(_loggerFactory)
                    .EnableSensitiveDataLogging()
                    .UseSqlite(_connString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<PingResult>(entity =>
            {
                entity.Property(e =>e.Id).ValueGeneratedOnAdd();
                entity.HasOne(x => x.Device)
                      .WithMany(x => x.PingResults)
                      .HasForeignKey(x => x.DeviceId);
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
