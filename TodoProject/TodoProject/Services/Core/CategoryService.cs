using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
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

        private const int ERROR_CODE = -1;

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IUnitOfWork unitOfWork,
            ILogger<CategoryService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public ModelStateDictionary ValidationErrors { get; private set; }

        public async Task<int> CreateAsync(CreateCategoryModel model)
        {
            _logger.LogInformation($"Creating Category: {model.Name}");

            if (await _unitOfWork.CategoryRespository.DoesCategoryExistAlready(model.Name))
            {
                _logger.LogError($"Category already exists: {model.Name}");
                ValidationErrors = new ModelStateDictionary();
                ValidationErrors.AddModelError(NAME_PROPERTY, "Category already exists.");
                return ERROR_CODE;
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

        public async Task<int> UpdateAsync(UpdateCategoryModel model)
        {
            _logger.LogInformation($"Updating Category: {model.CategoryId}");

            var category = await _unitOfWork.CategoryRespository.GetByIdAsync(model.CategoryId);
            if(category.Name.ToUpper().Equals(model.Name.ToUpper()))
            {
                return model.CategoryId;
            }

            var categoryExists = await _unitOfWork
                .CategoryRespository
                .DoesCategoryExistAlready(model.Name);

            if(categoryExists)
            {
                _logger.LogError($"Category already exists: {model.Name}");
                ValidationErrors = new ModelStateDictionary();
                ValidationErrors.AddModelError(NAME_PROPERTY, "Category already exists.");
                return ERROR_CODE;
            }

            category.Name = model.Name.CapitalizeFirstLetter();

            try
            {
                _unitOfWork.CategoryRespository.Update(category);
                var result = await _unitOfWork.SaveChangeAsync();
                _logger.LogInformation($"Category updated: {model.Name}");
                return model.CategoryId;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Exception updating Category: {model.Name} " +
                    $"|| {ex.Message} " +
                    $"|| {ex.StackTrace}");

                throw new Exception("Create Category Exception", ex);
            }
        }
    }
}
