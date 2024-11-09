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
    public class ChapterRepository : GenericRepository<Chapter>, IChapterRepository
    {
        public ChapterRepository(AppDbContext dbContext, ITimeService timeService, IClaimsService claimsService) : base(dbContext, timeService, claimsService)
        {
        }

        public async Task<Chapter?> GetChapterByNameAndSubjectInCurriculumAsync(string chapterName, string subjectInCurriculumId)
        {
            return await _dbContext.Chapters.FirstOrDefaultAsync(c => c.Name.Equals(chapterName) && c.SubjectInCurriculumId.Equals(subjectInCurriculumId));
        }
    }
}
