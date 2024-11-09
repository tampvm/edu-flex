using EduFlex.Service.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Interfaces
{
    public interface IPaymentService
    {
        // Get paginated payment history of a user
        Task<ResponseModel> GetUserPaymentHistoryAsync(string teacherId, int pageIndex, int pageSize);

        // Get detailed payment information by payment ID
        Task<ResponseModel> GetPaymentDetailsAsync(string paymentId);

        // Get paginated payment history for all users
        Task<ResponseModel> GetAllUserPaymentHistoryAsync(int pageIndex, int pageSize);

        Task<ResponseModel> GetRevenueByMonthAsync(int year);
        Task<ResponseModel> GetRevenueByQuarterAsync(int year);
        Task<ResponseModel> GetRevenueByYearAsync();
    }
}
