using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.AccountViewModels
{
    public class UpdateUserAccountDTO
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
