using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using SearchApp.Models;

namespace SearchApp.DataAccess.Interfaces
{
    /// <summary>
    /// Interface der Klasse UnitOfWork, erbt von IDisposable.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        bool IsConnectionOpen { get; }

        /// <summary>
        /// Fügt eine Entität zur Datenbank hinzu.
        /// </summary>
        /// <typeparam name="TEntity">Typ des Repository, dessen insert-Methode aufgerufen werden soll.</typeparam>
        /// <param name="entity">Hinzuzufügende Entität.</param>
        /// <example>
        /// IUnitOfWork unitOfWork = UnitOfWork<SATContext>();
        /// Kategorie kategorie = new Kategorie()...
        /// unitOfWork.insert<Kategorie>(kategorie);
        /// Dies würde eine neue Kategorie in die Datenbank speichern.
        /// </example>
        void Insert<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Aktualisiert eine existierende Entität in der Datenbank.
        /// </summary>
        /// <typeparam name="TEntity">Typ des Repository, dessen update-Methode aufgerufen werden soll.</typeparam>
        /// <param name="entity">Zu aktualisierende Entität.</param>
        /// <example>
        /// IUnitOfWork unitOfWork = UnitOfWork<SATContext>();
        /// Kategorie kategorie = new Kategorie()...
        /// unitOfWork.Update<Kategorie>(kategorie);
        /// Dies würde eine Kategorie in der Datenbank aktualisieren. 
        /// </example>
        void Update<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Liefert eine Entität anhand ihrer id oder ids.
        /// </summary>
        /// <typeparam name="TEntity">Typ des Repository, aus dem die Entität ausgelesen wird.</typeparam>
        /// <param name="ids">ID bzw IDs.</param>
        /// <returns>Die Entität.</returns>
        /// <example>
        /// IUnitOfWork unitOfWork = UnitOfWork<SATContext>();
        /// Kategorie kategorie = unitOfWork.GetById(1);
        /// Dies würde eine Kategorie mit der KategorieId 1 aus der Datenbank lesen.
        /// </example>
        TEntity GetById<TEntity>(params object[] ids) where TEntity : class;

        /// <summary>
        /// Liefert alle Entitäten des gegebenen Typs.
        /// </summary>
        /// <typeparam name="TEntity">Typ des Repository, aus dem die Entitäten ausgelesen werden.</typeparam>
        /// <returns>Queryable der Entitäten des Repository-Typs.</returns>
        /// <example>
        /// IUnitOfWork unitOfWork = UnitOfWork<SATContext>();
        /// List<Kategorie> kategorien = unitOfWork.Get<Kategorie>().ToList();
        /// Dies würde alle Kategorien aus der Datenbank lesen.
        /// </example>
        IQueryable<TEntity> Get<TEntity>() where TEntity : class;

        /// <summary>
        /// Liefert alle Entitäten des gegebenen Typs.
        /// </summary>
        /// <typeparam name="TEntity">Typ des Repository, aus dem die Entitäten ausgelesen werden.</typeparam>
        /// <param name="filter">Die Filteroption.</param>
        /// <param name="orderBy">Die Sortieroption.</param>
        /// <returns>Queryable der Entitäten des Repository-Typs.</returns>
        /// <example>
        /// IUnitOfWork unitOfWork = UnitOfWork<SATContext>();
        /// var sampleEntities = UnitOfWork.Get<Benutzer>(benutzer => benutzer.BenutzerId > 1, benutzer => benutzer.OrderBy(benutzer.BenutzerName)).ToList();
        /// UoW.Dispose();
        /// Dies würde eine Liste aller Benutzer mit der BenutzerId echt größer 1 liefern (Filteroption), welche sortiert wäre nach dem Benutzernamen (Sortieroption).
        /// </example>
        IQueryable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null) where TEntity : class;

        /// <summary>
        /// Löscht eine Entität anhand ihrer id oder ids.
        /// </summary>
        /// <typeparam name="TEntity">Typ des Repository, dessen Entität gelöscht werden soll</typeparam>
        /// <param name="ids">Id oder Ids der zu löschenden Entität.</param>
        /// <example>
        /// IUnitOfWork unitOfWork = UnitOfWork<SATContext>();
        /// unitOfWork.DeleteById<Kategorie>(1);
        /// Dies würde die Kategorie mit der KategorieId 1 aus der Datenbank entfernen.
        /// </example>
        void DeleteById<TEntity>(params object[] ids) where TEntity : class;

        /// <summary>
        /// Löscht die übergebene Entität.
        /// </summary>
        /// <typeparam name="TEntity">Typ des Repository, dessen Entität gelöscht werden soll.</typeparam>
        /// <param name="entity">Die zu löschendee Entität.</param>
        /// <example>
        /// IUnitOfWork unitOfWork = UnitOfWork<SATContext>();
        /// Kategorie kategorie = unitOfWork.GetById<Kategorie>(1);
        /// unitOfWork.Delete<Kategorie>(kategorie);
        /// Dies würde die Kategorie mit der KategorieId 1 aus der Datenbank entfernen.
        /// </example>
        void Delete<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Speichert alle Änderungen des Context-Objektes in die Datenbank.
        /// </summary>
        /// <param name="withDisposing">Falls true, so wird die Verbindung nach
        /// Speicherung geschlossen. Default - false
        /// </param>
        /// <example>
        /// IUnitOfWork unitOfWork = UnitOfWork<SATContext>();
        /// ...
        /// unitOfWork.SaveChanges();
        /// Dies würde alle Änderungen an der Datenbank, die unter "..." getätigt wurden,
        /// persistent in die Datenbank schreiben.
        /// </example>
        void SaveChanges(bool withDisposing = false);

        DataContext getContext();
    }
}
