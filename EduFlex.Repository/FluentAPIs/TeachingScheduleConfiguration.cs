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
    public class TeachingScheduleConfiguration : IEntityTypeConfiguration<TeachingSchedule>
    {
        public void Configure(EntityTypeBuilder<TeachingSchedule> builder)
        {
            builder.ToTable("TeachingSchedule");
            builder.HasKey(x => x.ScheduleId);
            builder.Property(x => x.ClassName).HasMaxLength(20).IsRequired();
            builder.HasOne(x => x.Teacher)
                .WithMany(x => x.TeachingSchedules)
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Planbook)
                .WithMany(x => x.TeachingSchedules)
                .HasForeignKey(x => x.PlanbookId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
