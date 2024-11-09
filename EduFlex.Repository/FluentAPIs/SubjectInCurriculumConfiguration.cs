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
    public class SubjectInCurriculumConfiguration : IEntityTypeConfiguration<SubjectInCurriculum>
    {
        public void Configure(EntityTypeBuilder<SubjectInCurriculum> builder)
        {
            builder.ToTable("SubjectInCurriculum");
            builder.HasKey(x => x.SubjectInCurriculumId);
            builder.HasOne(x => x.Subject)
                .WithMany(x => x.SubjectInCurriculums)
                .HasForeignKey(x => x.SubjectId);
            builder.HasOne(x => x.Curriculum)
                .WithMany(x => x.SubjectInCurriculums)
                .HasForeignKey(x => x.CurriculumId);
            builder.HasOne(x => x.Grade)
                .WithMany(x => x.SubjectInCurriculums)
                .HasForeignKey(x => x.GradeId);
            builder.HasMany(x => x.Chapters)
                .WithOne(x => x.SubjectInCurriculum)
                .HasForeignKey(x => x.SubjectInCurriculumId);
        }
    }
}
