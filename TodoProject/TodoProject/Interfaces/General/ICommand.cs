using System.Threading.Tasks;
using TodoProject.ResponseModels;

namespace TodoProject.Interfaces.General
{
    public interface ICommand<T> where T : class
    {
        Task<ResponseMessage> RunAsync(T model);
    }
}
