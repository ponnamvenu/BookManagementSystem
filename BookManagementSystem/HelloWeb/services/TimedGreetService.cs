namespace HelloWeb.services
{
    public class TimedGreetService : IGreetService
    {
        TimeUtils utils;
        public TimedGreetService(TimeUtils utils)
        {
            this.utils = utils;
        }
        public string Greet(string name)
        {
            return $"Good {utils.TimePrefix} {name}, Welcome to our Service!";
        }
    }
}
