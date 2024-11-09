using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ViewModels.ActivityViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduFlex.API.Controllers
{
	public class ActivityController : BaseController
	{
		private readonly IActivityService _activityService;

		public ActivityController(IActivityService activityService)
		{
			_activityService = activityService;
		}

		[HttpGet]
		public async Task<IActionResult> GetActivitiesByPlanbookIdAsync(string planbookId)
		{
			var response = await _activityService.GetAllByPlanbookIdAsync(planbookId);
			return Ok(response);
		}

		[HttpGet]
		public async Task<IActionResult> GetActivityByIdAsync(string activityId)
		{
			var response = await _activityService.GetActivityByIdAsync(activityId);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> CreateActivityAsync(CreateActivityDTO model)
		{
			var response = await _activityService.CreateActivityAsync(model);
			return Ok(response);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateActivityAsync(UpdateActivityDTO model)
		{
			var response = await _activityService.UpdateActivityAsync(model);
			return Ok(response);
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteActivityAsync(string activityId)
		{
			var response = await _activityService.DeleteActivityAsync(activityId);
			return Ok(response);
		}
	}
}
