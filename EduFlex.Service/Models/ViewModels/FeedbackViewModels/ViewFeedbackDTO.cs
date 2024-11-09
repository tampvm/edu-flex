using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.FeedbackViewModels
{
    public class ViewFeedbackDTO
    {
        public string FeedbackId { get; set; }
        public string Content { get; set; }
        public int? Rate { get; set; }
        public string TeacherName { get; set; }
        public string PlanbookTitle { get; set; }
    }
}
