using EduFlex.Domain.Entities;
using EduFlex.Repository.Data;
using EduFlex.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Repository.Repositories
{
    public class QuestionInExamRepository : IQuestionInExamRepository
    {
        private readonly AppDbContext _dbContext;

        public QuestionInExamRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
