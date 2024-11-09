using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
    public class TeachingSchedule : BaseEntity
    {
        // Primary Key
        public string ScheduleId { get; set; }

        // Attributes
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string ClassName { get; set; }

        // Foreign Key
        public string TeacherId { get; set; }
        public string PlanbookId { get; set; }

        // Navigation properties
        public virtual User Teacher { get; set; }
        public virtual Planbook Planbook { get; set; }
    }
}
