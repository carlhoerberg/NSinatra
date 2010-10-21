using NSinatra;

namespace Example
{
    public class App : NSinatraBase
    {
        public App()
        {
            Get("/", delegate 
            {
                var str = "Hello";
                return Content(str);
            });

            Get("/test", param =>
            {
                return NHaml("Index");
            });

            Get("/test/:id", param =>
            {
                var id = param.id;
                var data = string.Format("id = {0}", id);
                return Content(data);
            });
        }
    }
}
