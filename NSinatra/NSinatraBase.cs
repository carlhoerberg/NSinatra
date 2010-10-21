using System;
using System.Collections.Generic;
using System.Web;
using NSinatra.ActionResults;

namespace NSinatra
{
    public class NSinatraBase : IHttpHandler
    {
        private readonly IDictionary<string, Func<ActionResult>> getRoutes = new Dictionary<string, Func<ActionResult>>();
        private readonly IDictionary<string, Func<ActionResult>> postRoutes = new Dictionary<string, Func<ActionResult>>();
        
        public void Get(string route, Func<ActionResult> action)
        {
            getRoutes.Add(route, action);
        }

        public void Post(string route, Func<ActionResult> action)
        {
            postRoutes.Add(route, action);
        }

        public NHamlResult NHaml(string templateName)
        {
            return new NHamlResult(templateName);
        }

        public void ProcessRequest(HttpContext context)
        {
            var path = context.Request.Url.AbsolutePath;
            var routes = GetRoutes(context);

            ActionResult result;
            if (routes.ContainsKey(path))
            {
                var action = routes[path];
                result = action.Invoke();
            }
            else
            {
                result = new NotFoundResult();
            }
            var wrapper = new HttpContextWrapper(context);            
            result.WriteToResponse(wrapper);
        }

        private IDictionary<string, Func<ActionResult>> GetRoutes(HttpContext context)
        {
            IDictionary<string, Func<ActionResult>> routes;
            switch (context.Request.HttpMethod)
            {
                case "GET":
                    routes = getRoutes;
                    break;
                case "POST":
                    routes = postRoutes;
                    break;
                default:
                    throw new NotSupportedException("HttpMethod {0} is not supported");
            }
            return routes;
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
