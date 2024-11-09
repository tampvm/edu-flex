using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.AnswerViewModels
{
	public class UpdateAnswerDTO
	{
		public string? AnswerId { get; set; }
		public string AnswerText { get; set; }
		public bool IsCorrect { get; set; }
	}
}
