using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCms.Module.Api.ApiExtensions
{
    public class ErrorResponse
    {
        public ResponseStatus ResponseStatus { get; set; }
    }

    public class ResponseStatus
    {
        public string errorCode { get; set; }
        public string message { get; set; }
        public string stackTrace { get; set; }
        public List<ResponseError> errors { get; set; }

        public ResponseStatus()
        {
            errors = new List<ResponseError>();
        }
    }

    public class ResponseError
    {
        public string errorCode { get; set; }
        public string message { get; set; }
        public string fieldName { get; set; }
    }
}