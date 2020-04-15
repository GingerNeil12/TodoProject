using System.Threading.Tasks;
using TodoProject.Interfaces.Core;
using TodoProject.Interfaces.General;
using TodoProject.ResponseModels;
using TodoProject.ViewModels.Category;

namespace TodoProject.Commands.Core
{
    public class UpdateCategoryCommand : BaseCommand, ICommand<UpdateCategoryModel>
    {
        private const int ERROR_CODE = -1;

        private readonly ICategoryService _categoryService;

        public async Task<ResponseMessage> RunAsync(UpdateCategoryModel model)
        {
            var result = await _categoryService.UpdateAsync(model);
            if (result == ERROR_CODE)
            {
                return BadRequestResponse(_categoryService.ValidationErrors);
            }
            return OkResponse(result);
        }
    }
}
