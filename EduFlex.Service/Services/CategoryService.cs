using AutoMapper;
using EduFlex.Domain.Entities;
using EduFlex.Repository.Common;
using EduFlex.Repository.Interfaces;
using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.CategoryViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IUrlService _urlService;

		public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IUrlService urlService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_urlService = urlService;
		}

		public async Task<ResponseModel> GetAllCategoryAsync(int pageIndex, int pageSize)
		{
			var categories = await _unitOfWork.CategoryRepository.GetAsync(
				filter: r => r.Status.Equals(true),
				pageIndex: pageIndex,
				pageSize: pageSize
				);
			var categoryDtos = _mapper.Map<Pagination<ViewListCategoryDTO>>(categories);

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Categories retrieved successfully.",
				Data = categoryDtos
			};
		}

		public async Task<ResponseModel> GetCategoryByIdAsync(string id)
		{
			var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

			if (category == null)
			{
				return new ErrorResponseModel<object>
				{
					Success = false,
					Message = "Category not found."
				};
			}

			var result = _mapper.Map<ViewListCategoryDTO>(category);

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Category retrieved successfully.",
				Data = result
			};
		}

		public async Task<ResponseModel> CreateCategoryAsync(CreateCategoryDTO model)
		{
			try
			{
				var category = _mapper.Map<Category>(model);
				category.Url = _urlService.RemoveDiacritics(model.Name).Replace(" ", "-").ToLower();
				await _unitOfWork.CategoryRepository.AddAsync(category);
				await _unitOfWork.SaveChangeAsync();
				return new ResponseModel
				{
					Success = true,
					Message = "Category create successfully."
				};
			}
			catch (Exception ex)
			{
				return new ResponseModel
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ResponseModel> UpdateCategoryAsync(UpdateCategoryDTO model)
		{
			try
			{
				var category = await _unitOfWork.CategoryRepository.GetByIdAsync(model.Id);
				if (category == null)
				{
					return new ResponseModel
					{
						Success = false,
						Message = "Category not found."
					};
				}

				if (category.IsDeleted == true)
				{
					return new ResponseModel
					{
						Success = false,
						Message = "Can't modify category is deleted."
					};
				}

				var mapper = _mapper.Map(model, category);

				if (model.Url != null || string.IsNullOrEmpty(model.Url))
				{
					category.Url = _urlService.RemoveDiacritics(model.Name).Replace(" ", "-").ToLower();
				}
				else
				{
					category.Url = _urlService.RemoveDiacritics(model.Name).Replace(" ", "-").ToLower();
				}

				try
				{
					_unitOfWork.CategoryRepository.Update(mapper);
					await _unitOfWork.SaveChangeAsync();
					return new SuccessResponseModel<object>
					{
						Success = true,
						Message = "Category updated successfully.",
						Data = mapper
					};
				}
				catch (Exception ex)
				{
					return new ResponseModel
					{
						Success = false,
						Message = ex.Message
					};
				}
			}
			catch (Exception ex)
			{
				return new ResponseModel
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ResponseModel> DeleteCategoryAsync(string id)
		{
			try
			{
				var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
				if (category == null)
				{
					return new ResponseModel
					{
						Success = false,
						Message = "Category not found."
					};
				}

				if (category.IsDeleted == true)
				{
					return new ResponseModel
					{
						Success = false,
						Message = "Can't delete category is deleted."
					};
				}

				_unitOfWork.CategoryRepository.SoftRemove(category);
				await _unitOfWork.SaveChangeAsync();
				return new ResponseModel
				{
					Success = true,
					Message = "Category deleted successfully."
				};
			}
			catch (Exception ex)
			{
				return new ResponseModel
				{
					Success = false,
					Message = ex.Message
				};
			}
		}
	}
}
