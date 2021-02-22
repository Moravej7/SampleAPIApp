using Common.Utilities;
using Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        protected readonly ApplicationDbContext AppDbContext;
        private DbSet<TEntity> Entities { get; }
        private IQueryable<TEntity> Table => Entities;
        private IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

        public Repository(ApplicationDbContext dbContext)
        {
            AppDbContext = dbContext;
            Entities = AppDbContext.Set<TEntity>();
        }

        #region Async Methods
        public virtual Task<TEntity> GetByIdAsync(params object[] ids)
        {
            return Entities.FindAsync(ids).AsTask();
        }

        public virtual async Task AddAsync(TEntity entity, bool saveNow = true)
        {
            Assert.NotNull(entity, nameof(entity));
            await Entities.AddAsync(entity).ConfigureAwait(false);
            if (saveNow)
                await AppDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            Assert.NotNull(entities, nameof(entities));
            await Entities.AddRangeAsync(entities).ConfigureAwait(false);
            if (saveNow)
                await AppDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(TEntity entity, bool saveNow = true)
        {
            Assert.NotNull(entity, nameof(entity));
            Entities.Update(entity);
            if (saveNow)
            {
                await AppDbContext.SaveChangesAsync();
            }
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            Assert.NotNull(entities, nameof(entities));
            Entities.UpdateRange(entities);
            if (saveNow)
                await AppDbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity, bool saveNow = true)
        {
            Assert.NotNull(entity, nameof(entity));
            Entities.Remove(entity);
            if (saveNow)
                await AppDbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            Assert.NotNull(entities, nameof(entities));
            Entities.RemoveRange(entities);
            if (saveNow)
                await AppDbContext.SaveChangesAsync();
        }

        public virtual Task<List<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> func)
        {
            return Entities.Where(func).ToListAsync();
        }

        public virtual Task<List<TEntity>> GetAsync()
        {
            return TableNoTracking.ToListAsync();
        }
        #endregion

        #region Attach & Detach
        public virtual void Detach(TEntity entity)
        {
            Assert.NotNull(entity, nameof(entity));
            var entry = AppDbContext.Entry(entity);
            if (entry != null)
                entry.State = EntityState.Detached;
        }

        public virtual void Attach(TEntity entity)
        {
            Assert.NotNull(entity, nameof(entity));
            if (AppDbContext.Entry(entity).State == EntityState.Detached)
                Entities.Attach(entity);
        }
        #endregion

        #region Explicit Loading
        public virtual async Task LoadCollectionAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty)
            where TProperty : class
        {
            Attach(entity);

            var collection = AppDbContext.Entry(entity).Collection(collectionProperty);
            if (!collection.IsLoaded)
                await collection.LoadAsync().ConfigureAwait(false);
        }

        public virtual async Task LoadReferenceAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty)
            where TProperty : class
        {
            Attach(entity);
            var reference = AppDbContext.Entry(entity).Reference(referenceProperty);
            if (!reference.IsLoaded)
                await reference.LoadAsync().ConfigureAwait(false);
        }
        #endregion
    }
}
