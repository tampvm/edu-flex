using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.SubjectInCurriculumViewModels
{
    public class ViewListSubjectInCurriculumDTO
    {
        public string SubjectInCurriculumId { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Grade { get; set; }
        public string Curriculum { get; set; }
        public string Description { get; set; }
        public List<string> Chapters { get; set; } 
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime DeletedAt { get; set; }
        public string DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
