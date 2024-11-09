using AutoMapper;
using EduFlex.Domain.Entities;
using EduFlex.Repository.Common;
using EduFlex.Repository.Interfaces;
using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.SubjectViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Manage Subject For Admin
        public async Task<ResponseModel> GetAllSubjectAsync(string? keyword, int pageIndex, int pageSize)
        {
            var subjects = await _unitOfWork.SubjectRepository.GetAsync(
                                    filter: s => !s.IsDeleted && s.IsApproved && (string.IsNullOrEmpty(keyword) || s.Name.Contains(keyword)),
                                    orderBy: s => s.OrderBy(s => s.Name),
                                    pageIndex: pageIndex,
                                    pageSize: pageSize);

            var subjectDtos = _mapper.Map<Pagination<ViewListSubjectDTO>>(subjects);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Subjects retrieved successfully.",
                Data = subjectDtos
            };
        }

        public async Task<ResponseModel> GetSubjectByIdAsync(string subjectId)
        {
            var subject = await _unitOfWork.SubjectRepository.GetByIdAsync(
                                    id: subjectId,
                                    filter: s => !s.IsDeleted && s.IsApproved);

            if (subject is null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Subject not found."
                };
            }

            var subjectDto = _mapper.Map<ViewListSubjectDTO>(subject);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Subject retrieved successfully.",
                Data = subjectDto
            };
        }

        public async Task<ResponseModel> CreateSubjectAsync(CreateSubjectDTO model)
        {
            try
            {
                var existingSubject = await _unitOfWork.SubjectRepository.SubjectExistsAsync(model.Name);

                if (existingSubject is not null)
                {
                    if (existingSubject.IsDeleted)
                    {
                        existingSubject.Description = model.Description;
                        existingSubject.IsDeleted = false;

                        _unitOfWork.SubjectRepository.Update(existingSubject);
                        await _unitOfWork.SaveChangeAsync();

                        return new ResponseModel
                        {
                            Success = true,
                            Message = "Subject restored successfully.",
                        };
                    }

                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Subject already exists."
                    };
                }

                var subject = _mapper.Map<Subject>(model);

                await _unitOfWork.SubjectRepository.AddAsync(subject);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Subject created successfully.",
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while creating the subject.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ResponseModel> UpdateSubjectAsync(UpdateSubjectDTO model)
        {
            try
            {
                var subject = await _unitOfWork.SubjectRepository.GetByIdAsync(
                                            id: model.SubjectId,
                                            filter: s => !s.IsDeleted && s.IsApproved);
                if (subject is null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Subject not found."
                    };
                }

                var updatedSubject = _mapper.Map(model, subject);

                _unitOfWork.SubjectRepository.Update(updatedSubject);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Subject updated successfully.",
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while updating the subject.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ResponseModel> DeleteSubjectAsync(string subjectId)
        {
            try
            {
                var subject = await _unitOfWork.SubjectRepository.GetByIdAsync(
                                            id: subjectId,
                                            filter: s => s.IsApproved);

                if (subject is null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Subject not found."
                    };
                }

                if (subject.IsDeleted)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Subject already deleted."
                    };
                }

                _unitOfWork.SubjectRepository.SoftRemove(subject);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Subject deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while deleting the subject.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        #endregion

        #region Suggested Subject
        public async Task<ResponseModel> GetAllSuggestedSubjectAsync(string? keyword, int pageIndex, int pageSize)
        {
            var suggestedSubjects = await _unitOfWork.SubjectRepository.GetAsync(
                                                   filter: s => !s.IsDeleted && !s.IsApproved && (string.IsNullOrEmpty(keyword) || s.Name.Contains(keyword)),
                                                   orderBy: s => s.OrderBy(s => s.CreatedAt),
                                                   pageIndex: pageIndex,
                                                   pageSize: pageSize);

            var suggestedSubjectDtos = _mapper.Map<Pagination<ViewListSuggestedSubjectDTO>>(suggestedSubjects);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Suggested subjects retrieved successfully.",
                Data = suggestedSubjectDtos
            };
        }

        public async Task<ResponseModel> CreateSuggestedSubjectAsync(CreateSuggestedSubjectDTO model)
        {
            try
            {
                var existingSubject = await _unitOfWork.SubjectRepository.SubjectExistsAsync(model.Name);

                if (existingSubject is not null)
                {
                    if (existingSubject.IsDeleted)
                    {
                        existingSubject.Description = model.Description;
                        existingSubject.IsApproved = false;
                        existingSubject.IsDeleted = false;

                        _unitOfWork.SubjectRepository.Update(existingSubject);
                        await _unitOfWork.SaveChangeAsync();

                        return new ResponseModel
                        {
                            Success = true,
                            Message = "Suggested subject add successfully."
                        };
                    }

                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Subject already exists."
                    };
                }

                var suggestedSubject = _mapper.Map<Subject>(model);

                await _unitOfWork.SubjectRepository.AddAsync(suggestedSubject);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Suggested subject added successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while adding the suggested subject.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ResponseModel> ApproveSuggestedSubjectAsync(string subjectId)
        {
            try
            {
                var suggestedSubject = await _unitOfWork.SubjectRepository.GetByIdAsync(
                                                id: subjectId,
                                                filter: s => !s.IsApproved);

                if (suggestedSubject is null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Suggested subject not found."
                    };
                }

                if (suggestedSubject.IsDeleted)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Cannot approve a deleted suggested subject."
                    };
                }

                suggestedSubject.IsApproved = true;
                _unitOfWork.SubjectRepository.Update(suggestedSubject);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Suggested subject approved successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while approving the suggested subject.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ResponseModel> DeleteSuggestedSubjectAsync(string subjectId)
        {
            try
            {
                var suggestedSubject = await _unitOfWork.SubjectRepository.GetByIdAsync(
                                                                   id: subjectId,
                                                                   filter: s => !s.IsDeleted && !s.IsApproved);
                
                if (suggestedSubject is null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Suggested subject not found."
                    };
                }

                _unitOfWork.SubjectRepository.Delete(suggestedSubject);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Suggested subject deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while deleting the suggested subject.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        #endregion
    }
}
