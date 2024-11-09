using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.AuthViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseModel> LoginAsync(LoginDTO model);
        Task<ResponseModel> GoogleLoginAsync(GoogleLoginDTO model);
        Task<ResponseModel> FacebookLoginAsync(FacebookLoginDTO model);
        Task<ResponseModel> RefreshTokenAsync(RefreshTokenDTO model);
        Task<ResponseModel> SendRegisterVerificationCodeAsync(SendRegisterCodeDTO model);
        Task<ResponseModel> VerifyRegisterCodeAsync(VerifyRegisterCodeDTO model);
        Task<ResponseModel> RegisterAsync(RegisterDTO model);
        Task<ResponseModel> SendForgotPasswordVerificationCodeAsync(SendForgotPasswordCodeDTO model);
        Task<ResponseModel> VerifyForgotPasswordCodeAsync(VerifyForgotPasswordCodeDTO model);
        Task<ResponseModel> ResetPasswordAsync(ResetPasswordDTO model);
    }
}
