using AutoMapper;
using EduFlex.Domain.Entities;
using EduFlex.Repository.Interfaces;
using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.ActivityViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Services
{
	public class ActivityService : IActivityService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ActivityService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		#region Get Activities By Planbook Id
		public async Task<ResponseModel> GetAllByPlanbookIdAsync(string planbookId)
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

			var activities = await _unitOfWork.ActivityRepository.GetByPlanbookIdAsync(planbookId);

			var mapper = _mapper.Map<List<ViewListActivityDTO>>(activities);

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Activities retrieved successfully.",
				Data = mapper
			};
		}
		#endregion

		#region Get Activity By Id
		public async Task<ResponseModel> GetActivityByIdAsync(string activityId)
		{
			var activity = await _unitOfWork.ActivityRepository.GetByIdAsync(activityId);
			if (activity != null)
			{
				var mapper = _mapper.Map<ViewListActivityDTO>(activity);
				return new SuccessResponseModel<object>
				{
					Success = true,
					Message = "Activity retrieved successfully.",
					Data = mapper
				};
			}
			else
			{
				return new ResponseModel
				{
					Success = false,
					Message = "Activity not found."
				};
			}
		}
		#endregion

		#region Create Activity
		public async Task<ResponseModel> CreateActivityAsync(CreateActivityDTO model)
		{
			try
			{
				var planbook = await _unitOfWork.PlanbookRepository.GetByIdAsync(model.PlanbookId);
				if (planbook == null)
				{
					return new ResponseModel
					{
						Success = false,
						Message = "Planbook not found."
					};
				}
				else
				{
					var mapper = _mapper.Map<Activity>(model);
					await _unitOfWork.ActivityRepository.CreateActivityAsync(mapper);

					return new ResponseModel
					{
						Success = true,
						Message = "Activity created successfully."
					};
				}
			}
			catch (Exception ex)
			{
				return new ErrorResponseModel<string>
				{
					Success = false,
					Message = "An error occurred while creating the activity.",
					Errors = new List<string> { ex.Message }
				};
			}
		}
		#endregion

		#region Update Activity
		public async Task<ResponseModel> UpdateActivityAsync(UpdateActivityDTO model)
		{
			try
			{
				var activity = await _unitOfWork.ActivityRepository.GetByIdAsync(model.ActivityId);
				if (activity == null)
				{
					return new ResponseModel
					{
						Success = false,
						Message = "Activity not found."
					};
				}
				else
				{
					var mapper = _mapper.Map(model, activity);
					await _unitOfWork.ActivityRepository.UpdateActivityAsync(mapper);

					return new ResponseModel
					{
						Success = true,
						Message = "Activity updated successfully."
					};
				}
			}
			catch (Exception ex)
			{
				return new ErrorResponseModel<string>
				{
					Success = false,
					Message = "An error occurred while updating the activity.",
					Errors = new List<string> { ex.Message }
				};
			}
		}
		#endregion

		#region Delete Activity
		public async Task<ResponseModel> DeleteActivityAsync(string activityId)
		{
			try
			{
				var activity = await _unitOfWork.ActivityRepository.GetByIdAsync(activityId);
				if (activity == null)
				{
					return new ResponseModel
					{
						Success = false,
						Message = "Activity not found."
					};
				}
				else
				{
					await _unitOfWork.ActivityRepository.DeleteActivityAsync(activity);
					return new ResponseModel
					{
						Success = true,
						Message = "Activity deleted successfully."
					};
				}
			}
			catch (Exception ex)
			{
				return new ErrorResponseModel<string>
				{
					Success = false,
					Message = "An error occurred while deleting the activity.",
					Errors = new List<string> { ex.Message }
				};
			}
		}
		#endregion
	}
}
