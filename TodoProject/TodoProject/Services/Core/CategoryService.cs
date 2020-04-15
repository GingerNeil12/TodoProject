using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using TodoProject.Interfaces.Core;
using TodoProject.Interfaces.Database;
using TodoProject.ViewModels.Category;

namespace TodoProject.Services.Core
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IUnitOfWork unitOfWork,
            ILogger<CategoryService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public ModelStateDictionary ValidationErrors { get; private set; }

        public int Create(CreateCategoryModel model)
        {
            _logger.LogInformation($"Creating Category: {model.Name}");
            throw new NotImplementedException();
        }
    }
}
