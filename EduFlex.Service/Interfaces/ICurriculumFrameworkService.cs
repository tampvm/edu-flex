using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.CurriculumViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Interfaces
{
    public interface ICurriculumFrameworkService
    {
        Task<ResponseModel> GetAllCurriculumFrameworkAsync(string? keyword, int pageIndex, int pageSize);
        Task<ResponseModel> GetCurriculumFrameworkByIdAsync(string curriculumFrameworkId);
        Task<ResponseModel> CreateCurriculumFrameworkAsync(CreateCurriculumDTO model);
        Task<ResponseModel> UpdateCurriculumFrameworkAsync(UpdateCurriculumDTO model);
        Task<ResponseModel> DeleteCurriculumFrameworkAsync(string curriculumFrameworkId);
        Task<ResponseModel> GetAllSuggestedCurriculumAsync(string? keyword, int pageIndex, int pageSize);
        Task<ResponseModel> CreateSuggestedCurriculumAsync(CreateSuggestedCurriculumDTO model);
        Task<ResponseModel> ApproveSuggestedCurriculumAsync(string curriculumId);
        Task<ResponseModel> DeleteSuggestedCurriculumAsync(string curriculumId);
    }
}
