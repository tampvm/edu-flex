using EduFlex.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Interfaces
{
    public interface ITokenService
    {
        string GenerateVerificationCode();
        string GenerateJsonWebToken(User user, out DateTime tokenExpiryTime);
        string GenerateRefreshToken(out DateTime refreshTokenExpiryTime);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        string GenerateToken(string identifier, string method, string type, out DateTime expiryTime);
        bool ValidateToken(string identifier, string method, string type, string token);
    }
}
