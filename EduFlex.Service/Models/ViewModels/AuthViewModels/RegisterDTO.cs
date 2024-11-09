using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.AuthViewModels
{
    public class RegisterDTO
    {
        public string RegisterToken { get; set; } // truyền ẩn trong thẻ input sau khi verify
        public string PhoneNumberOrEmail { get; set; } // truyền ẩn trong thẻ input sau khi verify
        public string FirstName { get; set; } // Nhập
        public string LastName { get; set; } // Nhập
        public string Username { get; set; } // Nhập
        public string Password { get; set; } // Nhập
    }

    public class SendRegisterCodeDTO
    {
        public string PhoneNumberOrEmail { get; set; }
    }

    public class VerifyRegisterCodeDTO : SendRegisterCodeDTO
    {
        public string VerificationCode { get; set; }
    }
}
