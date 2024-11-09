using EduFlex.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EduFlex.API.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        #region Get All Users Payment History
        [HttpGet("GetAllPayments")]
        public async Task<IActionResult> GetAllUserPaymentHistoryAsync(int pageIndex = 0, int pageSize = 10)
        {
            var response = await _paymentService.GetAllUserPaymentHistoryAsync(pageIndex, pageSize);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region View User Payment History
        [HttpGet("history/{teacherId}")]
        //[Authorize]
        public async Task<IActionResult> GetUserPaymentHistoryAsync(string teacherId, int pageIndex = 0, int pageSize = 10)
        {
            var response = await _paymentService.GetUserPaymentHistoryAsync(teacherId, pageIndex, pageSize);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region View Payment Details
        [HttpGet("{paymentId}")]
        //[Authorize]
        public async Task<IActionResult> GetPaymentDetailsAsync(string paymentId)
        {
            var response = await _paymentService.GetPaymentDetailsAsync(paymentId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region View Revenue Reports

        // Revenue report by month
        [HttpGet("RevenueByMonth/{year}")]
        public async Task<IActionResult> GetRevenueByMonthAsync(int year)
        {
            var response = await _paymentService.GetRevenueByMonthAsync(year);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        // Revenue report by quarter
        [HttpGet("RevenueByQuarter/{year}")]
        public async Task<IActionResult> GetRevenueByQuarterAsync(int year)
        {
            var response = await _paymentService.GetRevenueByQuarterAsync(year);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        // Revenue report by year
        [HttpGet("RevenueByYear")]
        public async Task<IActionResult> GetRevenueByYearAsync()
        {
            var response = await _paymentService.GetRevenueByYearAsync();
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        #endregion
    }
}
