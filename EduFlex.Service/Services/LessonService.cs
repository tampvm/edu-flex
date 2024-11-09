using AutoMapper;
using EduFlex.Domain.Entities;
using EduFlex.Repository.Common;
using EduFlex.Repository.Interfaces;
using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.LessonViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Services
{
    public class LessonService : ILessonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LessonService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Manage Lesson For Academic Staff
        public async Task<ResponseModel> GetAllLessonAsync(string? keyword, int pageIndex, int pageSize)
        {
            var lessons = await _unitOfWork.LessonRepository.GetAsync(
                                                   filter: l => !l.IsDeleted && (string.IsNullOrEmpty(keyword) || l.Name.Contains(keyword)),
                                                   orderBy: l => l.OrderBy(l => l.Name),
                                                   includeProperties: "Chapter",
                                                   pageIndex: pageIndex,
                                                   pageSize: pageSize);

            var lessonDtos = _mapper.Map<Pagination<ViewListLessonDTO>>(lessons);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Lessons retrieved successfully.",
                Data = lessonDtos
            };
        }

        public async Task<ResponseModel> GetAllLessonByChapterIdAsync(string chapterId)
        {
            var lessons = await _unitOfWork.LessonRepository.GetAllAsync(
                                        filter: l => !l.IsDeleted && l.ChapterId.Equals(chapterId),
                                        orderBy: l => l.OrderBy(l => l.Name),
                                        includeProperties: "Chapter");

            var lessonDtos = _mapper.Map<List<ViewListLessonDTO>>(lessons);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Lessons retrieved successfully.",
                Data = lessonDtos
            };
        }

        public async Task<ResponseModel> GetLessonByIdAsync(string lessonId)
        {
            var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(
                                        id: lessonId,
                                        filter: l => !l.IsDeleted,
                                        includeProperties: "Chapter");

            if (lesson == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Lesson not found."
                };
            }

            var lessonDto = _mapper.Map<ViewListLessonDTO>(lesson);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Lesson retrieved successfully.",
                Data = lessonDto
            };
        }

        public async Task<ResponseModel> CreateLessonAsync(CreateLessonDTO model)
        {
            try
            {
                var existingLesson = await _unitOfWork.LessonRepository.GetLessonByNameAndChapterAsync(model.Name, model.ChapterId);

                if (existingLesson is not null)
                {
                    if (existingLesson.IsDeleted)
                    {
                        existingLesson.Objectives = model.Objectives;
                        existingLesson.Content = model.Content;
                        existingLesson.IsDeleted = false;

                        _unitOfWork.LessonRepository.Update(existingLesson);
                        await _unitOfWork.SaveChangeAsync();

                        return new ResponseModel
                        {
                            Success = true,
                            Message = "Lesson restored successfully."
                        };
                    }

                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Lesson already exists."
                    };
                }

                var existingChapter = await _unitOfWork.ChapterRepository.GetByIdAsync(model.ChapterId);

                if (existingChapter == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Chapter not found."
                    };
                }

                var lesson = _mapper.Map<Lesson>(model);

                await _unitOfWork.LessonRepository.AddAsync(lesson);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Lesson created successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while creating lesson.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ResponseModel> UpdateLessonAsync(UpdateLessonDTO model)
        {
            try
            {
                var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(model.LessonId);

                if (lesson == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Lesson not found."
                    };
                }

                var existingChapter = await _unitOfWork.ChapterRepository.GetByIdAsync(model.ChapterId);

                if (existingChapter == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Chapter not found."
                    };
                }

                _mapper.Map(model, lesson);

                _unitOfWork.LessonRepository.Update(lesson);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Lesson updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while updating lesson.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ResponseModel> DeleteLessonAsync(string lessonId)
        {
            try
            {
                var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(lessonId);

                if (lesson == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Lesson not found."
                    };
                }

                _unitOfWork.LessonRepository.SoftRemove(lesson);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Lesson deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while deleting lesson.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        #endregion
    }
}
