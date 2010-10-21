using System.Web;

namespace NSinatra
{
    public abstract class ActionResult
    {
        public abstract void WriteToResponse(HttpContextBase context);
    }
}