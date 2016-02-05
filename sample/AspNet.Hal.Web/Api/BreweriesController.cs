using System.Linq;
using AspNet.Hal.Web.Api.Resources;
using AspNet.Hal.Web.Data;
using Microsoft.AspNet.Mvc;

namespace AspNet.Hal.Web.Api
{
    [Route("[controller]")]
    public class BreweriesController : Controller
    {
        readonly IBeerDbContext beerDbContext;

        public BreweriesController(IBeerDbContext beerDbContext)
        {
            this.beerDbContext = beerDbContext;
        }

        [HttpGet]
        public BreweryListRepresentation Get()
        {
            var breweries = beerDbContext.Styles
                .ToList()
                .Select(s => new BreweryRepresentation
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToList();

            return new BreweryListRepresentation(breweries);
        }

        [HttpGet("{id}")]
        public BreweryRepresentation Get(int id)
        {
            var brewery = beerDbContext.Breweries.SingleOrDefault(b=>b.Id==id);

            return new BreweryRepresentation
            {
                Id = brewery.Id,
                Name = brewery.Name
            };
        }
    }
}