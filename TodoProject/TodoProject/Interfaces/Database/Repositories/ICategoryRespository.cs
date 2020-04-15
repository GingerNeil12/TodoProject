using TodoProject.Models;

namespace TodoProject.Interfaces.Database.Repositories
{
    public interface ICategoryRespository : IRepository<Category>
    {
        bool DoesCategoryExistAlready(string name);
    }
}
