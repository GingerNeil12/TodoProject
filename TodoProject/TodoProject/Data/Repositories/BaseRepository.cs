using System;

using Microsoft.EntityFrameworkCore;

using TodoProject.Interfaces.Database.Repositories;

namespace TodoProject.Data.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        protected DbSet<T> Set { get; }

        protected BaseRepository(ApplicationDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("ApplicationDbContext cannot be null.");
            }

            _context = context;
            Set = context.Set<T>();
        }

        public void Create(T model)
        {
            Set.Add(model);
        }

        public void Update(T model)
        {
            var entry = _context.Entry(model);
            if(entry.State == EntityState.Detached)
            {
                _context.Attach(model);
            }
            entry.State = EntityState.Modified;
        }
    }
}
