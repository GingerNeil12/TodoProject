using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using TodoProject.ViewModels.Category;

namespace TodoProject.Interfaces.Core
{
    public interface ICategoryService
    {
        Task<int> CreateAsync(CreateCategoryModel model);
        Task<int> UpdateAsync(UpdateCategoryModel model);
        ModelStateDictionary ValidationErrors { get; }
    }
}
