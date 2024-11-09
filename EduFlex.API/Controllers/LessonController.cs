using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ViewModels.LessonViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduFlex.API.Controllers
{
    public class LessonController : BaseController
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLessonAsync(string? keyword, int pageIndex = 0, int pageSize = 10)
        {
            var response = await _lessonService.GetAllLessonAsync(keyword, pageIndex, pageSize);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLessonByChapterIdAsync(string chapterId)
        {
            var response = await _lessonService.GetAllLessonByChapterIdAsync(chapterId);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetLessonByIdAsync(string lessonId)
        {
            var response = await _lessonService.GetLessonByIdAsync(lessonId);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLessonAsync(CreateLessonDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _lessonService.CreateLessonAsync(model);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLessonAsync(UpdateLessonDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _lessonService.UpdateLessonAsync(model);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteLessonAsync(string lessonId)
        {
            var response = await _lessonService.DeleteLessonAsync(lessonId);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
