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
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.ToTable("Lesson");
            builder.HasKey(x => x.LessonId);
            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
            builder.HasOne(x => x.Chapter)
                .WithMany(x => x.Lessons)
                .HasForeignKey(x => x.ChapterId);
            builder.HasMany(x => x.Planbooks)
                .WithOne(x => x.Lesson)
                .HasForeignKey(x => x.LessonId);
            builder.HasMany(x => x.QuestionBanks)
                .WithOne(x => x.Lesson)
                .HasForeignKey(x => x.LessonId);
        }
    }
}
