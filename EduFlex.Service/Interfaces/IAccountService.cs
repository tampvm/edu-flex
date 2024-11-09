using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.AccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Interfaces
{
    public interface IAccountService
    {
        Task<ResponseModel> GetUserProfileAsync(string userId);
        Task<ResponseModel> UpdateUserProfileAsync(UpdateUserProfileDTO model);
        Task<ResponseModel> UpdateUserAvatarAsync(UpdateUserAvatarDTO model);
        Task<ResponseModel> UpdateAvatarAsync(UpdateAvatarDTO model);
        Task<ResponseModel> ChangePasswordAsync(ChangePasswordDTO model);
        Task<ResponseModel> SendVerificationCodeAsync(NewPhoneNumberDTO model);
        Task<ResponseModel> VerifyAndUpdateNewPhoneNumberAsync(ChangePhoneNumberDTO model);
        Task<ResponseModel> SendVerificationCodeEmailAsync(NewEmailDTO model);
        Task<ResponseModel> VerifyAndUpdateNewEmailAsync(ChangeEmailDTO model);
        Task<ResponseModel> LinkAccountWithUsernameAsync(UpdateUserAccountDTO model);
        Task<ResponseModel> GetAllUserAsync(string? keyword, bool? status, int pageIndex, int pageSize);
        Task<ResponseModel> CreateUserAsync(CreateUserByAdminDTO model);
        Task<ResponseModel> UpdateUserAsync(UpdateUserByAdminDTO model);
        Task<ResponseModel> DeleteUserAsync(string userId);
        Task<ResponseModel> BlockOrUnBlockUserByAdmin(BlockOrUnBlockAccountDTO model);
    }
}
