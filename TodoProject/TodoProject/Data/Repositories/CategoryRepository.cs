using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> DoesCategoryExistAlready(string name)
        {
            var category = await GetByNameAsync(name);
            return category != null;
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            name = name.ToUpper();
            var category = await Set
                .Where(x => x.Name.ToUpper().Equals(name))
                .FirstOrDefaultAsync();
            return category;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var category = await Set
                .Where(x => x.Id.Equals(id))
                .FirstOrDefaultAsync();
            return category;
        }
    }
}
