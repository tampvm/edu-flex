using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Common
{
    public class GoogleSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
        public string TokenUrl { get; set; }
        public string UserInfoUrl { get; set; }
        public string GrantType { get; set; }
    }
}
