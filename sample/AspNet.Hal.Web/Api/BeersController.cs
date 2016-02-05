using System.Linq;
using System.Net;
using AspNet.Hal.Web.Api.Resources;
using AspNet.Hal.Web.Data;
using AspNet.Hal.Web.Data.Queries;
using AspNet.Hal.Web.Models;
using Microsoft.AspNet.Mvc;

namespace AspNet.Hal.Web.Api
{
    [Route("[controller]")]
    public class BeersController : Controller
    {
        public const int PageSize = 5;

        readonly IRepository<Beer> repository;
        private readonly IRepository<BeerRepresentation> _repository2;

        public BeersController(IRepository<Beer> repository, IRepository<BeerRepresentation> repository2)
        {
            this.repository = repository;
            _repository2 = repository2;
        }

        //[HttpGet]
        // GET beers
        public BeerListRepresentation Get(int page = 1)
        {
            var beers = _repository2.Find(new GetBeersQuery(), page, PageSize);

            var resourceList = new BeerListRepresentation(beers.ToList(), beers.TotalResults, beers.TotalPages, page, LinkTemplates.Beers.GetBeers);

            return resourceList;
        }

        [HttpGet("Search")]
        public BeerListRepresentation Search(string searchTerm, int page = 1)
        {
            var beers = _repository2.Find(new GetBeersQuery(b => b.Name.Contains(searchTerm)), page, PageSize);

            // snap page back to actual page found
            if (page > beers.TotalPages) page = beers.TotalPages;

            //var link = LinkTemplates.Beers.SearchBeers.CreateLink(new { searchTerm, page });
            var beersResource = new BeerListRepresentation(beers.ToList(), beers.TotalResults, beers.TotalPages, page,
                                                           LinkTemplates.Beers.SearchBeers,
                                                           new { searchTerm })
            {
                Page = page,
                TotalResults = beers.TotalResults
            };

            return beersResource;
        }

        [HttpPost]
        // POST beers
        public IActionResult Post(BeerRepresentation value)
        {
            var newBeer = new Beer(value.Name);
            repository.Add(newBeer);


            return Created(LinkTemplates.Beers.Beer.CreateUri(new { id = newBeer.Id }), null);
        }
    }
}