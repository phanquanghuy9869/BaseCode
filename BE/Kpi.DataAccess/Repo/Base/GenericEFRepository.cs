using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Kpi.DataAccess.DataContext;

namespace Kpi.DataAccess.Repo.Base
{
    public interface IGenericRepository<TEntity>
    {
        IQueryable<TEntity> GetTable();
        DbContext GetContext();
        IQueryable<TEntity> SelectAll();
        IQueryable<TEntity> SelectWhere(Expression<Func<TEntity, bool>> where);
        TEntity Get(Expression<Func<TEntity, bool>> where);
        TEntity Add(TEntity entity);
        bool Any(Expression<Func<TEntity, bool>> where);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity t);
        void UpdateByProperties(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> properties);
        TEntity Delete(TEntity t);
        IEnumerable<TEntity> Delete(Expression<Func<TEntity, bool>> where);

    }

    public class GenericEFRepository<TEntity> : IGenericRepository<TEntity>, IDisposable where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericEFRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetTable()
        {
            return _dbSet;
        }

        public DbContext GetContext()
        {
            return _context;
        }

        public IQueryable<TEntity> SelectAll()
        {
            return _dbSet;
        }

        public IQueryable<TEntity> SelectWhere(Expression<Func<TEntity, bool>> where)
        {
            return _dbSet.Where(where);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> where)
        {
            return _dbSet.FirstOrDefault(where);
        }

        public TEntity Add(TEntity entity)
        {
            return _dbSet.Add(entity);
        }

        public bool Any(Expression<Func<TEntity, bool>> where)
        {
            return _dbSet.Any(where);
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            return _dbSet.AddRange(entities);
        }

        public void Update(TEntity t)
        {
            _dbSet.Attach(t);
            _context.Entry(t).State = EntityState.Modified;
        }

        public TEntity Delete(TEntity t)
        {
            _dbSet.Attach(t);
            return _dbSet.Remove(t);
        }

        public IEnumerable<TEntity> Delete(Expression<Func<TEntity, bool>> where)
        {
            return _dbSet.RemoveRange(_dbSet.Where(where));
        }

        public IEnumerable<T> ExcuteCommand<T>(string StoreProcedureName, params object[] Params)
        {
            IEnumerable<T> ret;
            StringBuilder sParam = new StringBuilder();
            for (int i = 0; i <= Params.Count() - 1; i++)
            {
                if (i == 0)
                {
                    sParam.Append("@p" + i.ToString() + " ");
                }
                else
                {
                    sParam.Append(",@p" + i.ToString() + " ");
                }
            }
            ret = _context.Database.SqlQuery<T>("exec " + StoreProcedureName + " " + sParam.ToString(), Params);
            return ret;
        }

        /// <summary>
        /// Update property thay vì update cả object
        /// </summary>
        public void UpdateByProperties(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> properties)
        {
            this._dbSet.Attach(entity);
            var entry = this._context.Entry(entity);
            foreach (var prop in properties)
            {
                entry.Property(prop).IsModified = true;
            }
            this._context.Configuration.ValidateOnSaveEnabled = false;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

    public static class LinQHelper
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
