using EduFlex.Service.Common;
using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ViewModels.AuthViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Utils
{
    public class FacebookService : IFacebookService
    {
        public AppConfiguration _facebookSettings;

        public FacebookService(AppConfiguration facebookSettings)
        {
            _facebookSettings = facebookSettings;
        }

        public async Task<FacebookUserInfo> GetUserInfoFromFacebookAsync(string accessToken)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var url = $"https://graph.facebook.com/me?fields=id,email,first_name,last_name,picture&access_token={accessToken}";
                    var response = await httpClient.GetStringAsync(url);
                    var userInfo = JsonConvert.DeserializeObject<FacebookUserInfo>(response);

                    return userInfo;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
