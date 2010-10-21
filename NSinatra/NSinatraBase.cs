using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
            var path = context.Request.Url.AbsolutePath;
            var formParams = context.Request.Form;
            var queryParams = context.Request.QueryString;
            var result = GetActionResult(context.Request.HttpMethod, path, formParams, queryParams);
            var wrapper = new HttpContextWrapper(context);
            result.WriteToResponse(wrapper);
        }

        public bool IsReusable
        {
            get { return false; }
        }

        private ActionResult GetActionResult(string verb, string url, NameValueCollection formParams, NameValueCollection queryParams)
        {
            var routes = GetRoutes(verb);
            foreach (var route in routes)
            {
                var pattern = Regex.Replace(route.Key, "/:([^/]*)", "/([^/]*)");
                if (!Regex.IsMatch(url, "^" + pattern + "$")) continue;

                IDictionary<string, object> parameters = new ExpandoObject();

                foreach (string formKey in formParams)
                    parameters.Add(formKey, formParams[formKey]);
                foreach (string queryKey in queryParams)
                    parameters.Add(queryKey, queryParams[queryKey]);

                var urlParts = url.Split('/');
                var parts = route.Key.Split('/');
                for (var i = 0; i < parts.Length; i++)
                {
                    if (!parts[i].StartsWith(":")) continue;
                    var key = parts[i].Substring(1);
                    var value = urlParts[i];
                    parameters.Add(key, value);
                }
                var action = route.Value;
                return action.Invoke(parameters);
            }

            return new NotFoundResult();
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
                    throw new NotSupportedException(string.Format("HttpMethod {0} is not supported", verb));
            }
            return routes;
        }
    }
}
