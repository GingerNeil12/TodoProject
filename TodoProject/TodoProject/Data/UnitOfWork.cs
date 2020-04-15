using System.Threading.Tasks;
using TodoProject.Data.Repositories;
using TodoProject.Interfaces.Database;
using TodoProject.Interfaces.Database.Repositories;

namespace TodoProject.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private ICategoryRespository _categoryRepository;

        public ICategoryRespository CategoryRespository
        {
            get
            {
                if (_categoryRepository == null)
                {
                    _categoryRepository = new CategoryRepository(_context);
                }
                return _categoryRepository;
            }
        }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangeAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            _categoryRepository = null;
        }
    }
}
