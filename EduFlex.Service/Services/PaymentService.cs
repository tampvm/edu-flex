using AutoMapper;
using EduFlex.Repository.Common;
using EduFlex.Repository.Interfaces;
using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.PaymentViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduFlex.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Get all payments for a user (list view)
        public async Task<ResponseModel> GetUserPaymentHistoryAsync(string teacherId, int pageIndex, int pageSize)
        {
            var payments = await _unitOfWork.PaymentRepository.GetAsync(
                filter: p => p.TeacherId == teacherId,
                includeProperties: "Package",
                pageIndex: pageIndex,
                pageSize: pageSize
            );

            var paymentDtos = _mapper.Map<Pagination<PaymentDTO>>(payments);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "User payment history retrieved successfully.",
                Data = paymentDtos
            };
        }

        // Get payment details by payment ID
        public async Task<ResponseModel> GetPaymentDetailsAsync(string paymentId)
        {
            var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(
                paymentId,
                includeProperties: "Package"
            );

            if (payment == null)
            {
                return new ErrorResponseModel<object>
                {
                    Success = false,
                    Message = "Payment not found."
                };
            }

            var result = _mapper.Map<PaymentDetailDTO>(payment);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Payment details retrieved successfully.",
                Data = result
            };
        }

        // Get payment history of all users
        public async Task<ResponseModel> GetAllUserPaymentHistoryAsync(int pageIndex, int pageSize)
        {
            var payments = await _unitOfWork.PaymentRepository.GetAsync(
                filter: null,
                pageIndex: pageIndex,
                pageSize: pageSize
            );

            var paymentDtos = _mapper.Map<Pagination<PaymentDTO>>(payments);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "All users' payment history retrieved successfully.",
                Data = paymentDtos
            };
        }

        // Get revenue report by month for a specific year
        public async Task<ResponseModel> GetRevenueByMonthAsync(int year)
        {
            var payments = await _unitOfWork.PaymentRepository.GetAllAsync(
                filter: p => p.CreatedAt.Year == year 
            );

            var monthlyRevenue = payments
                .GroupBy(p => p.CreatedAt.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Revenue = g.Sum(p => p.TotalAmount)
                })
                .ToList();

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = $"Revenue report by month for the year {year} retrieved successfully.",
                Data = monthlyRevenue
            };
        }

        // Get revenue report by quarter for a specific year
        public async Task<ResponseModel> GetRevenueByQuarterAsync(int year)
        {
            var payments = await _unitOfWork.PaymentRepository.GetAllAsync(
                filter: p => p.CreatedAt.Year == year 
            );

            var quarterlyRevenue = payments
                .GroupBy(p => (p.CreatedAt.Month - 1) / 3 + 1) 
                .Select(g => new
                {
                    Quarter = g.Key,
                    Revenue = g.Sum(p => p.TotalAmount)
                })
                .ToList();

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = $"Revenue report by quarter for the year {year} retrieved successfully.",
                Data = quarterlyRevenue
            };
        }

        // Get revenue report by year
        public async Task<ResponseModel> GetRevenueByYearAsync()
        {
            var payments = await _unitOfWork.PaymentRepository.GetAllAsync();

            var yearlyRevenue = payments
                .GroupBy(p => p.CreatedAt.Year)
                .Select(g => new
                {
                    Year = g.Key,
                    Revenue = g.Sum(p => p.TotalAmount)
                })
                .ToList();

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Revenue report by year retrieved successfully.",
                Data = yearlyRevenue
            };
        }
    }
}
