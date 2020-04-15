using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using TodoProject.ViewModels.Category;

namespace TodoProject.Interfaces.Core
{
    public interface ICategoryService
    {
        Task<int> Create(CreateCategoryModel model);
        ModelStateDictionary ValidationErrors { get; }
    }
}
