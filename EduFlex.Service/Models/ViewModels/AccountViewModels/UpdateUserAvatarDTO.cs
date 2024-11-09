using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.AccountViewModels
{
    public class UpdateUserAvatarDTO
    {
        public string UserId { get; set; }
        public string AvatarUrl { get; set; }
    }

    public class UpdateAvatarDTO
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string GoogleEmail { get; set; }
        public string FacebookEmail { get; set; }
        public string AvatarUrl { get; set; }
    }
}
