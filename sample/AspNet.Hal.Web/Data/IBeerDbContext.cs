using AspNet.Hal.Web.Models;
using Microsoft.Data.Entity;

namespace AspNet.Hal.Web.Data
{
    public interface IBeerDbContext
    {
        DbSet<Beer> Beers { get; }
        DbSet<BeerStyle> Styles { get; }
        DbSet<Brewery> Breweries { get; set; }
        DbSet<Review> Reviews { get; set; }
        int SaveChanges();
        DbSet<T> Set<T>() where T : class;
    }
}