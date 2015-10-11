namespace Twitter.Data.Repositories
{
    using System.Linq;

    public interface IRepository<T>
    {
        IQueryable<T> All();

        T Find(object id);

        void Add(T entity);

        T Update(T entity);

        T Delete(T entity);

        T Delete(object id);
    }
}