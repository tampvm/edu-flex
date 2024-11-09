using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.AccountViewModels
{
    public class ChangePhoneNumberDTO
    {
        public string UserId { get; set; }
        public string NewPhoneNumber { get; set; }
        public string VerificationCode { get; set; }
    }

    public class NewPhoneNumberDTO
    {
        public string UserId { get; set; }

        public string NewPhoneNumber { get; set; }
    }
}
