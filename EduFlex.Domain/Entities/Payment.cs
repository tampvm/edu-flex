using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
	public class Payment : BaseEntity
	{
        // Primary Key
        public string PaymentId { get; set; }

        // Attributes
		public string PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
		public string FullName { get; set; }
        public string AddressText { get; set; }
        public string Status { get; set; }

        // Foreign Key
        public string TeacherId { get; set; }
		public string UserPackageId { get; set; }

		// Navigation properties
		public virtual User Teacher { get; set; }
		public virtual UserPackage UserPackage { get; set; }
	}
}
