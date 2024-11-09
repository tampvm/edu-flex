using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Domain.Entities
{
	public class Answer : BaseEntity
	{
		// Primary Key
		public string AnswerId { get; set; }

		// Attributes
		public string AnswerText { get; set; }
		public string IsCorrect { get; set; }

		// Foreign Key
		public string QuestionId { get; set; }

		// Navigation properties
		public virtual QuestionBank Question { get; set; }
	}
}
