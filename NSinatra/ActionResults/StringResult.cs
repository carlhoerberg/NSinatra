using System;
using System.Web;

namespace NSinatra.ActionResults
{
    public class StringResult : ActionResult
    {
        private readonly string data;

        public StringResult(string data)
        {
            this.data = data;
        }
        
        public override void WriteToResponse(HttpContextBase context)
        {
            context.Response.Write(data);
        }
    }
}