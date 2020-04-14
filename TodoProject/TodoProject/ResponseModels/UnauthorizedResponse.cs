using System.Net;

namespace TodoProject.ResponseModels
{
    public class UnauthorizedResponse : ApiResponse
    {
        public UnauthorizedResponse(string title = null)
            : base((int)HttpStatusCode.Unauthorized, title)
        {

        }
    }
}
