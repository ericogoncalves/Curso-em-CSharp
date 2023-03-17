using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Curso.Core.Data
{
    public abstract class ARepository<TEntity> : Interfaces.IARepository<TEntity>
        where TEntity : class
    {
        #region Private Fields
        private readonly DbContext _context;
        private DbSet<TEntity> _dbSet;
        #endregion Private Fields

        public ARepository(CoreDbContext context)
        {
            _context = context;
            if (_context != null)
                _dbSet = _context.Set<TEntity>();
        }

        public DbContext DbContext => _context;

        public void Commit() => _context.SaveChanges();
        public void Detach(TEntity entity) => _context.Entry(entity).State = EntityState.Detached;

        public void Delete(params TEntity[] pObjects)
        {
            //pObjects?.ForEach(Delete);
        }

        public TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<TEntity> GetAllByFilter(Expression<Func<TEntity, bool>> pFilter = null)
        {
            return _dbSet.Where(pFilter).ToList();
        }

        public TEntity Find(Expression<Func<TEntity, bool>> keys = null)
        {
            return _dbSet.FirstOrDefault(keys);
        }
        public virtual TEntity Find(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public virtual IQueryable<TEntity> SelectQuery(string query, params object[] parameters)
        {
            return _context.Set<TEntity>().FromSqlRaw(query, parameters).AsQueryable();
        }

        public virtual void Insert(TEntity entity)
        {
            if (HasProperty(entity, "UpdatedAt"))
                _context.Entry(entity).Property("UpdatedAt").CurrentValue = DateTime.Now;

            if (HasProperty(entity, "CreatedAt"))
                _context.Entry(entity).Property("CreatedAt").CurrentValue = DateTime.Now;

            _context.Add(entity);
            _context.Entry(entity).State = EntityState.Added;
            _context.SaveChanges();
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            InsertGraphRange(entities);
        }

        public virtual void InsertGraphRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            if (HasProperty(entity, "UpdatedAt"))
                _context.Entry(entity).Property("UpdatedAt").CurrentValue = DateTime.Now;

            if (HasProperty(entity, "CreatedAt"))
                _context.Entry(entity).Property("CreatedAt").IsModified = false;

            _context.Update(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(object id)
        {
            var entity = _dbSet.Find(id);
            Delete(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            //_context.SaveChanges();
        }

        public IQueryable<TEntity> Queryable()
        {
            return _dbSet;
        }

        public async Task<List<TEntity>> ToListAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public IQueryable<TEntity> QueryableDetach()
        {
            return _dbSet.AsNoTracking().AsQueryable();
        }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues);
        }

        public async Task<List<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> pFilter = null)
        {
            return await _dbSet.Where(pFilter).ToListAsync();
        }

        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await _dbSet.FindAsync(cancellationToken, keyValues);
        }

        public IIncludableQueryable<TEntity, object> Include(Expression<Func<TEntity, object>> entity)
        {
            return _dbSet.Include(entity);
        }

        private static bool HasProperty(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }
    }
}