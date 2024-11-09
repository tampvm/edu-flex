using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
	public class UserPackage : BaseEntity
	{
        // Primary Key
        public string UserPackageId { get; set; }

        // Attributes
        public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public bool IsActive { get; set; }

        // Foreign Key
        public string UserId { get; set; }
        public string PackageId { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
		public virtual ServicePackage Package { get; set; }
		public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
	}
}
