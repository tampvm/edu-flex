using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.TeachingScheduleModels
{
    public class ViewTeachingScheduleDTO
    {
        public string ScheduleId { get; set; }
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string ClassName { get; set; }
        public string? TeacherName { get; set; } 
        public string? PlanbookTitle { get; set; } 
    }
}
