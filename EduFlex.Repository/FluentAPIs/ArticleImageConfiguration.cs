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
    public class ArticleImageConfiguration : IEntityTypeConfiguration<ArticleImage>
    {
        public void Configure(EntityTypeBuilder<ArticleImage> builder)
        {
            builder.ToTable("ArticleImage");
            builder.HasKey(x => new { x.ArticleId, x.ImageId });
            builder.HasOne(x => x.Article)
                .WithMany(x => x.ArticleImages)
                .HasForeignKey(x => x.ArticleId);
            builder.HasOne(x => x.Image)
                .WithMany(x => x.ArticleImages)
                .HasForeignKey(x => x.ImageId);
        }
    }
}
