using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.RoleViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Interfaces
{
    public interface IRoleService
    {
        Task<ResponseModel> GetActiveRolesAsync(int pageIndex, int pageSize);
        Task<ResponseModel> CreateRoleAsync(CreateRoleDTO model);
        Task<ResponseModel> UpdateRoleAsync(UpdateRoleDTO model);
        Task<ResponseModel> DeleteRoleAsync(int roleId);
    }
}
