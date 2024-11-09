using AutoMapper;
using EduFlex.Domain.Entities;
using EduFlex.Repository.Common;
using EduFlex.Repository.Interfaces;
using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.ChapterViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Services
{
    public class ChapterService : IChapterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ChapterService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Manage Chapter For Academic Staff
        public async Task<ResponseModel> GetAllChapterAsync(string? keyword, int pageIndex, int pageSize)
        {
            var chapters = await _unitOfWork.ChapterRepository.GetAsync(
                                        filter: c => !c.IsDeleted && (string.IsNullOrEmpty(keyword) || c.Name.Contains(keyword)),
                                        orderBy: c => c.OrderBy(c => c.Name),
                                        includeProperties: "SubjectInCurriculum.Subject,SubjectInCurriculum.Grade,SubjectInCurriculum.Curriculum,Lessons",
                                        pageIndex: pageIndex,
                                        pageSize: pageSize);

            var chapterDtos = _mapper.Map<Pagination<ViewListChapterDTO>>(chapters);

            // Cập nhật SubjectInCurriculum cho từng DTO
            foreach (var chapterDto in chapterDtos.Items)
            {
                // Lấy thông tin từ SubjectInCurriculum của từng Chapter
                var subjectInCurriculum = chapters.Items.FirstOrDefault(c => c.ChapterId == chapterDto.ChapterId)?.SubjectInCurriculum;

                // Kiểm tra nếu SubjectInCurriculum không rỗng
                if (subjectInCurriculum != null)
                {
                    // Lấy 2 từ đầu tiên từ Curriculum.Name
                    var curriculumWords = subjectInCurriculum.Curriculum.Name.Split(' ').Take(2);

                    // Tạo chuỗi Subject - Grade - 2 từ Curriculum
                    chapterDto.SubjectInCurriculum = $"{subjectInCurriculum.Subject.Name} - {subjectInCurriculum.Grade.Name} - {string.Join(" ", curriculumWords)}";
                }
            }

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Chapters retrieved successfully.",
                Data = chapterDtos
            };
        }

        public async Task<ResponseModel> GetAllChapterBySubjectInCurriculumIdAsync(string subjectInCurriculumId)
        {
            var chapters = await _unitOfWork.ChapterRepository.GetAllAsync(
                                        filter: c => !c.IsDeleted && c.SubjectInCurriculumId.Equals(subjectInCurriculumId),
                                        orderBy: c => c.OrderBy(c => c.Name),
                                        includeProperties: "SubjectInCurriculum.Subject,SubjectInCurriculum.Grade,SubjectInCurriculum.Curriculum,Lessons");

            var chapterDtos = _mapper.Map<List<ViewListChapterDTO>>(chapters);

            // Cập nhật SubjectInCurriculum cho từng DTO
            foreach (var chapterDto in chapterDtos)
            {
                // Lấy thông tin từ SubjectInCurriculum của từng Chapter
                var subjectInCurriculum = chapters.FirstOrDefault(c => c.ChapterId == chapterDto.ChapterId)?.SubjectInCurriculum;

                // Kiểm tra nếu SubjectInCurriculum không rỗng
                if (subjectInCurriculum != null)
                {
                    // Lấy 2 từ đầu tiên từ Curriculum.Name
                    var curriculumWords = subjectInCurriculum.Curriculum.Name.Split(' ').Take(2);

                    // Tạo chuỗi Subject - Grade - 2 từ Curriculum
                    chapterDto.SubjectInCurriculum = $"{subjectInCurriculum.Subject.Name} - {subjectInCurriculum.Grade.Name} - {string.Join(" ", curriculumWords)}";
                }
            }

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Chapters retrieved successfully.",
                Data = chapterDtos
            };
        }

        public async Task<ResponseModel> GetChapterByIdAsync(string chapterId)
        {
            var chapter = await _unitOfWork.ChapterRepository.GetByIdAsync(
                                        id: chapterId,
                                        filter: c => !c.IsDeleted,
                                        includeProperties: "SubjectInCurriculum.Subject,SubjectInCurriculum.Grade,SubjectInCurriculum.Curriculum,Lessons");

            if (chapter == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Chapter not found."
                };
            }

            var chapterDto = _mapper.Map<ViewListChapterDTO>(chapter);

            // Lấy thông tin từ SubjectInCurriculum của từng Chapter
            var subjectInCurriculum = chapter.SubjectInCurriculum;

            // Kiểm tra nếu SubjectInCurriculum không rỗng
            if (subjectInCurriculum != null)
            {
                // Lấy 2 từ đầu tiên từ Curriculum.Name
                var curriculumWords = subjectInCurriculum.Curriculum.Name.Split(' ').Take(2);

                // Tạo chuỗi Subject - Grade - 2 từ Curriculum
                chapterDto.SubjectInCurriculum = $"{subjectInCurriculum.Subject.Name} - {subjectInCurriculum.Grade.Name} - {string.Join(" ", curriculumWords)}";
            }

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Chapter retrieved successfully.",
                Data = chapterDto
            };
        }

        public async Task<ResponseModel> CreateChapterAsync(CreateChapterDTO model)
        {
            try
            {
                var existingChapter = await _unitOfWork.ChapterRepository.GetChapterByNameAndSubjectInCurriculumAsync(model.Name, model.SubjectInCurriculumId);

                if (existingChapter is not null)
                {
                    if (existingChapter.IsDeleted)
                    {
                        existingChapter.Description = model.Description;
                        existingChapter.IsDeleted = false;

                        _unitOfWork.ChapterRepository.Update(existingChapter);
                        await _unitOfWork.SaveChangeAsync();

                        return new ResponseModel
                        {
                            Success = true,
                            Message = "Chapter restored successfully."
                        };
                    }

                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Chapter already exists."
                    };
                }

                var existingSubjectInCurriculum = await _unitOfWork.SubjectInCurriculumRepository.GetByIdAsync(model.SubjectInCurriculumId);

                if (existingSubjectInCurriculum is null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Subject in curriculum not found."
                    };
                }

                var chapter = _mapper.Map<Chapter>(model);

                await _unitOfWork.ChapterRepository.AddAsync(chapter);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Chapter created successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while creating chapter.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ResponseModel> UpdateChapterAsync(UpdateChapterDTO model)
        {
            try
            {
                var chapter = await _unitOfWork.ChapterRepository.GetByIdAsync(model.ChapterId);

                if (chapter is null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Chapter not found."
                    };
                }

                var existingSubjectInCurriculum = await _unitOfWork.SubjectInCurriculumRepository.GetByIdAsync(model.SubjectInCurriculumId);

                if (existingSubjectInCurriculum is null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Subject in curriculum not found."
                    };
                }

                _mapper.Map(model, chapter);

                _unitOfWork.ChapterRepository.Update(chapter);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Chapter updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while updating chapter.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ResponseModel> DeleteChapterAsync(string chapterId)
        {
            try
            {
                var chapter = await _unitOfWork.ChapterRepository.GetByIdAsync(chapterId);

                if (chapter is null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Chapter not found."
                    };
                }

                _unitOfWork.ChapterRepository.SoftRemove(chapter);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Chapter deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while deleting chapter.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        #endregion
    }
}
