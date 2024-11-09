using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
    public class Planbook : BaseEntity
    {
        // Primary Key
        public string PlanbookId { get; set; }

        // Attributes
        public string Title { get; set; }
        public string? SchoolName { get; set; }
        public string? TeacherName { get; set; }
        public string Subject { get; set; }
        public string? ClassName { get; set; }
        public int? DurationInPeriods { get; set; }
        public string? KnowledgeObjective { get; set; }
        public string? SkillsObjective { get; set; }
        public string? QualitiesObjective { get; set; }
        public string? TeachingTools { get; set; }
        public string? Notes { get; set; }
        public bool IsDefault { get; set; }

		// Foreign Key
		public string? CollectionId { get; set; }
        public string LessonId { get; set; }

        // Navigation properties
        public virtual PlanbookCollection PlanbookCollection { get; set; }
        public virtual Lesson Lesson { get; set; }
        public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();
        public virtual ICollection<TeachingSchedule> TeachingSchedules { get; set; } = new List<TeachingSchedule>();
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public virtual ICollection<PlanBookShare> PlanbookShares { get; set; } = new List<PlanBookShare>();
	}
}
