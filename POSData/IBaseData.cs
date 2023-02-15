using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace POSData
{
    public interface IBaseData<T>
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether database write operations
        ///     such as insert, delete or update should be committed immediately.
        /// </summary>
        //bool AutoCommitEnabled { get; set; }

        /// <summary>
        ///     Returns the data context associated with the repository.
        /// </summary>
        /// <remarks>
        ///     The context is likely shared among multiple repository types.
        ///     So committing data or changing configuration also affects other repositories.
        /// </remarks>
        DbContext Context { get; }

        /// <summary>
        ///     Returns the queryable entity set for the given type {T}.
        /// </summary>
        IQueryable<T> Table { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Creates a new instance of an entity of type {T}
        /// </summary>
        /// <returns>The new entity instance.</returns>
        T Create();

        /// <summary>
        ///     Marks an existing entity to be deleted from the store.
        /// </summary>
        /// <param name="entity">An entity instance that should be deleted from the database.</param>
        /// <remarks>Implementors should delegate this to the current <see cref="IDbContext" /></remarks>
        void Delete(T entity);

        /// <summary>
        ///     Gets an entity by id from the database or the local change tracker.
        /// </summary>
        /// <param name="id">The id of the entity. This can also be a composite key.</param>
        /// <returns>The resolved entity</returns>
        T GetById(object id);

        /// <summary>
        ///     Marks the entity instance to be saved to the store.
        /// </summary>
        /// <param name="entity">An entity instance that should be saved to the database.</param>
        /// <remarks>Implementors should delegate this to the current <see cref="IDbContext" /></remarks>
        void Insert(T entity);

        /// <summary>
        ///     Marks multiple entities to be saved to the store.
        /// </summary>
        /// <param name="list">The list of entity instances to be saved to the database</param>
        /// <param name="batchSize">
        ///     The number of entities to insert before saving to the database (if
        ///     <see cref="AutoCommitEnabled" /> is true)
        /// </param>
        void InsertRange(List<T> list, int batchSize = 100);

        /// <summary>
        ///     Marks the changes of an existing entity to be saved to the store.
        /// </summary>
        /// <param name="entity">An instance that should be updated in the database.</param>
        /// <remarks>Implementors should delegate this to the current <see cref="IDbContext" /></remarks>
        void Update(T entity);

        void UpdateRange(List<T> list);

        #endregion

    }
}
