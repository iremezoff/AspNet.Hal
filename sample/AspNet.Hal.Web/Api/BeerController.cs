using System.Linq;
using AspNet.Hal.Web.Api.Resources;
using AspNet.Hal.Web.Data;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;

namespace AspNet.Hal.Web.Api
{
    [Route("[controller]/{id}")]
    public class BeerController : Controller
    {
        readonly IBeerDbContext beerDbContext;

        public BeerController(IBeerDbContext beerDbContext)
        {
            this.beerDbContext = beerDbContext;
        }

        [HttpGet]
        // GET beers/5
        public BeerRepresentation Get(int id)
        {
            var beer = beerDbContext.Beers.Include(b => b.Brewery).Include(b => b.Style).Single(br => br.Id == id); // lazy loading isn't on for this query; force loading

            return new BeerRepresentation
            {
                Id = beer.Id,
                Name = beer.Name,
                BreweryId = beer.Brewery == null ? (int?)null : beer.Brewery.Id,
                BreweryName = beer.Brewery == null ? null : beer.Brewery.Name,
                StyleId = beer.Style == null ? (int?)null : beer.Style.Id,
                StyleName = beer.Style == null ? null : beer.Style.Name,
                ReviewIds = beerDbContext.Reviews.Where(r => r.Beer_Id == id).Select(r => r.Id).ToList()
            };
        }

        [HttpPut]
        // PUT beers/5
        public void Put(int id, string value)
        {
        }

        [HttpDelete]
        // DELETE beers/5
        public void Delete(int id)
        {
        }
    }
}