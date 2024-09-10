namespace EDRSM.API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "This is a bad request",
                401 => "You are not authorized to make this request",
                404 => "Resource found",
                500 => "An unexpected error occurred on the server. Our team has been notified and is working to resolve the issue. Please try again later.",
                _ => null
            };
        }
    }
}
