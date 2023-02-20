using AggregationApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AggregationApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ElectricityData> ElectricityData { get; set; }
    }
}
