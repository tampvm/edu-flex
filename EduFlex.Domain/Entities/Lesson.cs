using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
    public class Lesson : BaseEntity
    {
        // Primary Key
        public string LessonId { get; set; }

        // Attributes
        public string Name { get; set; }
        public string? Objectives { get; set; }
        public string? Content { get; set; }

        // Foreign Key
        public string ChapterId { get; set; }

        // Navigation properties
        public virtual Chapter Chapter { get; set; }
        public virtual ICollection<Planbook> Planbooks { get; set; } = new List<Planbook>();
        public virtual ICollection<QuestionBank> QuestionBanks { get; set; } = new List<QuestionBank>();
    }
}
