using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ViewModels.PlanbookViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduFlex.API.Controllers
{
	public class PlanbookController : BaseController
	{
		private readonly IPlanbookService _planbookService;

		public PlanbookController(IPlanbookService planbookService)
		{
			_planbookService = planbookService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllPlanbooksAsync(int pageIndex = 0, int pageSize = 10)
		{
			var response = await _planbookService.GetAllPlanbooksAsync(pageIndex, pageSize);
			return Ok(response);
		}

		[HttpGet]
		public async Task<IActionResult> GetPlanbookByIdAsync(string planbookId)
		{
			var response = await _planbookService.GetPlanbookByIdAsync(planbookId);
			return Ok(response);
		}

		[HttpGet]
		public async Task<IActionResult> GetPlanbookByCollectionIdAsync(string collectionId, int pageIndex = 0, int pageSize = 10)
		{
			var response = await _planbookService.GetPlanbookByCollectionIdAsync(collectionId, pageIndex, pageSize);
			return Ok(response);
		}

		[HttpGet]
		public async Task<IActionResult> GetPlanbookByLessonIdAsync(string lessonId, int pageIndex = 0, int pageSize = 10)
		{
			var response = await _planbookService.GetPlanbookByLessonIdAsync(lessonId, pageIndex, pageSize);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> CreatePlanbookAsync(CreatePlanbookDTO model)
		{
			if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _planbookService.CreatePlanbookAsync(model);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

		[HttpPut]
		public async Task<IActionResult> UpdatePlanbookAsync(UpdatePlanbookDTO model)
		{
			if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _planbookService.UpdatePlanbookAsync(model);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

		[HttpDelete]
        public async Task<IActionResult> DeletePlanbookAsync(string planbookId)
        {
            var response = await _planbookService.DeletePlanbookAsync(planbookId);
			if (response != null)
			{
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete]
		public async Task<IActionResult> SoftRemovePlanbookAsync(string planbookId)
		{
			var response = await _planbookService.SoftRemovePlanbookAsync(planbookId);
			return Ok(response);
		}

        [HttpPost]
        public async Task<IActionResult> CreatePlanbookFromTemplateAsync(string lessonId)
        {
            var response = await _planbookService.GetPlanbookFromTemplateAsync(lessonId);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

		[HttpPost]
        public async Task<IActionResult> CreatePlanbookUsingAIAsync(string lessonId)
		{
            var response = await _planbookService.GetPlanbookUsingAIAsync(lessonId);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> ClonePlanbookAsync(ClonePlanbookDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _planbookService.ClonePlanbookAsync(model);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
