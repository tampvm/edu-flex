using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ViewModels.TeachingScheduleModels;
using Microsoft.AspNetCore.Mvc;

namespace EduFlex.API.Controllers
{
    public class TeachingScheduleController : BaseController
    {
        private readonly ITeachingScheduleService _teachingScheduleService;

        public TeachingScheduleController(ITeachingScheduleService teachingScheduleService)
        {
            _teachingScheduleService = teachingScheduleService;
        }

        #region Get All Teaching Schedules
        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetAllTeachingSchedulesAsync(string? keyword, int pageIndex = 0, int pageSize = 10)
        {
            var response = await _teachingScheduleService.GetAllTeachingSchedulesAsync(keyword, pageIndex, pageSize);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Get Teaching Schedule by ID
        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetTeachingScheduleByIdAsync(string scheduleId)
        {
            var response = await _teachingScheduleService.GetTeachingScheduleByIdAsync(scheduleId);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
        #endregion

        #region Add New Teaching Schedule
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> AddTeachingScheduleAsync(CreateTeachingScheduleDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _teachingScheduleService.AddTeachingScheduleAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Update Teaching Schedule
        [HttpPut]
        //[Authorize]
        public async Task<IActionResult> UpdateTeachingScheduleAsync(UpdateTeachingScheduleDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _teachingScheduleService.UpdateTeachingScheduleAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Delete Teaching Schedule
        [HttpDelete]
        //[Authorize]
        public async Task<IActionResult> DeleteTeachingScheduleAsync(string scheduleId)
        {
            var response = await _teachingScheduleService.DeleteTeachingScheduleAsync(scheduleId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion
    }
}
