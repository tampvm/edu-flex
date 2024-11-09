using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.SubjectViewModels
{
    public class UpdateSubjectDTO
    {
        public string SubjectId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
