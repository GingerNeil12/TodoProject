using System.Net;

namespace TodoProject.ResponseModels
{
    public class OkResponse : ApiResponse
    {
        public object Result { get; }

        public OkResponse(object result)
            : base((int)HttpStatusCode.OK)
        {
            Result = result;
        }
    }
}
