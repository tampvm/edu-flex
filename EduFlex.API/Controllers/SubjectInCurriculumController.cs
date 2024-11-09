using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ViewModels.SubjectInCurriculumViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduFlex.API.Controllers
{
    public class SubjectInCurriculumController : BaseController
    {
        private readonly ISubjectInCurriculumService _subjectInCurriculumService;

        public SubjectInCurriculumController(ISubjectInCurriculumService subjectInCurriculumService)
        {
            _subjectInCurriculumService = subjectInCurriculumService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubjectInCurriculumAsync(string? keyword, int pageIndex = 0, int pageSize = 10)
        {
            var response = await _subjectInCurriculumService.GetAllSubjectInCurriculumAsync(keyword, pageIndex, pageSize);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubjectInCurriculumByCurriculumAndGradeAsync(string curriculum, string grade)
        {
            var response = await _subjectInCurriculumService.GetAllSubjectInCurriculumByCurriculumAndGradeAsync(curriculum, grade);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetSubjectInCurriculumByIdAsync(string subjectInCurriculumId)
        {
            var response = await _subjectInCurriculumService.GetSubjectInCurriculumByIdAsync(subjectInCurriculumId);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubjectInCurriculumAsync(CreateSubjectInCurriculumDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _subjectInCurriculumService.CreateSubjectInCurriculumAsync(model);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSubjectInCurriculumAsync(UpdateSubjectInCurriculumDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _subjectInCurriculumService.UpdateSubjectInCurriculumAsync(model);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSubjectInCurriculumAsync(string subjectInCurriculumId)
        {
            var response = await _subjectInCurriculumService.DeleteSubjectInCurriculumAsync(subjectInCurriculumId);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
