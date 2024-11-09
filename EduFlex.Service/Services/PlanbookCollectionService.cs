using AutoMapper;
using EduFlex.Domain.Entities;
using EduFlex.Repository.Common;
using EduFlex.Repository.Interfaces;
using EduFlex.Service.Interfaces;
using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.PlanbookCollectionViewModels;
using EduFlex.Service.Models.ViewModels.PlanbookViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Services
{
	public class PlanbookCollectionService : IPlanbookCollectionService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public PlanbookCollectionService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		#region Get All Planbook Collections By Teacher Id
		public async Task<ResponseModel> GetPlanbookCollectionsByTeacherIdAsync(string teacherId, int pageIndex, int pageSize)
		{
			try
			{
				var collections = await _unitOfWork.PlanbookCollectionRepository.GetAsync(
									filter: r => r.TeacherId == teacherId && r.IsDeleted == false,
									pageIndex: pageIndex,
									pageSize: pageSize
								 );

				var mapper = _mapper.Map<Pagination<ViewListPlanbookCollectionDTO>>(collections);

				// Get planbook count for each collection
				//	foreach (var collection in mapper.Items)
				//	{
				//		var planbooks = await _unitOfWork.PlanbookRepository.GetAsync(
				//							filter: r => r.CollectionId == collection.CollectionId
				//									&& r.IsDeleted == false
				//									);
				//		collection.PlanbookCount = planbooks.Items.Count;
				//	}

				var collectionIds = mapper.Items.Select(c => c.CollectionId).ToList();
				var planbooks = await _unitOfWork.PlanbookRepository.GetAsync(
									filter: r => collectionIds.Contains(r.CollectionId) && r.IsDeleted == false
								 );

				var planbookCountsByCollection = planbooks.Items
												.GroupBy(p => p.CollectionId)
												.ToDictionary(g => g.Key, g => g.Count());

				// Assign planbook counts to the respective collections
				foreach (var collection in mapper.Items)
				{
					if (planbookCountsByCollection.TryGetValue(collection.CollectionId, out var planbookCount))
					{
						collection.PlanbookCount = planbookCount;
					}
					else
					{
						collection.PlanbookCount = 0;
					}
				}

				return new SuccessResponseModel<object>
				{
					Message = "Planbook Collections retrieved successfully",
					Success = true,
					Data = mapper
				};
			}
			catch (Exception ex)
			{
				return new ResponseModel
				{
					Message = "An error occurred: " + ex.Message,
					Success = false
				};
			}
		}
		#endregion

		#region Get Created Planbook Collections By Teacher Id
		public async Task<ResponseModel> GetCreatedPlanbookCollectionsByTeacherIdAsync(string teacherId, int pageIndex, int pageSize)
		{
			try
			{
				var collections = await _unitOfWork.PlanbookCollectionRepository.GetAsync(
									filter: r => r.TeacherId == teacherId && r.IsDeleted == false && r.IsSaved,
									pageIndex: pageIndex,
									pageSize: pageSize
								 );

				var mapper = _mapper.Map<Pagination<ViewListPlanbookCollectionDTO>>(collections);

				var collectionIds = mapper.Items.Select(c => c.CollectionId).ToList();
				var planbooks = await _unitOfWork.PlanbookRepository.GetAsync(
								filter: r => collectionIds.Contains(r.CollectionId) && r.IsDeleted == false
				);

				var planbookCountsByCollection = planbooks.Items
												.GroupBy(p => p.CollectionId)
												.ToDictionary(g => g.Key, g => g.Count());

				foreach (var collection in mapper.Items)
				{
					collection.PlanbookCount = planbookCountsByCollection
												.TryGetValue(collection.CollectionId, out var count)
												? count
												: 0;
				}

				return new SuccessResponseModel<object>
				{
					Message = "Created Planbook Collections retrieved successfully",
					Success = true,
					Data = collections
				};
			}
			catch (Exception ex)
			{
				return new ResponseModel
				{
					Message = "An error occurred: " + ex.Message,
					Success = false
				};
			}
		}
		#endregion

		#region Get Saved Planbook Collections By Teacher Id
		public async Task<ResponseModel> GetSavedPlanbookCollectionsByTeacherIdAsync(string teacherId, int pageIndex, int pageSize)
		{
			try
			{
				var collections = await _unitOfWork.PlanbookCollectionRepository.GetAsync(
									filter: r => r.TeacherId == teacherId && r.IsDeleted == false && r.IsSaved,
									pageIndex: pageIndex,
									pageSize: pageSize
								 );

				var mapper = _mapper.Map<Pagination<ViewListPlanbookCollectionDTO>>(collections);

				var collectionIds = mapper.Items.Select(c => c.CollectionId).ToList();
				var planbooks = await _unitOfWork.PlanbookRepository.GetAsync(
								filter: r => collectionIds.Contains(r.CollectionId) && r.IsDeleted == false
				);

				var planbookCountsByCollection = planbooks.Items
												.GroupBy(p => p.CollectionId)
												.ToDictionary(g => g.Key, g => g.Count());

				foreach (var collection in mapper.Items)
				{
					collection.PlanbookCount = planbookCountsByCollection
												.TryGetValue(collection.CollectionId, out var count)
												? count
												: 0;
				}

				return new SuccessResponseModel<object>
				{
					Message = "Saved Planbook Collections retrieved successfully",
					Success = true,
					Data = collections
				};
			}
			catch (Exception ex)
			{
				return new ResponseModel
				{
					Message = "An error occurred: " + ex.Message,
					Success = false
				};
			}
		}
		#endregion

		#region Get Planbook Collection By Id
		public async Task<ResponseModel> GetCollectionByIdAsync(string collectionId)
		{
			var collection = await _unitOfWork.PlanbookCollectionRepository.GetByIdAsync(collectionId);
			if (collection == null)
			{
				return new ResponseModel
				{
					Message = "Planbook Collection not found",
					Success = false
				};
			}

			var mapper = _mapper.Map<ViewDetailPlanbookCollectionDTO>(collection);

			var planbooks = await _unitOfWork.PlanbookRepository.GetAsync(
								filter: r => r.CollectionId == collectionId
										&& r.IsDeleted == false
										);
			mapper.Planbooks = _mapper.Map<List<ViewListPlanbookDTO>>(planbooks.Items);
			mapper.PlanbookCount = planbooks.Items.Count;

			return new SuccessResponseModel<object>
			{
				Message = "Planbook Collection retrieved successfully",
				Success = true,
				Data = mapper
			};
		}
		#endregion

		#region Create Planbook Collection
		public async Task<ResponseModel> CreatePlanbookCollectionAsync(CreatePlanbookCollectionDTO model)
		{
			try
			{
				var collection = _mapper.Map<PlanbookCollection>(model);
				await _unitOfWork.PlanbookCollectionRepository.AddAsync(collection);
				await _unitOfWork.SaveChangeAsync();

				return new ResponseModel
				{
					Message = "Planbook Collection created successfully",
					Success = true
				};
			}
			catch (Exception ex)
			{
				return new ResponseModel
				{
					Message = ex.Message,
					Success = false
				};
			}
		}
		#endregion

		#region Update Planbook Collection
		public async Task<ResponseModel> UpdatePlanbookCollectionAsync(UpdatePlanbookCollectionDTO model)
		{
			try
			{
				var collection = await _unitOfWork.PlanbookCollectionRepository.GetByIdAsync(model.CollectionId);
				if (collection == null)
				{
					return new ResponseModel
					{
						Message = "Planbook Collection not found",
						Success = false
					};
				}

				if (collection.TeacherId != model.TeacherId)
				{
					return new ResponseModel
					{
						Message = "You are not authorized to update this collection",
						Success = false
					};
				}

				if (collection.IsDeleted)
				{
					return new ResponseModel
					{
						Message = "Can't modify a deleted Planbook Collection",
						Success = false
					};
				}

				if (model.PlanbookIds == null || !model.PlanbookIds.Any())
				{
					collection.Planbooks.Clear();
				}
				else
				{
					// Retrieve existing planbooks based on provided PlanbookIds
					var existingPlanbooks = await _unitOfWork.PlanbookRepository
										.GetAllAsync(pb => model.PlanbookIds.Contains(pb.PlanbookId));

					// Get list of current Planbook IDs in the collection to avoid duplication
					var currentPlanbookIds = collection.Planbooks.Select(pb => pb.PlanbookId).ToHashSet();

					foreach (var planbook in existingPlanbooks)
					{
						// Only add if the planbook is created by the user when IsSaved is false
						if (!collection.IsSaved && planbook.CreatedBy != model.TeacherId)
						{
							continue;
						}

						// Only add new planbooks that are not already in the collection
						if (!currentPlanbookIds.Contains(planbook.PlanbookId))
						{
							collection.Planbooks.Add(planbook);
						}
					}

					// Remove any planbooks from the collection that are no longer in the provided PlanbookIds
					collection.Planbooks = collection.Planbooks
										.Where(pb => model.PlanbookIds.Contains(pb.PlanbookId))
										.ToList();
				}

				// Map any additional properties to the collection
				_mapper.Map(model, collection);
				_unitOfWork.PlanbookCollectionRepository.Update(collection);
				await _unitOfWork.SaveChangeAsync();

				return new ResponseModel
				{
					Message = "Planbook Collection updated successfully",
					Success = true
				};
			}
			catch (Exception ex)
			{
				return new ResponseModel
				{
					Message = "An error occurred: " + ex.Message,
					Success = false
				};
			}
		}
		#endregion

		#region Delete Planbook Collection
		public async Task<ResponseModel> DeletePlanbookCollectionAsync(string collectionId, string teacherId)
		{
			try
			{
				var collection = await _unitOfWork.PlanbookCollectionRepository.GetByIdAsync(collectionId);
				if (collection == null)
				{
					return new ResponseModel
					{
						Message = "Planbook Collection not found",
						Success = false
					};
				}

				if (collection.TeacherId != teacherId)
				{
					return new ResponseModel
					{
						Message = "You are not authorized to delete this collection",
						Success = false
					};
				}

				if (collection.IsDeleted)
				{
					return new ResponseModel
					{
						Message = "Planbook Collection is already deleted",
						Success = false
					};
				}

				_unitOfWork.PlanbookCollectionRepository.SoftRemove(collection);
				await _unitOfWork.SaveChangeAsync();

				return new ResponseModel
				{
					Message = "Planbook Collection deleted successfully",
					Success = true
				};
			}
			catch (Exception ex)
			{
				return new ResponseModel
				{
					Message = ex.Message,
					Success = false
				};
			}
		}
		#endregion

		#region Save another planbook to collection
		public async Task<ResponseModel> SavePlanbookAsync(SavePlanbookDTO model)
		{
			try
			{
				var planbook = await _unitOfWork.PlanbookRepository.GetByIdAsync(model.PlanbookId);
				if (planbook == null)
				{
					return new ResponseModel
					{
						Success = false,
						Message = "Planbook not found."
					};
				}
				if (planbook.IsDeleted)
				{
					return new ResponseModel
					{
						Success = false,
						Message = "Can't save deleted planbook."
					};
				}

				var collection = await _unitOfWork.PlanbookCollectionRepository.GetByIdAsync(model.CollectionId);

				if (collection == null)
				{
					var existSavedCollection = await _unitOfWork.PlanbookCollectionRepository
						.CheckPlanbookCollectionIsSavedExistByTeacherId(model.TeacherId);

					if (!existSavedCollection)
					{
						var newCollection = _mapper.Map<PlanbookCollection>(model);
						newCollection.CollectionId = Guid.NewGuid().ToString();
						await _unitOfWork.PlanbookCollectionRepository.AddAsync(newCollection);
						await _unitOfWork.SaveChangeAsync();

						newCollection.Planbooks.Add(planbook);
						await _unitOfWork.SaveChangeAsync();
					}
				}
				else
				{
					if (collection.Planbooks.Any(pb => pb.PlanbookId == model.PlanbookId))
					{
						collection.Planbooks.Remove(planbook);
					}
					else
					{
						collection.Planbooks.Add(planbook);
					}
					await _unitOfWork.SaveChangeAsync();
				}

				return new ResponseModel
				{
					Success = true,
					Message = "Planbook saved/unsaved successfully."
				};
			}
			catch (Exception ex)
			{
				return new ResponseModel
				{
					Success = false,
					Message = "An error occurred while saving the planbook: " + ex.Message
				};
			}
		}
		#endregion
	}
}
