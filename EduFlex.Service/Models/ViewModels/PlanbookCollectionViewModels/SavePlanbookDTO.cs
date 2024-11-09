using EduFlex.Service.Models.ViewModels.PlanbookViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.PlanbookCollectionViewModels
{
	public class SavePlanbookDTO
	{
		public string? CollectionId { get; set; }
		public string TeacherId { get; set; }
		public string PlanbookId { get; set; }
	}
}
