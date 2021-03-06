﻿using System.Threading.Tasks;
using TodoProject.Interfaces.Core;
using TodoProject.Interfaces.General;
using TodoProject.ResponseModels;
using TodoProject.ViewModels.Category;

namespace TodoProject.Commands.Core
{
    public class CreateCategoryCommand : BaseCommand, ICommand<CreateCategoryModel>
    {
        private const int ERROR_CODE = -1;

        private readonly ICategoryService _categoryService;

        public CreateCategoryCommand(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<ResponseMessage> RunAsync(CreateCategoryModel model)
        {
            var createCategoryResult = await _categoryService.CreateAsync(model);
            if (createCategoryResult == ERROR_CODE)
            {
                return BadRequestResponse(_categoryService.ValidationErrors);
            }
            return CreatedResponse(createCategoryResult);
        }
    }
}
