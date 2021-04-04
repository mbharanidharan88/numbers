using Microsoft.EntityFrameworkCore;
using Numbers.Models.DbModels;

namespace Numbers.Database
{
    public class NumbersDbContext : DbContext
    {
        public NumbersDbContext(DbContextOptions<NumbersDbContext> options)
     : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.AddConfiguration(new BatchDetailConfiguration());
            modelBuilder.AddConfiguration(new BatchGeneratedNumberConfiguration());
            modelBuilder.AddConfiguration(new BatchMultipliedNumberConfiguration());
        }

        public DbSet<BatchDetail> BatchDetails { get; set; }

        public DbSet<BatchGeneratedNumber> BatchGeneratedNumbers { get; set; }

        public DbSet<BatchMultipliedNumber> BatchMultipliedNumbers { get; set; }

    }
}


