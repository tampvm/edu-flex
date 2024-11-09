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
	public class ArticleConfiguration : IEntityTypeConfiguration<Article>
	{
		public void Configure(EntityTypeBuilder<Article> builder)
		{
			builder.ToTable("Article");
			builder.HasKey(x => x.ArticleId);
			builder.HasMany(x => x.ArticleCategories)
				.WithOne(x => x.Article)
				.HasForeignKey(x => x.ArticleId);
        }
	}
}
