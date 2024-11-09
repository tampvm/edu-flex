using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.AccountViewModels
{
    public class ChangeEmailDTO
    {
        public string UserId { get; set; }
        public string NewEmail { get; set; }
        public string VerificationCode { get; set; }
    }

    public class NewEmailDTO
    {
        public string UserId { get; set; }
        public string NewEmail { get; set; }
    }
}
