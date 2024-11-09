using AutoMapper;
using EduFlex.Domain.Entities;
using EduFlex.Repository.Common;
using EduFlex.Repository.Interfaces;
using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.SubjectInCurriculumViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Services
{
    public class SubjectInCurriculumService : ISubjectInCurriculumService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubjectInCurriculumService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Manage Subject In Curriculum For Academic Staff
        public async Task<ResponseModel> GetAllSubjectInCurriculumAsync(string? keyword, int pageIndex, int pageSize)
        {
            var subjectsInCurriculum = await _unitOfWork.SubjectInCurriculumRepository.GetAsync(
                                                   filter: s => !s.IsDeleted && (string.IsNullOrEmpty(keyword) || 
                                                                                s.Subject.Name.Contains(keyword) || 
                                                                                s.Grade.Name.Contains(keyword) || 
                                                                                s.Curriculum.Name.Contains(keyword)),
                                                   orderBy: s => s.OrderBy(s => s.Subject.Name),
                                                   includeProperties: "Subject,Grade,Curriculum,Chapters",
                                                   pageIndex: pageIndex,
                                                   pageSize: pageSize);

            var subjectInCurriculumDtos = _mapper.Map<Pagination<ViewListSubjectInCurriculumDTO>>(subjectsInCurriculum);

            // Cập nhật Name để lấy Subject, Grade và chỉ 2 từ đầu tiên từ Curriculum
            foreach (var item in subjectInCurriculumDtos.Items)
            {
                var curriculumWords = !string.IsNullOrWhiteSpace(item.Curriculum) ?
                                      item.Curriculum.Split(' ').Take(2) :
                                      new string[] { };

                item.Name = string.Join(" - ", item.Subject, item.Grade, string.Join(" ", curriculumWords));
            }

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Subjects in curriculum retrieved successfully.",
                Data = subjectInCurriculumDtos
            };
        }

        public async Task<ResponseModel> GetAllSubjectInCurriculumByCurriculumAndGradeAsync(string curriculumIdOrName, string gradeIdOrName)
        {
            var subjectsInCurriculum = await _unitOfWork.SubjectInCurriculumRepository.GetAllAsync(
                                                    filter: s => !s.IsDeleted && (s.CurriculumId == curriculumIdOrName || s.Curriculum.Name == curriculumIdOrName)
                                                                              && (s.GradeId == gradeIdOrName || s.Grade.Name == gradeIdOrName),
                                                    includeProperties: "Subject,Grade,Curriculum,Chapters");

            var subjectInCurriculumDtos = _mapper.Map<List<ViewListSubjectInCurriculumDTO>>(subjectsInCurriculum);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Subjects in curriculum retrieved successfully.",
                Data = subjectInCurriculumDtos
            };
        }

        public async Task<ResponseModel> GetSubjectInCurriculumByIdAsync(string subjectInCurriculumId)
        {
            var subjectInCurriculum = await _unitOfWork.SubjectInCurriculumRepository.GetByIdAsync(
                                                    id: subjectInCurriculumId,
                                                    filter: s => !s.IsDeleted,
                                                    includeProperties: "Subject,Grade,Curriculum,Chapters");

            if (subjectInCurriculum == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Subject in curriculum not found."
                };
            }

            var subjectInCurriculumDto = _mapper.Map<ViewListSubjectInCurriculumDTO>(subjectInCurriculum);

            var curriculumWords = subjectInCurriculumDto.Curriculum.Split(' ').Take(2);
            subjectInCurriculumDto.Name = $"{subjectInCurriculumDto.Subject} - {subjectInCurriculumDto.Grade} - {string.Join(" ", curriculumWords)}";
            
            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Subject in curriculum retrieved successfully.",
                Data = subjectInCurriculumDto
            };
        }

        public async Task<ResponseModel> CreateSubjectInCurriculumAsync(CreateSubjectInCurriculumDTO model)
        {
            try
            {
                // Kiểm tra sự tồn tại của Subject, Grade, và Curriculum
                var validationResult = await ValidateSubjectGradeCurriculumAsync(model.SubjectId, model.GradeId, model.CurriculumId);
                if (!validationResult.Success)
                {
                    return validationResult;
                }

                // Kiểm tra trùng lặp Subject, Grade, Curriculum
                var existingSubjects = await _unitOfWork.SubjectInCurriculumRepository.GetAllAsync(
                    filter: s => s.SubjectId == model.SubjectId
                                 && s.GradeId == model.GradeId
                                 && s.CurriculumId == model.CurriculumId
                );

                if (existingSubjects.Any())
                {
                    // Nếu môn học đã bị xóa mềm, khôi phục nó
                    var subjectInCurriculum = existingSubjects.First();

                    if (subjectInCurriculum.IsDeleted)
                    {
                        subjectInCurriculum.Description = model.Description;
                        subjectInCurriculum.IsDeleted = false;

                        _unitOfWork.SubjectInCurriculumRepository.Update(subjectInCurriculum);
                        await _unitOfWork.SaveChangeAsync();

                        return new ResponseModel
                        {
                            Success = true,
                            Message = "Subject in curriculum restored successfully."
                        };
                    }

                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Subject in curriculum already exists."
                    };
                }

                var newSubjectInCurriculum = _mapper.Map<SubjectInCurriculum>(model);

                await _unitOfWork.SubjectInCurriculumRepository.AddAsync(newSubjectInCurriculum);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Subject in curriculum created successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while creating subject in curriculum.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ResponseModel> UpdateSubjectInCurriculumAsync(UpdateSubjectInCurriculumDTO model)
        {
            try
            {
                var subjectInCurriculum = await _unitOfWork.SubjectInCurriculumRepository.GetByIdAsync(model.SubjectInCurriculumId);

                if (subjectInCurriculum == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Subject in curriculum not found."
                    };
                }

                // Kiểm tra sự tồn tại của Subject, Grade, và Curriculum
                var validationResult = await ValidateSubjectGradeCurriculumAsync(model.SubjectId, model.GradeId, model.CurriculumId);
                if (!validationResult.Success)
                {
                    return validationResult;
                }

                _mapper.Map(model, subjectInCurriculum);

                _unitOfWork.SubjectInCurriculumRepository.Update(subjectInCurriculum);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Subject in curriculum updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while updating subject in curriculum.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ResponseModel> DeleteSubjectInCurriculumAsync(string subjectInCurriculumId)
        {
            try
            {
                var subjectInCurriculum = await _unitOfWork.SubjectInCurriculumRepository.GetByIdAsync(subjectInCurriculumId);

                if (subjectInCurriculum == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Subject in curriculum not found."
                    };
                }

                _unitOfWork.SubjectInCurriculumRepository.SoftRemove(subjectInCurriculum);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Subject in curriculum deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while deleting subject in curriculum.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        private async Task<ResponseModel> ValidateSubjectGradeCurriculumAsync(string subjectId, string gradeId, string curriculumId)
        {
            // Kiểm tra Subject
            var existingSubject = await _unitOfWork.SubjectRepository.GetByIdAsync(subjectId);
            if (existingSubject is null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Subject not found."
                };
            }

            // Kiểm tra Grade
            var existingGrade = await _unitOfWork.GradeRepository.GetByIdAsync(gradeId);
            if (existingGrade is null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Grade not found."
                };
            }

            // Kiểm tra Curriculum
            var existingCurriculum = await _unitOfWork.CurriculumFrameworkRepository.GetByIdAsync(curriculumId);
            if (existingCurriculum is null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Curriculum not found."
                };
            }

            // Nếu tất cả đều tồn tại
            return new ResponseModel
            {
                Success = true
            };
        }
        #endregion
    }
}
