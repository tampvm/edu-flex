using EduFlex.Domain.Entities;
using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.ServicePackageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Interfaces
{
    public interface IServicePackageService
    {
        Task<ResponseModel> GetAllServicePackagesAsync(string? keyword, int pageIndex, int pageSize);
        Task<ResponseModel> GetServicePackageByIdAsync(string packageId);
        Task<ResponseModel> AddServicePackageAsync(CreateServicePackageDTO model);
        Task<ResponseModel> UpdateServicePackageAsync(UpdateServicePackageDTO model);
        Task<ResponseModel> DeleteServicePackageAsync(string packageId);
    }
}
