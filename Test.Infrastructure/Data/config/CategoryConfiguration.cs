using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Entities.Product;

namespace Test.Infrastructure.Data.config
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c=>c.Name).IsRequired().HasMaxLength(30);
            builder.Property(c => c.Id).IsRequired();
            builder.HasData(
                new Category { Id = 1, Name = "test", Description = "test" }
                );
        }
    }
}
