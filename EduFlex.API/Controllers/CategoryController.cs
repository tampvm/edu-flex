using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ViewModels.CategoryViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduFlex.API.Controllers
{
	public class CategoryController : BaseController
	{
		private readonly ICategoryService _categoryService;
		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllCategoryAsync(int pageIndex = 0, int pageSize = 10)
		{
			var response = await _categoryService.GetAllCategoryAsync(pageIndex, pageSize);
			return Ok(response);
		}

		[HttpGet]
		public async Task<IActionResult> GetCategoryByIdAsync(string id)
		{
			var response = await _categoryService.GetCategoryByIdAsync(id);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> CreateCategoryAsync(CreateCategoryDTO model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var response = await _categoryService.CreateCategoryAsync(model);
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
		public async Task<IActionResult> UpdateCategoryAsync(UpdateCategoryDTO model)
		{
			var response = await _categoryService.UpdateCategoryAsync(model);
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
		public async Task<IActionResult> DeleteCategoryAsync(string id)
		{
			var response = await _categoryService.DeleteCategoryAsync(id);
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
