using EduFlex.Service.Models.ViewModels.AnswerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.QuestionBankViewModels
{
	public class ViewDetailQuestionDTO
	{
		public string QuestionId { get; set; }
		public string Question { get; set; }
		public string Type { get; set; }
		public string Plum { get; set; }

		public string ChapterId { get; set; }
		public string Chapter { get; set; }
		public string LessonId { get; set; }
		public string Lesson { get; set; }

		public List<ViewListAnswerDTO> Answers { get; set; }

		public DateTime CreatedAt { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? DeletedAt { get; set; }
		public string? DeletedBy { get; set; }
		public bool IsDeleted { get; set; }
	}
}
