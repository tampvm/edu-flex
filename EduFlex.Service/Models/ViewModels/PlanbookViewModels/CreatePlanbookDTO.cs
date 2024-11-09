using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.PlanbookViewModels
{
	public class CreatePlanbookDTO
	{
		public string Title { get; set; }
		public string? SchoolName { get; set; }
		public string? TeacherName { get; set; }
		public string Subject { get; set; }
		public string? ClassName { get; set; }
		public int DurationInPeriods { get; set; }
		public string KnowledgeObjective { get; set; }
		public string SkillsObjective { get; set; }
		public string QualitiesObjective { get; set; }
		public string TeachingTools { get; set; }
		public string? Notes { get; set; }
        public bool IsDefault { get; set; }
        public string? CollectionId { get; set; }
		public string LessonId { get; set; }

		public List<CreateActivityForPlanbookDTO>? Activities { get; set; }
	}

	public class CreateActivityForPlanbookDTO
	{
		public string Title { get; set; }
		public string? Objective { get; set; }
		public string? Content { get; set; }
		public string? Product { get; set; }
		public string? Implementation { get; set; }
        //public int Index { get; set; } // Không cần truyền Index vì sẽ tự động tạo Index cho activity
    }
}
