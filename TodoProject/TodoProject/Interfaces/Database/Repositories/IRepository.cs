namespace TodoProject.Interfaces.Database.Repositories
{
    public interface IRepository<T> where T : class
    {
        void Create(T model);
        void Update(T model);
    }
}
