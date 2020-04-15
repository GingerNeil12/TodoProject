using System;
using System.Threading.Tasks;
using TodoProject.Interfaces.Database.Repositories;
using TodoProject.Models;

namespace TodoProject.Interfaces.Database
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRespository CategoryRespository { get; }
        Task<int> SaveChangeAsync();
    }
}
