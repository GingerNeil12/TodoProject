using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoProject.General;
using TodoProject.Interfaces.General;
using TodoProject.ResponseModels;
using TodoProject.ViewModels.Category;

namespace TodoProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ApplicationController
    {
        private readonly ICommand<CreateCategoryModel> _createCategoryCommand;
        private readonly ICommand<UpdateCategoryModel> _updateCategoryCommand;

        public CategoryController(ICommand<CreateCategoryModel> createCategoryCommand, 
            ICommand<UpdateCategoryModel> updateCategoryCommand)
        {
            _createCategoryCommand = createCategoryCommand;
            _updateCategoryCommand = updateCategoryCommand;
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> CreateCategoryAsync(CreateCategoryModel model)
        {
            if(GetUserId() != model.UserId)
            {
                var forbidden = new ForbiddenResponse();
                return StatusCode(forbidden.Status, forbidden);
            }

            if (ModelState.IsValid)
            {
                var result = await _createCategoryCommand.RunAsync(model);
                return StatusCode(result.Status, result.Payload);
            }
            return new BadRequestObjectResult(new BadRequestResponse(ModelState));
        }

        [HttpPut]
        [Route("update/{id}")]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> UpdateCategoryAsync(int id, UpdateCategoryModel model)
        {
            if(GetUserId() != model.UserId || id != model.CategoryId)
            {
                var forbidden = new ForbiddenResponse();
                return StatusCode(forbidden.Status, forbidden);
            }

            if(ModelState.IsValid)
            {
                var result = await _updateCategoryCommand.RunAsync(model);
                return StatusCode(result.Status, result.Payload);
            }
            return new BadRequestObjectResult(new BadRequestResponse(ModelState));
        }
    }
}
