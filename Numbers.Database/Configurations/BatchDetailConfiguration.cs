using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Numbers.Models.DbModels;

namespace Numbers.Database
{
    internal class BatchDetailConfiguration : DbEntityConfiguration<BatchDetail>
    {
        public override void Configure(EntityTypeBuilder<BatchDetail> entity)
        {
            entity.ToTable("BatchDetail");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.BatchId).IsRequired();
            entity.Property(c => c.NumberOfBatches).IsRequired();
            entity.Property(c => c.NumberPerBatches).IsRequired();
            entity.Property(c => c.BatchTotal).IsRequired();
            entity.HasMany(c => c.GeneratedNumbers);
            entity.HasMany(c => c.MultipliedNumbers);
        }
    }
}
