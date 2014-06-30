using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SearchApp.Models;
using System.Data.Entity;
using System.Linq.Expressions;
using SearchApp.DataAccess.Interfaces;

namespace SearchApp.DataAccess.Implementation
{
    /// <summary>
    /// Implementierung des Interfaces IRepository. Für Kommentare zu den Methoden,
    /// siehe Datei "IRepository.cs".
    /// </summary>
    /// <typeparam name="TEntity">Der Typ der Entitäten.</typeparam>
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private DbSet<TEntity> set { get; set; }
        private DataContext context { get; set; }

        public GenericRepository(DataContext context)
        {
            set = context.Set<TEntity>();
            this.context = context;
        }

        public void Insert(TEntity entity)
        {
            set.Add(entity);
        }

        public void Update(TEntity entity)
        {
            if (context.Entry<TEntity>(entity).State == System.Data.Entity.EntityState.Detached)
            {
                set.Attach(entity);
            }
            else
            {
                context.Entry<TEntity>(entity).CurrentValues.SetValues(entity);
            }
           

            context.Entry<TEntity>(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public TEntity GetById(params object[] ids)
        {
            return set.Find(ids);
        }

        public IQueryable<TEntity> Get()
        {
            return set;
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = set;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public void DeleteById(params object[] ids)
        {
            Delete(GetById(ids));
        }

        public void Delete(TEntity entity)
        {
            if (context.Entry<TEntity>(entity).State == System.Data.Entity.EntityState.Detached)
            {
                set.Attach(entity);
            }

            set.Remove(entity);
        }
    }
}