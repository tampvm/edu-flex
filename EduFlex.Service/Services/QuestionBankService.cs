using AutoMapper;
using EduFlex.Domain.Entities;
using EduFlex.Repository.Common;
using EduFlex.Repository.Interfaces;
using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.AnswerViewModels;
using EduFlex.Service.Models.ViewModels.QuestionBankViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Services
{
	public class QuestionBankService : IQuestionBankService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public QuestionBankService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<ResponseModel> GetAllQuestionBankAsync(string? keyword, int pageIndex, int pageSize)
		{
			var questions = await _unitOfWork.QuestionBankRepository.GetAsync(
				filter: r => r.IsDeleted.Equals(false)
				&& (string.IsNullOrEmpty(keyword)
				|| r.Question.Contains(keyword)),
				orderBy: r => r.OrderBy(r => r.Question),
				pageIndex: pageIndex,
				pageSize: pageSize
				);
			var questionDtos = _mapper.Map<Pagination<ViewListQuestionBankDTO>>(questions);

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Question retrieved successfully.",
				Data = questionDtos
			};
		}

		public async Task<ResponseModel> GetQuestionBankByIdAsync(string questionId)
		{
			var question = await _unitOfWork.QuestionBankRepository.GetByQuestionIdAsync(questionId);
			if (question == null)
			{
				return new ResponseModel
				{
					Success = false,
					Message = "Question not found."
				};
			}

			var questionDto = _mapper.Map<ViewDetailQuestionDTO>(question);

			questionDto.Chapter = question.Chapter?.Name ?? string.Empty;
			questionDto.Lesson = question.Lesson?.Name ?? string.Empty;

			questionDto.Answers = question.Answers.Select(a => new ViewListAnswerDTO
			{
				AnswerId = a.AnswerId,
				AnswerText = a.AnswerText,
				IsCorrect = a.IsCorrect.ToString()
			}).ToList();

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Question retrieved successfully.",
				Data = questionDto
			};
		}

		public async Task<ResponseModel> CreateQuestionAsync(CreateQuestionDTO model)
		{
			try
			{
				var question = _mapper.Map<QuestionBank>(model);

				await _unitOfWork.QuestionBankRepository.AddAsync(question);
				await _unitOfWork.SaveChangeAsync();

				if (model.Answers != null && model.Answers.Count > 0)
				{
					foreach (var answer in model.Answers)
					{
						var existingAnswer = await _unitOfWork.AnswerRepository.GetByQuestionIdAndTextAsync(question.QuestionId, answer.AnswerText);
						if (existingAnswer == null)
						{
							var mapper = _mapper.Map<Answer>(answer);

							mapper.QuestionId = question.QuestionId;
							await _unitOfWork.AnswerRepository.CreateAnswerAsync(mapper);
						}
					}
				}

				return new ResponseModel
				{
					Success = true,
					Message = "Question create successfully."
				};
			}
			catch (Exception ex)
			{
				return new ResponseModel
				{
					Success = false,
					Message = "An error occurred while creating the question: " + ex.Message
				};
			}
		}

		public async Task<ResponseModel> UpdateQuestionAsync(UpdateQuestionDTO model)
		{
			try
			{
				var question = await _unitOfWork.QuestionBankRepository.GetByQuestionIdAsync(model.QuestionId);
				if (question == null)
				{
					return new ResponseModel
					{
						Success = false,
						Message = "Question not found."
					};
				}
				if (question.IsDeleted == true)
				{
					return new ResponseModel
					{
						Success = false,
						Message = "Can't modify question is deleted."
					};
				}

				_mapper.Map(model, question);
				_unitOfWork.QuestionBankRepository.Update(question);
				await _unitOfWork.SaveChangeAsync();

				var existingAnswers = await _unitOfWork.AnswerRepository.GetByQuestionIdAsync(model.QuestionId);
				var existingAnswerIds = existingAnswers.Select(a => a.AnswerId).ToList();
				foreach (var answer in existingAnswers)
				{
					var updatedAnswer = model.Answers.FirstOrDefault(a => a.AnswerId == answer.AnswerId);
					if (updatedAnswer != null)
					{
						_mapper.Map(updatedAnswer, answer);
						await _unitOfWork.AnswerRepository.UpdateAnswerAsync(answer);
					}
					else
					{
						await _unitOfWork.AnswerRepository.DeleteAnswerAsync(answer);
					}
				}

				foreach (var newAnswer in model.Answers)
				{
					var existingNewAnswer = await _unitOfWork.AnswerRepository.GetByQuestionIdAndTextAsync(question.QuestionId, newAnswer.AnswerText);
					if (!existingAnswerIds.Contains(newAnswer.AnswerId) && existingNewAnswer == null)
					{
						var answerToAdd = _mapper.Map<Answer>(newAnswer);
						await _unitOfWork.AnswerRepository.CreateAnswerAsync(answerToAdd);
					}
				}

				return new ResponseModel
				{
					Success = true,
					Message = "Question updated successfully."
				};
			}
			catch (Exception ex)
			{
				return new ResponseModel
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ResponseModel> DeleteQuestionAsync(string questionId)
		{
			try
			{
				var question = await _unitOfWork.QuestionBankRepository.GetByIdAsync(questionId);
				if (question == null)
				{
					return new ResponseModel
					{
						Success = false,
						Message = "Question not found."
					};
				}
				if (question.IsDeleted == true)
				{
					return new ResponseModel
					{
						Success = false,
						Message = "Question is already deleted."
					};
				}

				var existingAnswers = await _unitOfWork.AnswerRepository.GetByQuestionIdAsync(questionId);
				foreach (var answer in existingAnswers)
				{
					await _unitOfWork.AnswerRepository.DeleteAnswerAsync(answer);
				}

				_unitOfWork.QuestionBankRepository.Delete(question);
				await _unitOfWork.SaveChangeAsync();

				return new ResponseModel
				{
					Success = true,
					Message = "Question deleted successfully."
				};
			}
			catch (Exception ex)
			{
				return new ErrorResponseModel<object>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}
	}
}
