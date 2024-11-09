using EduFlex.Domain.Entities;
using EduFlex.Repository.Data;
using EduFlex.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Repository.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly AppDbContext _dbContext;

        public RoleRepository(AppDbContext dbContext, ITimeService timeService, IClaimsService claimsService) : base(dbContext, timeService, claimsService)
        {
            _dbContext = dbContext;
        }

        // Lấy vai trò theo tên
        public async Task<Role?> GetRoleByNameAsync(string roleName)
        {
            return await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName && !r.IsDeleted);
        }

        // Lấy danh sách người dùng trong vai trò
        public async Task<IEnumerable<User>> GetUsersInRoleAsync(string roleName)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
            {
                return new List<User>();
            }
            return await _dbContext.Users.Where(u => u.RoleId == role.RoleId).ToListAsync();
        }

        // Kiểm tra vai trò có tồn tại không
        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _dbContext.Roles.AnyAsync(r => r.Name == roleName);
        }
    }
}
