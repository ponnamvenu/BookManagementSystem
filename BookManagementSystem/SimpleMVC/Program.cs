using HelloWeb.services;

namespace SimpleMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //configure services
            builder.Services.AddSingleton<TimeUtils>();
            builder.Services.AddSingleton<IGreetService,ConfigurableGreetService>();

            builder.Services.AddMvc(opt =>
            {
                opt.EnableEndpointRouting = false;
            });

            var app = builder.Build();



            //configure Middleware

            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute("Default",
                                "{controller}/{action}/{id}",
                                    new
                                    {
                                        Id = "",
                                        Action = "Hello",
                                        Controller = "Greet"
                                    }
                                );
            });

            app.Run();
        }
    }
}