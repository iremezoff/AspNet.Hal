using System;
using System.Linq;
using System.Linq.Expressions;
using AspNet.Hal.Web.Api.Resources;
using AspNet.Hal.Web.Models;
using Microsoft.Data.Entity;

namespace AspNet.Hal.Web.Data.Queries
{
    /// <summary>
    /// Gets a list of beers, with no hypermedia on the resource
    /// </summary>
    public class GetBeersQuery : IPagedQuery<BeerRepresentation>
    {
        readonly Expression<Func<Beer, bool>> where;

        public GetBeersQuery(Expression<Func<Beer, bool>> where = null)
        {
            this.where = where ?? (b => true);
        }

        public PagedResult<BeerRepresentation> Execute(IBeerDbContext dbContext, int skip, int take)
        {
            var beers = dbContext
                .Beers
                .Include(b => b.Brewery).Include(b => b.Style)
                .Where(where)
                .OrderBy(b => b.Name)
                .Skip(skip)
                .Take(take)
                .ToList()
                .Select(b => new BeerRepresentation
                {
                    Id = b.Id,
                    Name = b.Name,
                    //BreweryId = b.Brewery.Id,
                    BreweryName = b.Brewery.Name,
                    //StyleId = b.Style.Id,
                    //StyleName = b.Style.Name
                })
                .ToList();

            var count = dbContext.Beers
                .Where(where)
                .Count();

            return new PagedResult<BeerRepresentation>(beers, count, skip, take);
        }
    }
}