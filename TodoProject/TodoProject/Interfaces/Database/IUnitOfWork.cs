using System;
using System.Threading.Tasks;

namespace TodoProject.Interfaces.Database
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangeAsync();
    }
}
