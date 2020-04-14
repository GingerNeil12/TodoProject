using System.Net;

namespace TodoProject.ResponseModels
{
    public class NotFoundResponse : ApiResponse
    {
        public NotFoundResponse(object id)
            : base((int)HttpStatusCode.NotFound, $"Resource not found with ID: {id}")
        {

        }
    }
}
