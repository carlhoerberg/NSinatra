using System;
using System.Web;

namespace NSinatra.ActionResults
{
    public class NotFoundResult : ActionResult
    {
        public override void WriteToResponse(HttpContextBase context)
        {
            context.Response.StatusCode = 404;
        }
    }
}