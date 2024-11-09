using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
	public class ServicePackage : BaseEntity
	{
        // Primary Key
        public string PackageId { get; set; }

        // Attributes
        public string PackageName { get; set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public decimal Discount { get; set; }
		//public int Duration { get; set; }
        public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int MaxPlanbooks { get; set; }

        // Navigation properties
        public virtual ICollection<UserPackage> UserPackages { get; set; } = new List<UserPackage>();
    }
}
