using System.Threading.Tasks;
using TodoProject.Models;

namespace TodoProject.Interfaces.Database.Repositories
{
    public interface ICategoryRespository : IRepository<Category>
    {
        Task<bool> DoesCategoryExistAlready(string name);
        Task<Category> GetByNameAsync(string name);
        Task<Category> GetByIdAsync(int id);
    }
}
