using EduFlex.Domain.Entities;
using EduFlex.Repository.Data;
using EduFlex.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Repository.Repositories
{
    public class ArticleImageRepository : IArticleImageRepository
    {
        private readonly AppDbContext _dbContext;

        public ArticleImageRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

		public async Task<ArticleImage?> GetByArticleIdAsync(string articleId)
		{
			return await _dbContext.ArticleImages
				.Where(ai => ai.ArticleId == articleId)
				.FirstOrDefaultAsync();
		}
	}
}
