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
	public class ImageConfiguration : IEntityTypeConfiguration<Image>
	{
		public void Configure(EntityTypeBuilder<Image> builder)
		{
			builder.ToTable("Image");
			builder.HasKey(x => x.ImageId);
			builder.HasMany(x => x.UserAvatars)
				.WithOne(x => x.Avatar)
				.HasForeignKey(x => x.AvatarId);
			builder.HasMany(x => x.UserBackgrounds)
				.WithOne(x => x.Background)
				.HasForeignKey(x => x.BackgroundId);
		}
	}
}
