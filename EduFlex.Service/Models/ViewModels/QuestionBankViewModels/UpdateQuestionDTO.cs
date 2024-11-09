using EduFlex.Service.Models.ViewModels.AnswerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.QuestionBankViewModels
{
	public class UpdateQuestionDTO
	{
		public string QuestionId { get; set; }
		public string Question { get; set; }
		public string Type { get; set; }
		public string Plum { get; set; }
		public string? ChapterId { get; set; }
		public string? LessonId { get; set; }

		public List<UpdateAnswerDTO> Answers { get; set; }
	}
}
