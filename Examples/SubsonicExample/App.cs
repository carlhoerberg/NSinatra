using System;
using System.Linq;
using NSinatra;
using SubsonicExample.Data;

namespace SubsonicExample
{
    public class App : NSinatraBase
    {
        public App()
        {
            Get("/", delegate
            {
                var contacts = Contact.All().OrderBy(c => c.Name).ToList();
                return NHaml("Index", contacts);
            });

            Get("/Create", p => NHaml("Create"));

            Get("/:id", param =>
            {
                int id = int.Parse(param.id);
                var contact = new Contact(c => c.Id == id);
                return NHaml("Details", contact);
            });

            Get("/Edit/:id", p =>
            {
                int id = Convert.ToInt32(p.id);
                var contact = new Contact(c => c.Id == id);
                return NHaml("Edit", contact);
            });

            Post("/", @params =>
            {
                var contact = new Contact
                {
                    Name = @params.Name,
                    Email = @params.Email
                };
                contact.Save();
                return Redirect("/" + contact.Id);
            });

            Post("/:id", param =>
            {
                int id = int.Parse(param.id);
                var contact = new Contact(c => c.Id == id)
                {
                    Name = param.Name,
                    Email = param.Email
                };
                contact.Save();
                return Redirect("/" + contact.Id);
            });
        }
    }
}
