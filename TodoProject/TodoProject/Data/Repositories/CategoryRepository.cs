using TodoProject.Interfaces.Database.Repositories;
using TodoProject.Models;

namespace TodoProject.Data.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, IRepository<Category>
    {
        public CategoryRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
