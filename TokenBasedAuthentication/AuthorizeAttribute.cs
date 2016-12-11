using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TokenBasedAuthentication
{
    public class AuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {
        public string Message { get; set; }
        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                //actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                //actionContext.Response.ReasonPhrase = "Invalid Login Details !";
                base.HandleUnauthorizedRequest(actionContext);
            }
            else {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
            }
        }
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //actionContext.Response.Content
            base.OnAuthorization(actionContext);
        }
    }
}