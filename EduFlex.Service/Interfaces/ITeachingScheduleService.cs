using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.TeachingScheduleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Interfaces
{
    public interface ITeachingScheduleService
    {
        Task<ResponseModel> GetAllTeachingSchedulesAsync(string? keyword, int pageIndex, int pageSize);

        Task<ResponseModel> GetTeachingScheduleByIdAsync(string scheduleId);

        Task<ResponseModel> AddTeachingScheduleAsync(CreateTeachingScheduleDTO model);

        Task<ResponseModel> UpdateTeachingScheduleAsync(UpdateTeachingScheduleDTO model);

        Task<ResponseModel> DeleteTeachingScheduleAsync(string scheduleId);
    }
}
