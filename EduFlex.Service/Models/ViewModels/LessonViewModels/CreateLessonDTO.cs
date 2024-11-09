using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.LessonViewModels
{
    public class CreateLessonDTO
    {
        public string Name { get; set; }
        public string? Objectives { get; set; }
        public string? Content { get; set; }
        public string ChapterId { get; set; }
    }
}
