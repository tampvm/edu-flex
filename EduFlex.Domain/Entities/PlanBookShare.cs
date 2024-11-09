using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
	public class PlanBookShare : BaseEntity
	{
		public string ShareId { get; set; }
		public string ShareType { get; set; }
		public string? ShareTo { get; set; } // User Id
		public string? ShareToEmail { get; set; }
		public bool IsEdited { get; set; }

		public string PlanBookId { get; set; }
		public string ShareBy { get; set; } // User Id

		public virtual Planbook PlanBook { get; set; }
		public virtual User User { get; set; }
	}
}
