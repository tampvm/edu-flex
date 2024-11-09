using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.PaymentViewModels
{
    public class PaymentDetailDTO
    {
        public string PaymentId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string TeacherId { get; set; }
        public string PackageName { get; set; }
        public string PackageDescription { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
