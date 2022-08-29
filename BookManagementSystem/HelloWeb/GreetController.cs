using Microsoft.AspNetCore.Mvc;

namespace HelloWeb
{
    public class GreetController
    {
        public string Hello()
        {
            return "Hello World";
        }

        public DateTime TellTheTime()
        {
            return DateTime.Now;
        }
    }
}
