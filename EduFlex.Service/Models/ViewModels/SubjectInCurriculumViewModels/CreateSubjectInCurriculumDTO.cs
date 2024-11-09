using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.SubjectInCurriculumViewModels
{
    public class CreateSubjectInCurriculumDTO
    {
        public string SubjectId { get; set; }
        public string CurriculumId { get; set; }
        public string GradeId { get; set; }
        public string? Description { get; set; }
    }
}
