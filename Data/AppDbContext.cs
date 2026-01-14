using Microsoft.EntityFrameworkCore;
using Raicu_Eva_Lab4.Models;

namespace Raicu_Eva_Lab4.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Schimbăm în singular pentru a se potrivi cu apelurile din PredictionController
        public DbSet<PredictionHistory> PredictionHistory { get; set; }
    }
}