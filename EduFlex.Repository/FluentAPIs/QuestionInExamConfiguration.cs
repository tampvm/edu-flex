using EduFlex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Repository.FluentAPIs
{
    public class QuestionInExamConfiguration : IEntityTypeConfiguration<QuestionInExam>
    {
        public void Configure(EntityTypeBuilder<QuestionInExam> builder)
        {
            builder.ToTable("QuestionInExam");
            builder.HasKey(x => x.QuestionInExamId);
            builder.HasOne(x => x.Exam)
                .WithMany(x => x.QuestionInExams)
                .HasForeignKey(x => x.ExamId);
            builder.HasOne(x => x.Question)
                .WithMany(x => x.QuestionInExams)
                .HasForeignKey(x => x.QuestionId);
        }
    }
}
