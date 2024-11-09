using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.ActivityViewModels
{
	public class CreateActivityDTO
	{
		public string Title { get; set; }
		public string? Objective { get; set; }
		public string? Content { get; set; }
		public string? Product { get; set; }
		public string? Implementation { get; set; }
		public int Index { get; set; }
		public string PlanbookId { get; set; }
	}
}
