using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SearchApp.Models;
using SearchApp.DataAccess.Interfaces;
using System.Data.Entity;

namespace SearchApp.DataAccess.Implementation
{
    /// <summary>
    /// Implementierung des Interfaces IUnitOfWork. Für Kommentare zu den Methoden,
    /// siehe Datei "IUnitOfWort.cs".
    /// </summary>
    /// <typeparam name="TDbContext">Der Typ des Context-Objektes.</typeparam>
    public class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DataContext
    {
        private IDictionary<Type, object> repositories = new Dictionary<Type, object>();
        private DataContext context;
        private bool disposed = false;

        public bool IsConnectionOpen { get { return context != null; } }

        public void Insert<TEntity>(TEntity entity) where TEntity : class
        {
            InitContext();

            GetRepo<TEntity>().Insert(entity);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            InitContext();

            GetRepo<TEntity>().Update(entity);
        }

        public TEntity GetById<TEntity>(params object[] ids) where TEntity : class
        {
            InitContext();

            return GetRepo<TEntity>().GetById(ids);
        }

        public IQueryable<TEntity> Get<TEntity>() where TEntity : class
        {
            InitContext();

            return GetRepo<TEntity>().Get();
        }

        public IQueryable<TEntity> Get<TEntity>(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null) where TEntity : class
        {
            InitContext();

            return GetRepo<TEntity>().Get(filter, orderBy);
        }

        public void DeleteById<TEntity>(params object[] ids) where TEntity : class
        {
            InitContext();

            GetRepo<TEntity>().DeleteById(ids);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            InitContext();

            GetRepo<TEntity>().Delete(entity);
        }

        public void Query(Action query)
        {
            InitContext();

            query.Invoke();

            SaveChanges(true);
        }

        public void SaveChanges(bool withDisposing = false)
        {
            context.SaveChanges();

            if (withDisposing)
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (context != null)
                    {
                        context.Dispose();
                        context = null;
                    }

                    repositories.Clear();
                }
            }
            this.disposed = true;
        }

        private void InitContext()
        {
            if (context == null)
            {
                context = typeof(TDbContext).GetConstructor(new Type[] { }).Invoke(new object[] { }) as DataContext;
            }
        }

        private IRepository<TEntity> GetRepo<TEntity>() where TEntity : class
        {
            if (!repositories.ContainsKey(typeof(TEntity)))
            {
                repositories.Add(new KeyValuePair<Type, object>(typeof(TEntity), new GenericRepository<TEntity>(context)));
            }

            return (IRepository<TEntity>)repositories[typeof(TEntity)];
        }

        public DataContext getContext()
        {
            InitContext();
            return context;
        }
    }
}