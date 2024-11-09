using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Models.ViewModels.AnswerViewModels
{
	public class ViewListAnswerDTO
	{
		public string AnswerId { get; set; }
		public string AnswerText { get; set; }
		public string IsCorrect { get; set; }
	}
}
