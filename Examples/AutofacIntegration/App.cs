using System;
using Autofac;
using NSinatra;
using System.Web;

namespace AutofacIntegration
{
    public class App : NSinatraBase, IHttpModule
    {
        private static IContainer container;
        public App()
        {
            Get("/", delegate
            {
                using (var requestScope = container.BeginLifetimeScope())
                {
                    var global = container.ResolveNamed<DateTime>("now");
                    var request = requestScope.ResolveNamed<DateTime>("now");
                    var content = string.Format("Global: {0:HH:mm:ss}<br/>Request:{1:HH:mm:ss}", global, request);
                    return Content(content);
                }
            });
        }

        public void Init(HttpApplication context)
        {
            var builder = new ContainerBuilder();
            builder.Register(c => DateTime.Now)
                .Named<DateTime>("now")
                .InstancePerLifetimeScope();
            container = builder.Build();
        }

        public void Dispose()
        {
            container.Dispose();
        }
    }
}