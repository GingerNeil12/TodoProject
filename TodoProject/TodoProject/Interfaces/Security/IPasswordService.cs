using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using TodoProject.ViewModels.Security;

namespace TodoProject.Interfaces.Security
{
    public interface IPasswordService
    {
        Task<ModelStateDictionary> ChangePasswordAsync(ChangePasswordModel model);
        Task ResetPasswordRequestAsync(ResetPasswordRequestModel model);
        Task<ModelStateDictionary> ResetPasswordAsync(ResetPasswordModel model);
    }
}
