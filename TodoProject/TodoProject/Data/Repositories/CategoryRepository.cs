using TodoProject.Interfaces.Database.Repositories;
using TodoProject.Models;

namespace TodoProject.Data.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRespository
    {
        public CategoryRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public bool DoesCategoryExistAlready(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}
