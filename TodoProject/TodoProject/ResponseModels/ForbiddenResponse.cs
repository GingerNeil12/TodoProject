using System.Net;

namespace TodoProject.ResponseModels
{
    public class ForbiddenResponse : ApiResponse
    {
        public ForbiddenResponse()
            : base((int)HttpStatusCode.Forbidden)
        {

        }
    }
}
