using EduFlex.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Repository.Interfaces
{
    public interface IChapterRepository : IGenericRepository<Chapter>
    {
        Task<Chapter?> GetChapterByNameAndSubjectInCurriculumAsync(string chapterName, string subjectInCurriculumId);
    }
}
