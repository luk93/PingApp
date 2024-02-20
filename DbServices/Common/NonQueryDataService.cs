using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PingApp.Db;
using PingApp.Models.Base;
using PingApp.Models;

namespace PingApp.DbServices.Common
{
    public class NonQueryDataService<T>(AppDbContextFactory contextFactory) where T : DbSetBaseModel
    {
        private readonly AppDbContextFactory _contextFactory = contextFactory;

        public async Task<T> Create(T entity)
        {
            using var context = _contextFactory.CreateDbContext();
            EntityEntry<T> newResult = await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
            return newResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            T? entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity != null)
            {
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteAll()
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.Set<T>().ToListAsync();
            if (entities.Count != 0)
            {
                context.Set<T>().RemoveRange(entities);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<T?> Update(int id, T entity)
        {
            using var context = _contextFactory.CreateDbContext();
            if (entity != null)
            {
                entity.Id = id;
                context.Set<T>().Update(entity);
                await context.SaveChangesAsync();
                return entity;
            }
            return null;
        }
    }
}
