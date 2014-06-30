using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using SearchApp.Models;

namespace SearchApp.DataAccess.Interfaces
{
    /// <summary>
    /// Interface der Klasse GenericRepository.
    /// </summary>
    /// <typeparam name="TEntity">Der Typ der verwalteten Entitäten.</typeparam>
    internal interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Fügt die angegebene Entität dem Repository hinzu.
        /// </summary>
        /// <param name="entity">Die Entität.</param>
        void Insert(TEntity entity);

        /// <summary>
        /// Aktualiert die angegebene Entität.
        /// </summary>
        /// <param name="entity">Die Entität.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Liefert die gesuchte Entität anhand ihrer Id.
        /// </summary>
        /// <param name="ids">Die Id der Entität.</param>
        /// <returns>Die gesuchte Entität.</returns>
        TEntity GetById(params object[] ids);

        /// <summary>
        /// Liefert alle Entitäten eines Repositories.
        /// </summary>
        /// <returns>Alle Entitäten.</returns>
        IQueryable<TEntity> Get();

        /// <summary>
        /// Liefert alle Entitäten eines Repositories. Ermöglicht Filterung und
        /// Sortierung der Datensätze.
        /// </summary>
        /// <param name="filter">Die Filteroption.</param>
        /// <param name="orderBy">Die Sortieroption.</param>
        /// <returns>Gefilterte und sortierte Entitäten.</returns>
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        /// <summary>
        /// Löscht eine Entität anhand ihrer Id.
        /// </summary>
        /// <param name="ids">Die Id der zu löschenden Entität.</param>
        void DeleteById(params object[] ids);

        /// <summary>
        /// Löscht eine übergebene Entität.
        /// </summary>
        /// <param name="entity">Die zu löschende Entität.</param>
        void Delete(TEntity entity);
        DataContext getContext();
    }
}
