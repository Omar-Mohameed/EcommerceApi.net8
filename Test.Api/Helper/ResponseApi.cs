
using System.Text.Json.Serialization;

namespace Ecom.Api.Helper
{
    public class ResponseApi // Wrapper Response
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? Data { get; set; }
        public ResponseApi(int statuscode, string message=null, object? data=null)
        {
            StatusCode = statuscode;
            Message = message ?? GetMsgFromStatusCode(statuscode);
            Data = data;
        }

        private string? GetMsgFromStatusCode(int statuscode)
        {
            return statuscode switch
            {
                200 => "OK",
                201 => "Created",
                204 => "No Content",
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                500 => "Internal Server Error",
                _ => null,
            };
        }
    }
}
