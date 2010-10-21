using NSinatra;
using NSinatra.ActionResults;

namespace Example
{
    public class App : NSinatraBase
    {
        public App()
        {
            Get("/", () =>
                         {
                             return new StringResult("Hello");
                         });

            Get("/test", () =>
                             {
                                 return NHaml("Index");
                             });            
        }
    }
}
