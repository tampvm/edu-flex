using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ResponseModels
{
    public class AuthenticationResponseModel : ResponseModel
    {
        [JsonPropertyOrder(3)]
        public string AccessToken { get; set; }

        [JsonPropertyOrder(4)]
        public string RefreshToken { get; set; }

        [JsonPropertyOrder(5)]
        public DateTime TokenExpiryTime { get; set; }
    }
}
