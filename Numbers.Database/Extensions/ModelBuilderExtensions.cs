using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Numbers.Database
{
    internal static class ModelBuilderExtensions
    {
        public static void AddConfiguration<TEntity>(
          this ModelBuilder modelBuilder,
          DbEntityConfiguration<TEntity> entityConfiguration) where TEntity : class
        {
            modelBuilder.Entity<TEntity>(entityConfiguration.Configure);
        }
    }
}
