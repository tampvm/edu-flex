using EduFlex.Domain.Entities;
using EduFlex.Repository.Data;
using EduFlex.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Repository.Repositories
{
	public class AnswerRepository : IAnswerRepository
	{
		private readonly AppDbContext _dbContext;
		public AnswerRepository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Answer>> GetByQuestionIdAsync(string questionId)
		{
			return await _dbContext.Answers.Where(a => a.QuestionId == questionId).ToListAsync();
		}

		public async Task<Answer> GetByQuestionIdAndTextAsync(string questionId, string answerText)
		{
			return await _dbContext.Answers
				.FirstOrDefaultAsync(a => a.QuestionId == questionId && a.AnswerText == answerText);
		}

		public async Task<Answer> CreateAnswerAsync(Answer answer)
		{
			await _dbContext.Answers.AddAsync(answer);
			await _dbContext.SaveChangesAsync();
			return answer;
		}

		public async Task<Answer> UpdateAnswerAsync(Answer answer)
		{
			_dbContext.Answers.Update(answer);
			await _dbContext.SaveChangesAsync();
			return answer;
		}

		public async Task<bool> DeleteAnswerAsync(Answer answer)
		{
			_dbContext.Answers.Remove(answer);
			await _dbContext.SaveChangesAsync();

			return true;
		}
	}
}
