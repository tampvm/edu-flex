using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
	public class Article : BaseEntity
	{
        // Primary Key
        public string ArticleId { get; set; }

        // Attributes
        public string Url { get; set; }
		public string Title { get; set; }
        public string? Excerpt { get; set; }
		public string? Content { get; set; }
		public string Status { get; set; }

        // Navigation properties
        public virtual ICollection<ArticleCategory> ArticleCategories { get; set; } = new List<ArticleCategory>();
        public virtual ICollection<ArticleImage> ArticleImages { get; set; } = new List<ArticleImage>();

    }
}
