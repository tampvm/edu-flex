using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
    public class Exam : BaseEntity
    {
        // Primary Key
        public string ExamId { get; set; }

        // Attributes
        public string Title { get; set; }
        public string Time { get; set; }

        // Foreign Key
        public string UserId { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual ICollection<QuestionInExam> QuestionInExams { get; set; } = new List<QuestionInExam>();
    }
}
