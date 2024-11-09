using AutoMapper;
using EduFlex.Domain.Entities;
using EduFlex.Repository.Common;
using EduFlex.Repository.Interfaces;
using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.RoleViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Role Management
        // Get all active roles
        public async Task<ResponseModel> GetActiveRolesAsync(int pageIndex, int pageSize)
        {
            var roles = await _unitOfWork.RoleRepository.GetAsync(
                filter: r => !r.IsDeleted,
                pageIndex: pageIndex,
                pageSize: pageSize
            );

            // Dùng AutoMapper để ánh xạ các thuộc tính của role sang DTO
            var rolesDto = _mapper.Map<Pagination<ViewListRoleDTO>>(roles);

            //// Ánh xạ các thuộc tính đặc biệt cần xử lý
            foreach (var roleDto in rolesDto.Items)
            {
                var role = await _unitOfWork.RoleRepository.GetByIdAsync(roleDto.Id);

                // Tìm người tạo, cập nhật, xóa và ánh xạ tên người dùng
                var createdByUser = await _unitOfWork.AccountRepository.GetByIdAsync(role.CreatedBy);
                var updatedByUser = !string.IsNullOrEmpty(role.UpdatedBy) ? await _unitOfWork.AccountRepository.GetByIdAsync(role.UpdatedBy) : null;
                var deletedByUser = !string.IsNullOrEmpty(role.DeletedBy) ? await _unitOfWork.AccountRepository.GetByIdAsync(role.DeletedBy) : null;

                // Cập nhật các trường CreatedBy, UpdatedBy, DeletedBy
                roleDto.CreatedBy = createdByUser?.Username ?? role.CreatedBy;
                roleDto.UpdatedBy = updatedByUser?.Username ?? role.UpdatedBy;
                roleDto.DeletedBy = deletedByUser?.Username ?? role.DeletedBy;
            }

            return new SuccessResponseModel<object>
            {
                Success = true,
                Message = "Roles retrieved successfully.",
                Data = rolesDto
            };
        }

        // Create a new role
        public async Task<ResponseModel> CreateRoleAsync(CreateRoleDTO model)
        {
            var existingRole = await _unitOfWork.RoleRepository.GetRoleByNameAsync(model.RoleName);

            if (existingRole != null)
            {
                if (existingRole.IsDeleted)
                {
                    // Role đã bị xóa mềm, cập nhật lại role này
                    existingRole.IsDeleted = false;
                    _unitOfWork.RoleRepository.Update(existingRole);
                    await _unitOfWork.SaveChangeAsync();

                    return new ResponseModel
                    {
                        Success = true,
                        Message = "Role restored successfully.",
                    };
                }
                return new ResponseModel
                {
                    Success = false,
                    Message = "Role already exists.",
                };
            }

            var newRole = _mapper.Map<Role>(model);

            await _unitOfWork.RoleRepository.AddAsync(newRole);
            await _unitOfWork.SaveChangeAsync();

            return new SuccessResponseModel<string>
            {
                Success = true,
                Message = "Role created successfully.",
                Data = newRole.Name
            };
        }

        // Update a role
        public async Task<ResponseModel> UpdateRoleAsync(UpdateRoleDTO model)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(model.Id);

            if (role is null || role.IsDeleted)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Role not found or has been deleted.",
                };
            }

            var updatedRole = _mapper.Map(model, role);

            _unitOfWork.RoleRepository.Update(updatedRole);
            await _unitOfWork.SaveChangeAsync();

            return new SuccessResponseModel<string>
            {
                Success = true,
                Message = "Role updated successfully.",
                Data = role.Name
            };
        }

        // Delete a role (soft delete)
        public async Task<ResponseModel> DeleteRoleAsync(int roleId)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(roleId);

            if (role is null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Role not found.",
                };
            }

            // Kiểm tra xem role có phải là role mặc định không
            if (role.IsDefault)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Cannot delete default role.",
                };
            }

            _unitOfWork.RoleRepository.SoftRemove(role);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel
            {
                Success = true,
                Message = "Role deleted successfully.",
            };
        }
        #endregion
    }
}
