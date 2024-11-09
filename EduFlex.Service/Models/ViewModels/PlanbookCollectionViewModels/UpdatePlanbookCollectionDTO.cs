using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.PlanbookCollectionViewModels
{
	public class UpdatePlanbookCollectionDTO
	{
		public string CollectionId { get; set; }

		public string CollectionName { get; set; }
		//public string CollectionType { get; set; }
		public bool IsSaved { get; set; }
		public string TeacherId { get; set; }

		public List<string>? PlanbookIds { get; set; }
	}
}
