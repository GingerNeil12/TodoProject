using System.Threading.Tasks;
using TodoProject.Interfaces.Core;
using TodoProject.Interfaces.General;
using TodoProject.Models;
using TodoProject.ResponseModels;
using TodoProject.ViewModels.Category;

namespace TodoProject.Commands.Core
{
    public class CreateCategoryCommand : BaseCommand, ICommand<CreateCategoryModel>
    {
        private const string CATEGORYID_PROPERTY = nameof(Category.Id);

        private readonly ICategoryService _categoryService;

        public CreateCategoryCommand(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public Task<ResponseMessage> RunAsync(CreateCategoryModel model)
        {
            return null;
        }
    }
}
