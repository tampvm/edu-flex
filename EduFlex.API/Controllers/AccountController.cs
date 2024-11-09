using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduFlex.API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #region View User Profile
        [HttpGet]
        public async Task<IActionResult> GetUserProfileAsync(string userId)
        {
            var response = await _accountService.GetUserProfileAsync(userId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Update User Profile
        [HttpPut]
        //[Authorize]
        public async Task<IActionResult> UpdateUserProfileAsync(UpdateUserProfileDTO model)
        {
            if (!ModelState.IsValid)
            {
                // Xử lý lỗi nếu dữ liệu không hợp lệ
                return BadRequest(ModelState);
            }

            var response = await _accountService.UpdateUserProfileAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut]
        //[Authorize]
        public async Task<IActionResult> UpdateUserAvatarAsync(UpdateUserAvatarDTO model)
        {
            var response = await _accountService.UpdateUserAvatarAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Change Password
        [HttpPut]
        //[Authorize]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _accountService.ChangePasswordAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Update User Phone Number Or Link Phone Number
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> SendVerificationCodeAsync(NewPhoneNumberDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _accountService.SendVerificationCodeAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut]
        //[Authorize]
        public async Task<IActionResult> VerifyAndUpdateNewPhoneNumberAsync(ChangePhoneNumberDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _accountService.VerifyAndUpdateNewPhoneNumberAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Update User Email Or Link Email
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> SendVerificationCodeEmailAsync(NewEmailDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _accountService.SendVerificationCodeEmailAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut]
        //[Authorize]
        public async Task<IActionResult> VerifyAndUpdateNewEmailAsync(ChangeEmailDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _accountService.VerifyAndUpdateNewEmailAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Link Account With Username
        [HttpPut]
        //[Authorize]
        public async Task<IActionResult> LinkAccountWithUsernameAsync(UpdateUserAccountDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _accountService.LinkAccountWithUsernameAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Manage User By Admin
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsersForAdminAsync(string? keyword, bool? status, int pageIndex = 0, int pageSize = 10)
        {
            var response = await _accountService.GetAllUserAsync(keyword, status, pageIndex, pageSize);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUserByAdminAsync(CreateUserByAdminDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _accountService.CreateUserAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserByAdminAsync(UpdateUserByAdminDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _accountService.UpdateUserAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserByAdminAsync(string userId)
        {
            var response = await _accountService.DeleteUserAsync(userId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> BlockOrUnBlockUserByAdmin(BlockOrUnBlockAccountDTO model)
        {
            var response = await _accountService.BlockOrUnBlockUserByAdmin(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion
    }
}
