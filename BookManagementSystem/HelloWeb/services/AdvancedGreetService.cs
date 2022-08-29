using System.Text;

namespace HelloWeb.services
{
    public class AdvancedGreetService : IGreetService
    {
        public string prefix;

        public string Prefix
        {
            get
            {
                if (TimedGreet)
                    return $"Good {TimeUtils.TimePrefix}";
                else
                    return prefix??"Hello ";
            }
        }


        public string suffix;

        public string Suffix => suffix ?? "Welcome to our service";
        
        public bool TimedGreet { get; set; }
        public int Times { get; set; } = 1;

        public TimeUtils TimeUtils { get; set; }


        public AdvancedGreetService(IConfiguration _configuration, TimeUtils utils)
        {
            // Prefix = _configuration["prefix"];
            // Suffix = _configuration["suffix"];
            TimeUtils= utils;

            prefix = _configuration["greeter:prefix"];
            suffix = _configuration["greeter:suffix"];
            TimedGreet =bool.Parse( _configuration["greeter:timedGreet"]);
            Times = (int) Convert.ChangeType(_configuration["greeter:times"], typeof(int));
        }

        

        public string Greet(string name)
        {
            var greeting = new StringBuilder();
            for(int i=0;i<Times;i++)
            {
                greeting.AppendLine($"{Prefix},{name},{Suffix}");
            }

            return greeting.ToString();
        }
    }
}
