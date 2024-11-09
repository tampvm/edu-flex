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
	public class ServicePackageConfiguration : IEntityTypeConfiguration<ServicePackage>
	{
		public void Configure(EntityTypeBuilder<ServicePackage> builder)
		{
			builder.ToTable("ServicePackage");
			builder.HasKey(x => x.PackageId);
			builder.Property(x => x.PackageName).HasMaxLength(100);
			builder.Property(x => x.Description);
			builder.HasMany(x => x.UserPackages)
				.WithOne(x => x.Package)
				.HasForeignKey(x => x.PackageId);
		}
	}
}
