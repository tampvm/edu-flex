using AutoMapper;
using EduFlex.Domain.Entities;
using EduFlex.Repository.Common;
using EduFlex.Repository.Interfaces;
using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.ArticleViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Services
{
	public class ArticleService : IArticleService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IUrlService _urlService;

		public ArticleService(IUnitOfWork unitOfWork, IMapper mapper, IUrlService urlService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_urlService = urlService;
		}

		#region Get All Article Not Trashed
		public async Task<ResponseModel> GetAllArticleAsync(int pageIndex, int pageSize)
		{
			var articles = await _unitOfWork.ArticleRepository.GetAsync(
				filter: r => r.IsDeleted == false,
				pageIndex: pageIndex,
				pageSize: pageSize
				);
			var articleDtos = _mapper.Map<Pagination<ViewListArticleDTO>>(articles);

			foreach (var item in articleDtos.Items)
			{
				var arImage = await _unitOfWork.ArticleImageRepository.GetByArticleIdAsync(item.ArticleId);

				if (arImage != null)
				{
					Image? image = await _unitOfWork.ImageRepository.GetByIdAsync(arImage.ImageId);
					if (image != null)
						item.Thumb = image.ImageUrl;
				}
			}

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Articles retrieved successfully.",
				Data = articleDtos
			};
		}
		#endregion

		#region Get All Articles by Category Id
		public async Task<ResponseModel> GetAllArticlesByCategoryIdAsync(string categoryId, int pageIndex, int pageSize)
		{
			var articles = await _unitOfWork.ArticleRepository.GetAsync(
				filter: r => r.ArticleCategories.Any(c => c.CategoryId == categoryId),
				pageIndex: pageIndex,
				pageSize: pageSize
			);

			var articleDtos = _mapper.Map<Pagination<ViewListArticleDTO>>(articles);

			foreach (var item in articleDtos.Items)
			{
				var arImage = await _unitOfWork.ArticleImageRepository.GetByArticleIdAsync(item.ArticleId);

				if (arImage != null)
				{
					Image? image = await _unitOfWork.ImageRepository.GetByIdAsync(arImage.ImageId);
					if (image != null)
						item.Thumb = image.ImageUrl;
				}
			}

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Articles retrieved successfully.",
				Data = articleDtos
			};
		}

		#endregion

		#region Get Article By Id
		public async Task<ResponseModel> GetArticleByIdAsync(string id)
		{
			var article = await _unitOfWork.ArticleRepository.GetByIdAsync(id);

			if (article == null)
			{
				return new ErrorResponseModel<object>
				{
					Success = false,
					Message = "Article not found."
				};
			}

			var result = _mapper.Map<ViewDetailArticleDTO>(article);

			var categories = await _unitOfWork.ArticleCategoryRepository.GetByArticleIdAsync(result.ArticleId);
			if (categories != null)
			{
				result.Categories = categories.Select(c => c.Category.Name).ToList();
			}

			var arImage = await _unitOfWork.ArticleImageRepository.GetByArticleIdAsync(result.ArticleId);

			if (arImage != null)
			{
				Image? image = await _unitOfWork.ImageRepository.GetByIdAsync(arImage.ImageId);
				if (image != null)
				{
					result.Thumb = image.ImageUrl;
				}
			}

			return new SuccessResponseModel<object>
			{
				Success = true,
				Message = "Article retrieved successfully.",
				Data = result
			};
		}
		#endregion

		#region Create Article
		public async Task<ResponseModel> CreateArticleAsync(CreateArticleDTO model)
		{
			try
			{
				var article = _mapper.Map<Article>(model);
				if (model.Slug != null)
				{
					article.Url = _urlService.RemoveDiacritics(model.Slug).Replace(" ", "-").ToLower();
				}
				else
				{
					article.Url = _urlService.RemoveDiacritics(model.Title).Replace(" ", "-").ToLower();
				}

				if (model.Content != null)
				{
					article.Excerpt = model.Content.Substring(0, 100);
				}

				article.Status = model.Status ?? "Draft";

				await _unitOfWork.ArticleRepository.AddAsync(article);

				if (model.Categories != null && model.Categories.Any())
				{
					foreach (var categoryId in model.Categories)
					{
						var articleCategory = new ArticleCategory
						{
							ArticleId = article.ArticleId,
							CategoryId = categoryId
						};
						article.ArticleCategories.Add(articleCategory);
					}
				}
				await _unitOfWork.SaveChangeAsync();

				if (model.Thumb != null)
				{
					var existingImage = await _unitOfWork.ImageRepository.FindByImageUrlAsync(model.Thumb);
					string imgId;
					if (existingImage != null)
					{
						imgId = existingImage.ImageId;
					}
					else
					{
						var thumb = new Image
						{
							ImageId = Guid.NewGuid().ToString(),
							ImageUrl = model.Thumb,
							Type = "Article Thumb"
						};
						await _unitOfWork.ImageRepository.AddAsync(thumb);
						await _unitOfWork.SaveChangeAsync();
						imgId = thumb.ImageId;
					}
					var articleImageThumb = new ArticleImage
					{
						ArticleId = article.ArticleId,
						ImageId = imgId
					};
					article.ArticleImages.Add(articleImageThumb);
					await _unitOfWork.SaveChangeAsync();
				}

				return new ResponseModel
				{
					Success = true,
					Message = "Article created successfully."
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
		#endregion

		#region Update Article
		public async Task<ResponseModel> UpdateArticleAsync(UpdateArticleDTO model)
		{
			try
			{
				var article = await _unitOfWork.ArticleRepository.GetByIdAsync(model.ArticleId);

				if (article == null)
				{
					return new ResponseModel
					{
						Success = false,
						Message = "Article not found."
					};
				}

				var mapper = _mapper.Map(model, article);
				if (model.Slug != null)
				{
					article.Url = _urlService.RemoveDiacritics(model.Slug).Replace(" ", "-").ToLower();
				}
				else
				{
					article.Url = _urlService.RemoveDiacritics(model.Title).Replace(" ", "-").ToLower();
				}

				if (model.Content != null)
				{
					article.Excerpt = model.Content.Substring(0, 100);
				}

				switch (model.Status)
				{
					case "Published":
						article.Status = "Published";
						break;
					case "Draft":
						article.Status = "Draft";
						break;
					case "InActive":
						article.Status = "InActive";
						break;
					case "Trashed":
						article.Status = "Trashed";
						_unitOfWork.ArticleRepository.SoftRemove(article);
						await _unitOfWork.SaveChangeAsync();
						break;
					default:
						article.Status = "Draft";
						break;
				}

				_unitOfWork.ArticleRepository.Update(mapper);
				await _unitOfWork.SaveChangeAsync();

				if (model.Categories != null)
				{
					var articleCategories = new List<ArticleCategory>();
					foreach (var categoryId in model.Categories)
					{
						var articleCategory = new ArticleCategory
						{
							ArticleId = article.ArticleId,
							CategoryId = categoryId.ToString()
						};
						articleCategories.Add(articleCategory);
						await _unitOfWork.SaveChangeAsync();
					}
				}

				if (model.Thumb != null)
				{
					var existingImage = await _unitOfWork.ImageRepository.FindByImageUrlAsync(model.Thumb);
					string imgId;
					if (existingImage != null)
					{
						imgId = existingImage.ImageId;
					}
					else
					{
						var thumb = new Image
						{
							ImageId = Guid.NewGuid().ToString(),
							ImageUrl = model.Thumb,
							Type = "Article Thumb"
						};
						await _unitOfWork.ImageRepository.AddAsync(thumb);
						await _unitOfWork.SaveChangeAsync();
						imgId = thumb.ImageId;
					}
					var articleImageThumb = new ArticleImage
					{
						ArticleId = article.ArticleId,
						ImageId = imgId
					};
					article.ArticleImages.Add(articleImageThumb);
					await _unitOfWork.SaveChangeAsync();
				}

				return new ResponseModel
				{
					Success = true,
					Message = "Article updated successfully."
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
		#endregion

		#region Delete Article
		public async Task<ResponseModel> DeleteArticleAsync(string id)
		{
			try
			{
				var article = await _unitOfWork.ArticleRepository.GetByIdAsync(id);

				if (article == null)
				{
					return new ResponseModel
					{
						Success = false,
						Message = "Article not found."
					};
				}

				if (article.IsDeleted == true)
				{
					return new ResponseModel
					{
						Success = false,
						Message = "Can't delete article is deleted."
					};
				}

				article.Status = "Trashed";
				_unitOfWork.ArticleRepository.SoftRemove(article);
				await _unitOfWork.SaveChangeAsync();

				return new ResponseModel
				{
					Success = true,
					Message = "Article deleted successfully."
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
		#endregion
	}
}
