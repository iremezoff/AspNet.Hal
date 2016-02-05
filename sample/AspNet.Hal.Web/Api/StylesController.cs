using System.Linq;
using System.Net;
using AspNet.Hal.Web.Api.Resources;
using AspNet.Hal.Web.Data;
using Microsoft.AspNet.Mvc;

namespace AspNet.Hal.Web.Api
{
    [Route("[controller]")]
    public class StylesController : Controller
    {
        readonly IBeerDbContext beerDbContext;

        public StylesController(IBeerDbContext beerDbContext)
        {
            this.beerDbContext = beerDbContext;
        }

        [HttpGet]
        public BeerStyleListRepresentation Get()
        {
            var beerStyles = beerDbContext.Styles
                .ToList()
                .Select(s => new BeerStyleRepresentation
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToList();

            return new BeerStyleListRepresentation(beerStyles);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var beerStyle = beerDbContext.Styles.SingleOrDefault(s => s.Id == id);
            if (beerStyle == null)
                return HttpNotFound();

            var beerStyleResource = new BeerStyleRepresentation
            {
                Id = beerStyle.Id,
                Name = beerStyle.Name
            };

            return Ok(beerStyleResource);
        }
    }
}