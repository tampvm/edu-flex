using AutoMapper;
using EduFlex.Domain.Entities;
using EduFlex.Repository.Interfaces;
using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.UserPackageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Services
{
    public class UserPackageService : IUserPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserPackageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Get all user packages for a specific user
        public async Task<ResponseModel> GetUserPackagesAsync(string userId)
        {
            var userPackages = await _unitOfWork.UserPackageRepository.GetAllAsync(
                filter: up => up.UserId == userId && !up.IsDeleted,
                includeProperties: "Package"
            );

            var userPackageDtos = _mapper.Map<List<ViewListUserPackageDTO>>(userPackages);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "User packages retrieved successfully.",
                Data = userPackageDtos
            };
        }

        // Get details of a specific user package
        public async Task<ResponseModel> GetUserPackageDetailsAsync(int userPackageId)
        {
            var userPackage = await _unitOfWork.UserPackageRepository.GetByIdAsync(
                userPackageId,
                includeProperties: "Package"
            );

            if (userPackage == null || userPackage.IsDeleted)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "User package not found."
                };
            }

            var userPackageDto = _mapper.Map<ViewUserPackageDTO>(userPackage);

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "User package details retrieved successfully.",
                Data = userPackageDto
            };
        }
    }
}
