using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
    public class SubjectInCurriculum : BaseEntity
    {
        // Primary Key
        public string SubjectInCurriculumId { get; set; }

        // Attributes
        public string? Description { get; set; }

        // Foreign Keys
        public string SubjectId { get; set; }
        public string CurriculumId { get; set; }
        public string GradeId { get; set; }

        // Navigation properties
        public virtual Subject Subject { get; set; }
        public virtual CurriculumFramework Curriculum { get; set; }
        public virtual Grade Grade { get; set; }
        public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
    }
}
