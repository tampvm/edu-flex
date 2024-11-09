using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
    public class Grade : BaseEntity
    {
        // Primary Key
        public string GradeId { get; set; }

        // Attributes
        public string Name { get; set; }
        public string? Description { get; set; }

        // Navigation properties
        public virtual ICollection<SubjectInCurriculum> SubjectInCurriculums { get; set; } = new List<SubjectInCurriculum>();
    }
}
