using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace DemoUnity.ServiceClients.Shared
{
    public class AccessTokenNotFoundException : Exception
    {
        public AccessTokenNotFoundException() : base("AccessToken is not found")
        { }
    }

    public class UnauthorizedException : HttpException
    {
        public UnauthorizedException(string error, HttpRequestMessage request = null) : base(HttpStatusCode.Unauthorized, error, request)
        {
            StatusCode = HttpStatusCode.Unauthorized;
        }
    }

    public class HttpException : Exception
    {
        public HttpStatusCode StatusCode { get; protected set; }
        public string Error { get; set; }

        public HttpException(HttpStatusCode statusCode, string error, HttpRequestMessage request = null,
            Exception innerException = null)
            : base(error, innerException)
        {
            var errors = new List<string>
            {
                error,
                $"RequestUri {request?.RequestUri}"
            };

            StatusCode = statusCode;
            Error = string.Join("\r\n   at ", errors);
        }
    }
}