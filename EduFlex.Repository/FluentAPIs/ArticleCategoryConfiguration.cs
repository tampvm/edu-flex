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
    public class ArticleCategoryConfiguration : IEntityTypeConfiguration<ArticleCategory>
    {
        public void Configure(EntityTypeBuilder<ArticleCategory> builder)
        {
            builder.ToTable("ArticleCategory");
            builder.HasKey(x => new { x.ArticleId, x.CategoryId });
            builder.HasOne(x => x.Article)
                .WithMany(x => x.ArticleCategories)
                .HasForeignKey(x => x.ArticleId);
            builder.HasOne(x => x.Category)
                .WithMany(x => x.ArticleCategories)
                .HasForeignKey(x => x.CategoryId);
        }
    }
}
