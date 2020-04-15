using System.Threading.Tasks;
using TodoProject.Data.Repositories;
using TodoProject.Interfaces.Database;
using TodoProject.Interfaces.Database.Repositories;
using TodoProject.Models;

namespace TodoProject.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private IRepository<Category> _categoryRepository;

        public IRepository<Category> CategoryRespository
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
