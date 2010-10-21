using NSinatra;

namespace Example
{
    public class App : NSinatraBase
    {
        public App()
        {
            Get("/", param => Content("<h1>Welcome to NSinatra!</h1>"));
            
            Get("/test/:id", param => NHaml("Index", param.id));
            
            Get("/:name", param => Content("The parameter name is " + param.name));
        }
    }
}
