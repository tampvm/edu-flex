using EduFlex.Service.Models.ViewModels.AuthViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Interfaces
{
    public interface IGoogleService
    {
        Task<GoogleTokenResponse> ExchangeAuthCodeForTokensAsync(string authCode);
        Task<GooglePayload> VerifyGoogleTokenAsync(string token);
    }
}
