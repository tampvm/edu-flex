using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
	public class QuestionBank : BaseEntity
	{
        // Primary Key
        public string QuestionId { get; set; }

        // Attributes
        public string Question { get; set; }
		public string Type { get; set; }
		public string Plum { get; set; }

        // Foreign Key
        public string ChapterId { get; set; }
        public string? LessonId { get; set; }

        // Navigation properties
        public virtual Chapter Chapter { get; set; }
        public virtual Lesson Lesson { get; set; }
		public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public virtual ICollection<QuestionInExam> QuestionInExams { get; set; } = new List<QuestionInExam>();
    }
}
