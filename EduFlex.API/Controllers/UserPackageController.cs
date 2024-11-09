using EduFlex.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EduFlex.API.Controllers
{
    public class UserPackageController : BaseController
    {
        private readonly IUserPackageService _userPackageService;

        public UserPackageController(IUserPackageService userPackageService)
        {
            _userPackageService = userPackageService;
        }

        #region Get User Packages
        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetUserPackagesAsync(string userId)
        {
            var response = await _userPackageService.GetUserPackagesAsync(userId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Get User Package Details
        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetUserPackageDetailsAsync(int userPackageId)
        {
            var response = await _userPackageService.GetUserPackageDetailsAsync(userPackageId);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
        #endregion
    }
}
