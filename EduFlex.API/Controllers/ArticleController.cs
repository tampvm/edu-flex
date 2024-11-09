using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ViewModels.ArticleViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduFlex.API.Controllers
{
	public class ArticleController : BaseController
	{
		private readonly IArticleService _articleService;

		public ArticleController(IArticleService articleService)
		{
			_articleService = articleService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllArticleAsync(int pageIndex = 0, int pageSize = 10)
		{
			var response = await _articleService.GetAllArticleAsync(pageIndex, pageSize);
			return Ok(response);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllArticlesByCategoryIdAsync(string categoryId, int pageIndex = 0, int pageSize = 10)
		{
			var response = await _articleService.GetAllArticlesByCategoryIdAsync(categoryId, pageIndex, pageSize);
			return Ok(response);
		}

		[HttpGet]
		public async Task<IActionResult> GetArticleByIdAsync(string id)
		{
			var response = await _articleService.GetArticleByIdAsync(id);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> CreateArticleAsync(CreateArticleDTO model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var response = await _articleService.CreateArticleAsync(model);
			if (response.Success)
			{
				return Ok(response);
			}
			else
			{
				return BadRequest(response);
			}
		}

		[HttpPut]
		public async Task<IActionResult> UpdateArticleAsync(UpdateArticleDTO model)
		{
			var response = await _articleService.UpdateArticleAsync(model);
			if (response.Success)
			{
				return Ok(response);
			}
			else
			{
				return BadRequest(response);
			}
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteArticleAsync(string id)
		{
			var response = await _articleService.DeleteArticleAsync(id);
			if (response.Success)
			{
				return Ok(response);
			}
			else
			{
				return BadRequest(response);
			}
		}
	}
}
