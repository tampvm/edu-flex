using EduFlex.Service.Models.ViewModels.ActivityViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.PlanbookViewModels
{
	public class UpdatePlanbookDTO
	{
		public string PlanbookId { get; set; }
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
        //public string CollectionId { get; set; } // Không cho phép sửa CollectionId
        //public string LessonId { get; set; } // Không cho phép sửa LessonId

        public List<UpdateActivityForPlanbookDTO>? Activities { get; set; }
	}

    public class UpdateActivityForPlanbookDTO
    {
        public string ActivityId { get; set; }
        public string Title { get; set; }
        public string Objective { get; set; }
        public string Content { get; set; }
        public string Product { get; set; }
        public string Implementation { get; set; }
        //public int Index { get; set; } // Không cần truyền Index vì sẽ tự động tạo Index cho activity
    }
}
