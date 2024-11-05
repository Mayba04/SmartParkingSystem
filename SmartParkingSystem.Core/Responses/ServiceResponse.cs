using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core.Responses
{
    public class ServiceResponse<PayloadType, ErrorType>
    {
        public ServiceResponse(bool success = false, string message = "", PayloadType payload = default, IEnumerable<ErrorType> errors = default,
             string accessToken = "", string refreshToken = "")
        {
            Success = success;
            Message = message;
            Payload = payload;
            Errors = errors ?? Enumerable.Empty<ErrorType>();
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public PayloadType Payload { get; set; } = default;
        public IEnumerable<ErrorType> Errors { get; set; } = Enumerable.Empty<ErrorType>();
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
    public class ServiceResponse : ServiceResponse<object, object>
    {
        public ServiceResponse(bool success = false, string message = "",
            object payload = default, IEnumerable<object> errors = default,
            string accessToken = "", string refreshToken = "")
            : base(success, message, payload, errors, accessToken, refreshToken) { }
    }
}
