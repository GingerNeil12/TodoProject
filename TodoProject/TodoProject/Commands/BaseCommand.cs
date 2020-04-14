using Microsoft.AspNetCore.Mvc.ModelBinding;
using TodoProject.ResponseModels;

namespace TodoProject.Commands
{
    public abstract class BaseCommand
    {
        protected ResponseMessage OkResponse(object result)
        {
            return CreateResponseMessage(new OkResponse(result));
        }

        protected ResponseMessage CreatedResponse(object id)
        {
            return CreateResponseMessage(new CreatedResponse(id));
        }

        protected ResponseMessage BadRequestResponse(ModelStateDictionary modelState)
        {
            return CreateResponseMessage(new BadRequestResponse(modelState));
        }

        protected ResponseMessage UnauthorizedResponse(string title = null)
        {
            return CreateResponseMessage(new UnauthorizedResponse(title));
        }

        protected ResponseMessage ForbiddenResponse()
        {
            return CreateResponseMessage(new ForbiddenResponse());
        }

        protected ResponseMessage NotFoundResponse(object id)
        {
            return CreateResponseMessage(new NotFoundResponse(id));
        }

        protected ResponseMessage InternalErrorResponse()
        {
            return CreateResponseMessage(new InternalServerResponse());
        }

        private ResponseMessage CreateResponseMessage(ApiResponse apiResponse)
        {
            return new ResponseMessage()
            {
                Status = apiResponse.Status,
                Payload = apiResponse
            };
        }
    }
}
