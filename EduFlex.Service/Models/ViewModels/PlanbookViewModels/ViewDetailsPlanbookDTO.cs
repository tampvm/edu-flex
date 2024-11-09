using EduFlex.Service.Models.ViewModels.ActivityViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.PlanbookViewModels
{
    public class ViewDetailsPlanbookDTO
    {
        public string PlanbookId { get; set; }
        public string Title { get; set; } // Tiêu đề
        public string SchoolName { get; set; }
        public string TeacherName { get; set; }
        public string Subject { get; set; } // Môn học
        public string ClassName { get; set; }
        public string DurationInPeriods { get; set; }
        public string KnowledgeObjective { get; set; } // Mục tiêu kiến thức
        public string SkillsObjective { get; set; } // Mục tiêu kỹ năng
        public string QualitiesObjective { get; set; } // Mục tiêu phẩm chất
        public string TeachingTools { get; set; } // Thiết bị dạy học
        public string Notes { get; set; } // Ghi chú (Có hoặc không)
        public bool IsDefault { get; set; }
        public string CollectionId { get; set; }
        public string CollectionName { get; set; }
        public string LessonId { get; set; }
        public string LessonName { get; set; }

        // Activities
        public List<ViewListActivityDTO> Activities { get; set; } // Các hoạt động

        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
