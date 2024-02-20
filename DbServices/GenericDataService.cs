using Microsoft.EntityFrameworkCore;
using PingApp.Db;
using PingApp.DbServices.Common;
using PingApp.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.DbServices
{
    public class GenericDataService<T>(AppDbContextFactory contextFactory) : IDataService<T> where T : DbSetBaseModel
    {
        private readonly AppDbContextFactory _contextFactory = contextFactory;
        private readonly NonQueryDataService<T> _nonQueryDataService = new(contextFactory);

        public async Task<T?> Create(T entity)
        {
            return await _nonQueryDataService.Create(entity) ?? null;
        }

        public async Task<bool> Delete(int id)
        {
            return await _nonQueryDataService.Delete(id);
        }

        public async Task<bool> DeleteAll()
        {
            return await _nonQueryDataService.DeleteAll();
        }

        public async Task<T?> Get(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id) ?? null;
        }

        public async Task<IEnumerable<T>?> GetAll()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Set<T>().ToListAsync() ?? null;
        }

        public async Task<T?> Update(int id, T entity)
        {
            return await _nonQueryDataService.Update(id, entity) ?? null;
        }
    }
}
