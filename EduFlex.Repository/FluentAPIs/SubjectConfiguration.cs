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
	public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
	{
		public void Configure(EntityTypeBuilder<Subject> builder)
		{
			builder.ToTable("Subject");
			builder.HasKey(x => x.SubjectId);
			builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
			builder.HasMany(x => x.SubjectInCurriculums)
				.WithOne(x => x.Subject)
				.HasForeignKey(x => x.SubjectId);
        }
	}
}
