using EduFlex.Service.Models.ViewModels.PlanbookViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.PlanbookCollectionViewModels
{
	public class ViewDetailPlanbookCollectionDTO
	{
		public string CollectionId { get; set; }
		public string CollectionName { get; set; }
		//public string CollectionType { get; set; }
		public bool IsSaved { get; set; }
		public string TeacherId { get; set; }
		//public string TeacherName { get; set; }

		public int PlanbookCount { get; set; }
		public List<ViewListPlanbookDTO> Planbooks { get; set; }

		public DateTime CreatedAt { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? DeletedAt { get; set; }
		public string? DeletedBy { get; set; }
		public bool IsDeleted { get; set; }
	}
}
