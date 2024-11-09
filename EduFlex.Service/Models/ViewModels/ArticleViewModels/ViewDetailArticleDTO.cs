using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.ArticleViewModels
{
	public class ViewDetailArticleDTO
	{
		public string ArticleId { get; set; }
		public string Url { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string Status { get; set; }
		public string Thumb { get; set; }

		public List<string>? Categories { get; set; }

		public DateTime CreatedAt { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? DeletedAt { get; set; }
		public string? DeletedBy { get; set; }
		public bool IsDeleted { get; set; }
	}
}
