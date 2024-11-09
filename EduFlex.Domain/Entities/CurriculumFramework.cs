using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
    public class CurriculumFramework : BaseEntity
    {
        // Primary Key
        public string CurriculumId { get; set; }

        // Attributes
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsApproved { get; set; }

        // Navigation properties
        public virtual ICollection<SubjectInCurriculum> SubjectInCurriculums { get; set; } = new List<SubjectInCurriculum>();
    }
}
