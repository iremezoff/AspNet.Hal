namespace AspNet.Hal.Web.Data
{
    public interface ICommand
    {
        void Execute(IBeerDbContext dbContext);
    }
}