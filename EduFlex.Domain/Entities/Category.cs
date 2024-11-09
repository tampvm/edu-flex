using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
	public class Category : BaseEntity
	{
        // Primary Key
        public string CategoryId { get; set; }

        // Attributes
        public string Name { get; set; }
		public string Url { get; set; }
		public string? Description { get; set; }
		public bool Status { get; set; }

        // Navigation properties
        public virtual ICollection<ArticleCategory> ArticleCategories { get; set; } = new List<ArticleCategory>();

    }
}
