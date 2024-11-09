using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.PlanbookCollectionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Interfaces
{
    public interface IPlanbookCollectionService
    {
		Task<ResponseModel> GetPlanbookCollectionsByTeacherIdAsync(string teacherId, int pageIndex, int pageSize);
		Task<ResponseModel> GetCreatedPlanbookCollectionsByTeacherIdAsync(string teacherId, int pageIndex, int pageSize);
		Task<ResponseModel> GetSavedPlanbookCollectionsByTeacherIdAsync(string teacherId, int pageIndex, int pageSize);
		Task<ResponseModel> GetCollectionByIdAsync(string collectionId);
		Task<ResponseModel> CreatePlanbookCollectionAsync(CreatePlanbookCollectionDTO model);
		Task<ResponseModel> UpdatePlanbookCollectionAsync(UpdatePlanbookCollectionDTO model);
		Task<ResponseModel> DeletePlanbookCollectionAsync(string collectionId, string teacherId);
		Task<ResponseModel> SavePlanbookAsync(SavePlanbookDTO model);
	}
}
