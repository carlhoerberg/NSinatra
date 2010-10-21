using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using NSinatra.ActionResults;
using System.Text.RegularExpressions;

namespace NSinatra
{
    public class NSinatraBase : IHttpHandler
    {
        private readonly IDictionary<string, Func<dynamic, ActionResult>> getRoutes = new Dictionary<string, Func<dynamic, ActionResult>>();
        private readonly IDictionary<string, Func<dynamic, ActionResult>> postRoutes = new Dictionary<string, Func<dynamic, ActionResult>>();

        public void Get(string route, Func<dynamic, ActionResult> action)
        {
            getRoutes.Add(route, action);
        }

        public void Post(string route, Func<dynamic, ActionResult> action)
        {
            postRoutes.Add(route, action);
        }

        public NHamlResult NHaml(string templateName)
        {
            return new NHamlResult(templateName);
        }

        public StringResult Content(string content)
        {
            return new StringResult(content);
        }

        public void ProcessRequest(HttpContext context)
        {
            var url = context.Request.Url.AbsolutePath;
            var routes = GetRoutes(context.Request.HttpMethod);

            ActionResult result = null;
            foreach (var path in routes.Keys)
            {
                var pattern = Regex.Replace(path, "/:([^/]*)", "/([^/]*)");
                if (!Regex.IsMatch(url, pattern)) continue;

                var urlParts = url.Split('/');
                var patternParts = path.Split('/');
                IDictionary<string, object> param = new ExpandoObject();
                for (var i = 0; i < patternParts.Length; i++)
                {
                    if (!patternParts[i].StartsWith(":")) continue;
                    param.Add(patternParts[i].Substring(1), urlParts[i]);
                }
                var action = routes[path];
                result = action.Invoke(param);
            }
            result = result ?? new NotFoundResult();
            var wrapper = new HttpContextWrapper(context);
            result.WriteToResponse(wrapper);
        }

        private IDictionary<string, Func<dynamic, ActionResult>> GetRoutes(string verb)
        {
            IDictionary<string, Func<dynamic, ActionResult>> routes;
            switch (verb)
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
