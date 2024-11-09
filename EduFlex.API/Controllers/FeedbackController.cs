using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ViewModels.FeedbackViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EduFlex.API.Controllers
{
    public class FeedbackController : BaseController
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        #region Get Feedback by Planbook ID
        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetFeedbackByPlanbookIdAsync(string planbookId, int pageIndex = 0, int pageSize = 10)
        {
            var response = await _feedbackService.GetFeedbackByPlanbookIdAsync(planbookId, pageIndex, pageSize);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Submit Feedback
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> SubmitFeedbackAsync(CreateFeedbackDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _feedbackService.SubmitFeedbackAsync(model);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion
    }
}
