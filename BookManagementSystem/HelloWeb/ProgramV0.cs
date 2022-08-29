class ProgramV0
{
    static void Main0(string []args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();



        app.Run(async context =>
        {
            await context.Response.WriteAsync(DateTime.Now.ToLongDateString());
        });



        app.Run(TellTime);

        app.Run(SayHello);

        app.Run(); //runs server and waits for request
    }

    static async Task TellTime(HttpContext context)
    {
        await context.Response.WriteAsync($"{DateTime.Now.ToLongTimeString()}");
    }

    static Task SayHello(HttpContext context)
    {
        return context.Response.WriteAsync($".Net Core says Hello at :" +
            $"{context.Request.Method} {context.Request.Path}");
    }
}



