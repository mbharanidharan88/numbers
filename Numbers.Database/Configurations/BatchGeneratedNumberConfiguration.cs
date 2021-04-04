using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Numbers.Models.DbModels;

namespace Numbers.Database
{
    internal class BatchGeneratedNumberConfiguration : DbEntityConfiguration<BatchGeneratedNumber>
    {
        public override void Configure(EntityTypeBuilder<BatchGeneratedNumber> entity)
        {
            entity.ToTable("BatchGeneratedNumber");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.GeneratedNumber).IsRequired();
            entity.HasOne(x => x.BatchDetail)
                .WithMany(x => x.GeneratedNumbers);
        }
    }
}
