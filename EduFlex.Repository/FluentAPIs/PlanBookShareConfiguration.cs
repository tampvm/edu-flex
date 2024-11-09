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
	public class PlanBookShareConfiguration : IEntityTypeConfiguration<PlanBookShare>
	{
		public void Configure(EntityTypeBuilder<PlanBookShare> builder)
		{
			builder.ToTable("PlanBookShare");
			builder.HasKey(x => x.ShareId);
			builder.HasOne(x => x.PlanBook)
				.WithMany(x => x.PlanbookShares)
				.HasForeignKey(x => x.PlanBookId);
			builder.HasOne(x => x.User)
				.WithMany(x => x.PlanbookShares)
				.HasForeignKey(x => x.ShareBy);
		}
	}
}
