using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using NSinatra.ActionResults;

namespace NSinatra
{
    public class NSinatraBase : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var wrapper = new HttpContextWrapper(context);
            IDictionary<string, object> parameters = new ExpandoObject();
            AddFormAndQueryStringParameters(parameters, wrapper.Request);

            var path = context.Request.Url.AbsolutePath;
            var result = GetActionResult(context.Request.HttpMethod, path, parameters);

            result.WriteToResponse(wrapper);
        }

        private static void AddFormAndQueryStringParameters(IDictionary<string, object> parameters, HttpRequestBase request)
        {
            foreach (string key in request.QueryString)
                parameters.Add(key, request.QueryString[key]);

            foreach (string key in request.Form)
                parameters.Add(key, request.Form[key]);
        }

        public bool IsReusable
        {
            get { return false; }
        }

        private readonly IList<Route> routes = new List<Route>();

        public void Get(string route, Func<dynamic, ActionResult> action)
        {
            routes.Add(new Route("GET", route, action));
        }

        public void Post(string route, Func<dynamic, ActionResult> action)
        {
            routes.Add(new Route("POST", route, action));
        }

        private ActionResult GetActionResult(string verb, string url, IDictionary<string, object> parameters)
        {
            var route = routes.FirstOrDefault(r => r.Verb == verb && r.DoesMatchUrl(url));
            if (route == null) return new NotFoundResult();

            foreach (var part in route.GetMatchingPartsFromUrl(url))
                parameters.Add(part.Key, part.Value);

            return route.Action.Invoke(parameters);
        }

        public NHamlResult NHaml(string templateName)
        {
            return new NHamlResult(templateName);
        }

        public NHamlResult NHaml(string templateName, object viewData)
        {
            return new NHamlResult(templateName, viewData);
        }

        public StringResult Content(string content)
        {
            return new StringResult(content);
        }
    }
}
