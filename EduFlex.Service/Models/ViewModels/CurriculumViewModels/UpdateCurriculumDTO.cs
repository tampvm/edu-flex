using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.CurriculumViewModels
{
    public class UpdateCurriculumDTO
    {
        public string CurriculumId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
