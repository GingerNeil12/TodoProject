using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TodoProject.Helpers;
using TodoProject.Interfaces.Core;
using TodoProject.Interfaces.Database;
using TodoProject.Models;
using TodoProject.ViewModels.Category;

namespace TodoProject.Services.Core
{
    public class CategoryService : ICategoryService
    {
        private const string NAME_PROPERTY = nameof(Category.Name);

        private const int CREATE_ERROR_CODE = -1;

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IUnitOfWork unitOfWork,
            ILogger<CategoryService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public ModelStateDictionary ValidationErrors { get; private set; }

        public async Task<int> Create(CreateCategoryModel model)
        {
            _logger.LogInformation($"Creating Category: {model.Name}");

            if (_unitOfWork.CategoryRespository.DoesCategoryExistAlready(model.Name))
            {
                _logger.LogError($"Category already exists: {model.Name}");
                ValidationErrors = new ModelStateDictionary();
                ValidationErrors.AddModelError(NAME_PROPERTY, "Category already exists.");
                return CREATE_ERROR_CODE;
            }

            var category = new Category()
            {
                Name = model.Name.CapitalizeFirstLetter()
            };

            try
            {
                _unitOfWork.CategoryRespository.Create(category);
                var result = await _unitOfWork.SaveChangeAsync();
                _logger.LogInformation($"Category Created: {model.Name}");
                return category.Id;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Exception creating Category: {model.Name} " +
                    $"|| {ex.Message} " +
                    $"|| {ex.StackTrace}");

                throw new Exception("Create Category Exception", ex);
            }
        }
    }
}
