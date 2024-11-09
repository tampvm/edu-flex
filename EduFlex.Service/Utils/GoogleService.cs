using EduFlex.Service.Common;
using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ViewModels.AuthViewModels;
using Google.Apis.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Utils
{
    public class GoogleService : IGoogleService
    {
        public AppConfiguration _googleSettings;

        public GoogleService(AppConfiguration googleSettings)
        {
            _googleSettings = googleSettings;
        }

        public async Task<GoogleTokenResponse> ExchangeAuthCodeForTokensAsync(string authCode)
        {
            // Đổi auth code lấy access token và id token
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var values = new Dictionary<string, string>
                    {
                        { "code", authCode },
                        { "client_id", _googleSettings.Authentication.Google.ClientId }, // Replace with your client ID
                        { "client_secret", _googleSettings.Authentication.Google.ClientSecret }, // Replace with your client secret
                        { "redirect_uri", _googleSettings.Authentication.Google.RedirectUri }, // Replace with your redirect URI
                        { "grant_type", _googleSettings.Authentication.Google.GrantType }
                    };

                    var content = new FormUrlEncodedContent(values);
                    var response = await httpClient.PostAsync(_googleSettings.Authentication.Google.TokenUrl, content);
                    response.EnsureSuccessStatusCode();

                    var responseString = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<GoogleTokenResponse>(responseString);

                    return tokenResponse;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<GooglePayload> VerifyGoogleTokenAsync(string token)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _googleSettings.Authentication.Google.ClientId }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
                return new GooglePayload
                {
                    Sub = payload.Subject,
                    Email = payload.Email,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    PictureUrl = payload.Picture
                };
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
