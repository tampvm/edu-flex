using AutoMapper;
using EduFlex.Domain.Entities;
using EduFlex.Domain.Enums;
using EduFlex.Repository.Interfaces;
using EduFlex.Service.Common;
using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.AccountViewModels;
using EduFlex.Service.Models.ViewModels.AuthViewModels;
using EduFlex.Service.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EduFlex.Service.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IGoogleService _googleService;
        private readonly IFacebookService _facebookService;
        private readonly IAccountService _accountService;

        public AuthService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ITimeService timeService,
            IPasswordService passwordHasher,
            IEmailSender emailSender,
            ISmsSender smsSender,
            IMemoryCache cache,
            ITokenService tokenService,
            IGoogleService googleService,
            IFacebookService facebookService,
            IAccountService accountService) 
            : base(unitOfWork, mapper, timeService, passwordHasher, emailSender, smsSender, cache, tokenService)
        {
            _googleService = googleService;
            _facebookService = facebookService;
            _accountService = accountService;
        }

        #region Login
        // Login
        public async Task<ResponseModel> LoginAsync(LoginDTO model)
        {
            try
            {
                var user = await _unitOfWork.AccountRepository.GetUserByEmailOrUsernameOrPhoneNumberAsync(model.Username);

                if (user != null)
                {
                    // Kiểm tra nếu người dùng bị khóa
                    if (!user.Status)
                    {
                        return new AuthenticationResponseModel
                        {
                            Success = false,
                            Message = "User account is blocked. Please contact support."
                        };
                    }

                    // Kiểm tra mật khẩu
                    var result = _passwordHasher.VerifyPassword(user.PasswordHash, model.Password);

                    if (result)
                    {
                        var accessToken = _tokenService.GenerateJsonWebToken(user, out DateTime tokenExpiryTime);
                        var refreshToken = _tokenService.GenerateRefreshToken(out DateTime refreshTokenExpiryTime);

                        user.RefreshToken = refreshToken;
                        user.RefreshTokenExpiryTime = refreshTokenExpiryTime;
                        user.LastLogin = _timeService.GetCurrentTime();

                        _unitOfWork.AccountRepository.Update(user);
                        await _unitOfWork.SaveChangeAsync();

                        return new AuthenticationResponseModel
                        {
                            Success = true,
                            Message = "Login success",
                            AccessToken = accessToken,
                            RefreshToken = refreshToken,
                            TokenExpiryTime = tokenExpiryTime
                        };
                    }
                    else
                    {
                        return new AuthenticationResponseModel
                        {
                            Success = false,
                            Message = "Wrong password!",
                        };
                    }
                }
                else
                {
                    return new AuthenticationResponseModel
                    {
                        Success = false,
                        Message = "User not found!",
                    };
                }
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while processing login.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        // Generate JWT
        //private string GenerateJsonWebToken(User user, out DateTime tokenExpiryTime)
        //{
        //    var jwt = _appConfiguration.JWT;

        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.JWTSecretKey));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    var claims = new List<Claim>
        //    {
        //        //new Claim(JwtRegisteredClaimNames.Sub, user.Username),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        //new Claim(JwtRegisteredClaimNames.Name, user.Username),
        //        new Claim(ClaimTypes.Name, user.UserId),
        //        new Claim ("userId", user.UserId.ToString())
        //    };

        //    var roles = user.Role.Name;

        //    // Thêm các claim về vai trò
        //    //foreach (var role in roles)
        //    //{
        //    //    claims.Add(new Claim(ClaimTypes.Role, role));
        //    //}

        //    claims.Add(new Claim(ClaimTypes.Role, roles));

        //    tokenExpiryTime = _timeService.GetCurrentTime().AddMinutes(jwt.AccessTokenDurationInMinutes);

        //    var token = new JwtSecurityToken(
        //        issuer: jwt.Issuer,
        //        audience: jwt.Audience,
        //        claims: claims,
        //        expires: tokenExpiryTime,
        //        signingCredentials: credentials);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
        #endregion

        #region Social Login
        // Google login
        public async Task<ResponseModel> GoogleLoginAsync(GoogleLoginDTO model)
        {
            GooglePayload payload;

            if (model.IsCredential)
            {
                payload = await _googleService.VerifyGoogleTokenAsync(model.GoogleToken);
            }
            else
            {
                var tokenResponse = await _googleService.ExchangeAuthCodeForTokensAsync(model.GoogleToken);
                if (tokenResponse == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Invalid Google auth code."
                    };
                }

                payload = await _googleService.VerifyGoogleTokenAsync(tokenResponse.IdToken);
            }

            if (payload == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Invalid Google token."
                };
            }

            var socialLoginDto = new SocialLoginDTO
            {
                Email = payload.Email,
                FirstName = payload.FirstName,
                LastName = payload.LastName,
                PictureUrl = payload.PictureUrl,
                ProviderId = payload.Sub,
                Provider = "Google"
            };

            return await ProcessSocialLoginAsync(socialLoginDto);
        }

        // Facebook login
        public async Task<ResponseModel> FacebookLoginAsync(FacebookLoginDTO model)
        {
            var userInfo = await _facebookService.GetUserInfoFromFacebookAsync(model.AccessToken);
            if (userInfo == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Invalid Facebook access token."
                };
            }

            var socialLoginDto = new SocialLoginDTO
            {
                Email = userInfo.Email,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                PictureUrl = userInfo.Picture.Data.Url,
                ProviderId = userInfo.Id,
                Provider = "Facebook"
            };

            return await ProcessSocialLoginAsync(socialLoginDto);
        }

        // Process social login
        private async Task<ResponseModel> ProcessSocialLoginAsync(SocialLoginDTO dto)
        {
            try
            {
                // Tìm hoặc tạo người dùng dựa trên GoogleEmail hoặc FacebookEmail
                var user = await _unitOfWork.AccountRepository.FindByAnyCriteriaAsync(null, null, null,
                    dto.Provider == "Google" ? dto.Email : null,
                    dto.Provider == "Facebook" ? dto.Email : null
                );

                if (user == null)
                {
                    user = _mapper.Map<User>(dto);

                    // Thiết lập các thuộc tính cho Google hoặc Facebook email
                    if (dto.Provider == "Google")
                    {
                        user.GoogleEmail = dto.Email;
                    }
                    else if (dto.Provider == "Facebook")
                    {
                        user.FacebookEmail = dto.Email;
                    }

                    user.RoleId = (await _unitOfWork.RoleRepository.GetRoleByNameAsync(RoleEnums.Teacher.ToString())).RoleId;
                    user.CreatedBy = user.UserId;

                    await _unitOfWork.AccountRepository.AddAsync(user);
                    await _unitOfWork.SaveChangeAsync();

                    // Cập nhật avatar sau khi tạo người dùng
                    var updateUserAvatarDto = new UpdateAvatarDTO
                    {
                        GoogleEmail = dto.Provider == "Google" ? dto.Email : null,
                        FacebookEmail = dto.Provider == "Facebook" ? dto.Email : null,
                        AvatarUrl = dto.PictureUrl
                    };

                    // Cập nhật ảnh đại diện
                    await _accountService.UpdateAvatarAsync(updateUserAvatarDto);
                }

                // Kiểm tra nếu người dùng bị khóa
                if (!user.Status)
                {
                    return new AuthenticationResponseModel
                    {
                        Success = false,
                        Message = "User account is blocked. Please contact support."
                    };
                }

                // Tạo JWT token hoặc phản hồi xác thực khác
                var accessToken = _tokenService.GenerateJsonWebToken(user, out DateTime tokenExpiryTime);
                var refreshToken = _tokenService.GenerateRefreshToken(out DateTime refreshTokenExpiryTime);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = refreshTokenExpiryTime;
                user.LastLogin = _timeService.GetCurrentTime();

                _unitOfWork.AccountRepository.Update(user);
                await _unitOfWork.SaveChangeAsync();

                return new AuthenticationResponseModel
                {
                    Success = true,
                    Message = "Login successful.",
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    TokenExpiryTime = tokenExpiryTime
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ResponseModel
                {
                    Success = false,
                    Message = "An error occurred while processing login."
                };
            }
        }
        #endregion

        #region Refresh Token
        // Refresh token
        public async Task<ResponseModel> RefreshTokenAsync(RefreshTokenDTO model)
        {
            try
            {
                var principal = _tokenService.GetPrincipalFromExpiredToken(model.AccessToken);
                var userId = principal.Identity.Name;

                var user = await _unitOfWork.AccountRepository.GetByIdAsync(userId);

                if (user == null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    return new AuthenticationResponseModel
                    {
                        Success = false,
                        Message = "Invalid refresh token"
                    };
                }

                // Kiểm tra nếu token vẫn còn trong thời hạn sử dụng
                var validToClaim = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
                if (validToClaim != null && DateTime.UtcNow < DateTime.UnixEpoch.AddSeconds(Convert.ToInt64(validToClaim)))
                {
                    return new AuthenticationResponseModel
                    {
                        Success = false,
                        Message = "Token is still valid. No need to refresh."
                    };
                }

                var newAccessToken = _tokenService.GenerateJsonWebToken(user, out DateTime tokenExpiryTime);
                var newRefreshToken = _tokenService.GenerateRefreshToken(out DateTime refreshTokenExpiryTime);

                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = refreshTokenExpiryTime;

                _unitOfWork.AccountRepository.Update(user);
                await _unitOfWork.SaveChangeAsync();

                return new AuthenticationResponseModel
                {
                    Success = true,
                    Message = "Token refreshed successfully",
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    TokenExpiryTime = tokenExpiryTime
                };
            }
            catch (Exception)
            {
                return new AuthenticationResponseModel
                {
                    Success = false,
                    Message = "Failed to refresh token"
                };
            }
        }
        #endregion

        #region Register
        // Send verification code to phone number or email when user register
        public async Task<ResponseModel> SendRegisterVerificationCodeAsync(SendRegisterCodeDTO model)
        {
            try
            {
                if (IsEmail(model.PhoneNumberOrEmail))
                {
                    var user = await _unitOfWork.AccountRepository.GetUserByEmailAsync(model.PhoneNumberOrEmail);
                    if (user != null)
                    {
                        return new ResponseModel
                        {
                            Success = false,
                            Message = "Email already in use."
                        };
                    }

                    var emailCode = _tokenService.GenerateVerificationCode();
                    await _emailSender.SendEmailAsync(model.PhoneNumberOrEmail, "Xác thực tài khoản", $"Mã xác thực của bạn là: {emailCode}, mã sẽ hết hiệu lực sau 10 phút.");
                    _cache.Set(model.PhoneNumberOrEmail, emailCode, TimeSpan.FromMinutes(10));
                }
                else if (IsPhoneNumber(model.PhoneNumberOrEmail))
                {
                    var user = await _unitOfWork.AccountRepository.GetUserByPhoneNumberAsync(model.PhoneNumberOrEmail);
                    if (user != null)
                    {
                        return new ResponseModel
                        {
                            Success = false,
                            Message = "Phone number already in use."
                        };
                    }

                    var phoneCode = _tokenService.GenerateVerificationCode();
                    await _smsSender.SendSmsAsync(model.PhoneNumberOrEmail, $"Mã xác thực của bạn là: {phoneCode}, mã sẽ hết hiệu lực sau 10 phút.");
                    _cache.Set(model.PhoneNumberOrEmail, phoneCode, TimeSpan.FromMinutes(10));
                }
                else
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Invalid phone number or email format."
                    };
                }

                DateTime expiryTime = DateTime.UtcNow.ToLocalTime().AddMinutes(10); // Thời gian hết hạn 10 phút từ bây giờ

                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Verification code sent.",
                    Data = new
                    {
                        CodeExpiryTime = expiryTime
                    }
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while sending verification code.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        // Verify phone number or email by verification code when user register
        public async Task<ResponseModel> VerifyRegisterCodeAsync(VerifyRegisterCodeDTO model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.PhoneNumberOrEmail) || string.IsNullOrEmpty(model.VerificationCode))
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Phone number or email and verification code are required."
                    };
                }

                if (IsEmail(model.PhoneNumberOrEmail))
                {
                    // Kiểm tra email tồn tại
                    var user = await _unitOfWork.AccountRepository.GetUserByEmailAsync(model.PhoneNumberOrEmail);
                    if (user != null)
                    {
                        return new ResponseModel
                        {
                            Success = false,
                            Message = "Email already in use."
                        };
                    }

                    // Xác thực mã
                    if (_cache.TryGetValue(model.PhoneNumberOrEmail, out string savedEmailCode) && savedEmailCode == model.VerificationCode)
                    {
                        _cache.Remove(model.PhoneNumberOrEmail);
                        var emailToken = _tokenService.GenerateToken(model.PhoneNumberOrEmail, "email", "register", out DateTime expiryTime);

                        return new SuccessResponseModel<object>
                        {
                            Success = true,
                            Message = "Email verified.",
                            Data = new
                            {
                                Token = emailToken,
                                TokenExpiryTime = expiryTime
                            }
                        };
                    }
                }
                else if (IsPhoneNumber(model.PhoneNumberOrEmail))
                {
                    // Kiểm tra số điện thoại tồn tại
                    var user = await _unitOfWork.AccountRepository.GetUserByPhoneNumberAsync(model.PhoneNumberOrEmail);
                    if (user != null)
                    {
                        return new ResponseModel
                        {
                            Success = false,
                            Message = "Phone number already in use."
                        };
                    }

                    // Xác thực mã
                    if (_cache.TryGetValue(model.PhoneNumberOrEmail, out string savedPhoneCode) && savedPhoneCode == model.VerificationCode)
                    {
                        _cache.Remove(model.PhoneNumberOrEmail);
                        var phoneToken = _tokenService.GenerateToken(model.PhoneNumberOrEmail, "phone", "register", out DateTime expiryTime);

                        return new SuccessResponseModel<object>
                        {
                            Success = true,
                            Message = "Phone number verified.",
                            Data = new
                            {
                                Token = phoneToken,
                                TokenExpiryTime = expiryTime
                            }
                        };
                    }
                }

                return new ResponseModel
                {
                    Success = false,
                    Message = "Invalid verification code."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while verifying code.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        // Register
        public async Task<ResponseModel> RegisterAsync(RegisterDTO model)
        {
            try
            {
                if (IsEmail(model.PhoneNumberOrEmail))
                {
                    if (!_tokenService.ValidateToken(model.PhoneNumberOrEmail, "email", "register", model.RegisterToken))
                    {
                        return new ResponseModel
                        {
                            Success = false,
                            Message = "Invalid or expired email registration token."
                        };
                    }

                    var emailExists = await _unitOfWork.AccountRepository.GetUserByEmailAsync(model.PhoneNumberOrEmail);
                    if (emailExists != null)
                    {
                        return new ResponseModel
                        {
                            Success = false,
                            Message = $"{model.PhoneNumberOrEmail} already exists"
                        };
                    }
                }
                else if (IsPhoneNumber(model.PhoneNumberOrEmail))
                {
                    if (!_tokenService.ValidateToken(model.PhoneNumberOrEmail, "phone", "register", model.RegisterToken))
                    {
                        return new ResponseModel
                        {
                            Success = false,
                            Message = "Invalid or expired phone registration token."
                        };
                    }

                    var phoneExists = await _unitOfWork.AccountRepository.GetUserByPhoneNumberAsync(model.PhoneNumberOrEmail);
                    if (phoneExists != null)
                    {
                        return new ResponseModel
                        {
                            Success = false,
                            Message = $"{model.PhoneNumberOrEmail} already exists"
                        };
                    }
                }
                else
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Invalid phone number or email format."
                    };
                }

                // Kiểm tra xem username có đủ độ dài yêu cầu không (tối thiểu 6 ký tự)
                if (model.Username.Length < 6 || model.Username.Length > 20)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Username must be between 6 and 20 characters long."
                    };
                }

                // Kiểm tra username đã tồn tại chưa
                var userExists = await _unitOfWork.AccountRepository.GetUserByUsernameAsync(model.Username);
                if (userExists != null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = $"{model.Username} already exists"
                    };
                }

                var user = _mapper.Map<User>(model);

                // Kiểm tra mật khẩu có đúng định dạng không
                var passwordErrors = _passwordHasher.ValidatePassword(model.Password);
                if (passwordErrors.Any())
                {
                    return new ErrorResponseModel<string>
                    {
                        Success = false,
                        Message = "Password is not in correct format.",
                        Errors = passwordErrors.ToList()
                    };
                }

                // Băm mật khẩu
                user.PasswordHash = _passwordHasher.HashPassword(model.Password);

                // Gán email hoặc số điện thoại cho người dùng dựa vào input
                if (IsEmail(model.PhoneNumberOrEmail))
                {
                    user.Email = model.PhoneNumberOrEmail;
                    user.EmailConfirmed = true;
                }
                else if (IsPhoneNumber(model.PhoneNumberOrEmail))
                {
                    user.PhoneNumber = model.PhoneNumberOrEmail;
                    user.PhoneNumberConfirmed = true;
                }

                user.RoleId = (await _unitOfWork.RoleRepository.GetRoleByNameAsync(RoleEnums.Teacher.ToString())).RoleId;
                user.CreatedBy = user.UserId;

                await _unitOfWork.AccountRepository.AddAsync(user);
                await _unitOfWork.SaveChangeAsync();

                // Xóa token trong cache sau khi đăng ký thành công
                _cache.Remove($"{model.PhoneNumberOrEmail}_phonge_register_token");
                _cache.Remove($"{model.PhoneNumberOrEmail}_email_register_token");

                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Registration success.",
                    Data = new
                    {
                        UserId = user.UserId,
                        FullName = user.FirstName + " " + user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        Username = user.Username,
                        Email = user.Email
                    }
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while registering.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        // Check email format
        private bool IsEmail(string input)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(input);
                return addr.Address == input;
            }
            catch
            {
                return false;
            }
        }

        // Check phone number format
        private bool IsPhoneNumber(string input)
        {
            var phoneNumberPattern = @"^(\+84\s?\d{9}|0\d{9})$";

            return Regex.IsMatch(input, phoneNumberPattern);
        }
        #endregion

        #region Forgot Password
        // Send verification code to phone number or email when user forgot password
        public async Task<ResponseModel> SendForgotPasswordVerificationCodeAsync(SendForgotPasswordCodeDTO model)
        {
            try
            {
                var user = await _unitOfWork.AccountRepository.GetUserByEmailOrUsernameOrPhoneNumberAsync(model.PhoneNumberOrEmail, includeUsername: false);

                if (user is null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "User not found."
                    };
                }

                string code = _tokenService.GenerateVerificationCode();
                DateTime expiryTime = DateTime.UtcNow.ToLocalTime().AddMinutes(10);

                if (model.PhoneNumberOrEmail.Contains("@"))
                {

                    await _emailSender.SendEmailAsync(model.PhoneNumberOrEmail, "Quên mật khẩu?", $"Mã xác thực của bạn là: {code}, mã sẽ hết hiệu lực sau 10 phút.");
                }
                else
                {
                    await _smsSender.SendSmsAsync(model.PhoneNumberOrEmail, $"Mã xác thực của bạn là: {code}, mã sẽ hết hiệu lực sau 10 phút.");
                }

                _cache.Set(model.PhoneNumberOrEmail, code, TimeSpan.FromMinutes(10)); // Lưu mã xác thực vào bộ đệm với thời gian hết hạn 10 phút

                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Verification code sent.",
                    Data = new
                    {
                        CodeExpiryTime = expiryTime
                    }
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while sending verification code.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        // Verify forgot password code by phone number or email
        public async Task<ResponseModel> VerifyForgotPasswordCodeAsync(VerifyForgotPasswordCodeDTO model)
        {
            try
            {
                var user = await _unitOfWork.AccountRepository.GetUserByEmailOrUsernameOrPhoneNumberAsync(model.PhoneNumberOrEmail, includeUsername: false);

                if (user is null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "User not found."
                    };
                }

                // Lấy mã từ bộ nhớ cache
                if (_cache.TryGetValue(model.PhoneNumberOrEmail, out string? cachedCode) && cachedCode == model.VerificationCode)
                {
                    // Mã xác thực hợp lệ, tạo token reset password
                    var resetToken = _tokenService.GenerateToken(
                        model.PhoneNumberOrEmail,
                        model.PhoneNumberOrEmail.Contains("@") ? "email" : "phone",
                        "reset",
                        out DateTime expiryTime
                    );

                    return new SuccessResponseModel<object>
                    {
                        Success = true,
                        Message = "Verification successful. Proceed to password reset.",
                        Data = new
                        {
                            ResetToken = resetToken,
                            ResetTokenExpiryTime = expiryTime
                        }
                    };
                }

                return new ResponseModel
                {
                    Success = false,
                    Message = "Invalid verification code."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while verifying code.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        // Reset password
        public async Task<ResponseModel> ResetPasswordAsync(ResetPasswordDTO model)
        {
            try
            {
                var user = await _unitOfWork.AccountRepository.GetUserByEmailOrUsernameOrPhoneNumberAsync(model.PhoneNumberOrEmail, includeUsername: false);

                if (user is null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "User not found."
                    };
                }

                string method = model.PhoneNumberOrEmail.Contains("@") ? "email" : "phone";
                if (!_tokenService.ValidateToken(model.PhoneNumberOrEmail, method, "reset", model.ResetPasswordToken))
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Invalid or expired reset token."
                    };
                }

                // Kiểm tra mật khẩu mới có đúng định dạng không
                var passwordErrors = _passwordHasher.ValidatePassword(model.NewPassword);
                if (passwordErrors.Any())
                {
                    return new ErrorResponseModel<string>
                    {
                        Success = false,
                        Message = "Password is not in correct format.",
                        Errors = passwordErrors.ToList()
                    };
                }

                // Băm mật khẩu mới
                user.PasswordHash = _passwordHasher.HashPassword(model.NewPassword);
                user.UpdatedBy = user.UserId;

                _unitOfWork.AccountRepository.Update(user);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Password has been reset successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while resetting password.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        #endregion
    }
}
