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
    public class PlanbookCollectionConfiguration : IEntityTypeConfiguration<PlanbookCollection>
    {
        public void Configure(EntityTypeBuilder<PlanbookCollection> builder)
        {
            builder.ToTable("PlanbookCollection");
            builder.HasKey(x => x.CollectionId);
            builder.HasOne(x => x.Teacher)
                .WithMany(x => x.PlanbookCollections)
                .HasForeignKey(x => x.TeacherId);
            builder.HasMany(x => x.Planbooks)
                .WithOne(x => x.PlanbookCollection)
                .HasForeignKey(x => x.CollectionId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa Planbooks khi xóa PlanbookCollection
        }
    }
}
