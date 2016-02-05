using System.Linq;
using AspNet.Hal.Web.Api.Resources;
using AspNet.Hal.Web.Data;
using AspNet.Hal.Web.Data.Queries;
using Microsoft.AspNet.Mvc;

namespace AspNet.Hal.Web.Api
{
    [Route("breweries/{id}/beers")]
    public class BeersFromBreweryController : Controller
    {
        readonly IRepository<BeerRepresentation> repository;

        public BeersFromBreweryController(IRepository<BeerRepresentation> repository)
        {
            this.repository = repository;
        }

        //[HttpGet("{id}")]
        public BeerListRepresentation Get(int id, int page = 1)
        {
            var beers = repository.Find(new GetBeersQuery(b => b.Brewery.Id == id), page, BeersController.PageSize);
            return new BeerListRepresentation(beers.ToList(), beers.TotalResults, beers.TotalPages, page, LinkTemplates.Breweries.AssociatedBeers, new { id });
        }
    }
}