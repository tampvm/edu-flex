using EduFlex.Domain.Entities;
using EduFlex.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Repository.Interfaces
{
	public interface IAnswerRepository
	{
		Task<List<Answer>> GetByQuestionIdAsync(string questionId);
		Task<Answer> GetByQuestionIdAndTextAsync(string questionId, string answerText);
		Task<Answer> CreateAnswerAsync(Answer answer);
		Task<Answer> UpdateAnswerAsync(Answer answer);
		Task<bool> DeleteAnswerAsync(Answer answer);
	}
}
