using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.SubjectInCurriculumViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Interfaces
{
    public interface ISubjectInCurriculumService
    {
        Task<ResponseModel> GetAllSubjectInCurriculumAsync(string? keyword, int pageIndex, int pageSize);
        Task<ResponseModel> GetAllSubjectInCurriculumByCurriculumAndGradeAsync(string curriculumIdOrName, string gradeIdOrName);        
        Task<ResponseModel> GetSubjectInCurriculumByIdAsync(string subjectInCurriculumId);
        Task<ResponseModel> CreateSubjectInCurriculumAsync(CreateSubjectInCurriculumDTO model);
        Task<ResponseModel> UpdateSubjectInCurriculumAsync(UpdateSubjectInCurriculumDTO model);
        Task<ResponseModel> DeleteSubjectInCurriculumAsync(string subjectInCurriculumId);
    }
}
