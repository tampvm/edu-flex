using AutoMapper;
using EduFlex.Domain.Entities;
using EduFlex.Repository.Common;
using EduFlex.Repository.Interfaces;
using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.TeachingScheduleModels;
using System;


namespace EduFlex.Service.Services
{
    public class TeachingScheduleService : ITeachingScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TeachingScheduleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel> GetAllTeachingSchedulesAsync(string? keyword, int pageIndex, int pageSize)
        {
            var schedules = await _unitOfWork.TeachingScheduleRepository.GetAsync(
                                filter: s => !s.IsDeleted && (string.IsNullOrEmpty(keyword) || s.ClassName.Contains(keyword)),
                                orderBy: s => s.OrderBy(s => s.Date),
                                includeProperties: "Teacher,Planbook", 
                                pageIndex: pageIndex,
                                pageSize: pageSize
            );

            var scheduleDtos = _mapper.Map<Pagination<ViewTeachingScheduleDTO>>(schedules);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Teaching schedules retrieved successfully.",
                Data = scheduleDtos
            };
        }

        public async Task<ResponseModel> GetTeachingScheduleByIdAsync(string scheduleId)
        {
            var schedule = await _unitOfWork.TeachingScheduleRepository.GetByIdAsync(
                scheduleId,
                includeProperties: "Teacher,Planbook" 
            );

            if (schedule == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Teaching schedule not found."
                };
            }

            var result = _mapper.Map<ViewTeachingScheduleDTO>(schedule);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Teaching schedule retrieved successfully.",
                Data = result
            };
        }

        public async Task<ResponseModel> AddTeachingScheduleAsync(CreateTeachingScheduleDTO model)
        {
            try
            {
                var schedule = _mapper.Map<TeachingSchedule>(model);
                schedule.ScheduleId = Guid.NewGuid().ToString(); 

                await _unitOfWork.TeachingScheduleRepository.AddAsync(schedule);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Teaching schedule created successfully.",
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while creating the teaching schedule.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ResponseModel> UpdateTeachingScheduleAsync(UpdateTeachingScheduleDTO model)
        {
            try
            {
                var schedule = await _unitOfWork.TeachingScheduleRepository.GetByIdAsync(model.ScheduleId);
                if (schedule == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Teaching schedule not found."
                    };
                }

                if (schedule.IsDeleted == true)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Cannot modify a deleted teaching schedule."
                    };
                }

                _mapper.Map(model, schedule);

                _unitOfWork.TeachingScheduleRepository.Update(schedule);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Teaching schedule updated successfully.",
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while updating the teaching schedule.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ResponseModel> DeleteTeachingScheduleAsync(string scheduleId)
        {
            try
            {
                // Fetch the teaching schedule by ID
                var schedule = await _unitOfWork.TeachingScheduleRepository.GetByIdAsync(scheduleId);

                // Check if the schedule exists
                if (schedule == null)
                {
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Teaching schedule not found."
                    };
                }

                _unitOfWork.TeachingScheduleRepository.Delete(schedule); 
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Teaching schedule deleted permanently."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while deleting the teaching schedule.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

    }
}