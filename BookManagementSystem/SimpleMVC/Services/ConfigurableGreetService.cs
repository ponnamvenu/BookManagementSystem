using System.Text;

namespace HelloWeb.services
{

    class GreeterConfig
    {
        public string Prefix { get; set; } = "Hello";
        public string Suffix { get; set; } = " Welcome to our service";
        public bool TimedGreet { get; set; } //=false
        public int Times { get; set; } = 1;


    }

    public class ConfigurableGreetService : IGreetService
    {


        public TimeUtils TimeUtils { get; set; }
        private GreeterConfig greeterConfig=new GreeterConfig();


        public ConfigurableGreetService(IConfiguration _configuration, TimeUtils utils)
        {
            TimeUtils= utils;
            _configuration.Bind("greeter", greeterConfig);

           
        }

        

        public string Greet(string name)
        {
            var prefix = greeterConfig.TimedGreet ?  $"Good {TimeUtils.TimePrefix}": greeterConfig.Prefix ;
            var greeting = new StringBuilder();
            for(int i=0;i<greeterConfig.Times;i++)
            {
                greeting.AppendLine($"{prefix}, {name}, {greeterConfig.Suffix}");
            }

            return greeting.ToString();
        }
    }
}
