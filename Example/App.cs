using NSinatra;

namespace Example
{
    public class App : NSinatraBase
    {
        public App()
        {
            Get("/", param => Content("<h1>Welcome to NSinatra!</h1>"));
            
            Get("/test", param => NHaml("Index"));

            Get("/test/:id", param =>
            {
                var data = string.Format("id = {0}", param.id);
                return Content(data);
            });

            Get("/:name", param => Content("The parameter name is " + param.name));

        }
    }
}
