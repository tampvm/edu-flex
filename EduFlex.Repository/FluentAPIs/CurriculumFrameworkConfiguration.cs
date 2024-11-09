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
    public class CurriculumFrameworkConfiguration : IEntityTypeConfiguration<CurriculumFramework>
    {
        public void Configure(EntityTypeBuilder<CurriculumFramework> builder)
        {
            builder.ToTable("CurriculumFramework");
            builder.HasKey(x => x.CurriculumId);
            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
            builder.HasMany(x => x.SubjectInCurriculums)
                .WithOne(x => x.Curriculum)
                .HasForeignKey(x => x.CurriculumId);
        }
    }
}
