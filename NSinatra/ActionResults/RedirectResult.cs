using System.Web;

namespace NSinatra.ActionResults
{
    public class RedirectResult : ActionResult
    {
        private readonly string url;
        private readonly bool permanent;

        public RedirectResult(string url, bool permanent = false)
        {
            this.url = url;
            this.permanent = permanent;
        }

        public override void WriteToResponse(HttpContextBase context)
        {
            if (permanent)
                context.Response.RedirectPermanent(url);
            else
                context.Response.Redirect(url);
        }
    }
}