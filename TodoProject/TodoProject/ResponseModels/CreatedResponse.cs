using System.Net;

namespace TodoProject.ResponseModels
{
    public class CreatedResponse : ApiResponse
    {
        public object Id { get; }

        public CreatedResponse(object id)
            : base((int)HttpStatusCode.Created)
        {
            Id = id;
        }
    }
}
