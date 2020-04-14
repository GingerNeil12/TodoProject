using System;
using System.Net;

namespace TodoProject.ResponseModels
{
    public class ApiResponse
    {
        public int Status { get; }
        public string Title { get; }
        public DateTime RequestDate => DateTime.Now;

        public ApiResponse(int status, string title = null)
        {
            Status = status;
            Title = title ?? GetDefaultResponseForStatus(status);
        }

        private static string GetDefaultResponseForStatus(int status)
        {
            switch (status)
            {
                case (int)HttpStatusCode.OK:
                    return "Request completed successfully.";
                case (int)HttpStatusCode.Created:
                    return "Resource has been Created.";
                case (int)HttpStatusCode.BadRequest:
                    return "One or more validation errors found.";
                case (int)HttpStatusCode.Unauthorized:
                    return "Access to the requested resource is Unauthorized";
                case (int)HttpStatusCode.Forbidden:
                    return "Access to the requested resource is Forbidden";
                case (int)HttpStatusCode.InternalServerError:
                    return "An unhandled error occured.";
                default:
                    return string.Empty;
            }
        }
    }
}
