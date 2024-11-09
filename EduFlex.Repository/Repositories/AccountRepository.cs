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
    public class AccountRepository : GenericRepository<User> , IAccountRepository
    {
        //private readonly AppDbContext _dbContext;

        public AccountRepository(AppDbContext dbContext, ITimeService timeService, IClaimsService claimsService) : base(dbContext, timeService, claimsService)
        {
            //_dbContext = dbContext;
        }

        // Lấy người dùng theo email
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        // Lấy người dùng theo tên đăng nhập
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        // Lấy người dùng theo số điện thoại
        public async Task<User?> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        // Lấy người dùng theo email, tên đăng nhập hoặc số điện thoại
        public async Task<User?> GetUserByEmailOrUsernameOrPhoneNumberAsync(string emailOrUsernameOrPhoneNumber, bool includeUsername = true)
        {
            return await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == emailOrUsernameOrPhoneNumber || 
                                          includeUsername && u.Username == emailOrUsernameOrPhoneNumber || 
                                          u.PhoneNumber == emailOrUsernameOrPhoneNumber);
        }

        // Lấy người dùng theo email, số điện thoại, tên đăng nhập, email google hoặc email facebook
        public async Task<User?> FindByAnyCriteriaAsync(string email, string phoneNumber, string userName, string googleEmail, string facebookEmail)
        {
            return await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(a => (!string.IsNullOrEmpty(email) && a.Email == email) ||
                                          (!string.IsNullOrEmpty(phoneNumber) && a.PhoneNumber == phoneNumber) ||
                                          (!string.IsNullOrEmpty(userName) && a.Username == userName) ||
                                          (!string.IsNullOrEmpty(googleEmail) && a.GoogleEmail == googleEmail) ||
                                          (!string.IsNullOrEmpty(facebookEmail) && a.FacebookEmail == facebookEmail));
        }
    }
}
