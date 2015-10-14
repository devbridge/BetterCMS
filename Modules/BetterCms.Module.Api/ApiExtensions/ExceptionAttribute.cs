using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace BetterCms.Module.Api.ApiExtensions
{
    public class ExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var response = new ErrorResponse { ResponseStatus = new ResponseStatus() { errorCode = actionExecutedContext.Exception.GetType().Name, message = actionExecutedContext.Exception.Message, stackTrace = actionExecutedContext.Exception.StackTrace } };
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, response);
        }
    }
}