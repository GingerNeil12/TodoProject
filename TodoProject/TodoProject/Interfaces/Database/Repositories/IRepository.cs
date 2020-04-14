using System.Threading.Tasks;

namespace TodoProject.Interfaces.Database.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task CreateAsync(T model);
        Task UpdateAsync(T model);
    }
}
