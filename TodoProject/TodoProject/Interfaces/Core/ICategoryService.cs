using Microsoft.AspNetCore.Mvc.ModelBinding;
using TodoProject.ViewModels.Category;

namespace TodoProject.Interfaces.Core
{
    public interface ICategoryService
    {
        ModelStateDictionary Create(CreateCategoryModel model);
    }
}
