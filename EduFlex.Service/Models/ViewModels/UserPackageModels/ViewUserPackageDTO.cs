using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.UserPackageModels
{
    public class ViewUserPackageDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string PackageId { get; set; }
        public string PackageName { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsExpired { get; set; }
    }
}
