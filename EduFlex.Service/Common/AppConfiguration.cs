using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Common
{
    public class AppConfiguration
    {
        public string[] DatabaseConnection { get; set; }
        public JWTSettings JWT { get; set; }
        public EmailSettings Email { get; set; }
        public AuthenticationSettings Authentication { get; set; }
        public TwilioSettings Twilio { get; set; }
        public FirebaseSettings Firebase { get; set; }
        public OpenAISettings OpenAI { get; set; }
    }
}
