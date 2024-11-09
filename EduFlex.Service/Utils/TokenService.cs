using EduFlex.Domain.Entities;
using EduFlex.Repository.Interfaces;
using EduFlex.Service.Common;
using EduFlex.Service.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Utils
{
    public class TokenService : ITokenService
    {
        private readonly AppConfiguration _appConfiguration;
        private readonly IMemoryCache _cache;
        private readonly ITimeService _timeService;

        public TokenService(AppConfiguration appConfiguration, IMemoryCache cache, ITimeService timeService)
        {
            _appConfiguration = appConfiguration;
            _cache = cache;
            _timeService = timeService;
        }

        // Tạo mã xác thực ngẫu nhiên
        public string GenerateVerificationCode()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        // Tạo Json Web Token
        public string GenerateJsonWebToken(User user, out DateTime tokenExpiryTime)
        {
            var jwt = _appConfiguration.JWT;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.JWTSecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserId),
                new Claim ("userId", user.UserId.ToString())
            };

            var roles = user.Role.Name;

            // Thêm các claim về vai trò
            //foreach (var role in roles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role));
            //}

            claims.Add(new Claim(ClaimTypes.Role, roles));

            tokenExpiryTime = _timeService.GetCurrentTime().AddMinutes(jwt.AccessTokenDurationInMinutes); // Thời gian hết hạn của token

            var token = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: claims,
                expires: tokenExpiryTime,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Tạo refresh token
        public string GenerateRefreshToken(out DateTime refreshTokenExpiryTime)
        {
            byte[] randomNumber = RandomNumberGenerator.GetBytes(32);
            refreshTokenExpiryTime = DateTime.UtcNow.AddDays(_appConfiguration.JWT.RefreshTokenDurationInDays);

            return Convert.ToBase64String(randomNumber);
        }

        // Lấy thông tin từ token
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfiguration.JWT.JWTSecretKey)),
                ValidateLifetime = false // Không cần kiểm tra thời hạn
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        // Tạo token và lưu vào cache sau khi xác thực sdt hoặc email thành công của quá trình đăng ký hoặc quên mật khẩu
        public string GenerateToken(string identifier, string method, string type, out DateTime expiryTime)
        {
            var token = Guid.NewGuid().ToString();
            expiryTime = DateTime.UtcNow.ToLocalTime().AddMinutes(10);

            // Lưu token vào cache
            _cache.Set($"{identifier}_{method}_{type}_token", token, TimeSpan.FromMinutes(10));

            return token;
        }

        // Xác thực token đăng ký hoặc quên mật khẩu
        public bool ValidateToken(string identifier, string method, string type, string token)
        {
            if (_cache.TryGetValue($"{identifier}_{method}_{type}_token", out string? cachedToken) && cachedToken == token)
            {
                // Xóa token khỏi cache ngay sau khi xác thực thành công
                //_cache.Remove($"{identifier}_{method}_{type}_token");
                return true;
            }
            return false;
        }
    }
}
