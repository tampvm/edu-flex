using AutoMapper;
using EduFlex.Domain.Entities;
using EduFlex.Repository.Common;
using EduFlex.Repository.Interfaces;
using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.FeedbackViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Get feedback for a specific Planbook
        public async Task<ResponseModel> GetFeedbackByPlanbookIdAsync(string planbookId, int pageIndex, int pageSize)
        {
            var feedbacks = await _unitOfWork.FeedbackRepository.GetAsync(
                filter: f => f.PlanbookId == planbookId && !f.IsDeleted,
                includeProperties: "Teacher,Planbook",
                pageIndex: pageIndex,
                pageSize: pageSize
            );

            var feedbackDtos = _mapper.Map<Pagination<ViewFeedbackDTO>>(feedbacks);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Feedback retrieved successfully.",
                Data = feedbackDtos
            };
        }

        // Submit new feedback for a Planbook
        public async Task<ResponseModel> SubmitFeedbackAsync(CreateFeedbackDTO model)
        {
            try
            {
                var feedback = _mapper.Map<Feedback>(model);
                feedback.FeedbackId = Guid.NewGuid().ToString();

                await _unitOfWork.FeedbackRepository.AddAsync(feedback);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseModel
                {
                    Success = true,
                    Message = "Feedback submitted successfully."
                };
            }
            catch (Exception ex)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while submitting feedback.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
