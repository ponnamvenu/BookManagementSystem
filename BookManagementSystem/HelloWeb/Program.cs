using ConceptArchitect.Web;
using HelloWeb.services;


class Program
{ 
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //Service Registration


        builder.Services.AddSingleton<SimpleGreetService>();

         builder.Services.AddSingleton<TimeUtils>();

        builder.Services.AddSingleton<IGreetService,TimedGreetService>();
        builder.Services.AddSingleton<IGreetService, AdvancedGreetService>();
        builder.Services.AddSingleton<IGreetService, ConfigurableGreetService>();

        builder.Services.AddMvc(opt =>
        {
            opt.EnableEndpointRouting = false;
        });


        //WebApplication creation and Middleware configuration
        var app = builder.Build();

        Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");

        if (app.Environment.IsDevelopment())
            //app.UseDeveloperMode();
            app.UseDeveloperExceptionPage(); //builtin
        else
            app.UseSimpleError();


        if (app.Environment.EnvironmentName == "HarryPotter")
        {
            app
                .UseUrlResponse("/hogwards", _ => "Welcome to Hogwards, The school of Wizards and Witchcraft!")
                .UseUrlResponse("/platform", _ => "Platform 9 3/4");
        }




        app.UseUrlResponse("/greeter", c =>
        {
            var pathFragments = c.Request.Path.Value.Split('/');
            var name = pathFragments[pathFragments.Length - 1]; //last path
            if (pathFragments.Length < 3)
                throw new InvalidOperationException("Name Not Supplied");

            var service=app.Services.GetService<IGreetService>();
            return service.Greet(name);
        });


        app.UseUrlResponse("/single-greet", c =>
        {
            var pathFragments= c.Request.Path.Value.Split('/');
            var name= pathFragments[pathFragments.Length-1]; //last path
            if (pathFragments.Length < 3)
                name = "World";

            var service = app.Services.GetService<SimpleGreetService>();

            return service.Greet(name);
        });

        app.UseUrlResponse("/simple-greet", c =>
        {
            var name = c.Request.Query["name"].ToString();

            if (string.IsNullOrEmpty(name))
                name = "World";           

            

            var service = new SimpleGreetService();
            return service.Greet(name);
        });


        //ConfigureAppV1(app);

        app.UseMvc(routes =>
        {
            routes.MapRoute("DefaultRoute", "{controller}/{action}/{id}", new
            {
                Id=""
            });
        });

        app.Run(); //runs server and waits for request


    }

    private static void ConfigureAppV1(WebApplication app)
    {
        app
          .UseSimpleError();
        //.UseStaticResource()

        app.UseDefaultFiles(); //treat index.html as default
        app.UseStaticFiles();




        app.UseReferer("/images", async (context, next) =>
        {
            await context.Response.WriteAsync($"You Got It : {context.Request.Path}");
        }, "http://booksweb.org")
           .UseUrlResponse("/handle-login", c =>
           {
               var req = c.Request;
               var email = req.Form["email"];
               var password = req.Form["password"];
               if (email == "admin@booksweb.org" && password == "nimda")
               {
                   return "Welcome Admin!!!";
               }
               else
               {
                   c.Response.StatusCode = 401;
                   return "Invalid Credentials";
               }

           })
          .UseUrlResponse("date", _ => DateTime.Now.ToLongDateString())
          .UseUrlResponse("time", _ => DateTime.Now.ToLongTimeString())
          .UseUrlResponse<Exception>("/error", c => throw new Exception($"{c.Request.Path.ToString().Replace("/error", "")}"))
          .UseUrlResponse("/path", c => c.Request.Path)
          ;
    }

    private static void AppConfigurationLifeCycle(WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            Console.WriteLine("Inside Date Handler");

            if (context.Request.Path == "/date")
            {
                Console.WriteLine("Sending date...");
                await context.Response.WriteAsync(DateTime.Now.ToLongDateString());
            }
            else
            {
                Console.WriteLine("Passing to next");
                await next(context);
            }


        });

        app.Run(async context =>
        {
            Console.WriteLine($"{context.Request.Method} {context.Request.Path}");
            await context.Response.WriteAsync($"Hello from {context.Request.Method} {context.Request.Path}");
        });



        Console.WriteLine("Server Started...");
    }

    private static void AddAppHandlers(WebApplication app)
    {
        app
            .UseUrlResponse("date", _ => DateTime.Now.ToLongDateString())
            .UseUrlResponse("time", _ => DateTime.Now.ToLongTimeString())
            .UseUrlResponse<Exception>("/error", c => throw new Exception($"{c.Request.Path.ToString().Replace("/error", "")}"))
            .UseUrlResponse("/path", c => c.Request.Path);
    }

    private static void UrlConfigurationV3(WebApplication app)
    {
        app
            .UseUrlResponse("/date", _ => DateTime.Now.ToLongDateString() + "\n")
            .UseUrlResponse("/time", _ => DateTime.Now.ToLongTimeString() + "\n")
            .UseUrlResponse("/path", c => c.Request.Path)
            .Run(async context =>
            {
                await context.Response.WriteAsync($"{context.Request.Method} {context.Request.Path}\n");
            });
    }

    private static WebApplication DateTimeConfigurationRouteV2(WebApplication app)
    {
        return app
                    .UseOnUrl("/date", async context =>
                    {
                        await context.Response.WriteAsync($"{DateTime.Now.ToLongDateString()}\n");
                    })
                    .UseOnUrl("/time", async context =>
                    {
                        await context.Response.WriteAsync($"{DateTime.Now.ToLongTimeString()}");
                    });
    }

    private static void DateTimeConfigurationRouteV1(WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            if (context.Request.Path.ToString().Contains("/date"))
                await context.Response.WriteAsync(DateTime.Now.ToLongDateString() + "\n");
            else
                await next(context);
        });

        app.Use(async (context, next) =>
        {
            if (context.Request.Path.ToString().Contains("/time"))
                await context.Response.WriteAsync(DateTime.Now.ToLongTimeString() + "\n");
            else
                await next(context);
        });
    }



  

}

