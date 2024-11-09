using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.LessonViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Interfaces
{
    public interface ILessonService
    {
        Task<ResponseModel> GetAllLessonAsync(string? keyword, int pageIndex, int pageSize);
        Task<ResponseModel> GetAllLessonByChapterIdAsync(string chapterId);
        Task<ResponseModel> GetLessonByIdAsync(string lessonId);
        Task<ResponseModel> CreateLessonAsync(CreateLessonDTO model);
        Task<ResponseModel> UpdateLessonAsync(UpdateLessonDTO model);
        Task<ResponseModel> DeleteLessonAsync(string lessonId);
    }
}
