using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.ChapterViewModels
{
    public class ViewListChapterDTO
    {
        public string ChapterId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SubjectInCurriculum { get; set; }
        public List<string> Lessons { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime DeletedAt { get; set; }
        public string DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
