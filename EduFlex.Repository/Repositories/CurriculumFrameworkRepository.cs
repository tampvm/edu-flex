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
    public class CurriculumFrameworkRepository : GenericRepository<CurriculumFramework>, ICurriculumFrameworkRepository
    {
        public CurriculumFrameworkRepository(AppDbContext dbContext, ITimeService timeService, IClaimsService claimsService) : base(dbContext, timeService, claimsService)
        {
        }

        public async Task<CurriculumFramework?> CurriculumFrameworkExistsAsync(string curriculumFrameworkName)
        {
            return await _dbContext.CurriculumFrameworks.FirstOrDefaultAsync(c => c.Name.Equals(curriculumFrameworkName));
        }
    }
}
