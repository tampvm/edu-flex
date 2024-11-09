using EduFlex.Service.Models.ResponseModels;
using EduFlex.Service.Models.ViewModels.ArticleViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Interfaces
{
    public interface IArticleService
    {
		Task<ResponseModel> GetAllArticleAsync(int pageIndex, int pageSize);
		Task<ResponseModel> GetAllArticlesByCategoryIdAsync(string categoryId, int pageIndex, int pageSize);
		Task<ResponseModel> GetArticleByIdAsync(string id);
		Task<ResponseModel> CreateArticleAsync(CreateArticleDTO model);
		Task<ResponseModel> UpdateArticleAsync(UpdateArticleDTO model);
		Task<ResponseModel> DeleteArticleAsync(string id);
	}
}
