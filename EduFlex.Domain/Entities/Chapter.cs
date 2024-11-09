using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
    public class Chapter : BaseEntity
    {
        // Primary Key
        public string ChapterId { get; set; }

        // Attributes
        public string Name { get; set; }
        public string? Description { get; set; }

        // Foreign Key
        public string SubjectInCurriculumId { get; set; }

        // Navigation properties
        public virtual SubjectInCurriculum SubjectInCurriculum { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public virtual ICollection<QuestionBank> QuestionBanks { get; set; } = new List<QuestionBank>();
    }
}
