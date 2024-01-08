using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infraestructure.Core.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _dbcontext;
        private readonly DbSet<TEntity> _entities;

        public Repository(DbContext dbcontext)
        {
            this._dbcontext = dbcontext;
            this._entities = dbcontext.Set<TEntity>();
        }
       
        private IQueryable<TEntity> PerformInclusionsAndSelect(IEnumerable<Expression<Func<TEntity, object>>> includeProperties,
                                               IQueryable<TEntity> query)
        {
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty.ToPropertyPath()));
        }

        private IQueryable<TEntity> PerformInclusions(IEnumerable<Expression<Func<TEntity, object>>> includeProperties,
                                               IQueryable<TEntity> query)
        {
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        #region IRepository<TEntity> Members

        private IQueryable<TEntity> AsQueryable()
        {
            return _entities.AsQueryable<TEntity>();
        }

        public IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = AsQueryable();
            return PerformInclusions(includeProperties, query);
        }

        public IEnumerable<TEntity> GetAllSelect(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = AsQueryable();
            return PerformInclusionsAndSelect(includeProperties, query);
        }

      

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = AsQueryable().AsNoTracking();
            query = PerformInclusions(includeProperties, query);
            return query.Where(where);
        }

        public IEnumerable<TEntity> FindAllSelect(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = AsQueryable().AsNoTracking();
            query = PerformInclusionsAndSelect(includeProperties, query);
            return query.Where(where);
        }
        
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = AsQueryable().AsNoTracking();
            query = PerformInclusions(includeProperties, query);
            return query.AsNoTracking().FirstOrDefault(where);
        }

        public TEntity FirstOrDefaultSelect(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = AsQueryable();
            query = PerformInclusionsAndSelect(includeProperties, query);
            return query.AsNoTracking().FirstOrDefault(where);
        }

      

        public void Insert(TEntity entity)
        {
            _entities.Add(entity);
        }

        public void Insert(IEnumerable<TEntity> entities)
        {
            foreach (var e in entities)
            {
                _dbcontext.Entry(e).State = EntityState.Added;
            }
        }

        public void Update(TEntity entity)
        {
            _entities.Attach(entity);
            _dbcontext.Entry(entity).State = EntityState.Modified;
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            foreach (var e in entities)
            {
                _dbcontext.Entry(e).State = EntityState.Modified;
            }
        }

        public void Delete(TEntity entity)
        {
            if (_dbcontext.Entry(entity).State == EntityState.Detached)
            {
                _entities.Attach(entity);
            }
            _entities.Remove(entity);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            foreach (var e in entities)
            {
                _dbcontext.Entry(e).State = EntityState.Deleted;
            }
        }

        public void Delete(object id)
        {
            TEntity entityToDelete = _entities.Find(id);
            _entities.Remove(entityToDelete);
        }

        #endregion IRepository<TEntity> Members


        #region SQL Queries

        // Execute query, return int
        public int ExecuteSqlCommand(string query, params object[] parameters)
        {
            return _dbcontext.Database.ExecuteSqlRaw(query, parameters);
        }

        #endregion SQL Queries
    }

}
