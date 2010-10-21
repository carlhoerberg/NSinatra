using System;
using System.Web;
using NHaml;

namespace NSinatra.ActionResults
{
    public class NHamlResult : ActionResult
    {
        private readonly string templateName;

        public NHamlResult(string templateName)
        {
            this.templateName = templateName;
        }

        public override void WriteToResponse(HttpContextBase context)
        {
            var path = context.Server.MapPath(string.Format("/Views/{0}.haml", templateName));
            var engine = new TemplateEngine();
            var compiledTemplate = engine.Compile(path);
            var instance = compiledTemplate.CreateInstance();
            instance.Render(context.Response.Output);
        }
    }
}