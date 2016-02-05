using System.Linq;
using AspNet.Hal.Web.Api.Resources;
using AspNet.Hal.Web.Data;
using AspNet.Hal.Web.Data.Queries;
using Microsoft.AspNet.Mvc;

namespace AspNet.Hal.Web.Api
{
    [Route("styles/{id}/beers")]
    public class BeersFromStyleController : Controller
    {
        readonly IRepository<BeerRepresentation> repository;

        public BeersFromStyleController(IRepository<BeerRepresentation> repository)
        {
            this.repository = repository;
        }

        public BeerListRepresentation Get(int id, int page = 1)
        {
            var beers = repository.Find(new GetBeersQuery(b => b.Style.Id == id), page, BeersController.PageSize);
            var resourceList = new BeerListRepresentation(
                beers.ToList(), beers.TotalResults, beers.TotalPages, page,
                LinkTemplates.BeerStyles.AssociatedBeers, new { id });
            return resourceList;
        }
    }
}