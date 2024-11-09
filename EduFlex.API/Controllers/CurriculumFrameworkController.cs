using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ViewModels.CurriculumViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduFlex.API.Controllers
{
    public class CurriculumFrameworkController : BaseController
    {
        private readonly ICurriculumFrameworkService _curriculumFrameworkService;

        public CurriculumFrameworkController(ICurriculumFrameworkService curriculumFrameworkService)
        {
            _curriculumFrameworkService = curriculumFrameworkService;
        }

        #region Manage Curriculum Framework For Admin
        [HttpGet]
        public async Task<IActionResult> GetAllCurriculumFrameworkAsync(string? keyword, int pageIndex = 0, int pageSize = 10)
        {
            var response = await _curriculumFrameworkService.GetAllCurriculumFrameworkAsync(keyword, pageIndex, pageSize);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCurriculumFrameworkByIdAsync(string curriculumFrameworkId)
        {
            var response = await _curriculumFrameworkService.GetCurriculumFrameworkByIdAsync(curriculumFrameworkId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCurriculumFrameworkAsync(CreateCurriculumDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _curriculumFrameworkService.CreateCurriculumFrameworkAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCurriculumFrameworkAsync(UpdateCurriculumDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _curriculumFrameworkService.UpdateCurriculumFrameworkAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCurriculumFrameworkAsync(string curriculumFrameworkId)
        {
            var response = await _curriculumFrameworkService.DeleteCurriculumFrameworkAsync(curriculumFrameworkId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Suggested Curriculum For Admin
        [HttpGet]
        public async Task<IActionResult> GetAllSuggestedCurriculumAsync(string? keyword, int pageIndex = 0, int pageSize = 10)
        {
            var response = await _curriculumFrameworkService.GetAllSuggestedCurriculumAsync(keyword, pageIndex, pageSize);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSuggestedCurriculumAsync(CreateSuggestedCurriculumDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _curriculumFrameworkService.CreateSuggestedCurriculumAsync(model);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut]
        public async Task<IActionResult> ApproveSuggestedCurriculumAsync(string curriculumId)
        {
            var response = await _curriculumFrameworkService.ApproveSuggestedCurriculumAsync(curriculumId);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSuggestedCurriculumAsync(string curriculumId)
        {
            var response = await _curriculumFrameworkService.DeleteSuggestedCurriculumAsync(curriculumId);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion
    }
}
