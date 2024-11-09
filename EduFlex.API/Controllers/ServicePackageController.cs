using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ViewModels.ServicePackageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EduFlex.API.Controllers
{
    public class ServicePackageController : BaseController
    {
        private readonly IServicePackageService _servicePackageService;

        public ServicePackageController(IServicePackageService servicePackageService)
        {
            _servicePackageService = servicePackageService;
        }

        #region Get All Service Packages
        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetAllServicePackagesAsync(string? keyword, int pageIndex = 0, int pageSize = 10)
        {
            var response = await _servicePackageService.GetAllServicePackagesAsync(keyword, pageIndex, pageSize);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Get Service Package by ID
        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetServicePackageByIdAsync(string packageId)
        {
            var response = await _servicePackageService.GetServicePackageByIdAsync(packageId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Add New Service Package
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> AddServicePackageAsync(CreateServicePackageDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _servicePackageService.AddServicePackageAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Update Service Package
        [HttpPut]
        //[Authorize]
        public async Task<IActionResult> UpdateServicePackageAsync(UpdateServicePackageDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _servicePackageService.UpdateServicePackageAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Delete Service Package
        [HttpDelete("{packageId}")]
        //[Authorize]
        public async Task<IActionResult> DeleteServicePackageAsync(string packageId)
        {
            var response = await _servicePackageService.DeleteServicePackageAsync(packageId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion
    }
}
