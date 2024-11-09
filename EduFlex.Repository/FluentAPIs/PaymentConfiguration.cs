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
	public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
	{
		public void Configure(EntityTypeBuilder<Payment> builder)
		{
			builder.ToTable("Payment");
			builder.HasKey(x => x.PaymentId);
			builder.HasOne(x => x.Teacher)
				.WithMany(x => x.Payments)
				.HasForeignKey(x => x.TeacherId);
            builder.HasOne(x => x.UserPackage)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.UserPackageId);
        }
	}
}
