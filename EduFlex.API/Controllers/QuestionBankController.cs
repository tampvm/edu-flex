using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ViewModels.QuestionBankViewModels;
using EduFlex.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduFlex.API.Controllers
{
	public class QuestionBankController : BaseController
	{
		private readonly IQuestionBankService _questionBankService;
		public QuestionBankController(IQuestionBankService questionBankService)
		{
			_questionBankService = questionBankService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllQuestionBankAsync(string? keyword, int pageIndex = 0, int pageSize = 10)
		{
			var response = await _questionBankService.GetAllQuestionBankAsync(keyword, pageIndex, pageSize);
			return Ok(response);
		}

		[HttpGet]
		public async Task<IActionResult> GetQuestionBankByIdAsync(string id)
		{
			var response = await _questionBankService.GetQuestionBankByIdAsync(id);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> CreateQuestionAsync(CreateQuestionDTO model)
		{
			var response = await _questionBankService.CreateQuestionAsync(model);
			return Ok(response);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateQuestionAsync(UpdateQuestionDTO model)
		{
			var response = await _questionBankService.UpdateQuestionAsync(model);
			return Ok(response);
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteQuestionAsync(string id)
		{
			var response = await _questionBankService.DeleteQuestionAsync(id);
			return Ok(response);
		}
	}
}
