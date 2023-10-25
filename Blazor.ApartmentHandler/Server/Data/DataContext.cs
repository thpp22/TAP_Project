using Microsoft.EntityFrameworkCore;
using Blazor.ApartmentHandler.Shared.Entities;

namespace TAP.ApartmentHandler.Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            //Database.EnsureCreated();
        }

        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Bill> Bills { get; set; }

    }
}
