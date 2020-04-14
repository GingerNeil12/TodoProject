using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using TodoProject.ViewModels.Account;

namespace TodoProject.Interfaces.Account
{
    public interface IUserService
    {
        Task<ModelStateDictionary> RegisterAccountAsync(RegisterModel model);
        Task<ModelStateDictionary> UpdateAccountAsync(UpdateAccountModel model);
    }
}
