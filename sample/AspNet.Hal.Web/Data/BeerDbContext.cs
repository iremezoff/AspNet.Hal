using AspNet.Hal.Web.Models;
using DbUp;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;

namespace AspNet.Hal.Web.Data
{
    public class BeerDbContext : DbContext, IBeerDbContext
    {
        public DbSet<Beer> Beers { get; set; }
        public DbSet<BeerStyle> Styles { get; set; }
        public DbSet<Brewery> Breweries { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public BeerDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var beerMap = modelBuilder.Entity<Beer>();
            beerMap.HasKey(e => e.Id);
            beerMap.Property(e => e.Id).UseSqlServerIdentityColumn();
            beerMap.HasOne(e => e.Brewery).WithMany();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);

        //    optionsBuilder.UseSqlServer(@"Server=(localDb)\v11.0;Database=QuizPromo;Trusted_Connection=True;Integrated Security=True");
        //}
    }
}