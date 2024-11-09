using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.AuthViewModels
{
    public class ResetPasswordDTO : SendForgotPasswordCodeDTO
    {
        public string ResetPasswordToken { get; set; }
        public string NewPassword { get; set; }
    }

    public class SendForgotPasswordCodeDTO
    {
        public string PhoneNumberOrEmail { get; set; }
    }

    public class VerifyForgotPasswordCodeDTO : SendForgotPasswordCodeDTO
    {
        public string VerificationCode { get; set; }
    }
}
