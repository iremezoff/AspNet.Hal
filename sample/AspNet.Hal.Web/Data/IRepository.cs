using System.Collections.Generic;

namespace AspNet.Hal.Web.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(object id);
        IEnumerable<TEntity> FindAll();
        IEnumerable<TEntity> Find(IQuery<TEntity> query);
        PagedResult<TEntity> Find(IPagedQuery<TEntity> query, int pageNumber, int itemsPerPage);
        TEntity FindFirst(IQuery<TEntity> query);
        TEntity FindFirstOrDefault(IQuery<TEntity> query);
        TEntity FindFirstOrDefault(IPagedQuery<TEntity> query);
        void Execute(ICommand command);
        void Add(TEntity entity);
        void Remove(TEntity entity);
    }
}