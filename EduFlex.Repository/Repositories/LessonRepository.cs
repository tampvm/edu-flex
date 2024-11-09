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
    public class LessonRepository : GenericRepository<Lesson>, ILessonRepository
    {
        public LessonRepository(AppDbContext dbContext, ITimeService timeService, IClaimsService claimsService) : base(dbContext, timeService, claimsService)
        {
        }

        public async Task<Lesson?> GetLessonByNameAndChapterAsync(string lessonName, string chapterId)
        {
            return await _dbContext.Lessons.FirstOrDefaultAsync(l => l.Name.Equals(lessonName) && l.ChapterId.Equals(chapterId));
        }

        public async Task<Lesson?> GetByIdAsync(string lessonId)
        {
            return await _dbContext.Lessons
                .Include(l => l.Chapter)                                 
                .ThenInclude(c => c.SubjectInCurriculum)                  
                .ThenInclude(s => s.Subject)                              
                .ThenInclude(g => g.SubjectInCurriculums)                 
                .Include(l => l.Chapter.SubjectInCurriculum.Grade)        
                .FirstOrDefaultAsync(l => l.LessonId == lessonId);
        }
    }
}
