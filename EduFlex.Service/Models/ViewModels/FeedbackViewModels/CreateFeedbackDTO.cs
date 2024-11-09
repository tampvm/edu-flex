using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.FeedbackViewModels
{
    public class CreateFeedbackDTO
    {
        public string Content { get; set; }
        public int? Rate { get; set; }
        public string TeacherId { get; set; }
        public string PlanbookId { get; set; }
    }
}