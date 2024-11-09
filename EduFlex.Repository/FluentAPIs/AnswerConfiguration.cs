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
	public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
	{
		public void Configure(EntityTypeBuilder<Answer> builder)
		{
			builder.ToTable("Answer");
			builder.HasKey(x => x.AnswerId);
			builder.HasOne(x => x.Question)
				.WithMany(x => x.Answers)
				.HasForeignKey(x => x.QuestionId);
		}
	}
}
