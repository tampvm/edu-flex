using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
    public class QuestionInExam
    {
        // Primary Key
        public string QuestionInExamId { get; set; }

        // Foreign Key
        public string ExamId { get; set; }
        public string QuestionId { get; set; }
        public int Index { get; set; }

        // Navigation properties
        public virtual Exam Exam { get; set; }
        public virtual QuestionBank Question { get; set; }
    }
}
