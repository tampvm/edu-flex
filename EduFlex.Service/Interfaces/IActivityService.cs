using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.ActivityViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Interfaces
{
	public interface IActivityService
	{
		Task<ResponseModel> GetAllByPlanbookIdAsync(string planbookId);
		Task<ResponseModel> GetActivityByIdAsync(string activityId);
		Task<ResponseModel> CreateActivityAsync(CreateActivityDTO model);
		Task<ResponseModel> UpdateActivityAsync(UpdateActivityDTO model);
		Task<ResponseModel> DeleteActivityAsync(string activityId);
	}
}
