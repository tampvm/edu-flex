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
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.ToTable("Feedback");
            builder.HasKey(x => x.FeedbackId);
            builder.HasOne(x => x.Teacher)
                .WithMany(x => x.Feedbacks)
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Planbook)
                .WithMany(x => x.Feedbacks)
                .HasForeignKey(x => x.PlanbookId)
                .OnDelete(DeleteBehavior.Restrict); // Ngăn xóa cascade
        }
    }
}
