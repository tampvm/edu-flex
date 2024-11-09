using EduFlex.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EduFlex.Service.Utils
{
    public class PasswordService : IPasswordService
    {
        private const int MinimumLength = 6;

        #region ValidatePassword
        public IEnumerable<string> ValidatePassword(string password)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(password))
            {
                errors.Add("Password cannot be empty.");
            }
            else
            {
                if (password.Length < MinimumLength)
                {
                    errors.Add($"Password must be at least {MinimumLength} characters.");
                }

                if (!password.Any(char.IsUpper))
                {
                    errors.Add("Password must have at least one uppercase (\'A\'-\'Z\').");
                }

                if (!password.Any(char.IsLower))
                {
                    errors.Add("Password must have at least one lowercase (\'a\'-\'z\').");
                }

                if (!password.Any(char.IsDigit))
                {
                    errors.Add("Password must have at least one digit (\'0\'-\'9\').");
                }

                if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?""{}|<>]"))
                {
                    errors.Add("Password must have at least one non alphanumeric character.");
                }
            }

            return errors;
        }
        #endregion

        #region BCrypt
        // Băm mật khẩu
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Xác minh mật khẩu băm với mật khẩu được cung cấp
        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        }
        #endregion

        #region PBKDF2
        // Băm mật khẩu bằng PBKDF2
        public string HashPasswordPBKDF2(string password)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(password, 16, 10000, HashAlgorithmName.SHA256))
            {
                var salt = rfc2898.Salt;
                var hash = rfc2898.GetBytes(32);
                var hashBytes = new byte[48];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 32);
                return Convert.ToBase64String(hashBytes);
            }
        }

        // Xác minh mật khẩu
        public bool VerifyPasswordPBKDF2(string hashedPassword, string providedPassword)
        {
            var hashBytes = Convert.FromBase64String(hashedPassword);
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            using (var rfc2898 = new Rfc2898DeriveBytes(providedPassword, salt, 10000, HashAlgorithmName.SHA256))
            {
                var hash = rfc2898.GetBytes(32);
                for (int i = 0; i < 32; i++)
                {
                    if (hashBytes[i + 16] != hash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        #endregion
    }
}
