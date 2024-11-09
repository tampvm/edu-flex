using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.AuthViewModels
{
    public class GoogleLoginDTO
    {
        public string GoogleToken { get; set; }

        public bool IsCredential { get; set; }
    }

    public class GooglePayload
    {
        public string Sub { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PictureUrl { get; set; }

        // Các thuộc tính khác từ payload của Google token có thể được thêm vào đây nếu cần
    }

    public class GoogleTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        // Các thuộc tính khác từ phản hồi Google nếu cần
    }
}
