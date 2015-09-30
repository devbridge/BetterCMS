using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace BetterCms.Module.Api.ApiExtensions
{
    public class ValidationAtttibute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false)
            {
                var response = new ErrorResponse() { ResponseStatus = new ResponseStatus() { errorCode = "400" } };

                foreach (KeyValuePair<string, ModelState> keyValue in actionContext.ModelState)
                {
                    var msg = keyValue.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault();
                    if (msg != null)
                    {
                        response.ResponseStatus.errors.Add(new ResponseError() { errorCode = "400", fieldName = keyValue.Key, message = msg });
                    }
                }

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, response);
            }
        }
    }
}