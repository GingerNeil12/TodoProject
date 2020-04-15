using System.Linq;
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
            name = name.ToUpper();
            var category = Set
                .Where(x => x.Name.ToUpper().Equals(name))
                .FirstOrDefault();
            return category != null;
        }
    }
}
