using Microsoft.AspNetCore.Mvc.ModelBinding;
using TodoProject.ViewModels.Category;

namespace TodoProject.Interfaces.Core
{
    public interface ICategoryService
    {
        int Create(CreateCategoryModel model);
        ModelStateDictionary ValidationErrors { get; }
    }
}
