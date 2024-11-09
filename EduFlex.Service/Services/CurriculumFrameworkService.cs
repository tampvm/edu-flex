using AutoMapper;
using EduFlex.Domain.Entities;
using EduFlex.Repository.Common;
using EduFlex.Repository.Interfaces;
using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.CurriculumViewModels;
using EduFlex.Service.Models.ViewModels.SubjectViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Services
{
    public class CurriculumFrameworkService : ICurriculumFrameworkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CurriculumFrameworkService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Manage Curriculum For Admin
        public async Task<ResponseModel> GetAllCurriculumFrameworkAsync(string? keyword, int pageIndex, int pageSize)
        {
            var curriculumFrameworks = await _unitOfWork.CurriculumFrameworkRepository.GetAsync(
                                                   filter: c => !c.IsDeleted && c.IsApproved && (string.IsNullOrEmpty(keyword) || c.Name.Contains(keyword)),
                                                   orderBy: c => c.OrderBy(c => c.Name),
                                                   pageIndex: pageIndex,
                                                   pageSize: pageSize);

            var curriculumFrameworkDtos = _mapper.Map<Pagination<ViewListCurriculumDTO>>(curriculumFrameworks);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Curriculum Frameworks retrieved successfully.",
                Data = curriculumFrameworkDtos
            };
        }

        public async Task<ResponseModel> GetCurriculumFrameworkByIdAsync(string curriculumFrameworkId)
        {
            var curriculumFramework = await _unitOfWork.CurriculumFrameworkRepository.GetByIdAsync(
                                                    id: curriculumFrameworkId,
                                                    filter: c => !c.IsDeleted && c.IsApproved);

            if (curriculumFramework is null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Curriculum Framework not found."
                };
            }

            var curriculumFrameworkDto = _mapper.Map<ViewListCurriculumDTO>(curriculumFramework);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Curriculum Framework retrieved successfully.",
                Data = curriculumFrameworkDto
            };
        }

        public async Task<ResponseModel> CreateCurriculumFrameworkAsync(CreateCurriculumDTO model)
        {
            try
            {
                var existingCurriculumFramework = await _unitOfWork.CurriculumFrameworkRepository.CurriculumFrameworkExistsAsync(model.Name);

                if (existingCurriculumFramework is not null)
                {
                    if (existingCurriculumFramework.IsDeleted)
                    {
                        existingCurriculumFramework.Description = model.Description;
                        existingCurriculumFramework.IsDeleted = false;

                        _unitOfWork.CurriculumFrameworkRepository.Update(existingCurriculumFramework);
                        await _unitOfWork.SaveChangeAsync();

                        return new ResponseModel
                        {
                            Success = true,
                            Message = "Curriculum Framework restored successfully.",
                        };
                    }

                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Curriculum Framework already exists."
                    };
                }

                var curriculumFramework = _mapper.Map<CurriculumFramework>(model);

                await _unitOfWork.CurriculumFrameworkRepository.AddAsync(curriculumFramework);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Curriculum Framework added successfully.",
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while creating the curriculum framework.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ResponseModel> UpdateCurriculumFrameworkAsync(UpdateCurriculumDTO model)
        {
            try
            {
                var curriculumFramework = await _unitOfWork.CurriculumFrameworkRepository.GetByIdAsync(
                                                        id: model.CurriculumId,
                                                        filter: c => !c.IsDeleted && c.IsApproved);

                if (curriculumFramework is null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Curriculum Framework not found."
                    };
                }

                var updatedCurriculumFramework = _mapper.Map(model, curriculumFramework);

                _unitOfWork.CurriculumFrameworkRepository.Update(updatedCurriculumFramework);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Curriculum Framework updated successfully.",
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while updating the curriculum framework.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ResponseModel> DeleteCurriculumFrameworkAsync(string curriculumFrameworkId)
        {
            var curriculumFramework = await _unitOfWork.CurriculumFrameworkRepository.GetByIdAsync(
                                                    id: curriculumFrameworkId,
                                                    filter: c => c.IsApproved);

            if (curriculumFramework is null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Curriculum Framework not found."
                };
            }

            if (curriculumFramework.IsDeleted)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Curriculum Framework already deleted."
                };
            }

            _unitOfWork.CurriculumFrameworkRepository.SoftRemove(curriculumFramework);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel
            {
                Success = true,
                Message = "Curriculum Framework deleted successfully.",
            };
        }
        #endregion

        #region Suggested Curriculum For Admin
        public async Task<ResponseModel> GetAllSuggestedCurriculumAsync(string? keyword, int pageIndex, int pageSize)
        {
            var suggestedCurriculum = await _unitOfWork.CurriculumFrameworkRepository.GetAsync(
                                                           filter: c => !c.IsDeleted && !c.IsApproved && (string.IsNullOrEmpty(keyword) || c.Name.Contains(keyword)),
                                                           orderBy: c => c.OrderBy(c => c.Name),
                                                           pageIndex: pageIndex,
                                                           pageSize: pageSize);

            var suggestedCurriculumDtos = _mapper.Map<Pagination<ViewListSuggestedCurriculumDTO>>(suggestedCurriculum);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Suggested Curriculum retrieved successfully.",
                Data = suggestedCurriculumDtos
            };
        }

        public async Task<ResponseModel> CreateSuggestedCurriculumAsync(CreateSuggestedCurriculumDTO model)
        {
            try
            {
                var existingCurriculum = await _unitOfWork.CurriculumFrameworkRepository.CurriculumFrameworkExistsAsync(model.Name);

                if (existingCurriculum is not null)
                {
                    if (existingCurriculum.IsDeleted)
                    {
                        existingCurriculum.Description = model.Description;
                        existingCurriculum.IsApproved = false;
                        existingCurriculum.IsDeleted = false;

                        _unitOfWork.CurriculumFrameworkRepository.Update(existingCurriculum);
                        await _unitOfWork.SaveChangeAsync();

                        return new ResponseModel
                        {
                            Success = true,
                            Message = "Suggested curriculum add successfully."
                        };
                    }

                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Curriculum already exists."
                    };
                }

                var suggestedCurriculum = _mapper.Map<CurriculumFramework>(model);

                await _unitOfWork.CurriculumFrameworkRepository.AddAsync(suggestedCurriculum);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Suggested curriculum added successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while adding the suggested curriculum.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ResponseModel> ApproveSuggestedCurriculumAsync(string curriculumId)
        {
            try
            {
                var suggestedCurriculum = await _unitOfWork.CurriculumFrameworkRepository.GetByIdAsync(
                                                id: curriculumId,
                                                filter: s => !s.IsApproved);

                if (suggestedCurriculum is null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Suggested curriculum not found."
                    };
                }

                if (suggestedCurriculum.IsDeleted)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Cannot approve a deleted suggested curriculum."
                    };
                }

                suggestedCurriculum.IsApproved = true;
                _unitOfWork.CurriculumFrameworkRepository.Update(suggestedCurriculum);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Suggested curriculum approved successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while approving the suggested curriculum.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ResponseModel> DeleteSuggestedCurriculumAsync(string curriculumId)
        {
            try
            {
                var suggestedCurriculum = await _unitOfWork.CurriculumFrameworkRepository.GetByIdAsync(
                                                                   id: curriculumId,
                                                                   filter: s => !s.IsDeleted && !s.IsApproved);

                if (suggestedCurriculum is null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Suggested curriculum not found."
                    };
                }

                _unitOfWork.CurriculumFrameworkRepository.Delete(suggestedCurriculum);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Suggested curriculum deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while deleting the suggested curriculum.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        #endregion
    }
}
