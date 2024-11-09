using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.ServicePackageViewModels
{
    public class UpdateServicePackageDTO
    {
        public string PackageId { get; set; }
        public string PackageName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Duration { get; set; }
        public int MaxLessonPlans { get; set; }
    }
}
