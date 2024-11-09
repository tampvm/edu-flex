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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(x => x.UserId);
            builder.Property(x => x.FirstName).HasMaxLength(50);
            builder.Property(x => x.LastName).HasMaxLength(50);
            builder.Property(x => x.Username).HasMaxLength(20);
            builder.Property(x => x.Email).HasMaxLength(30);
            builder.Property(x => x.PhoneNumber).HasMaxLength(10);
            builder.Property(x => x.GoogleEmail).HasMaxLength(30);
            builder.Property(x => x.FacebookEmail).HasMaxLength(30);
            builder.HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Avatar)
                .WithMany(x => x.UserAvatars)
                .HasForeignKey(x => x.AvatarId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Background)
                .WithMany(x => x.UserBackgrounds)
                .HasForeignKey(x => x.BackgroundId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.Payments)
                .WithOne(x => x.Teacher)
                .HasForeignKey(x => x.TeacherId);
            builder.HasMany(x => x.UserPackages)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
            builder.HasMany(x => x.PlanbookCollections)
                .WithOne(x => x.Teacher)
                .HasForeignKey(x => x.TeacherId);
            builder.HasMany(x => x.TeachingSchedules)
                .WithOne(x => x.Teacher)
                .HasForeignKey(x => x.TeacherId);
            builder.HasMany(x => x.Feedbacks)
                .WithOne(x => x.Teacher)
                .HasForeignKey(x => x.TeacherId);
            builder.HasMany(x => x.PlanbookShares)
				.WithOne(x => x.User)
				.HasForeignKey(x => x.ShareBy);
            builder.HasMany(x => x.Exams)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
        }
    }
}
