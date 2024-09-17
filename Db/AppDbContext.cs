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
using Serilog;

namespace PingApp.Db
{
    public partial class AppDbContext : DbContext
    {
        public virtual DbSet<Device> Devices { get; set; } = null!;
        public virtual DbSet<PingResult> PingResults { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
           
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .EnableSensitiveDataLogging()
                    .LogTo(Log.Information, LogLevel.Information);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasMany(d => d.PingResults)
                    .WithOne(p => p.Device)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<PingResult>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasOne(x => x.Device)
                      .WithMany(x => x.PingResults)
                      .HasForeignKey(x => x.DeviceId);
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
