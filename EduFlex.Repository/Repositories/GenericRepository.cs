using EduFlex.Domain.Entities;
using EduFlex.Repository.Common;
using EduFlex.Repository.Data;
using EduFlex.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Repository.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected AppDbContext _dbContext;
        protected DbSet<TEntity> _dbSet;
        private readonly ITimeService _timeService;
        private readonly IClaimsService _claimsService;

        public GenericRepository(AppDbContext dbContext, ITimeService timeService, IClaimsService claimsService)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
            _timeService = timeService;
            _claimsService = claimsService;
        }

        // Get all with filter, order by, include properties or not
        public async Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        // Get all with filter, order by, include properties or not with pagination
        public async Task<Pagination<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null, // Optional parameter for pagination (page number)
            int? pageSize = null)  // Optional parameter for pagination (number of records per page)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var totalItemsCount = await query.CountAsync();

            // Implementing pagination
            if (pageIndex.HasValue && pageIndex.Value == -1)
            {
                pageSize = totalItemsCount; // Set pageSize to total count
                pageIndex = 0; // Reset pageIndex to 0
            }
            else if (pageIndex.HasValue && pageSize.HasValue)
            {
                // Ensure the pageIndex and pageSize are valid
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value : 0;
                int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10; // Assuming a default pageSize of 10 if an invalid value is passed

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            var items = await query.ToListAsync();

            return new Pagination<TEntity>
            {
                TotalItemsCount = totalItemsCount,
                PageSize = pageSize ?? totalItemsCount,
                PageIndex = pageIndex ?? 0,
                Items = items
            };
        }

        // Get by Id
        public async Task<TEntity?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        // Get by Id with filter and include properties
        public async Task<TEntity?> GetByIdAsync(object id, Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            // Filter
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Include properties
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            var keyName = _dbContext.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties
                .Select(x => x.Name).Single();

            var parameter = Expression.Parameter(typeof(TEntity));
            var property = Expression.Property(parameter, keyName);
            var equal = Expression.Equal(property, Expression.Constant(id));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, parameter);

            //// Apply ID filter
            //query = query.Where(lambda);

            return await query.FirstOrDefaultAsync(lambda);
        }

        // Add entity
        public async Task AddAsync(TEntity entity)
        {
            entity.CreatedAt = _timeService.GetCurrentTime();
            var currentUserId = _claimsService.GetCurrentUserId();

            // Check if ClaimService is available
            if (currentUserId != Guid.Empty)
            {
                entity.CreatedBy = currentUserId.ToString();
            }

            await _dbSet.AddAsync(entity);
        }

        // Add range of entities
        public async Task AddRangeAsync(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreatedAt = _timeService.GetCurrentTime();
                entity.CreatedBy = _claimsService.GetCurrentUserId().ToString();
            }
            await _dbSet.AddRangeAsync(entities);
        }

        // Update entity
        public void Update(TEntity entity)
        {
            var currentUserId = _claimsService.GetCurrentUserId();

            // Check if ClaimService is available
            if (currentUserId != Guid.Empty)
            {
                entity.UpdatedAt = _timeService.GetCurrentTime();
                entity.UpdatedBy = _claimsService.GetCurrentUserId().ToString();
            }

            _dbSet.Update(entity);
        }

        // Update range of entities
        public void UpdateRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.UpdatedAt = _timeService.GetCurrentTime();
                entity.UpdatedBy = _claimsService.GetCurrentUserId().ToString();
            }
            _dbSet.UpdateRange(entities);
        }

        // Soft remove entity
        public void SoftRemove(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = _timeService.GetCurrentTime();
            entity.DeletedBy = _claimsService.GetCurrentUserId().ToString();
            _dbSet.Update(entity);
        }

        // Soft remove range of entities
        public void SoftRemoveRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                entity.DeletedAt = _timeService.GetCurrentTime();
                entity.DeletedBy = _claimsService.GetCurrentUserId().ToString();
            }
            _dbSet.UpdateRange(entities);
        }

        // Delete entity by Id
        public void Delete(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        // Delete entity
        public void Delete(TEntity entityToDelete)
        {
            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        // Count entities with filter
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }
    }
}
