using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
    public class Feedback : BaseEntity
    {
        // Primary Key
        public string FeedbackId { get; set; }

        // Attributes
        public string? Content { get; set; }
        public int? Rate { get; set; }
        public string Type { get; set; }
        public bool IsFlagged { get; set; }

		// Foreign Key
		public string TeacherId { get; set; }
        public string PlanbookId { get; set; }

        // Navigation properties
        public virtual User Teacher { get; set; }
        public virtual Planbook Planbook { get; set; }
    }
}
