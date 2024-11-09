using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Common
{
    public class JWTSettings
    {
        public string JWTSecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenDurationInMinutes { get; set; }
        public int RefreshTokenDurationInDays { get; set; }
    }
}
