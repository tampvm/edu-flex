using EduFlex.Domain.Entities;
using EduFlex.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Repository.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        Task<Pagination<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null, 
            int? pageSize = null);
        Task<TEntity?> GetByIdAsync(object id);
        Task<TEntity?> GetByIdAsync(object id, Expression<Func<TEntity, bool>> filter = null, string includeProperties = "");
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(List<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(List<TEntity> entities);
        void SoftRemove(TEntity entity);
        void SoftRemoveRange(List<TEntity> entities);
        void Delete(object id);
        void Delete(TEntity entity);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null);   
    }
}
