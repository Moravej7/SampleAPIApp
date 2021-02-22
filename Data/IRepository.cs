using Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task AddAsync(TEntity entity, bool saveNow = true);
        Task AddRangeAsync(IEnumerable<TEntity> entities, bool saveNow = true);
        void Attach(TEntity entity);
        Task DeleteAsync(TEntity entity, bool saveNow = true);
        Task DeleteRangeAsync(IEnumerable<TEntity> entities, bool saveNow = true);
        void Detach(TEntity entity);
        Task<TEntity> GetByIdAsync(params object[] ids);
        Task LoadCollectionAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty) where TProperty : class;
        Task LoadReferenceAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty) where TProperty : class;
        Task UpdateAsync(TEntity entity, bool saveNow = true);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities, bool saveNow = true);
        Task<List<TEntity>> GetAsync();
        Task<List<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> func);
    }
}