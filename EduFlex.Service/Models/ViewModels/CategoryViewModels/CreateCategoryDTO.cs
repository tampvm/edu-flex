using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.CategoryViewModels
{
	public class CreateCategoryDTO
	{
		[Required(ErrorMessage = "Name is required.")]
		public string Name { get; set; }
		//public string Url { get; set; }
		public string? Description { get; set; }
	}
}
