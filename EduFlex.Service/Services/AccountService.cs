using AutoMapper;
using EduFlex.Domain.Entities;
using EduFlex.Repository.Common;
using EduFlex.Repository.Interfaces;
using EduFlex.Service.Common;
using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.AccountViewModels;
using EduFlex.Service.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Services
{
    public class AccountService : BaseService, IAccountService
    {
        public AccountService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ITimeService timeService,
            IPasswordService passwordHasher,
            IEmailSender emailSender,
            ISmsSender smsSender,
            IMemoryCache cache,
            ITokenService tokenService)
            : base(unitOfWork, mapper, timeService, passwordHasher, emailSender, smsSender, cache, tokenService)
        {
        }

        #region Manage User Profile
        // Get existingUser profile
        public async Task<ResponseModel> GetUserProfileAsync(string userId)
        {
            try
            {
                var user = await _unitOfWork.AccountRepository.GetByIdAsync(id: userId, includeProperties: "Role,Avatar,Background");
                if (user == null)
                {
                    return new ResponseModel { Success = false, Message = "User not found." };
                }

                var userProfileDTO = _mapper.Map<ViewUserProfileDTO>(user);
                userProfileDTO.Role = user.Role.Name;
                userProfileDTO.Avatar = user.Avatar?.ImageUrl;
                userProfileDTO.Background = user.Background?.ImageUrl;

                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "User profile found.",
                    Data = userProfileDTO
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the user profile.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        // Update existingUser profile
        public async Task<ResponseModel> UpdateUserProfileAsync(UpdateUserProfileDTO model)
        {
            try
            {
                var user = await _unitOfWork.AccountRepository.GetByIdAsync(model.UserId);
                if (user == null)
                {
                    return new ResponseModel { Success = false, Message = "User not found." };
                }

                // Sử dụng mapper để cập nhật thuộc tính của đối tượng existingUser từ model
                _mapper.Map(model, user);

                _unitOfWork.AccountRepository.Update(user);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel { Success = true, Message = "User profile updated successfully." };

            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while updating user profile.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        // Update existingUser avatar
        public async Task<ResponseModel> UpdateUserAvatarAsync(UpdateUserAvatarDTO model)
        {
            try
            {
                var user = await _unitOfWork.AccountRepository.GetByIdAsync(model.UserId);

                if (user == null)
                {
                    return new ResponseModel { Success = false, Message = "User not found." };
                }

                // Nếu người dùng đã có AvatarId, cập nhật avatar cũ
                if (!string.IsNullOrEmpty(user.AvatarId))
                {
                    var oldAvatar = await _unitOfWork.ImageRepository.GetByIdAsync(user.AvatarId);
                    if (oldAvatar != null)
                    {
                        oldAvatar.ImageUrl = model.AvatarUrl;
                        _unitOfWork.ImageRepository.Update(oldAvatar);
                    }
                }
                else
                {
                    // Nếu người dùng chưa có Avatar, thêm ảnh mới
                    var newAvatar = new Image
                    {
                        ImageId = Guid.NewGuid().ToString(),
                        ImageUrl = model.AvatarUrl,
                        Type = "Avatar",
                        CreatedBy = user.UserId
                    };

                    await _unitOfWork.ImageRepository.AddAsync(newAvatar);
                    await _unitOfWork.SaveChangeAsync();

                    // Gán AvatarId cho người dùng
                    user.AvatarId = newAvatar.ImageId;
                    _unitOfWork.AccountRepository.Update(user);
                }

                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel { Success = true, Message = "User avatar updated successfully." };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while updating user avatar.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ResponseModel> UpdateAvatarAsync(UpdateAvatarDTO model)
        {
            try
            {
                // Tìm người dùng dựa trên các tiêu chí tìm kiếm khác nhau
                var user = await _unitOfWork.AccountRepository.FindByAnyCriteriaAsync(
                    model.Email, model.PhoneNumber, model.UserName, model.GoogleEmail, model.FacebookEmail
                );

                if (user == null)
                {
                    return new ResponseModel { Success = false, Message = "User not found." };
                }

                // Tạo một bản ghi mới cho hình ảnh avatar
                var newAvatar = new Image
                {
                    ImageId = Guid.NewGuid().ToString(),
                    ImageUrl = model.AvatarUrl,
                    Type = "Avatar",
                    CreatedBy = user.UserId
                };

                await _unitOfWork.ImageRepository.AddAsync(newAvatar);
                await _unitOfWork.SaveChangeAsync();

                // Cập nhật avatar cho existingUser
                user.AvatarId = newAvatar.ImageId;

                _unitOfWork.AccountRepository.Update(user);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel { Success = true, Message = "Avatar updated successfully." };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while updating avatar.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        #endregion

        #region Change Password
        public async Task<ResponseModel> ChangePasswordAsync(ChangePasswordDTO model)
        {
            try
            {
                var user = await _unitOfWork.AccountRepository.GetByIdAsync(model.UserId);
                if (user == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "User not found."
                    };
                }

                // Kiểm tra mật khẩu nhập vào có trùng với mật khẩu hiện tại của người dùng không
                if (!_passwordHasher.VerifyPassword(user.PasswordHash, model.CurrentPassword))
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Current password is incorrect."
                    };
                }

                // Kiểm tra mật khẩu mới có đúng định dạng không
                var passwordErrors = _passwordHasher.ValidatePassword(model.NewPassword);
                if (passwordErrors.Any())
                {
                    return new ErrorResponseModel<object>
                    {
                        Success = false,
                        Message = "Password is not in correct format.",
                        Errors = passwordErrors.ToList()
                    };
                }

                // Mã hóa mật khẩu mới
                user.PasswordHash = _passwordHasher.HashPassword(model.NewPassword);

                _unitOfWork.AccountRepository.Update(user);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Password changed successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while changing password.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        #endregion

        #region Update User Phone Number Or Link Phone Number
        // Send verification code to the new phone number when existingUser wants to change phone number
        public async Task<ResponseModel> SendVerificationCodeAsync(NewPhoneNumberDTO model)
        {
            try
            {
                var user = await _unitOfWork.AccountRepository.GetByIdAsync(model.UserId);
                if (user == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "User not found."
                    };
                }

                // Kiểm tra xem số điện thoại mới đã được sử dụng chưa
                var existingUser = await _unitOfWork.AccountRepository.GetUserByPhoneNumberAsync(model.NewPhoneNumber);
                if (existingUser != null && existingUser.UserId != user.UserId)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Phone number is already in use."
                    };
                }

                // Tạo mã xác thực và gửi mã xác thực đến số điện thoại mới
                var code = _tokenService.GenerateVerificationCode();
                await _smsSender.SendSmsAsync(model.NewPhoneNumber, $"Mã xác thực của bạn là: {code}, mã sẽ hết hiệu lực sau 10 phút.");

                _cache.Set(model.NewPhoneNumber, code, TimeSpan.FromMinutes(10)); // Lưu mã xác thực vào cache với thời gian hết hạn 10 phút
                DateTime expiryTime = DateTime.UtcNow.ToLocalTime().AddMinutes(10); // Thời gian hết hạn 10 phút từ bây giờ

                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Verification code sent to new phone number.",
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

        // Verify the new phone number with the verification code and update the phone number
        public async Task<ResponseModel> VerifyAndUpdateNewPhoneNumberAsync(ChangePhoneNumberDTO model)
        {
            try
            {
                var user = await _unitOfWork.AccountRepository.GetByIdAsync(model.UserId);
                if (user == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "User not found."
                    };
                }

                // Xác thực mã xác thực
                if (_cache.TryGetValue(model.NewPhoneNumber, out string? cachedCode) && cachedCode == model.VerificationCode)
                {
                    _cache.Remove(model.NewPhoneNumber); // Xóa mã xác thực khỏi cache

                    // Cập nhật số điện thoại mới cho người dùng
                    user.PhoneNumber = model.NewPhoneNumber;
                    user.PhoneNumberConfirmed = true;

                    _unitOfWork.AccountRepository.Update(user);
                    await _unitOfWork.SaveChangeAsync();

                    return new SuccessResponseModel<object>
                    {
                        Success = true,
                        Message = "Phone number updated successfully.",
                        Data = new
                        {
                            UserId = user.UserId,
                            NewPhoneNumber = user.PhoneNumber
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
                    Message = "An error occurred while verifying new phone number.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        #endregion

        #region Update User Email Or Link Email
        // Send verification code to the new email when existingUser wants to change email
        public async Task<ResponseModel> SendVerificationCodeEmailAsync(NewEmailDTO model)
        {
            try
            {
                var user = await _unitOfWork.AccountRepository.GetByIdAsync(model.UserId);
                if (user == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "User not found."
                    };
                }

                // Kiểm tra xem email mới đã được sử dụng chưa
                var existingUser = await _unitOfWork.AccountRepository.GetUserByEmailAsync(model.NewEmail);
                if (existingUser != null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Email is already in use."
                    };
                }

                // Tạo mã xác thực và gửi mã xác thực đến email mới
                var code = _tokenService.GenerateVerificationCode();
                await _emailSender.SendEmailAsync(model.NewEmail, "Verification Code", $"Mã xác thực của bạn là: {code}, mã sẽ hết hiệu lực sau 10 phút.");

                _cache.Set(model.NewEmail, code, TimeSpan.FromMinutes(10)); // Lưu mã xác thực vào cache với thời gian hết hạn 10 phút
                DateTime expiryTime = DateTime.UtcNow.ToLocalTime().AddMinutes(10); // Thời gian hết hạn 10 phút từ bây giờ

                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Verification code sent to new email.",
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

        // Verify the new email with the verification code
        public async Task<ResponseModel> VerifyAndUpdateNewEmailAsync(ChangeEmailDTO model)
        {
            try
            {
                var user = await _unitOfWork.AccountRepository.GetByIdAsync(model.UserId);
                if (user == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "User not found."
                    };
                }

                // Xác thực mã xác thực
                if (_cache.TryGetValue(model.NewEmail, out string? cachedCode) && cachedCode == model.VerificationCode)
                {
                    _cache.Remove(model.NewEmail); // Xóa mã xác thực khỏi cache

                    // Cập nhật email mới cho người dùng
                    user.Email = model.NewEmail;
                    user.EmailConfirmed = true;

                    _unitOfWork.AccountRepository.Update(user);
                    await _unitOfWork.SaveChangeAsync();

                    return new SuccessResponseModel<object>
                    {
                        Success = true,
                        Message = "Email updated successfully.",
                        Data = new
                        {
                            UserId = user.UserId,
                            NewEmail = user.Email
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
                    Message = "An error occurred while verifying new email.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        #endregion

        #region Link Account With Username
        public async Task<ResponseModel> LinkAccountWithUsernameAsync(UpdateUserAccountDTO model)
        {
            try
            {
                var user = await _unitOfWork.AccountRepository.GetByIdAsync(model.UserId);
                if (user == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "User not found."
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

                // Kiểm tra xem username đã được sử dụng chưa
                var existingUser = await _unitOfWork.AccountRepository.GetUserByUsernameAsync(model.Username);
                if (existingUser != null && existingUser.UserId != model.UserId)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Username is already taken."
                    };
                }

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

                user.Username = model.Username;
                user.PasswordHash = _passwordHasher.HashPassword(model.Password);

                _unitOfWork.AccountRepository.Update(user);
                await _unitOfWork.SaveChangeAsync();

                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Username updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while updating user account.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        #endregion

        #region Manage User For Admin
        // Get all users with filter by last name, first name, phone number, status
        public async Task<ResponseModel> GetAllUserAsync(string? keyword, bool? status, int pageIndex, int pageSize)
        {
            try
            {
                var users = await _unitOfWork.AccountRepository.GetAsync(
                               filter: u => !u.IsDeleted && (string.IsNullOrEmpty(keyword) ||
                                                            u.LastName.Contains(keyword) ||
                                                            u.FirstName.Contains(keyword) ||
                                                            u.PhoneNumber.Contains(keyword)) &&
                                                            (!status.HasValue || u.Status == status.Value),
                               orderBy: u => u.OrderBy(u => u.UserId),
                               includeProperties: "Role,Avatar",
                               pageIndex: pageIndex,
                               pageSize: pageSize
                               );

                var userDtos = _mapper.Map<Pagination<ViewListUserDTO>>(users);

                foreach (var userDto in userDtos.Items)
                {
                    var user = await _unitOfWork.AccountRepository.GetByIdAsync(userDto.UserId);

                    userDto.CreatedBy = string.IsNullOrEmpty(user.CreatedBy) ? null : (await _unitOfWork.AccountRepository.GetByIdAsync(user.CreatedBy))?.Username ?? user.CreatedBy;
                    userDto.UpdatedBy = string.IsNullOrEmpty(user.UpdatedBy) ? null : (await _unitOfWork.AccountRepository.GetByIdAsync(user.UpdatedBy))?.Username ?? user.UpdatedBy;
                    userDto.DeletedBy = string.IsNullOrEmpty(user.DeletedBy) ? null : (await _unitOfWork.AccountRepository.GetByIdAsync(user.DeletedBy))?.Username ?? user.DeletedBy;
                }

                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Users found.",
                    Data = userDtos
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while retrieving users.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        // Add a new user by admin
        public async Task<ResponseModel> CreateUserAsync(CreateUserByAdminDTO model)
        {
            try
            {
                // Kiểm tra xem username có đủ độ dài yêu cầu không (tối thiểu 6 ký tự)
                if (model.Username.Length < 6 || model.Username.Length > 20)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Username must be between 6 and 20 characters long."
                    };
                }

                // Kiểm tra xem username đã được sử dụng chưa
                var existingUser = await _unitOfWork.AccountRepository.GetUserByUsernameAsync(model.Username);
                if (existingUser != null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Username is already taken."
                    };
                }

                // Kiểm tra tên role có tồn tại không
                var existingRole = await _unitOfWork.RoleRepository.GetRoleByNameAsync(model.RoleName);
                if (existingRole is null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Role not exists."
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

                // Gán RoleId cho existingUser
                user.RoleId = existingRole.RoleId;

                // Tạo mới existingUser
                await _unitOfWork.AccountRepository.AddAsync(user);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "User created successfully.",
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while creating user.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        // Update user by admin
        public async Task<ResponseModel> UpdateUserAsync(UpdateUserByAdminDTO model)
        {
            try
            {
                // Kiểm tra người dùng có tồn tại không
                var existingUser = await _unitOfWork.AccountRepository.GetByIdAsync(model.UserId);
                if (existingUser == null)
                {
                    return new ResponseModel 
                    { 
                        Success = false, 
                        Message = "User not found." 
                    };
                }

                // Kiểm tra tên role có tồn tại không
                var existingRole = await _unitOfWork.RoleRepository.GetRoleByNameAsync(model.RoleName);
                if (existingRole is null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Role not exists."
                    };
                }

                // Sử dụng mapper để cập nhật thuộc tính của đối tượng existingUser từ model
                var user = _mapper.Map(model, existingUser);

                // Gán RoleId cho user
                user.RoleId = existingRole.RoleId;

                _unitOfWork.AccountRepository.Update(user);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "User updated successfully."
                };

            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while updating user.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        // Delete user by admin
        public async Task<ResponseModel> DeleteUserAsync(string userId)
        {
            try
            {
                var user = await _unitOfWork.AccountRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "User not found."
                    };
                }

                // Xóa người dùng (xóa mềm)
                _unitOfWork.AccountRepository.SoftRemove(user);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "User deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while deleting user.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        // Block or unblock user by admin
        public async Task<ResponseModel> BlockOrUnBlockUserByAdmin(BlockOrUnBlockAccountDTO model)
        {
            try
            {
                var user = await _unitOfWork.AccountRepository.GetByIdAsync(model.UserId);

                if (user == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "User not found."
                    };
                }

                user.Status = model.Status;

                _unitOfWork.AccountRepository.Update(user);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = model.Status ? "User unblocked successfully." : "User blocked successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while blocking or unblocking user.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        #endregion
    }
}
