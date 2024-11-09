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
    public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.ToTable("Chapter");
            builder.HasKey(x => x.ChapterId);
            builder.HasOne(x => x.SubjectInCurriculum)
                .WithMany(x => x.Chapters)
                .HasForeignKey(x => x.SubjectInCurriculumId);
            builder.HasMany(x => x.Lessons)
                .WithOne(x => x.Chapter)
                .HasForeignKey(x => x.ChapterId);
            builder.HasMany(x => x.QuestionBanks)
                .WithOne(x => x.Chapter)
                .HasForeignKey(x => x.ChapterId);
        }
    }
}
