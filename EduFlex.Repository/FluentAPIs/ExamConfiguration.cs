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
    public class ExamConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.ToTable("Exam");
            builder.HasKey(x => x.ExamId);
            builder.Property(x => x.Title).HasMaxLength(100);
            builder.Property(x => x.Time).HasMaxLength(20);
            builder.HasOne(x => x.User)
                .WithMany(x => x.Exams)
                .HasForeignKey(x => x.UserId);
            builder.HasMany(x => x.QuestionInExams)
                .WithOne(x => x.Exam)
                .HasForeignKey(x => x.ExamId);
        }
    }
}
