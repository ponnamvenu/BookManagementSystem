namespace HelloWeb.services
{
    public class SimpleGreetService : IGreetService
    {
        static int instanceCount;
        public int Id { get; set; } = ++instanceCount;

        public string Greet(string name)
        {
            return $"Hello {name}, Welcome to Simple Greet Service #{Id}";
        }
    }
}
