using System.Net;
namespace Service.Helper
{
    public class ApiResponse
    {
        public ApiResponse(object? data, int statusCode, string message)
        {
            Data = data;
            StatusCode = statusCode;
            Message = message;
        }

        public ApiResponse()
        {
        }

        public object? Data { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResponse SucessResponse(object? data)
        {
            return new ApiResponse(data, (int)HttpStatusCode.OK, "Success");
        }
        public ApiResponse CreatedResponse(object? data)
        {
            return new ApiResponse(data, (int)HttpStatusCode.Created, "Created");
        }
        public ApiResponse BadRequestResponse(string message)
        {
            return new ApiResponse(null, (int)HttpStatusCode.BadRequest, message);
        }
        public ApiResponse NotFoundResponse(string message)
        {
            return new ApiResponse(null, (int)HttpStatusCode.NotFound, message);
        }
        public ApiResponse UnauthorizedResponse(string message)
        {
            return new ApiResponse(null, (int)HttpStatusCode.Unauthorized, message);
        }
        public ApiResponse ForbiddenResponse(string message)
        {
            return new ApiResponse(null, (int)HttpStatusCode.Forbidden, message);
        }
        public ApiResponse InternalServerErrorResponse(string message)
        {
            return new ApiResponse(null, (int)HttpStatusCode.InternalServerError, message);
        }
    }
}

