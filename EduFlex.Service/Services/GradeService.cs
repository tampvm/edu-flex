using AutoMapper;
using EduFlex.Domain.Entities;
using EduFlex.Repository.Common;
using EduFlex.Repository.Interfaces;
using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.GradeViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Services
{
    public class GradeService : IGradeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GradeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Manage Grade For Admin
        public async Task<ResponseModel> GetAllGradeAsync(string? keyword, int pageIndex, int pageSize)
        {
            var grades = await _unitOfWork.GradeRepository.GetAsync(
                                                   filter: g => !g.IsDeleted && (string.IsNullOrEmpty(keyword) || g.Name.Contains(keyword)),
                                                   orderBy: g => g.OrderBy(g => g.Name),
                                                   pageIndex: pageIndex,
                                                   pageSize: pageSize);

            var gradeDtos = _mapper.Map<Pagination<ViewListGradeDTO>>(grades);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Grades retrieved successfully.",
                Data = gradeDtos
            };
        }

        public async Task<ResponseModel> GetGradeByIdAsync(string gradeId)
        {
            var grade = await _unitOfWork.GradeRepository.GetByIdAsync(gradeId);

            if (grade == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Grade not found."
                };
            }

            var gradeDto = _mapper.Map<ViewListGradeDTO>(grade);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Grade retrieved successfully.",
                Data = gradeDto
            };
        }

        public async Task<ResponseModel> CreateGradeAsync(CreateGradeDTO model)
        {
            try
            {
                var existingGrade = await _unitOfWork.GradeRepository.GradeExistsAsync(model.Name);

                if (existingGrade)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Grade already exists.",
                    };
                }

                var grade = _mapper.Map<Grade>(model);

                await _unitOfWork.GradeRepository.AddAsync(grade);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Grade added successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while adding grade.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ResponseModel> UpdateGradeAsync(UpdateGradeDTO model)
        {
            try
            {
                var grade = await _unitOfWork.GradeRepository.GetByIdAsync(model.GradeId);

                if (grade == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Grade not found."
                    };
                }

                var updatedGrade = _mapper.Map(model, grade);

                _unitOfWork.GradeRepository.Update(updatedGrade);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Grade updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while updating grade.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ResponseModel> DeleteGradeAsync(string gradeId)
        {
            try
            {
                var grade = await _unitOfWork.GradeRepository.GetByIdAsync(gradeId);

                if (grade == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Grade not found."
                    };
                }

                if (grade.IsDeleted)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Cannot delete a deleted grade."
                    };
                }

                _unitOfWork.GradeRepository.SoftRemove(grade);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Grade deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while deleting grade.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        #endregion
    }
}
