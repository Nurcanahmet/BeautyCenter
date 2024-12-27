using Guzellik.Models;
using Microsoft.EntityFrameworkCore;

namespace Guzellik.Models
{
    public class RandevuDbContext: DbContext
    {
        public RandevuDbContext(DbContextOptions<RandevuDbContext> options) : base(options)
        { }

        public DbSet<Randevu> Randevus { get; set; }

        
    }
}


