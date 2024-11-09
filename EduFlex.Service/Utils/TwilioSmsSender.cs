using EduFlex.Service.Common;
using EduFlex.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace EduFlex.Service.Utils
{
    public class TwilioSmsSender : ISmsSender
    {
        private readonly AppConfiguration _twilioSettings;

        public TwilioSmsSender(AppConfiguration twilioSettings)
        {
            _twilioSettings = twilioSettings;
            TwilioClient.Init(_twilioSettings.Twilio.AccountSid1 + _twilioSettings.Twilio.AccountSid2, _twilioSettings.Twilio.AuthToken1 + _twilioSettings.Twilio.AuthToken2);
        }

        public async Task SendSmsAsync(string number, string message)
        {
            // Remove leading 0 and add +84 prefix
            number = "+84" + number.TrimStart('0');

            var messageResource = await MessageResource.CreateAsync(
                body: message,
                from: new Twilio.Types.PhoneNumber(_twilioSettings.Twilio.PhoneNumber),
                to: number
            );
        }
    }
}
