using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Numbers.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Numbers.Database
{
    internal class BatchMultipliedNumberConfiguration : DbEntityConfiguration<BatchMultipliedNumber>
    {
        public override void Configure(EntityTypeBuilder<BatchMultipliedNumber> entity)
        {
            entity.ToTable("BatchMultipliedNumber");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.MultipliedNumber).IsRequired();
            entity.HasOne(x => x.BatchDetail)
                .WithMany(x => x.MultipliedNumbers);
        }
    }
}
