using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Raicu_Eva_Lab4.Models;
using System.Collections.Generic;

namespace Raicu_Eva_Lab4.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<PredictionHistory> PredictionHistory { get; set; }
    }
}
