using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AspNet.Hal.Web.Data;
using AspNet.Hal.Web.Models;

namespace AspNet.Hal.Web
{
    public class BeerRepository<TBeer> : IRepository<TBeer> where TBeer : class
    {
        readonly IBeerDbContext beerDbContext;

        public BeerRepository(IBeerDbContext beerDbContext)
        {
            this.beerDbContext = beerDbContext;
        }

        public TBeer Get(object id)
        {
            // I know, this is dirty kludge but there is no support for Find method in EF7. So I don't want to introduce interface into original "domain"
            ParameterExpression param = Expression.Parameter(typeof(TBeer));
            Expression<Func<TBeer, bool>> findEmulationExpr =
                Expression.Lambda<Func<TBeer, bool>>(
                    Expression.Equal(Expression.Property(param, typeof(TBeer).GetProperty("Id")),
                        Expression.Constant(id)), param);

            return beerDbContext.Set<TBeer>().SingleOrDefault(findEmulationExpr);
        }

        public IEnumerable<TBeer> FindAll()
        {
            return beerDbContext.Set<TBeer>();
        }

        public IEnumerable<TBeer> Find(IQuery<TBeer> query)
        {
            return query.Execute(beerDbContext);
        }

        public PagedResult<TBeer> Find(IPagedQuery<TBeer> query, int pageNumber, int itemsPerPage)
        {
            return query.Execute(beerDbContext, (pageNumber - 1) * itemsPerPage, itemsPerPage);
        }

        public TBeer FindFirst(IQuery<TBeer> query)
        {
            return query.Execute(beerDbContext).First();
        }

        public TBeer FindFirstOrDefault(IQuery<TBeer> query)
        {
            return query.Execute(beerDbContext).FirstOrDefault();
        }


        public TBeer FindFirstOrDefault(IPagedQuery<TBeer> query)
        {
            return query.Execute(beerDbContext, 0, 1).FirstOrDefault();
        }

        public void Execute(ICommand command)
        {
            command.Execute(beerDbContext);
        }

        public void Add(TBeer entity)
        {
            beerDbContext.Set<TBeer>().Add(entity);
            beerDbContext.SaveChanges();
        }

        public void Remove(TBeer entity)
        {
            beerDbContext.Set<TBeer>().Remove(entity);
            beerDbContext.SaveChanges();
        }
    }
}