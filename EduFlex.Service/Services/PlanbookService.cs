using AutoMapper;
using EduFlex.Domain.Entities;
using EduFlex.Repository.Common;
using EduFlex.Repository.Interfaces;
using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.ActivityViewModels;
using EduFlex.Service.Models.ViewModels.PlanbookViewModels;
using EduFlex.Service.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Services
{
	public class PlanbookService : IPlanbookService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
        private readonly IOpenAIService _openAIService;

        public PlanbookService(IUnitOfWork unitOfWork, IMapper mapper, IOpenAIService openAIService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
            _openAIService = openAIService;
        }

		#region Get All Planbooks
		public async Task<ResponseModel> GetAllPlanbooksAsync(int pageIndex, int pageSize)
		{
			var planbooks = await _unitOfWork.PlanbookRepository.GetAsync(
							filter: r => r.IsDeleted == false,
							pageIndex: pageIndex,
							pageSize: pageSize
							);
			var mappers = _mapper.Map<Pagination<ViewListPlanbookDTO>>(planbooks);
			foreach (var item in mappers.Items)
			{
				var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(item.LessonId);
				if (lesson != null)
				{
					item.LessonName = lesson.Name;
				}
			}

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Planbooks retrieved successfully.",
				Data = mappers
			};
		}
		#endregion

		#region Get Planbook By Id
		public async Task<ResponseModel> GetPlanbookByIdAsync(string planbookId)
		{
			var planbook = await _unitOfWork.PlanbookRepository.GetByIdAsync(planbookId);
			if (planbook == null)
			{
				return new ResponseModel
				{
					Success = false,
					Message = "Planbook not found."
				};
			}

			var mapper = _mapper.Map<ViewDetailPlanbookDTO>(planbook);

			var collection = await _unitOfWork.PlanbookCollectionRepository.GetByIdAsync(planbook.CollectionId);
			if (collection != null)
			{
				mapper.CollectionName = collection.CollectionName;
			}

			var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(planbook.LessonId);
			if (lesson != null)
			{
				mapper.LessonName = lesson.Name;
			}

			var activities = await _unitOfWork.ActivityRepository.GetByPlanbookIdAsync(planbookId);
			mapper.Activities = _mapper.Map<List<ViewListActivityDTO>>(activities);

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Planbook retrieved successfully.",
				Data = mapper
			};
		}
		#endregion

		#region Get Planbook By Collection Id
		public async Task<ResponseModel> GetPlanbookByCollectionIdAsync(string collectionId, int pageIndex, int pageSize)
		{
			var planbooks = await _unitOfWork.PlanbookRepository.GetAsync(
							filter: r => r.CollectionId == collectionId && r.IsDeleted == false,
							pageIndex: pageIndex,
							pageSize: pageSize
							);
			var mappers = _mapper.Map<Pagination<ViewListPlanbookDTO>>(planbooks);
			foreach (var item in mappers.Items)
			{
				var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(item.LessonId);
				if (lesson != null)
				{
					item.LessonName = lesson.Name;
				}
			}

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Planbooks retrieved successfully.",
				Data = mappers
			};
		}
		#endregion

		#region Get Planbook By Lesson Id
		public async Task<ResponseModel> GetPlanbookByLessonIdAsync(string lessonId, int pageIndex, int pageSize)
		{
			var planbooks = await _unitOfWork.PlanbookRepository.GetAsync(
							filter: r => !r.IsDeleted && r.LessonId == lessonId,
							pageIndex: pageIndex,
							pageSize: pageSize
							);
			var mappers = _mapper.Map<Pagination<ViewListPlanbookDTO>>(planbooks);
			foreach (var item in mappers.Items)
			{
				var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(item.LessonId);
				if (lesson != null)
				{
					item.LessonName = lesson.Name;
				}
			}

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Planbooks retrieved successfully.",
				Data = mappers
			};
		}
		#endregion

		// ham nay chua lam xong
		#region Get Planbook By User Id 
		public async Task<ResponseModel> GetPlanbookByUserIdAsync(string userId, int pageIndex, int pageSize)
		{
			var planbooks = await _unitOfWork.PlanbookRepository.GetAsync(
							//filter: r => r.UserId == userId && r.IsDeleted == false, 
							pageIndex: pageIndex,
							pageSize: pageSize
							);
			var mappers = _mapper.Map<Pagination<ViewListPlanbookDTO>>(planbooks);
			foreach (var item in mappers.Items)
			{
				var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(item.LessonId);
				if (lesson != null)
				{
					item.LessonName = lesson.Name;
				}
			}

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Planbooks retrieved successfully.",
				Data = mappers
			};
		}
        #endregion

        #region Create Planbook
        //public async Task<ResponseModel> CreatePlanbookAsync(CreatePlanbookDTO model)
        //{
        //	try
        //	{
        //		var planbookDto = _mapper.Map<Planbook>(model);
        //		await _unitOfWork.PlanbookRepository.AddAsync(planbookDto);
        //		await _unitOfWork.SaveChangeAsync();

        //		if (model.Activities != null && model.Activities.Any())
        //		{
        //			var activities = _mapper.Map<List<Activity>>(model.Activities);
        //			foreach (var activity in activities)
        //			{
        //				activity.PlanbookId = planbookDto.PlanbookId;
        //				await _unitOfWork.ActivityRepository.CreateActivityAsync(activity);
        //			}
        //			await _unitOfWork.SaveChangeAsync();
        //		}

        //		return new ResponseModel
        //		{
        //			Success = true,
        //			Message = "Planbook created successfully."
        //		};
        //	}
        //	catch (Exception ex)
        //	{
        //		return new ErrorResponseModel<string>
        //		{
        //			Success = false,
        //			Message = "An error occurred while creating the planbookDto.",
        //			Errors = new List<string> { ex.Message }
        //		};
        //	}
        //}

        public async Task<ResponseModel> CreatePlanbookAsync(CreatePlanbookDTO model)
        {
            try
            {
                // Kiểm tra lesson có tồn tại không
                var existingLesson = await _unitOfWork.LessonRepository.GetByIdAsync(model.LessonId);

                if (existingLesson is null)
				{
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Lesson not found."
                    };
                }

                // Kiểm tra collection có tồn tại không
                if (!string.IsNullOrEmpty(model.CollectionId))
				{
                    var existingCollection = await _unitOfWork.PlanbookCollectionRepository.GetByIdAsync(model.CollectionId);

                    if (existingCollection is null)
                    {
                        return new ResponseModel
                        {
                            Success = false,
                            Message = "Collection not found."
                        };
                    }
                }

                // Tạo planbookDto
                var planbook = _mapper.Map<Planbook>(model);

                await _unitOfWork.PlanbookRepository.AddAsync(planbook);
                //await _unitOfWork.SaveChangeAsync();

                if (model.Activities != null && model.Activities.Any())
                {
                    var activities = _mapper.Map<List<Activity>>(model.Activities);
					int index = 1; // tự động tạo index cho activity, không cần truyền index trong DTO
                    
					foreach (var activity in activities)
                    {
                        activity.PlanbookId = planbook.PlanbookId;
                        activity.Index = index++;
                    }

                    await _unitOfWork.ActivityRepository.CreateRangeActivityAsync(activities);
                    //await _unitOfWork.SaveChangeAsync();
                }

                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Planbook created successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while creating the planbook.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
		#endregion

		#region Update Planbook
		public async Task<ResponseModel> UpdatePlanbookAsync(UpdatePlanbookDTO model)
		{
			try
			{
                // Kiểm tra planbookDto có tồn tại không
                var planbook = await _unitOfWork.PlanbookRepository.GetByIdAsync(model.PlanbookId);
				if (planbook is null)
				{
					return new ResponseModel
					{
						Success = false,
						Message = "Planbook not found."
					};
				}

                // Cập nhật Planbook
                _mapper.Map(model, planbook);
                _unitOfWork.PlanbookRepository.Update(planbook);

                if (model.Activities != null && model.Activities.Any())
                {
                    var existingActivities = await _unitOfWork.ActivityRepository.GetByPlanbookIdAsync(planbook.PlanbookId);

                    // Cập nhật activity trong planbookDto sẽ có 3 trường hợp:
                    // 1. Cập nhật thông tin cho các activity đã tồn tại
                    // 2. Thêm mới các activity
                    // 3. Có thể xóa activity đã tồn tại

                    // Tạo 1 list chứa các hoạt động cần cập nhật và một list chứa các hoạt động mới cần thêm
                    var activitiesToUpdate = new List<Activity>();
                    var newActivities = new List<Activity>();

                    // Tạo một HashSet chứa các UserPackageId của các hoạt động cần giữ lại
                    var activityIdsToKeep = model.Activities.Select(a => a.ActivityId).ToHashSet();

                    // Lọc ra các hoạt động không còn trong danh sách cần giữ
                    var activitiesToDelete = existingActivities
                        .Where(a => !activityIdsToKeep.Contains(a.ActivityId))
                        .ToList();

                    // Xác định index tiếp theo dựa trên hoạt động có Index cao nhất
                    int nextIndex = existingActivities.Any()
                        ? existingActivities.Max(a => a.Index) + 1
                        : 0;

                    foreach (var activityDto in model.Activities)
                    {
                        // Kiểm tra nếu hoạt động đã tồn tại
                        var existingActivity = existingActivities.FirstOrDefault(a => a.ActivityId == activityDto.ActivityId);
                        if (existingActivity != null)
                        {
                            // Map thông tin từ DTO vào hoạt động đã tồn tại và thêm vào list cần cập nhật
                            _mapper.Map(activityDto, existingActivity);
                            activitiesToUpdate.Add(existingActivity);
                        }
                        else
                        {
                            // Tạo hoạt động mới và gán Index tiếp theo
                            var newActivity = _mapper.Map<Activity>(activityDto);
                            newActivity.PlanbookId = planbook.PlanbookId;
                            newActivity.Index = nextIndex++;
                            newActivities.Add(newActivity);
                        }
                    }

                    // Xóa các hoạt động không còn trong danh sách
                    if (activitiesToDelete.Any())
                    {
                        _unitOfWork.ActivityRepository.DeleteRangeActivityAsync(activitiesToDelete);
                    }

                    // Cập nhật và thêm mới các hoạt động
                    if (activitiesToUpdate.Any())
                    {
                        _unitOfWork.ActivityRepository.UpdateRangeActivityAsync(activitiesToUpdate);
                    }
                    if (newActivities.Any())
                    {
                        await _unitOfWork.ActivityRepository.CreateRangeActivityAsync(newActivities);
                    }
                }

                // Lưu tất cả các thay đổi
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
				{
					Success = true,
					Message = "Planbook updated successfully."
				};
			}
			catch (Exception ex)
			{
				return new ErrorResponseModel<string>
				{
					Success = false,
					Message = "An error occurred while updating the planbook.",
					Errors = new List<string> { ex.Message }
				};
			}
		}
        #endregion

        #region Delete Planbook
        public async Task<ResponseModel> DeletePlanbookAsync(string planbookId)
        {
            try
            {
                var planbook = await _unitOfWork.PlanbookRepository.GetByIdAsync(planbookId);
                if (planbook == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Planbook not found."
                    };
                }

                _unitOfWork.PlanbookRepository.Delete(planbook);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Planbook deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while deleting the planbook.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        #endregion

        #region Soft Remove Planbook
        public async Task<ResponseModel> SoftRemovePlanbookAsync(string planbookId)
		{
			try
			{
				var planbook = await _unitOfWork.PlanbookRepository.GetByIdAsync(planbookId);
				if (planbook == null)
				{
					return new ResponseModel
					{
						Success = false,
						Message = "Planbook not found."
					};
				}

				_unitOfWork.PlanbookRepository.SoftRemove(planbook);
				await _unitOfWork.SaveChangeAsync();

				return new ResponseModel
				{
					Success = true,
					Message = "Planbook deleted successfully."
				};
			}
			catch (Exception ex)
			{
				return new ErrorResponseModel<string>
				{
					Success = false,
					Message = "An error occurred while deleting the planbook.",
					Errors = new List<string> { ex.Message }
				};
			}
		}
        #endregion

        #region Create Planbook From Template
        public async Task<ResponseModel> GetPlanbookFromTemplateAsync(string lessonId)
        {
            // Lấy danh sách các planbookDto mặc định cho bài học
            var defaultPlanbooks = await _unitOfWork.PlanbookRepository.GetAllAsync(
                                                filter: p => p.IsDefault && p.LessonId.Equals(lessonId),
                                                includeProperties: "PlanbookCollection,Lesson,Activities");

            if (defaultPlanbooks is null || !defaultPlanbooks.Any())
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "No default planbooks found for the specified lesson."
                };
            }

            // Chọn một planbookDto ngẫu nhiên từ danh sách
            var random = new Random();
            var templatePlanbook = defaultPlanbooks.ElementAt(random.Next(defaultPlanbooks.Count()));

            // Map thông tin planbookDto và các hoạt động từ planbookDto mẫu
            var planbookDto = _mapper.Map<CreatePlanbookDTO>(templatePlanbook);
            planbookDto.IsDefault = false;
            var activities = templatePlanbook.Activities.OrderBy(a => a.Index).ToList(); // Sắp xếp theo Index
            planbookDto.Activities = _mapper.Map<List<CreateActivityForPlanbookDTO>>(activities);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Planbook template retrieved successfully.",
                Data = planbookDto
            };
        }
        #endregion

        #region Create Planbook Using AI
        //public async Task<ResponseModel> GetPlanbookUsingAIAsync(string lessonId)
        //{
        //    try
        //    {
        //        var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(
        //                                    id: lessonId,
        //                                    filter: l => !l.IsDeleted,
        //                                    includeProperties: "Chapter,Chapter.SubjectInCurriculum,Chapter.SubjectInCurriculum.Subject,Chapter.SubjectInCurriculum.Curriculum,Chapter.SubjectInCurriculum.Grade");

        //        if (lesson is null)
        //        {
        //            return new ResponseModel
        //            {
        //                Success = false,
        //                Message = "Lesson not found."
        //            };
        //        }

        //        var chapter = lesson.Chapter;
        //        var subject = chapter.SubjectInCurriculum.Subject.Name + " - " + chapter.SubjectInCurriculum.Grade.Name + " - " + chapter.SubjectInCurriculum.Curriculum.Name;

        //        // Tạo các prompt cho AI để lấy các thông tin cho kế hoạch giảng dạy
        //        var titlePrompt = $"Hãy tạo tiêu đề thích hợp cho kế hoạch giảng dạy của bài học {lesson.Name}, chỉ cần một tiêu đề";
        //        var knowledgeObjectivePrompt = $"Hãy tạo mục tiêu kiến thức cho bài học {lesson.Name} thuộc chương {chapter.Name} của môn học {subject}";
        //        var skillsObjectivePrompt = $"Hãy tạo mục tiêu kỹ năng cho bài học {lesson.Name} thuộc chương {chapter.Name} của môn học {subject}";
        //        var qualitiesObjectivePrompt = $"Hãy tạo mục tiêu phẩm chất cho bài học {lesson.Name} thuộc chương {chapter.Name} của môn học {subject}";
        //        var teachingToolsPrompt = $"Hãy tạo danh sách thiết bị dạy học cho bài học {lesson.Name} thuộc chương {chapter.Name} của môn học {subject}";
        //        var notesPrompt = $"Hãy tạo ghi chú cho bài học {lesson.Name} thuộc chương {chapter.Name} của môn học {subject}";

        //        // Gọi OpenAI Service để lấy các thông tin từ các prompt
        //        var title = await _openAIService.GeneratePlanbookFieldAsync(titlePrompt);
        //        var knowledgeObjective = await _openAIService.GeneratePlanbookFieldAsync(knowledgeObjectivePrompt);
        //        var skillsObjective = await _openAIService.GeneratePlanbookFieldAsync(skillsObjectivePrompt);
        //        var qualitiesObjective = await _openAIService.GeneratePlanbookFieldAsync(qualitiesObjectivePrompt);
        //        var teachingTools = await _openAIService.GeneratePlanbookFieldAsync(teachingToolsPrompt);
        //        var notes = await _openAIService.GeneratePlanbookFieldAsync(notesPrompt);

        //        // Tạo đối tượng CreatePlanbookDTO và điền thông tin vào
        //        var planbookDto = new CreatePlanbookDTO
        //        {
        //            Title = title,
        //            SchoolName = "",  // Giáo viên có thể tự điền
        //            TeacherName = "",  // Giáo viên có thể tự điền
        //            Subject = subject,
        //            ClassName = "",  // Giáo viên có thể tự điền
        //            DurationInPeriods = 1,  // Tiết học, mặc định là 1
        //            KnowledgeObjective = knowledgeObjective,
        //            SkillsObjective = skillsObjective,
        //            QualitiesObjective = qualitiesObjective,
        //            TeachingTools = teachingTools,
        //            Notes = notes,
        //            IsDefault = false,
        //            CollectionId = null,  // Khi call API này, FE sẽ tự truyền CollectionId
        //            LessonId = lessonId,
        //            Activities = new List<CreateActivityForPlanbookDTO>(), // Các hoạt động sẽ được tạo sau
        //        };

        //        // Tiến hành tạo các hoạt động cho bài học
        //        var activityPrompts = new List<string>
        //        {
        //            "Hoạt động 1: Xác định vấn đề",
        //            "Hoạt động 2: Hình thành kiến thức mới",
        //            "Hoạt động 3: Luyện tập",
        //            "Hoạt động 4: Vận dụng"
        //        };

        //        foreach (var activityPrompt in activityPrompts)
        //        {
        //            //var activityTitle = await _openAIService.GeneratePlanbookFieldAsync($"Hãy tạo nội dung cho {activityPrompt} trong bài học {lesson.Name}");
        //            var activityObjective = await _openAIService.GeneratePlanbookFieldAsync($"Hãy tạo mục tiêu cho {activityPrompt} trong bài học {lesson.Name}");
        //            var activityContent = await _openAIService.GeneratePlanbookFieldAsync($"Hãy tạo nội dung chi tiết cho {activityPrompt} trong bài học {lesson.Name}");
        //            var activityProduct = await _openAIService.GeneratePlanbookFieldAsync($"Hãy tạo sản phẩm cho {activityPrompt} trong bài học {lesson.Name}");
        //            var activityImplementation = await _openAIService.GeneratePlanbookFieldAsync($"Hãy tạo hướng dẫn tổ chức thực hiện cho {activityPrompt} trong bài học {lesson.Name}");

        //            planbookDto.Activities.Add(new CreateActivityForPlanbookDTO
        //            {
        //                Title = activityPrompt,
        //                Objective = activityObjective,
        //                Content = activityContent,
        //                Product = activityProduct,
        //                Implementation = activityImplementation
        //            });
        //        }

        //        return new SuccessResponseModel<object>
        //        {
        //            Success = true,
        //            Message = "Planbook created successfully using AI.",
        //            Data = planbookDto
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ErrorResponseModel<object>
        //        {
        //            Success = false,
        //            Message = "An error occurred while creating the planbookDto using AI.",
        //            Errors = new List<string> { ex.Message }
        //        };
        //    }
        //}

        // Tối ưu tôc độ trả về của OpenAI Service bằng cách gửi nhiều prompt cùng lúc
        public async Task<ResponseModel> GetPlanbookUsingAIAsync(string lessonId)
        {
            try
            {
                var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(
                                id: lessonId,
                                filter: l => !l.IsDeleted,
                                includeProperties: "Chapter,Chapter.SubjectInCurriculum,Chapter.SubjectInCurriculum.Subject,Chapter.SubjectInCurriculum.Curriculum,Chapter.SubjectInCurriculum.Grade");

                if (lesson is null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Lesson not found."
                    };
                }

                var chapter = lesson.Chapter;
                var subject = chapter.SubjectInCurriculum.Subject.Name + " - " + chapter.SubjectInCurriculum.Grade.Name + " - " + chapter.SubjectInCurriculum.Curriculum.Name;

                // Tạo các prompt cho AI để lấy các thông tin cho kế hoạch giảng dạy
                var prompts = new List<string>
                {
                    $"Hãy tạo tiêu đề thích hợp cho kế hoạch giảng dạy của bài học {lesson.Name}, chỉ cần một tiêu đề",
                    $"Hãy tạo mục tiêu kiến thức cho bài học {lesson.Name} thuộc chương {chapter.Name} của môn học {subject}",
                    $"Hãy tạo mục tiêu kỹ năng cho bài học {lesson.Name} thuộc chương {chapter.Name} của môn học {subject}",
                    $"Hãy tạo mục tiêu phẩm chất cho bài học {lesson.Name} thuộc chương {chapter.Name} của môn học {subject}",
                    $"Hãy tạo danh sách thiết bị dạy học cho bài học {lesson.Name} thuộc chương {chapter.Name} của môn học {subject}",
                    $"Hãy tạo ghi chú cho bài học {lesson.Name} thuộc chương {chapter.Name} của môn học {subject}"
                };

                // Gửi các prompt cho OpenAI Service để lấy các thông tin cho kế hoạch giảng dạy
                var aiResponses = await Task.WhenAll(prompts.Select(prompt => _openAIService.GeneratePlanbookFieldAsync(prompt)));

                // Map AI response cho các trường thông tin của planbookDto
                var planbookDto = new CreatePlanbookDTO
                {
                    Title = aiResponses[0],
                    SchoolName = "",  // Giáo viên có thể tự điền
                    TeacherName = "", // Giáo viên có thể tự điền
                    Subject = subject,
                    ClassName = "",   // Giáo viên có thể tự điền
                    DurationInPeriods = 1, // Tiết học, mặc định là 1
                    KnowledgeObjective = aiResponses[1],
                    SkillsObjective = aiResponses[2],
                    QualitiesObjective = aiResponses[3],
                    TeachingTools = aiResponses[4],
                    Notes = aiResponses[5],
                    IsDefault = false,
                    CollectionId = null,  // Khi call API này, FE sẽ tự truyền CollectionId
                    LessonId = lessonId,
                    Activities = new List<CreateActivityForPlanbookDTO>() // Các hoạt động sẽ được tạo sau
                };

                // Tiến hành tạo các hoạt động cho bài học
                var activityPrompts = new List<string>
                {
                    "Hoạt động 1: Xác định vấn đề",
                    "Hoạt động 2: Hình thành kiến thức mới",
                    "Hoạt động 3: Luyện tập",
                    "Hoạt động 4: Vận dụng"
                };

                // Tạo một list các task để gửi các prompt cho AI và lấy kết quả cho các hoạt động
                var activityTasks = activityPrompts.Select(async activityPrompt =>
                {
                    var objective = await _openAIService.GeneratePlanbookFieldAsync($"Hãy tạo mục tiêu cho {activityPrompt} trong bài học {lesson.Name}");
                    var content = await _openAIService.GeneratePlanbookFieldAsync($"Hãy tạo nội dung chi tiết cho {activityPrompt} trong bài học {lesson.Name}");
                    var product = await _openAIService.GeneratePlanbookFieldAsync($"Hãy tạo sản phẩm cho {activityPrompt} trong bài học {lesson.Name}");
                    var implementation = await _openAIService.GeneratePlanbookFieldAsync($"Hãy tạo hướng dẫn tổ chức thực hiện cho {activityPrompt} trong bài học {lesson.Name}");

                    return new CreateActivityForPlanbookDTO
                    {
                        Title = activityPrompt,
                        Objective = objective,
                        Content = content,
                        Product = product,
                        Implementation = implementation
                    };
                }).ToList();

                // Chờ tất cả các task hoàn thành và lấy kết quả
                var activities = await Task.WhenAll(activityTasks);

                // Thêm các hoạt động vào planbookDto
                planbookDto.Activities.AddRange(activities);

                return new SuccessResponseModel<object>
                {
                    Success = true,
                    Message = "Planbook created successfully using AI.",
                    Data = planbookDto
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while creating the planbook using AI.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        #endregion

        #region Clone Planbook
        public async Task<ResponseModel> ClonePlanbookAsync(ClonePlanbookDTO model)
        {
            try
            {
                // Kiểm tra planbookDto có tồn tại không
                var planbook = await _unitOfWork.PlanbookRepository.GetByIdAsync(model.PlanbookId);
                if (planbook is null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Planbook not found."
                    };
                }

                // Kiểm tra collection có tồn tại không
                if (!string.IsNullOrEmpty(model.CollectionId))
                {
                    var existingCollection = await _unitOfWork.PlanbookCollectionRepository.GetByIdAsync(model.CollectionId);

                    if (existingCollection is null)
                    {
                        return new ResponseModel
                        {
                            Success = false,
                            Message = "Collection not found."
                        };
                    }
                }

                // Clone Planbook
                var clonePlanbook = new Planbook
                {
                    PlanbookId = Guid.NewGuid().ToString(),
                    Title = planbook.Title,
                    SchoolName = planbook.SchoolName,
                    TeacherName = planbook.TeacherName,
                    Subject = planbook.Subject,
                    ClassName = planbook.ClassName,
                    DurationInPeriods = planbook.DurationInPeriods,
                    KnowledgeObjective = planbook.KnowledgeObjective,
                    SkillsObjective = planbook.SkillsObjective,
                    QualitiesObjective = planbook.QualitiesObjective,
                    TeachingTools = planbook.TeachingTools,
                    Notes = planbook.Notes,
                    IsDefault = false,
                    CollectionId = model.CollectionId,
                    LessonId = planbook.LessonId,
                    Activities = new List<Activity>()
                };

                // Clone các hoạt động
                var activities = await _unitOfWork.ActivityRepository.GetByPlanbookIdAsync(model.PlanbookId);
                var cloneActivities = activities.Select(a => new Activity
                {
                    ActivityId = Guid.NewGuid().ToString(),
                    PlanbookId = clonePlanbook.PlanbookId,
                    Title = a.Title,
                    Objective = a.Objective,
                    Content = a.Content,
                    Product = a.Product,
                    Implementation = a.Implementation,
                    Index = a.Index
                }).ToList();

                await _unitOfWork.PlanbookRepository.AddAsync(clonePlanbook);
                await _unitOfWork.ActivityRepository.CreateRangeActivityAsync(cloneActivities);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Planbook cloned successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while cloning the planbook.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        #endregion
    }
}
