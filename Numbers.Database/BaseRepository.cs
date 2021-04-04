using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Numbers.Models.DbModels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace Numbers.Database
{
    public abstract class BaseRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly NumbersDbContext _context;

        protected BaseRepository(NumbersDbContext context) => _context = context;

        //public async Task<IEnumerable<TEntity>> GetAll(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null) => await _context.Set<TEntity>().Include(x => include).ToListAsync();

        public async Task<IEnumerable<TEntity>> GetAll(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null) => await _context.Set<TEntity>().ToListAsync();

        public Task<TEntity> LastBatch =>  _context.Set<TEntity>().LastOrDefaultAsync();

        public async Task AddAsync(TEntity entity) => await _context.AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<TEntity> entities) => await _context.AddRangeAsync(entities);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
