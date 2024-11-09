using EduFlex.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Repository.Interfaces
{
	public interface IActivityRepository
	{
		Task<List<Activity>> GetByPlanbookIdAsync(string planbookId);
		Task<Activity?> GetByIdAsync(string activityId);
		Task<Activity> CreateActivityAsync(Activity activity);
		Task CreateRangeActivityAsync(IEnumerable<Activity> activities);
        Task<Activity> UpdateActivityAsync(Activity activity);
		void UpdateRangeActivityAsync(IEnumerable<Activity> activities);
        Task<bool> DeleteActivityAsync(Activity activity);
		void DeleteRangeActivityAsync(IEnumerable<Activity> activities);
    }
}
