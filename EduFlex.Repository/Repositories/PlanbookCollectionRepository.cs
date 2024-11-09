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
	public class PlanbookCollectionRepository : GenericRepository<PlanbookCollection>, IPlanbookCollectionRepository
	{
		public PlanbookCollectionRepository(AppDbContext dbContext, ITimeService timeService, IClaimsService claimsService) : base(dbContext, timeService, claimsService)
		{
		}

		public async Task<bool> CheckPlanbookCollectionIsSavedExistByTeacherId(string teacherId)
		{
			return await _dbContext.PlanbookCollections.AnyAsync(c => c.TeacherId == teacherId && c.IsSaved);
		}
	}
}
