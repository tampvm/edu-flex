using EduFlex.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Repository.Interfaces
{
    public interface IAccountRepository : IGenericRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByPhoneNumberAsync(string phoneNumber);
        Task<User?> GetUserByEmailOrUsernameOrPhoneNumberAsync(string emailOrUsernameOrPhoneNumber, bool includeUsername = true);
        Task<User?> FindByAnyCriteriaAsync(string email, string phoneNumber, string userName, string googleEmail, string facebookEmail);
    }
}
