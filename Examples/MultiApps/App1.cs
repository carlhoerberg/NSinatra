using NSinatra;

namespace MultiApps
{
    public class App1 : NSinatraBase
    {
        public App1()
        {
            Get("/", delegate { return Content("Hello App1"); });
        }
    }

    public class App2 : NSinatraBase
    {
        public App2()
        {
            Get("/App2", delegate { return Content("Hello App2"); });
        }
    }
}