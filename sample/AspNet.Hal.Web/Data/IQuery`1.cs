using System.Collections.Generic;

namespace AspNet.Hal.Web.Data
{
    public interface IQuery<out TResult>
    {
        IEnumerable<TResult> Execute(IBeerDbContext dbContext);
    }
}