using HelloWeb.services;

namespace SimpleMVC.Controllers
{
    public class GreetController
    {
        IGreetService service;
        public GreetController(IGreetService service)
        {
            this.service = service;
        }

        public string Hello()
        {
            return "Hello World!!!";
        }

        public string To(string id)
        {
            return service.Greet(id);
        }
    }
}
