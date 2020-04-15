using System;
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
        private const string NAME_PROPERTY = nameof(CreateCategoryModel.Name);

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IUnitOfWork unitOfWork,
            ILogger<CategoryService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public ModelStateDictionary Create(CreateCategoryModel model)
        {
            _logger.LogInformation($"Creating Category: {model.Name}");

            var modelState = new ModelStateDictionary();

            // Check to see if the Category exists

            var category = new Category()
            {
                Name = model.Name.CapitalizeFirstLetter()
            };

            try
            {
                _unitOfWork.CategoryRespository.Create(category);
                _unitOfWork.SaveChangeAsync().Wait();
                return modelState;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Creating Category: {model.Name} " +
                    $"|| {ex.Message} " +
                    $"|| {ex.StackTrace}");

                throw new Exception("Create Category", ex);
            }
        }
    }
}
