using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contexts
{
    public class WeatherContext : DbContext
    {
        public WeatherContext(DbContextOptions<WeatherContext> options)
            : base(options)
        {
        }

        public DbSet<WeatherRecord> WeatherRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WeatherRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}
