using System;
using System.Web;
using NHaml;

namespace NSinatra.ActionResults
{
    public class NHamlResult : ActionResult
    {
        private readonly string templateName;
        private readonly dynamic viewData;

        public NHamlResult(string templateName)
        {
            this.templateName = templateName;
        }

        public NHamlResult(string templateName, dynamic viewData)
        {
            this.templateName = templateName;
            this.viewData = viewData;
        }

        public override void WriteToResponse(HttpContextBase context)
        {
            var path = context.Server.MapPath(string.Format("/Views/{0}.haml", templateName));
            var engine = new TemplateEngine();
            var compiledTemplate = engine.Compile(path, typeof(NSinatraNHamlView));
            var instance = (NSinatraNHamlView)compiledTemplate.CreateInstance();
            instance.ViewData = viewData;
            instance.Render(context.Response.Output);
        }

        public class NSinatraNHamlView : Template
        {
            public dynamic ViewData { get; set; }
        }
    }

    
}