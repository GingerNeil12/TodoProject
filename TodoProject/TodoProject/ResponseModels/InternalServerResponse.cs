using System.Net;

namespace TodoProject.ResponseModels
{
    public class InternalServerResponse : ApiResponse
    {
        public InternalServerResponse()
            : base((int)HttpStatusCode.InternalServerError)
        {

        }
    }
}
