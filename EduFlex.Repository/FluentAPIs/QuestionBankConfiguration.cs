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
	public class QuestionBankConfiguration : IEntityTypeConfiguration<QuestionBank>
	{
		public void Configure(EntityTypeBuilder<QuestionBank> builder)
		{
			builder.ToTable("QuestionBank");
			builder.HasKey(x => x.QuestionId);
			builder.HasOne(x => x.Chapter)
				.WithMany(x => x.QuestionBanks)
				.HasForeignKey(x => x.ChapterId);
			builder.HasOne(x => x.Lesson)
				.WithMany(x => x.QuestionBanks)
				.HasForeignKey(x => x.LessonId);
			builder.HasMany(x => x.Answers)
				.WithOne(x => x.Question)
				.HasForeignKey(x => x.QuestionId);
            builder.HasMany(x => x.QuestionInExams)
                .WithOne(x => x.Question)
                .HasForeignKey(x => x.QuestionId);
        }
	}
}
