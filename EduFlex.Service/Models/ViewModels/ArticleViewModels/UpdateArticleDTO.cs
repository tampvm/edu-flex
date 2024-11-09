using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.ArticleViewModels
{
	public class UpdateArticleDTO
	{
		[Required(ErrorMessage = "ArticleId is required.")]
		public string ArticleId { get; set; }
		[Required(ErrorMessage = "Title is required.")]
		public string Title { get; set; }
		public string? Slug { get; set; }
		public string? Content { get; set; }
		public string Status { get; set; }
		public string? Thumb { get; set; }
		public List<string>? Categories { get; set; }
	}
}
