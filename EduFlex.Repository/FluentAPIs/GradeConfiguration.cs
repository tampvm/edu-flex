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
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.ToTable("Grade");
            builder.HasKey(x => x.GradeId);
            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
            builder.HasMany(x => x.SubjectInCurriculums)
                .WithOne(x => x.Grade)
                .HasForeignKey(x => x.GradeId);
        }
    }
}
